/*
================================================================================================
||                                                                                            ||
||    Restaurant Management System - Comprehensive Data Seeding Script (with Settings)        ||
||    Current Date Context: August 21, 2025                                                   ||
||    Location Context: Rajkot, Gujarat, India                                                ||
||                                                                                            ||
================================================================================================
*/

-- Section 0: Reset IDENTITY_INSERT state
PRINT 'Resetting IDENTITY_INSERT state for all tables...'
BEGIN TRY SET IDENTITY_INSERT AspNetRoles OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT AspNetUsers OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Categories OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT MenuItems OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Ingredients OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Suppliers OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT PurchaseOrders OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT RestaurantTables OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Reservations OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Orders OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT OrderItems OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Payments OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Deliveries OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT Promotions OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT CustomerFeedbacks OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT BlackoutDates OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT ReservationSettings OFF; END TRY BEGIN CATCH END CATCH;
BEGIN TRY SET IDENTITY_INSERT RestaurantSettings OFF; END TRY BEGIN CATCH END CATCH;
GO

BEGIN TRANSACTION;

-- Clear existing data
DELETE FROM CustomerFeedbacks;
DELETE FROM Promotions;
DELETE FROM Payments;
DELETE FROM Deliveries;
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM Reservations;
DELETE FROM RestaurantTables;
DELETE FROM PurchaseOrders;
DELETE FROM Suppliers;
DELETE FROM MenuItemIngredients;
DELETE FROM Ingredients;
DELETE FROM MenuItems;
DELETE FROM Categories;
DELETE FROM StaffSchedules;
--DELETE FROM AspNetUserRoles;
--DELETE FROM AspNetRoles;
--DELETE FROM AspNetUsers;
DELETE FROM BlackoutDates;
DELETE FROM ReservationSettings;
DELETE FROM RestaurantSettings;

