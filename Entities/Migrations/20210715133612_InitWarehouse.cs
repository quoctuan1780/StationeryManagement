using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
SET IDENTITY_INSERT [dbo].[ReceiptRequest] ON 
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (9, CAST(N'2021-07-13T00:55:38.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Đã duyệt')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (10, CAST(N'2021-07-13T01:41:20.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Từ chối')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1002, CAST(N'2021-07-13T07:39:48.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Từ chối')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1009, CAST(N'2021-07-13T23:02:50.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Đã duyệt')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1010, CAST(N'2021-07-14T10:58:35.7312448' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Từ chối')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1011, CAST(N'2021-07-14T10:59:44.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Đã duyệt')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1012, CAST(N'2021-07-14T14:58:37.9417841' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Đã duyệt')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1013, CAST(N'2021-07-14T18:55:51.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Từ chối')
INSERT [dbo].[ReceiptRequest] ([ReceiptRequestId], [CreateDate], [UserId], [Status]) VALUES (1014, CAST(N'2021-07-14T20:13:17.0000000' AS DateTime2), N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', N'Từ chối')
SET IDENTITY_INSERT [dbo].[ReceiptRequest] OFF

INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (9, 1, 5, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (9, 4, 30, N'Chờ xác nhận', CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (10, 4, 5, N'Chờ xác nhận', CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1002, 4, 5, N'Chờ xác nhận', CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1014, 4, 30, N'Chờ xác nhận', CAST(16000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1002, 30, N'Chờ xác nhận', CAST(7000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1003, 30, N'Chờ xác nhận', CAST(7000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1004, 30, N'Chờ xác nhận', CAST(17000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1005, 30, N'Chờ xác nhận', CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1006, 30, N'Chờ xác nhận', CAST(18200.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1013, 1006, 5, N'Chờ xác nhận', CAST(16000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1007, 30, N'Chờ xác nhận', CAST(18500.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1008, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1008, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1009, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1009, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1010, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1010, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1011, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1011, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1012, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1012, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1010, 1013, 30, N'Chờ xử lý', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1011, 1013, 30, N'Chờ xác nhận', CAST(14000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1009, 1016, 30, N'Chờ xác nhận', CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1012, 1017, 30, N'Chờ xử lý', CAST(16000.00 AS Decimal(18, 2)))
INSERT [dbo].[ReceiptRequestDetail] ([ReceiptRequestId], [ProductDetailId], [Quantity], [Status], [Price]) VALUES (1012, 1018, 30, N'Chờ xử lý', CAST(16000.00 AS Decimal(18, 2)))

SET IDENTITY_INSERT [dbo].[ImportWarehouse] ON 
INSERT [dbo].[ImportWarehouse] ([ImportWarehouseId], [CreateDate], [Total], [Status], [UserId], [ReceiptRequestId]) VALUES (6, CAST(N'2021-07-13T00:56:37.5215874' AS DateTime2), CAST(35.00 AS Decimal(18, 2)), N'Hoàn thành', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', 9)
INSERT [dbo].[ImportWarehouse] ([ImportWarehouseId], [CreateDate], [Total], [Status], [UserId], [ReceiptRequestId]) VALUES (1002, CAST(N'2021-07-13T23:05:47.9134297' AS DateTime2), CAST(210.00 AS Decimal(18, 2)), N'Hoàn thành', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', 1009)
INSERT [dbo].[ImportWarehouse] ([ImportWarehouseId], [CreateDate], [Total], [Status], [UserId], [ReceiptRequestId]) VALUES (1003, CAST(N'2021-07-14T11:00:38.1372989' AS DateTime2), CAST(180.00 AS Decimal(18, 2)), N'Hoàn thành', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', 1011)
INSERT [dbo].[ImportWarehouse] ([ImportWarehouseId], [CreateDate], [Total], [Status], [UserId], [ReceiptRequestId]) VALUES (1004, CAST(N'2021-07-14T18:51:36.3511534' AS DateTime2), CAST(60.00 AS Decimal(18, 2)), N'Đang nhập hàng', N'd2f38b82-2579-494c-9a67-e0cd6d6fd3a4', 1012)
SET IDENTITY_INSERT [dbo].[ImportWarehouse] OFF

INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (6, 1, 5, CAST(14000.00 AS Decimal(18, 2)), 5, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (6, 4, 30, CAST(13000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1002, 30, CAST(7000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1003, 30, CAST(7000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1004, 30, CAST(17000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1005, 30, CAST(18000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1006, 30, CAST(18200.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1007, 30, CAST(18500.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1008, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1009, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1010, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1011, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1012, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1003, 1013, 30, CAST(14000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1002, 1016, 30, CAST(19000.00 AS Decimal(18, 2)), 30, N'Hoàn thành')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1004, 1017, 30, CAST(16000.00 AS Decimal(18, 2)), 10, N'Đang nhập hàng')
INSERT [dbo].[ImportWarehouseDetail] ([ImportWarehouseId], [ProductDetailId], [Quantity], [ImportPrice], [ActualQuantity], [Status]) VALUES (1004, 1018, 30, CAST(16000.00 AS Decimal(18, 2)), 0, N'Đang nhập hàng')
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
