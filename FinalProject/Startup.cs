using Common;
using Entities.Data;
using Entities.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Hubs;
using Services.Interfacies;
using Services.Services;
using System;
using static Common.RoleConstant;

namespace FinalProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static IProductService productService;
        private static IAprioriBackground aprioriBackground;

        private readonly BackgroundWork backgroundWork = new(productService, aprioriBackground);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(Constant.CONNECTION_STRING),
                sqlServerOptions => sqlServerOptions.CommandTimeout(300)));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ShopDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            #region Register Dependencies
            services.AddSingleton(Configuration);
            services.AddTransient<IHubService, HubService>();
            services.AddTransient<IHubShipperService, HubShipperService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IProductDetailService, ProductDetailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPayPalService, PayPalService>();
            services.AddScoped<IPayPalService, PayPalService>();
            services.AddScoped<IMoMoService, MoMoService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<ISearchItemService, SearchItemService>();
            services.AddScoped<IRecommendationService, RecommandationService>();
            services.AddScoped<IDeliveryAddressService, DeliveryAddressService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IWorkflowHistoryService, WorkflowHistoryService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IFileGuideService, FileGuideService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ISaleDetailService, SaleDetailService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IBillDetailService, BillDetailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAprioriBackground, AprioriBackground>();
            #endregion

            #region Cookie manually
            services.AddAuthentication(defaultScheme: ROLE_CUSTOMER)
            .AddCookie(ROLE_CUSTOMER, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Google:ClientId"];
                options.ClientSecret = Configuration["Google:ClientSecret"];
                options.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddFacebook(options =>
            {
                options.AppId = Configuration["Facebook:AppId"];
                options.AppSecret = Configuration["Facebook:AppSecret"];
                options.CallbackPath = "/signin-facebook";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddAuthentication(defaultScheme: ROLE_ADMIN)
            .AddCookie(ROLE_ADMIN, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/Admin/Account/Login");
                options.LogoutPath = "/Admin/Account/Logout";
                options.AccessDeniedPath = "/Admin/Account/AccessDenied";
            });

            services.AddAuthentication(defaultScheme: ROLE_SHIPPER)
            .AddCookie(ROLE_SHIPPER, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/Shipper/Account/Login");
                options.LogoutPath = "/Shipper/Account/Logout";
                options.AccessDeniedPath = "/Shipper/Account/AccessDenied";
            });

            services.AddAuthentication(defaultScheme: ROLE_WAREHOUSE_MANAGER)
            .AddCookie(ROLE_WAREHOUSE_MANAGER, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/Warehouse/Account/Login");
                options.LogoutPath = "/Warehouse/Account/Logout";
                options.AccessDeniedPath = "/Warehouse/Account/AccessDenied";
            });
            #endregion

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString(Constant.CONNECTION_STRING), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            services.AddMemoryCache();
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            #region Hangfire configure

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            recurringJobManager.AddOrUpdate("UpdateSalePriceOfProduct", () => backgroundWork.DoTask(), Cron.Daily());

            recurringJobManager.AddOrUpdate("UpdateRecommendation", () => backgroundWork.SaveRecommendation(), Cron.Daily());

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalServer>("/signalServer");

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
