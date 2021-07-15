using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitProvince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
                        SET IDENTITY_INSERT [dbo].[Provinces] ON 
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (201, N'Hà Nội', 4)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (202, N'Hồ Chí Minh', 8)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (203, N'Đà Nẵng', 511)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (204, N'Đồng Nai', 61)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (205, N'Bình Dương', 65)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (206, N'Bà Rịa - Vũng Tàu', 64)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (207, N'Gia Lai', 59)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (208, N'Khánh Hòa', 58)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (209, N'Lâm Đồng', 63)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (210, N'Đắk Lắk', 500)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (211, N'Long An', 72)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (212, N'Tiền Giang', 73)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (213, N'Bến Tre', 75)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (214, N'Trà Vinh', 74)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (215, N'Vĩnh Long', 70)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (216, N'Đồng Tháp', 67)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (217, N'An Giang', 76)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (218, N'Sóc Trăng', 79)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (219, N'Kiên Giang', 77)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (220, N'Cần Thơ', 710)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (221, N'Vĩnh Phúc', 211)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (223, N'Thừa Thiên - Huế', 54)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (224, N'Hải Phòng', 31)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (225, N'Hải Dương', 320)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (226, N'Thái Bình', 36)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (227, N'Hà Giang', 219)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (228, N'Tuyên Quang', 27)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (229, N'Phú Thọ', 210)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (230, N'Quảng Ninh', 33)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (231, N'Nam Định', 350)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (232, N'Hà Nam', 351)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (233, N'Ninh Bình', 30)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (234, N'Thanh Hóa', 37)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (235, N'Nghệ An', 38)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (236, N'Hà Tĩnh', 39)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (237, N'Quảng Bình', 52)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (238, N'Quảng Trị', 53)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (239, N'Bình Phước', 651)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (240, N'Tây Ninh', 66)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (241, N'Đắk Nông', 501)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (242, N'Quảng Ngãi', 55)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (243, N'Quảng Nam', 510)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (244, N'Thái Nguyên', 280)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (245, N'Bắc Kạn', 281)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (246, N'Cao Bằng', 26)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (247, N'Lạng Sơn', 25)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (248, N'Bắc Giang', 240)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (249, N'Bắc Ninh', 241)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (250, N'Hậu Giang', 711)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (252, N'Cà Mau', 780)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (253, N'Bạc Liêu', 781)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (258, N'Bình Thuận', 62)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (259, N'Kon Tum', 60)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (260, N'Phú Yên', 57)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (261, N'Ninh Thuận', 68)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (262, N'Bình Định', 56)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (263, N'Yên Bái', 29)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (264, N'Lai Châu', 231)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (265, N'Điện Biên', 230)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (266, N'Sơn La', 22)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (267, N'Hòa Bình', 218)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (268, N'Hưng Yên', 321)
                        INSERT [dbo].[Provinces] ([ProvinceId], [ProvinceName], [Code]) VALUES (269, N'Lào Cai', 20)
                        SET IDENTITY_INSERT [dbo].[Provinces] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
