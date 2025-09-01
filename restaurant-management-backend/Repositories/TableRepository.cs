using AutoMapper;
using Microsoft.EntityFrameworkCore;
using restaurant_management_backend.Data;
using restaurant_management_backend.Dtos.Table;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public TableRepository(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        // Tables

        public async Task<ApiResponse<List<TableModel>>> GetAllTablesAsync()
        {
            var tables = await _context.RestaurantTables.Include(t => t.Reservations).ToListAsync();
            return ApiResponse<List<TableModel>>.SuccessResponse(tables);
        }

        public async Task<ApiResponse<TableModel>> GetTableByIdAsync(Guid tableId)
        {

            var table = await _context.RestaurantTables
                .Include(t => t.Reservations)
                .Include(t => t.Orders)
                .FirstOrDefaultAsync(t => t.TableId == tableId);

            if(table == null)
                return ApiResponse<TableModel>.FailureResponse("No table found", 404);

            return ApiResponse<TableModel>.SuccessResponse(table);
        }

        public async Task<ApiResponse<TableDto>> CreateTableAsync(UpsertTableRequestDto dto)
        {
            var tableExists = await _context.RestaurantTables.AnyAsync(t => t.TableNumber == dto.TableNumber);
            if (tableExists)
            {
                return ApiResponse<TableDto>.FailureResponse($"A table with number {dto.TableNumber} already exists.", 409); // 409 Conflict
            }

            var table = _mapper.Map<TableModel>(dto);
            table.Status = TableStatusEnum.Available; // Default status for a new table

            _context.RestaurantTables.Add(table);
            await _context.SaveChangesAsync();

            var tableDto = _mapper.Map<TableDto>(table);
            return ApiResponse<TableDto>.SuccessResponse(tableDto, "Table created successfully.", 201);
        }

        public async Task<ApiResponse<TableDto>> UpdateTableAsync(Guid tableId, UpsertTableRequestDto dto)
        {
            var table = await _context.RestaurantTables.FindAsync(tableId);
            if (table == null)
            {
                return ApiResponse<TableDto>.FailureResponse("Table not found.", 404);
            }

            // Check if another table already has the new number
            var tableExists = await _context.RestaurantTables.AnyAsync(t => t.TableNumber == dto.TableNumber && t.TableId != tableId);
            if (tableExists)
            {
                return ApiResponse<TableDto>.FailureResponse($"A table with number {dto.TableNumber} already exists.", 409);
            }

            _mapper.Map(dto, table);
            await _context.SaveChangesAsync();
            var tableDto = _mapper.Map<TableDto>(table);
            return ApiResponse<TableDto>.SuccessResponse(tableDto, "Table updated successfully.");
        }

        public async Task<ApiResponse<object>> DeleteTableAsync(Guid tableId)
        {
            var table = await _context.RestaurantTables.FindAsync(tableId);
            if (table == null)
            {
                return ApiResponse<object>.FailureResponse("Table not found.", 404);
            }

            // Add safety check: cannot delete a table that has active orders or reservations
            var hasActiveLinks = await _context.Orders.AnyAsync(o => o.TableId == tableId && o.OrderStatus != OrderStatusEnum.Completed) ||
                                 await _context.Reservations.AnyAsync(r => r.TableId == tableId && r.Status == ReservationStatusEnum.Confirmed);

            if (hasActiveLinks)
            {
                return ApiResponse<object>.FailureResponse("Cannot delete a table with active orders or reservations.", 400);
            }

            _context.RestaurantTables.Remove(table);
            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Table deleted successfully.");
        }


        public async Task<ApiResponse<object>> UpdateTableStatusAsync(Guid tableId, UpdateTableStatusDto dto)
        {
            var table = await _context.RestaurantTables.FindAsync(tableId);
            if (table == null)
                return ApiResponse<object>.FailureResponse("Table not found.", 404);

            table.Status = dto.Status;
            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Table status updated successfully.");
        }

        // Reservation workflow

        public async Task<ApiResponse<ReservationDto>> CreateReservationAsync(CreateReservationDto dto)
        {
            var user = await _currentUser.GetUser();
            if (user == null)
                return ApiResponse<ReservationDto>.FailureResponse("User not found", 401);

            if(user.PhoneNumber == null)
                return ApiResponse<ReservationDto>.FailureResponse("Please add your phone number in your profile.", 401);

            var reservation = _mapper.Map<ReservationModel>(dto);
            reservation.Status = ReservationStatusEnum.Pending;
            reservation.UserId = user.Id;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var reservationDto = _mapper.Map<ReservationDto>(reservation);
            return ApiResponse<ReservationDto>.SuccessResponse(reservationDto, "Reservation created successfully.", 201);
        }

        public async Task<ApiResponse<object>> UpdateReservationStatusAsync(Guid reservationId, UpdateReservationStatusDto dto)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
                return ApiResponse<object>.FailureResponse("Reservation not found.", 404);

            if (dto.NewStatus == ReservationStatusEnum.Confirmed)
            {
                if (!dto.TableId.HasValue)
                    return ApiResponse<object>.FailureResponse("A table must be assigned to confirm reservation.", 400);

                var table = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == dto.TableId);
                if (table == null)
                    return ApiResponse<object>.FailureResponse("Table not found.", 404);

                //if (reservation.PartySize > table.Capacity)
                //    return ApiResponse<object>.FailureResponse($"Table capacity is {table.Capacity}, but party size is {reservation.PartySize}.", 400);

                // Check if table is already reserved/occupied at the same time
                //var windowStart = reservation.ReservationTime.AddMinutes(-1);
                //var windowEnd = reservation.ReservationTime.AddMinutes(30);
                //bool overlap = await _context.Reservations.AnyAsync(r =>
                //    r.TableId == table.TableId
                //    && r.ReservationTime <= windowEnd
                //    && r.Status == ReservationStatusEnum.Confirmed);
                //    //&& r.ReservationTime >= windowStart
                //    //r.ReservationTime == reservation.ReservationTime && // (optional: add time window, e.g. +/- 1 hour)
                //if (overlap)
                //    return ApiResponse<object>.FailureResponse("Table already reserved for this time slot.", 400);

                reservation.TableId = dto.TableId;
                reservation.Status = ReservationStatusEnum.Confirmed;
                table.Status = TableStatusEnum.Reserved;
            }
            else if (dto.NewStatus == ReservationStatusEnum.Seated)
            {
                if (reservation.Table != null)
                    reservation.Table.Status = TableStatusEnum.Occupied;
                    
                reservation.Status = ReservationStatusEnum.Seated;
            }
            else if (dto.NewStatus == ReservationStatusEnum.Cancelled || dto.NewStatus == ReservationStatusEnum.NoShow)
            {
                if (reservation.Table != null)
                    reservation.Table.Status = TableStatusEnum.Available;

                reservation.Status = dto.NewStatus;
            }
            else
            {
                reservation.Status = dto.NewStatus; // fallback
            }

            await _context.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, $"Reservation status updated to {dto.NewStatus}.");
        }

        public async Task<ApiResponse<List<ReservationDto>>> GetReservationsByDateAsync(DateTime date)
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.ReservationTime.Date == date.Date)
                .ToListAsync();

            var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);
            return ApiResponse<List<ReservationDto>>.SuccessResponse(reservationDtos);
        }

        // Implement half slot based reservation.
        // Implement telling probability of successful reservation at thier time slot for premium customers. 
        // For a user One table reservation for the one day.
    }
}
