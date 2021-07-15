using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[Category] ON 
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1, N'Truyện Tranh', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (2, N'Thước kẻ', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1002, N'Bút chì', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1003, N'Giấy A4', 0, 1)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1004, N'Giấy A3', 0, 1)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1005, N'Giấy A2', 0, 1)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1006, N'Vở viết', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1007, N'Kéo', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1008, N'Giấy in', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1009, N'Sổ tay', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1010, N'Tẩy', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1011, N'Hộp đựng bút', 0, 0)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [status], [IsDeleted]) VALUES (1012, N'Máy tính Casio', 0, 0)
SET IDENTITY_INSERT [dbo].[Category] OFF

SET IDENTITY_INSERT [dbo].[Product] ON 
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1, N'Truyện Doraemon tập 1: Thăm công viên Khủng Long ', CAST(16000.00 AS Decimal(18, 2)), CAST(N'2021-07-12T14:49:25.0000000' AS DateTime2), NULL, 1, 0, 0, CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (2, N'Bộ dụng cụ 3 thước kẻ', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-12T16:16:12.0000000' AS DateTime2), NULL, 2, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1002, N'Bút chì 2B', CAST(5000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T13:21:33.1740958' AS DateTime2), NULL, 1002, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1003, N'Truyện Doraemon tập 2: Bí mật hành tinh màu tím', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:41:35.0000000' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1004, N'Truyện Doraemon tập 3: Pho tượng thần khổng lồ', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:48:46.2306431' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1005, N'Truyện Doraemon tập 4: Lâu đài dưới đáy biển', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:49:26.7552462' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1006, N'Truyện Doraemon tập 5: Nobita lạc vào xứ quỷ', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:49:52.5539004' AS DateTime2), NULL, 1, 0, 0, CAST(11900.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1007, N'Truyện Doraemon tập 6: Tên độc tài vũ trụ', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:51:54.5157253' AS DateTime2), NULL, 1, 0, 0, CAST(11900.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1008, N'Truyện Doraemon tập 7: Cuộc xâm lăng của binh đoàn rô bốt', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:52:16.3519190' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1009, N'Truyện Doraemon tập 8: Cuộc phưu lưu vào lòng đất', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:53:51.1687313' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1010, N'Truyện Doraemon tập 9: Chiến thắng quỷ Kamat', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:54:13.0534519' AS DateTime2), NULL, 1, 0, 0, CAST(11900.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1011, N'Truyện Doraemon tập 10: Ngôi sao cảm', CAST(14000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:54:30.8447913' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1012, N'Truyện Doraemon tập 11: Nobita đến xứ Ba Tư', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T14:59:58.9383568' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1013, N'Truyện Doraemon tập 12: Vương Quốc Trên Mây', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T15:00:18.7442080' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1014, N'Truyện Doraemon tập 13: Bí mật mê cung Bliki', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T15:00:40.2544473' AS DateTime2), NULL, 1, 0, 0, CAST(12750.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1015, N'Truyện Doraemon tập 14: Ba chàng hiệp sĩ mộng mơ', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T15:01:01.3437141' AS DateTime2), NULL, 1, 0, 0, CAST(12750.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1016, N'Truyện Doraemon tập 15: Lạc vào thế giới côn trùng', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T15:01:23.7737468' AS DateTime2), NULL, 1, 0, 0, CAST(12750.00 AS Decimal(18, 2)))
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [DateCreate], [Description], [CategoryId], [Total], [IsDeleted], [SalePrice]) VALUES (1017, N'Truyện Doraemon tập 16: Hành trình qua giải ngân hà', CAST(15000.00 AS Decimal(18, 2)), CAST(N'2021-07-13T15:01:45.8924197' AS DateTime2), NULL, 1, 0, 0, CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Product] OFF

