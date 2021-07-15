using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (1, CAST(N'2021-07-13T23:38:26.9405089' AS DateTime2), CAST(107000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (2, CAST(N'2021-07-14T00:01:07.4965062' AS DateTime2), CAST(114000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (3, CAST(N'2021-07-14T00:01:19.5351742' AS DateTime2), CAST(67000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (4, CAST(N'2021-07-14T00:01:24.2245861' AS DateTime2), CAST(85000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'MoMo')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (5, CAST(N'2021-07-14T10:37:57.6639994' AS DateTime2), CAST(111000.00 AS Decimal(18, 2)), N'6353aab3-667c-4009-8e97-136c16fe3322', N'MoMo')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (6, CAST(N'2021-07-14T10:47:42.9353558' AS DateTime2), CAST(114000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'PayPal')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (7, CAST(N'2021-07-14T11:06:39.7332019' AS DateTime2), CAST(213000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (8, CAST(N'2021-07-14T21:07:40.6592250' AS DateTime2), CAST(63000.00 AS Decimal(18, 2)), N'005ae501-eb7c-4426-9db5-9e0a6fb50992', N'PayPal')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (9, CAST(N'2021-07-14T21:18:04.3495129' AS DateTime2), CAST(91200.00 AS Decimal(18, 2)), N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (10, CAST(N'2021-07-14T21:19:46.5871951' AS DateTime2), CAST(62000.00 AS Decimal(18, 2)), N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (11, CAST(N'2021-07-14T21:23:28.4462963' AS DateTime2), CAST(48000.00 AS Decimal(18, 2)), N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD')
INSERT [dbo].[Bill] ([BillId], [CreateDate], [Total], [UserId], [PaymentMethod]) VALUES (12, CAST(N'2021-07-14T21:25:24.3480183' AS DateTime2), CAST(27900.00 AS Decimal(18, 2)), N'64796e78-c0c2-45d9-af1b-e64aae884143', N'COD')
SET IDENTITY_INSERT [dbo].[Bill] OFF
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1, 4, 2, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (1, 1006, 5, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (2, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (2, 3, 3, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (2, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (3, 1, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (3, 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (4, 1, 5, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (5, 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (5, 1004, 3, CAST(0.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (5, 1007, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (6, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (6, 1003, 3, CAST(0.00 AS Decimal(18, 2)), CAST(5500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (6, 1005, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (7, 1, 4, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (7, 3, 3, CAST(0.00 AS Decimal(18, 2)), CAST(17000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (7, 1010, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (7, 1016, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (8, 1, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (8, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (9, 1012, 3, CAST(0.00 AS Decimal(18, 2)), CAST(14400.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (9, 1013, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (10, 1004, 2, CAST(0.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (10, 1010, 2, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (11, 1016, 3, CAST(0.00 AS Decimal(18, 2)), CAST(16000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (12, 4, 1, CAST(0.00 AS Decimal(18, 2)), CAST(13500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[BillDetail] ([BillId], [ProductDetailId], [Quantity], [SalePrice], [Price], [Status]) VALUES (12, 1012, 1, CAST(0.00 AS Decimal(18, 2)), CAST(14400.00 AS Decimal(18, 2)), NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
