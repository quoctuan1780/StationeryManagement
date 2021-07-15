using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Init notification
            migrationBuilder.Sql(@"
                                    GO
                                    ALTER TABLE [dbo].[Users]
                                    DROP COLUMN [Address]
                                    GO
                                    INSERT INTO [dbo].[NotificationType] VALUES
                                    (N'Thông báo đặt hàng'),
                                    (N'Thông báo khuyến mại'),
                                    (N'Thông báo nhập kho'),
                                    (N'Thông báo xuất kho'),
                                    (N'Thông báo xóa'),
                                    (N'Thông báo yêu cầu nhập kho'),
                                    (N'Thông báo từ chối đơn nhập kho'),
                                    (N'Thông báo hủy đơn hàng'),
                                    (N'Thông báo giao hàng');
                                    
                                    GO
                                    INSERT INTO [dbo].[Rating] VALUES
                                    (N'Một sao', 1),
                                    (N'Hai sao', 2),
                                    (N'Ba sao', 3),
                                    (N'Bốn sao', 4),
                                    (N'Năm sao', 5);"
                        );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
