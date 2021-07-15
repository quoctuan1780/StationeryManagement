using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitRating_Recommendation_Sale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [dbo].[Recommendation] ON 
INSERT [dbo].[Recommendation] ([RecommendtionId], [CreateDate]) VALUES (1, CAST(N'2021-07-14T23:52:47.0746244' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Recommendation] OFF

SET IDENTITY_INSERT [dbo].[RecommendationDetail] ON 
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 1, N'1006 ', N'4 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 2, N'1 1016 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 3, N'1016 3 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 4, N'1 1010 ', N'1016 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 5, N'1 1016 ', N'1010 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 6, N'1010 1016 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 7, N'1010 1016 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 8, N'1010 3 ', N'1016 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 9, N'1016 3 ', N'1010 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 10, N'1 1010 ', N'1016 3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 11, N'1 1016 ', N'1010 3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 12, N'1010 1016 ', N'1 3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 13, N'1010 3 ', N'1 1016 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 14, N'1016 3 ', N'1 1010 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 15, N'1 1010 1016 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 16, N'1 1010 3 ', N'1016 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 17, N'1010 3 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 18, N'1 1016 3 ', N'1010 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 19, N'1 1010 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 20, N'1 1005 ', N'1003 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 21, N'1007 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 22, N'1007 ', N'1004 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 23, N'1003 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 24, N'1005 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 25, N'1003 ', N'1005 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 26, N'1005 ', N'1003 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 27, N'1013 ', N'1012 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 28, N'3 4 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 29, N'1007 ', N'1004 3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 30, N'1004 1007 ', N'3 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 31, N'1004 3 ', N'1007 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 32, N'1007 3 ', N'1004 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 33, N'1003 ', N'1 1005 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 34, N'1005 ', N'1 1003 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 35, N'1 1003 ', N'1005 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 36, N'1003 1005 ', N'1 ')
INSERT [dbo].[RecommendationDetail] ([RecommendationId], [ProductId], [ProductDetailId], [RecommandationDetailId], [Input], [Output]) VALUES (1, NULL, NULL, 37, N'1010 1016 3 ', N'1 ')
SET IDENTITY_INSERT [dbo].[RecommendationDetail] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