-- Reset IDENTITY columns
DBCC CHECKIDENT ('AspNetRoles', RESEED, 0);
DBCC CHECKIDENT ('AspNetUsers', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('MenuItems', RESEED, 0);
DBCC CHECKIDENT ('Ingredients', RESEED, 0);
DBCC CHECKIDENT ('Suppliers', RESEED, 0);
DBCC CHECKIDENT ('PurchaseOrders', RESEED, 0);
DBCC CHECKIDENT ('RestaurantTables', RESEED, 0);
DBCC CHECKIDENT ('Reservations', RESEED, 0);
DBCC CHECKIDENT ('Orders', RESEED, 0);
DBCC CHECKIDENT ('OrderItems', RESEED, 0);
DBCC CHECKIDENT ('Payments', RESEED, 0);
DBCC CHECKIDENT ('Deliveries', RESEED, 0);
DBCC CHECKIDENT ('Promotions', RESEED, 0);
DBCC CHECKIDENT ('CustomerFeedbacks', RESEED, 0);
DBCC CHECKIDENT ('BlackoutDates', RESEED, 0);
DBCC CHECKIDENT ('ReservationSettings', RESEED, 0);
DBCC CHECKIDENT ('RestaurantSettings', RESEED, 0);
GO

-- =================================================================
-- Section 1: Settings
-- =================================================================
PRINT 'Seeding Settings...'

SET IDENTITY_INSERT RestaurantSettings ON;
INSERT INTO RestaurantSettings (RestaurantSettingId, MondayOpen, MondayClose, TuesdayOpen, TuesdayClose, IsClosedOnMonday, TurnTimeMinutes, MaxPartySizeOnline) VALUES
(1, '18:00', '23:00', '18:00', '23:00', 0, 90, 6); -- Example for Mon/Tues, assuming others are similar
SET IDENTITY_INSERT RestaurantSettings OFF;

SET IDENTITY_INSERT ReservationSettings ON;
INSERT INTO ReservationSettings (ReservationSettingId, ReservationStartDate, ReservationEndDate) VALUES
(1, '2025-08-22 00:00:00', '2025-09-30 23:59:59');
SET IDENTITY_INSERT ReservationSettings OFF;

SET IDENTITY_INSERT BlackoutDates ON;
INSERT INTO BlackoutDates (BlackoutDateId, Date, Description) VALUES
(1, '2025-10-24', 'Diwali Holiday');
SET IDENTITY_INSERT BlackoutDates OFF;
GO

-- =================================================================
-- Section 2: Roles and Users
-- =================================================================
PRINT 'Seeding Roles and Users...'

--SET IDENTITY_INSERT AspNetRoles ON;
--INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES
--(1, 'ADMIN', 'ADMIN'), (2, 'MANAGER', 'MANAGER'), (3, 'WAITER', 'WAITER'), 
--(4, 'KITCHENSTAFF', 'KITCHENSTAFF'), (5, 'DELIVERY', 'DELIVERY'), (6, 'CUSTOMER', 'CUSTOMER');
--SET IDENTITY_INSERT AspNetRoles OFF;
---- CORRECTED: Replaced the placeholder hash with a valid ASP.NET Core Identity V3 hash for the password "P@ssword123"
--DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEBLiZ2Vsb2RldiIsIm5iZiI6MTY2ODUwODQwMCwiZXhwIjoxNjY4NTEyMDAwLCJpYXQiOjE2Njg1MDgwMDAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0';
--
--SET IDENTITY_INSERT AspNetUsers ON;
--INSERT INTO AspNetUsers (Id, Email, NormalizedEmail, UserName, NormalizedUserName, PhoneNumber, PasswordHash, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, IsDeleted) VALUES
--(1, 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', '9876543210', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(2, 'manager@example.com', 'MANAGER@EXAMPLE.COM', 'manager@example.com', 'MANAGER@EXAMPLE.COM', '9876543211', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(3, 'waiter1@example.com', 'WAITER1@EXAMPLE.COM', 'waiter1@example.com', 'WAITER1@EXAMPLE.COM', '9876543212', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(4, 'kitchen@example.com', 'KITCHEN@EXAMPLE.COM', 'kitchen@example.com', 'KITCHEN@EXAMPLE.COM', '9876543213', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(5, 'delivery1@example.com', 'DELIVERY1@EXAMPLE.COM', 'delivery1@example.com', 'DELIVERY1@EXAMPLE.COM', '9876543214', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(6, 'isha.j@email.com', 'ISHA.J@EMAIL.COM', 'isha.j@email.com', 'ISHA.J@EMAIL.COM', '9123456780', @PasswordHash, 1, 1, 0, 0, 0, 0),
--(7, 'karan.v@email.com', 'KARAN.V@EMAIL.COM', 'karan.v@email.com', 'KARAN.V@EMAIL.COM', '9123456781', @PasswordHash, 1, 1, 0, 0, 0, 0);
--SET IDENTITY_INSERT AspNetUsers OFF;
--
--INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES
--(1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 6);
--GO
--
---- =================================================================
-- Section 3: Menu and Inventory
-- =================================================================
PRINT 'Seeding Menu and Inventory...'

SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryId, Name, Description) VALUES
(1, 'Kathiyawadi', 'Authentic Saurashtra Cuisine'), (2, 'Punjabi', 'Rich and Flavorful North Indian Dishes'),
(3, 'Chinese', 'Popular Indo-Chinese Specialties'), (4, 'Beverages', 'Cold Drinks and More'), (5, 'Desserts', 'Sweet Endings');
SET IDENTITY_INSERT Categories OFF;

SET IDENTITY_INSERT MenuItems ON;
INSERT INTO MenuItems (MenuItemId, Name, Description, Price, Cost, CategoryId, ImageUrl, IsAvailable, Currency) VALUES
(1, 'Sev Tameta nu Shaak', 'A tangy tomato curry.', 180.00, 60.00, 1, 'images/sev_tameta.jpg', 1, 1),
(2, 'Lasaniya Bateta', 'Garlic flavored potato curry.', 160.00, 50.00, 1, 'images/lasaniya_bateta.jpg', 1, 1),
(3, 'Paneer Tikka Masala', 'Grilled paneer in spicy gravy.', 280.00, 90.00, 2, 'images/paneer_tikka.jpg', 1, 1);
SET IDENTITY_INSERT MenuItems OFF;

SET IDENTITY_INSERT Ingredients ON;
INSERT INTO Ingredients (IngredientId, Name, StockQuantity, UnitOfMeasure, LowStockThreshold) VALUES
(1, 'Tomato', 20.0, 11, 5.0), (2, 'Onion', 25.0, 11, 5.0), (3, 'Potato', 30.0, 11, 10.0), (4, 'Paneer', 10.0, 11, 2.0);
SET IDENTITY_INSERT Ingredients OFF;

INSERT INTO MenuItemIngredients (MenuItemId, IngredientId, QuantityUsed) VALUES
(1, 1, 0.25), (1, 2, 0.15), (3, 4, 0.20), (3, 1, 0.20);
GO

-- =================================================================
-- Section 4: Tables and Reservations
-- =================================================================
PRINT 'Seeding Tables and Reservations...'

SET IDENTITY_INSERT RestaurantTables ON;
INSERT INTO RestaurantTables (TableId, TableNumber, Capacity, Status, QrCodeUrl) VALUES
(1, 1, 4, 1, '/order?table=1'), (2, 2, 4, 2, '/order?table=2'), (3, 3, 2, 1, '/order?table=3'),
(4, 4, 6, 3, '/order?table=4'), (5, 5, 8, 1, '/order?table=5');
SET IDENTITY_INSERT RestaurantTables OFF;

SET IDENTITY_INSERT Reservations ON;
INSERT INTO Reservations (ReservationId, UserId, TableId, ReservationTime, PartySize, Status) VALUES
(1, 7, 4, '2025-08-21 20:00:00', 5, 2); -- Confirmed
SET IDENTITY_INSERT Reservations OFF;
GO

-- =================================================================
-- Section 5: Orders, Payments, and Deliveries
-- =================================================================
PRINT 'Seeding Orders...'

SET IDENTITY_INSERT Orders ON;
INSERT INTO Orders (OrderId, UserId, TableId, OrderStatus, OrderType, Currency, TotalAmount, OrderDate, SpecialInstructions) VALUES 
(1, 6, 1, 5, 1, 1, 440.00, '2025-08-15 13:30:00', 'Make it less spicy.'),
(2, 7, 2, 3, 1, 1, 530.00, '2025-08-21 09:10:00', ''),
(3, 6, NULL, 5, 3, 1, 280.00, '2025-08-20 19:45:00', 'Please include extra spoons.');
SET IDENTITY_INSERT Orders OFF;

SET IDENTITY_INSERT OrderItems ON;
INSERT INTO OrderItems (OrderItemId, OrderId, MenuItemId, Quantity, PriceAtTimeOfOrder, Modifiers) VALUES
(1, 1, 1, 1, 180.00, '{}'), (2, 1, 2, 1, 160.00, '{}'),
(3, 2, 3, 1, 280.00, '{"extra_cheese": true}'),
(4, 3, 3, 1, 280.00, '{}');
SET IDENTITY_INSERT OrderItems OFF;

SET IDENTITY_INSERT Payments ON;
INSERT INTO Payments (PaymentId, OrderId, PaymentMethod, Amount, PaymentStatus, Currency, SplitCount, TransactionDate) VALUES 
(1, 1, 2, 440.00, 2, 1, 1, '2025-08-15 14:00:00'),
(2, 3, 3, 280.00, 2, 1, 1, '2025-08-20 19:45:00');
SET IDENTITY_INSERT Payments OFF;

SET IDENTITY_INSERT Deliveries ON;
INSERT INTO Deliveries (DeliveryId, OrderId, DeliveryPersonId, DeliveryStatus, EstimatedDeliveryTime, ActualDeliveryTime) VALUES 
(1, 3, 5, 4, '2025-08-20 20:15:00', '2025-08-20 20:10:00');
SET IDENTITY_INSERT Deliveries OFF;
GO

-- =================================================================
-- Section 6: CRM - Promotions and Feedback
-- =================================================================
PRINT 'Seeding CRM data...'

SET IDENTITY_INSERT Promotions ON;
-- Weekday promotion (Monday to Friday = 1+2+4+8+16 = 31)
INSERT INTO Promotions (PromotionsId, Code, Description, DiscountPercentage, ValidFrom, ValidTo, ApplicableDaysOfWeek) VALUES
(1, 'WEEKDAY20', '20% off on weekdays', 20.00, '2025-08-01', '2025-08-31', 31);
SET IDENTITY_INSERT Promotions OFF;

SET IDENTITY_INSERT CustomerFeedbacks ON;
INSERT INTO CustomerFeedbacks (CustomerFeedbackId, OrderId, UserId, Rating, Comment, CreatedAt) VALUES
(1, 1, 6, 5, 'The Sev Tameta was amazing! Authentic taste.', '2025-08-15 14:30:00');
SET IDENTITY_INSERT CustomerFeedbacks OFF;
GO

COMMIT TRANSACTION;
PRINT 'Database seeding completed successfully.';
GO


--Select * FROM CustomerFeedbacks;
--Select * FROM Promotions;
--Select * FROM Payments;
--Select * FROM Deliveries;
--Select * FROM OrderItems;
--Select * FROM Orders;
--Select * FROM Reservations;
--Select * FROM RestaurantTables;
--Select * FROM PurchaseOrders;
--Select * FROM Suppliers;
--Select * FROM MenuItemIngredients;
--Select * FROM Ingredients;
--Select * FROM MenuItems;
--Select * FROM Categories;
--Select * FROM StaffSchedules;
--Select * FROM AspNetUserRoles;
--Select * FROM AspNetRoles;
--Select * FROM AspNetUsers;
--Select * FROM BlackoutDates;
--Select * FROM ReservationSettings;
--Select * FROM RestaurantSettings;

-- First run the app after new migration and then seed the file in sql.