SET IDENTITY_INSERT [dbo].[ProductDetails] ON 
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1, 1, N'Việt Nam', 0.5, N'Đỏ', 40, 20, 20, 0, CAST(16500.00 AS Decimal(18, 2)), CAST(14025.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (2, 1, N'Việt Nam', 0.8, N'Xanh', 30, 0, 30, 1, CAST(17000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (3, 1, N'Việt Nam', 0.5, N'Xanh', 30, 10, 20, 0, CAST(17000.00 AS Decimal(18, 2)), CAST(14450.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (4, 2, N'Việt Nam', 0.02, N'Trắng trong', 25, 5, 20, 0, CAST(15000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1002, 1002, N'Việt Nam', 0.002, N'Vàng', 30, 0, 30, 0, CAST(5500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1003, 1002, N'Việt Nam', 0.002, N'Xanh', 30, 3, 27, 0, CAST(5500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1004, 1003, N'Việt Nam', 0.05, N'trắng', 30, 5, 25, 0, CAST(15000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1005, 1004, N'Việt Nam', 0.5, N'vàng', 30, 3, 27, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1006, 1005, N'Việt Nam', 0.5, N'vàng', 30, 5, 25, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1007, 1006, N'Việt Nam', 0.5, N'vàng', 30, 2, 28, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1008, 1007, N'Việt Nam', 0.5, N'vàng', 30, 0, 30, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1009, 1008, N'Việt Nam', 0.5, N'vàng', 30, 0, 30, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1010, 1009, N'Việt Nam', 0.5, N'vàng', 30, 5, 25, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1011, 1010, N'Việt Nam', 0.5, N'vàng', 30, 0, 30, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1012, 1011, N'Việt Nam', 0.5, N'vàng', 30, 4, 26, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1013, 1012, N'Việt Nam', 0.5, N'vàng', 30, 3, 27, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1014, 1013, N'Việt Nam', 0.5, N'vàng', 0, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1015, 1014, N'Việt Nam', 0.5, N'vàng', 0, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1016, 1015, N'Việt Nam', 0.5, N'vàng', 30, 6, 24, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1017, 1016, N'Việt Nam', 0.5, N'vàng', 10, 0, 10, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1018, 1017, N'Việt Nam', 0.5, N'vàng', 0, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1019, 2, N'Việt Nam', 0.02, N'Xanh', 0, 0, 0, 0, CAST(14500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1020, 1015, N'Nhật Bản', 0.5, N'xanh', 0, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(13600.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1025, 1009, N'Việt Nam', 0.5, N'vàng', 30, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[ProductDetails] ([ProductDetailId], [ProductId], [Origin], [Weight], [Color], [Quantity], [QuantityOrdered], [RemainingQuantity], [IsDeleted], [Price], [SalePrice]) VALUES (1026, 1015, N'Việt Nam', 0.5, N'vàng', 30, 0, 0, 0, CAST(16000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[ProductDetails] OFF
SET IDENTITY_INSERT [dbo].[ProductImages] ON 
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1, 1, N'AAm5SOI42fQofsTPHf6bnbDAtdPj6MRAI1UkiM9frHtujuCFIfwyeAExsB4ASFVQ212442948_1726524477558494_779842319507388649_n.jpg', 0, 1)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (2, 2, N'AHUEsILnZzOUNbsKsI4syXuvVeZJIXmBHenaMqDBZBjQuYSSo0ahqXx4f7ipoaAthc k.png', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1002, 1002, N'AFKcNxUyFO12C0PMnFcpGEcto27tqmsm5ykEOrZMs1zyaYSQSeHUguFl9W6EnrzLogGim 10.png', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1003, 1003, N'AEZFobCpRUNgp1y3bl9FyeSBE9ikhAp1X9OPdGB5TaJvWO2ttTXKhUtN77wlTm9gGim 10.png', 0, 1)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1004, 1004, N'AMBueQklQcbFr0ltVa39r37XGHlZcSDuJUu7wGGc7LuisllWbOcyje5DinxO9IjAdoraemon tap 3.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1005, 1005, N'APOnNvZo62NeujuCiX7Jf9tZ71cwyDJDkKESCmgGca81of3uCtuOSdS5fAhXpEn3gdoraemon tap 4.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1006, 1006, N'ABC8iF3xjDKfhCgrm2wbg8PMgEScysQpjsvrMSKD6MrWxkIKck5jOK4VDcIZb96Adoraemon tap 5.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1007, 1007, N'AFqbVzeDKzqOuSM0B5CfkfZxOuQPgtU4fhPkFp2ZLL4rOl3TFd9XuyYQQrQdMRgdoraemon tap 6.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1008, 1008, N'APrY8BRo7sYKAQTwm8NLT3H0O3hZE8zentOtIJHCNsH4TsGzSO2QJfu4Iw9i3E3VQdoraemon tap 7.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1009, 1009, N'AOpbzip6yKN4v5OgIxJjLdtLAnsz1x1GBydCXaYJPID4L35oRMoVBYpgjeNvpvS6Hwdoraemon tap 8.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1010, 1010, N'ABiLUOlE02vPP1tDHBM2OvG9ZIyU2PX9aQc7PIxeKG8lQRypAxcXS7Nzv9RzwCdgdoraemon tap 9.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1011, 1011, N'AP6ADSp5N6acriU3b0GdM1K2ll68e5nyVhr3OYfW7bEXl7DHZ3uHdPiCZIDeqGIQdoraemon tap 10.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1012, 1012, N'ADyyQINRaA7DX5bq6dqJ3VN4xAnXZw1E3U7OGbL7iZdN5yDne2dLKDrj3YJ0ga2lQdoraemon tap 11.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1013, 1013, N'AKG5LxpA0cyuwi3jDicqbZ1wdA5odiGUYxKu9l7uLPZ5EIeKX9f7PcG3e9XZSAdoraemon tap 12.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1014, 1014, N'ACzkV10ATZMutrmTI23mI0DiYk731ugngvskqZUZZrIoVInrKavf4srPgVReVDqwdoraemon tap 13.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1015, 1015, N'AILpRuoO3LO7HiaxCdhnMYmVnuRXkdFWYHcdjCtSgm7HZchn1WBc38qcbTXkxmwdoraemon tap 14.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1016, 1016, N'AJZ0JYjgtIkAAupTmpr1CYBzKxx2jSBQqhLNvyHIUW29UZ1S9HwUOAuG63Vnf79qwdoraemon tap 15.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1017, 1017, N'AGnOtNfGXNJNb7WnQLlESo6UQRL3cUCKNkIPX8UckcLI5y2tbwxJsQXtafNLTgAdoraemon tap 16.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1018, 1003, N'AAVUGemwp6zHBuClc4ZyuU9kKrkxMkD6FvOHvMGjyL2o8Gr8Uif5YCrafA7rlUdwdoraemon tap 2.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1019, 1, N'APtgot6ZhClSmlbWGKmHFDjjWecmTXxxzb56xPa7gHmhyL5y7uteTlpwiSwdoraemon tap 1.jpg', 0, 0)
INSERT [dbo].[ProductImages] ([ProductImageId], [ProductId], [Image], [PrimaryImage], [IsDeleted]) VALUES (1020, 2, N'AC0WUnX9QspGmuhVQIQfwfMJ7qcz3bSJsDJ9uKMdlGtjUncKWU2o8Nh7hj1MJIejgGim 15 1.png', 0, 0)
SET IDENTITY_INSERT [dbo].[ProductImages] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
