﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitOrder_PaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (7, CAST(85000.00 AS Decimal(18, 2)), CAST(N'2021-07-12T17:00:34.2766485' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'MoMo', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:49.9703598' AS DateTime2), NULL, CAST(N'2021-07-14T00:01:49.9703002' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:41.9886403' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T00:01:24.2225363' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (8, CAST(67000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T00:25:15.9151258' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:55.4464629' AS DateTime2), NULL, CAST(N'2021-07-14T00:01:55.4464626' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:41.9886874' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T00:01:19.5283772' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1002, CAST(114000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T09:10:44.9570598' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:02:00.6777073' AS DateTime2), NULL, CAST(N'2021-07-14T00:02:00.6777071' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:41.9886879' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T00:01:07.4045928' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1003, CAST(33000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T09:22:44.1559920' AS DateTime2), N'Đã hủy', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD', N'e708d9fc-0244-43aa-8092-96f0304d2007', CAST(N'2021-07-13T09:23:02.6359084' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'Không đủ hàng hóa', N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1004, CAST(63000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T20:57:02.5327183' AS DateTime2), N'Chờ lấy hàng', N'Đường 1 - Phường 4 - Quận 5 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'PayPal', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:07:40.6517404' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:07:40.6517409' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1005, CAST(107000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T23:08:07.9426932' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:02:05.9983260' AS DateTime2), NULL, CAST(N'2021-07-14T00:02:05.9983258' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T00:01:41.9886884' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-13T23:38:26.6908007' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1007, CAST(111000.00 AS Decimal(18, 2)), CAST(N'2021-07-14T10:36:18.7052210' AS DateTime2), N'Đã nhận hàng', N'Đường 42 - Phường 7 - Thành phố Vũng Tàu - Bà Rịa - Vũng Tàu', N'6353aab3-667c-4009-8e97-136c16fe3322', N'MoMo', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T10:45:06.2678697' AS DateTime2), NULL, CAST(N'2021-07-14T10:45:06.2678036' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T10:38:52.5804273' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T10:37:57.6455680' AS DateTime2), NULL, N'Vũng Tàu')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1008, CAST(114000.00 AS Decimal(18, 2)), CAST(N'2021-07-14T10:46:55.1959413' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'PayPal', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T10:47:59.1421073' AS DateTime2), NULL, CAST(N'2021-07-14T10:47:59.1421071' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T10:47:52.2256983' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T10:47:42.9280414' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1009, CAST(213000.00 AS Decimal(18, 2)), CAST(N'2021-07-14T11:06:25.2956365' AS DateTime2), N'Đã nhận hàng', N'Số 3 - Phường Tân Phú - Quận 9 - Hồ Chí Minh', N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T11:06:54.2321555' AS DateTime2), NULL, CAST(N'2021-07-14T11:06:54.2319918' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T11:06:47.2253232' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T11:06:39.7298033' AS DateTime2), NULL, N'Hồ Chí Minh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1010, CAST(91200.00 AS Decimal(18, 2)), CAST(N'2021-07-14T21:16:17.1085594' AS DateTime2), N'Đang giao hàng', N'Sô 4 - Xã Đức Mỹ - Huyện Càng Long - Trà Vinh', N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T23:16:20.4765181' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T23:16:14.8901124' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:18:04.3413528' AS DateTime2), NULL, N'Trà Vinh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1011, CAST(62000.00 AS Decimal(18, 2)), CAST(N'2021-07-14T21:17:26.3792204' AS DateTime2), N'Chờ lấy hàng', N'Sô 4 - Xã Đức Mỹ - Huyện Càng Long - Trà Vinh', N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:19:46.5813356' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:19:46.5813359' AS DateTime2), NULL, N'Trà Vinh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1012, CAST(48000.00 AS Decimal(18, 2)), CAST(N'2021-07-14T21:22:40.6457306' AS DateTime2), N'Chờ lấy hàng', N'Sô 4 - Xã Đức Mỹ - Huyện Càng Long - Trà Vinh', N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD', N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T23:16:14.8901554' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'b79f6c7f-8164-46df-89be-774012aed7fd', CAST(N'2021-07-14T23:16:14.8901557' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:23:28.4330932' AS DateTime2), NULL, N'Trà Vinh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1013, CAST(27900.00 AS Decimal(18, 2)), CAST(N'2021-07-14T21:24:57.5710191' AS DateTime2), N'Chờ lấy hàng', N'Sô 4 - Xã Đức Mỹ - Huyện Càng Long - Trà Vinh', N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:25:24.3442521' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', CAST(N'2021-07-14T21:25:24.3442526' AS DateTime2), NULL, N'Trà Vinh')
INSERT [dbo].[Order] ([OrderId], [Total], [OrderDate], [Status], [Address], [UserId], [PaymentMethod], [ModifiedBy], [ModifiedDate], [AdminId], [ReceivedDate], [ShipperId], [ShipperPickOrderDate], [WarehouseId], [ExportWarehouseDate], [Note], [Province]) VALUES (1014, CAST(62600.00 AS Decimal(18, 2)), CAST(N'2021-07-15T01:53:50.3426897' AS DateTime2), N'Đã hủy', N'Đường 42 - Phường 7 - Thành phố Vũng Tàu - Bà Rịa - Vũng Tàu', N'6353aab3-667c-4009-8e97-136c16fe3322', N'MoMo', N'e708d9fc-0244-43aa-8092-96f0304d2007', CAST(N'2021-07-15T01:56:38.5350133' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'Hủy do đặt sai số lượng', N'Vũng Tàu')
SET IDENTITY_INSERT [dbo].[Order] OFF

INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (7, 1, 5, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (8, 1, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1002, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1003, 1, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đang chuẩn bị hàng')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1004, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1008, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1009, 1, 4, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (8, 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1002, 3, 3, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1007, 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1009, 3, 3, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1002, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1004, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1005, 4, 2, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1013, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1014, 1002, 3, CAST(0.00 AS Decimal(18, 2)), CAST(5500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1008, 1003, 3, CAST(0.00 AS Decimal(18, 2)), CAST(5500.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1014, 1003, 3, CAST(0.00 AS Decimal(18, 2)), CAST(5500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1007, 1004, 3, CAST(0.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1011, 1004, 2, CAST(0.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1008, 1005, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1005, 1006, 5, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1007, 1007, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1009, 1010, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1011, 1010, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1014, 1010, 1, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1010, 1012, 3, CAST(0.00 AS Decimal(18, 2)), CAST(14400.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1013, 1012, 1, CAST(0.00 AS Decimal(18, 2)), CAST(14400.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1010, 1013, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1009, 1016, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1012, 1016, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), N'Đã chuẩn bị xong')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1014, 1016, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)), NULL)

SET IDENTITY_INSERT [dbo].[MoMoPayment] ON 

INSERT [dbo].[MoMoPayment] ([MoMoPaymentId], [OrderId], [MoMoOrderId], [PayType], [ResponseTime], [Amount], [TransId]) VALUES (7, 7, N'3e8c7966-ba97-41dd-a43d-4f8be61b3fc3', N'qr', CAST(N'2021-07-12T17:00:31.0000000' AS DateTime2), N'85000', N'2544432309')
INSERT [dbo].[MoMoPayment] ([MoMoPaymentId], [OrderId], [MoMoOrderId], [PayType], [ResponseTime], [Amount], [TransId]) VALUES (1002, 1007, N'731a17ff-3a5c-4513-bf62-fb6793485626', N'qr', CAST(N'2021-07-14T10:36:16.0000000' AS DateTime2), N'111000', N'2546733198')
INSERT [dbo].[MoMoPayment] ([MoMoPaymentId], [OrderId], [MoMoOrderId], [PayType], [ResponseTime], [Amount], [TransId]) VALUES (1003, 1014, N'6a3ec85b-62eb-45f9-8669-f311bec9b2cc', N'qr', CAST(N'2021-07-15T01:53:49.0000000' AS DateTime2), N'62600', N'2547280274')
SET IDENTITY_INSERT [dbo].[MoMoPayment] OFF
SET IDENTITY_INSERT [dbo].[PayPalPayment] ON 

INSERT [dbo].[PayPalPayment] ([PayPalPaymentId], [OrderId], [Token], [PayerId], [LinkDetail], [CaptureId]) VALUES (1, 1004, N'6LB01124UF956252Y', N'7XQM988FWWXKA', N'https://api.sandbox.paypal.com/v2/checkout/orders/6LB01124UF956252Y', N'3AB11172XH924780H')
INSERT [dbo].[PayPalPayment] ([PayPalPaymentId], [OrderId], [Token], [PayerId], [LinkDetail], [CaptureId]) VALUES (2, 1008, N'40R41904CH487992P', N'7XQM988FWWXKA', N'https://api.sandbox.paypal.com/v2/checkout/orders/40R41904CH487992P', N'0A510482XL376980L')
SET IDENTITY_INSERT [dbo].[PayPalPayment] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}