using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[Sale] ON 
INSERT [dbo].[Sale] ([SaleId], [SaleName], [Discount], [IsDeleted], [Description], [Image], [SaleType], [SaleEndDate], [SaleStartDate], [StatusSale], [FromOrderPrice]) VALUES (1, N'Giảm 15%', CAST(15.00 AS Decimal(18, 2)), 1, N'Giảm giá 15%', N'AGQzfGznaFIKDepVi6TVpO37gs1tvk01733qEI5CG6H0ToQnupK9a4aEp6W4Zy2wGim 15.png', N'Khuyến mãi cho sản phẩm', CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Sale] ([SaleId], [SaleName], [Discount], [IsDeleted], [Description], [Image], [SaleType], [SaleEndDate], [SaleStartDate], [StatusSale], [FromOrderPrice]) VALUES (2, N'Giảm 10%', CAST(10.00 AS Decimal(18, 2)), 0, N'Giảm giá 10%', N'AEx6KgDZqlrNr02GzzuRoJ9JfT0PdnG8uEQLvfSvFLuR1IAXwaCWaTVymo3OgHQQGim 10.png', N'Khuyến mãi cho sản phẩm', CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Sale] ([SaleId], [SaleName], [Discount], [IsDeleted], [Description], [Image], [SaleType], [SaleEndDate], [SaleStartDate], [StatusSale], [FromOrderPrice]) VALUES (1002, N'Giảm 15%', CAST(15.00 AS Decimal(18, 2)), 1, N'Giảm 15%', N'ABkqLsQncUWCl0ahJVMRYqVG8zRL7ioeEntS8RCHf8f6mSULBGB3dKmuNDHXEBQGim 15.png', N'Khuyến mãi cho sản phẩm', CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Sale] ([SaleId], [SaleName], [Discount], [IsDeleted], [Description], [Image], [SaleType], [SaleEndDate], [SaleStartDate], [StatusSale], [FromOrderPrice]) VALUES (1003, N'Giảm 15%', CAST(15.00 AS Decimal(18, 2)), 0, N'Khuyến mại 15% cho các mặt hàng truyện tranh', N'ANoqmAiRzpK45BfLXOzNrMcW6ohrS6vyCGwDk0wlADZG2JfmAUWdYf6fM0ntauwGim 15.png', N'Khuyến mãi cho sản phẩm', CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Sale] ([SaleId], [SaleName], [Discount], [IsDeleted], [Description], [Image], [SaleType], [SaleEndDate], [SaleStartDate], [StatusSale], [FromOrderPrice]) VALUES (1004, N'Giảm 15%', CAST(15.00 AS Decimal(18, 2)), 0, N'Giảm 15% cho các sản phẩm bút', N'ACZ93oOhx07HAuLAxmq8zgf3k9QWOJqzTYkOzzAKzMA9k5Dv5wqp2Urqpuq6AGim 15 1.png', N'Khuyến mãi cho sản phẩm', CAST(N'2021-07-16T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Sale] OFF
SET IDENTITY_INSERT [dbo].[SaleProduct] ON 

INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1, 1, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), 1, 1)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (2, 2, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), 1, 2)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (2, 2, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), 1, 3)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1012, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), 1, 15)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1015, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), 1, 16)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1007, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), 1, 17)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1012, CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), 1, 18)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1015, CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), 1, 19)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1002, 1007, CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-13T00:00:00.0000000' AS DateTime2), 1, 20)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (2, 2, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), 1, 23)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (2, 1011, CAST(N'2021-07-12T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), 1, 24)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1003, 1014, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), 0, 25)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1003, 1015, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), 0, 26)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1003, 1016, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), 0, 27)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1003, 1006, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), 0, 28)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1003, 1007, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-19T00:00:00.0000000' AS DateTime2), 0, 29)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1004, 1, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-16T00:00:00.0000000' AS DateTime2), 0, 30)
INSERT [dbo].[SaleProduct] ([SaleId], [ProductId], [SaleStartDate], [SaleEndDate], [IsDeleted], [SaleDetailId]) VALUES (1004, 1010, CAST(N'2021-07-14T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-16T00:00:00.0000000' AS DateTime2), 0, 31)
SET IDENTITY_INSERT [dbo].[SaleProduct] OFF
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
