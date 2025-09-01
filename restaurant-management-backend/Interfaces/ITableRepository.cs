using restaurant_management_backend.Dtos.Table;
using restaurant_management_backend.Models.OrderAndOperations;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Interfaces
{
    public interface ITableRepository
    {
        public Task<ApiResponse<List<TableModel>>> GetAllTablesAsync();

        public Task<ApiResponse<TableModel>> GetTableByIdAsync(Guid tableId);

        public Task<ApiResponse<TableDto>> CreateTableAsync(UpsertTableRequestDto dto);

        public Task<ApiResponse<TableDto>> UpdateTableAsync(Guid tableId, UpsertTableRequestDto dto);

        public Task<ApiResponse<object>> DeleteTableAsync(Guid tableId);

        public Task<ApiResponse<object>> UpdateTableStatusAsync(Guid tableId, UpdateTableStatusDto dto);

        public Task<ApiResponse<ReservationDto>> CreateReservationAsync(CreateReservationDto dto);

        public Task<ApiResponse<object>> UpdateReservationStatusAsync(Guid reservationId, UpdateReservationStatusDto dto);

        public Task<ApiResponse<List<ReservationDto>>> GetReservationsByDateAsync(DateTime date);

    }
}
