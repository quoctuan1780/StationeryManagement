using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using Ganss.Excel;
using System.Globalization;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class StatisticalController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public StatisticalController(IOrderService orderService, IAccountService accountService, IAddressService addressService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _addressService = addressService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Revenue by year
        public IActionResult RevenueByYear()
        {
            return View();
        }

        public async Task<string> GetRevenueByYear(int? startDate, int? endDate)
        {
            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            var result = await _orderService.GetRevenueByYearAsync(startDate.Value, endDate.Value);

            if (result is null || result.Equals("[]"))
            {
                return "NULL";
            }

            return result;
        }

        public async Task<string> ExportExcel(int? startDate, int? endDate)
        {

            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            var result = await _orderService.GetRevenueExportExcelByYearAsync(startDate.Value, endDate.Value);

            if (result != null)
            {
                var excelMapper = new ExcelMapper();

                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                var datetime = DateTime.Now.Date.ToShortDateString().Replace(SLASH, EMPTY) + DateTime.Now.Ticks.ToString();

                await excelMapper.SaveAsync($"{path}\\{datetime}_Doanh thu theo năm.xlsx", result, "Doanh thu theo năm", xlsx: true);

                return CODE_SUCCESS.ToString();
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
        #endregion

        #region Revenue by month
        public IActionResult RevenueByMonth()
        {
            return View();
        }

        public async Task<string> GetRevenueByMonth(string startDate, string endDate)
        {
            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueByMonthAsync(startDateConvert, endDateConvert);

            if (result is null || result.Equals("[]"))
            {
                return "NULL";
            }

            return result;
        }

        public async Task<string> ExportExcelByMonth(string startDate, string endDate)
        {

            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueExportExcelByMonthAsync(startDateConvert, endDateConvert);

            if (result != null)
            {
                var excelMapper = new ExcelMapper();

                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                var datetime = DateTime.Now.Date.ToShortDateString().Replace(SLASH, EMPTY) + DateTime.Now.Ticks.ToString();

                await excelMapper.SaveAsync($"{path}\\{datetime}_Doanh thu theo tháng.xlsx", result, "Doanh thu theo tháng", xlsx: true);

                return CODE_SUCCESS.ToString();
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
        #endregion

        #region Revenue by customer
        public async Task<IActionResult> RevenueByCustomer()
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();

            return View();
        }

        public async Task<string> GetRevenueByCustomer(string startDate, string endDate, string customer)
        {
            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueByCustomerAsync(startDateConvert, endDateConvert, customer);

            if (result is null || result.Equals("[]"))
            {
                return "NULL";
            }

            return result;
        }

        public async Task<string> ExportExcelByCustomer(string startDate, string endDate, string customer)
        {

            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueExportExcelByCustomerAsync(startDateConvert, endDateConvert, customer);

            if (result != null)
            {
                var excelMapper = new ExcelMapper();

                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                var datetime = DateTime.Now.Date.ToShortDateString().Replace(SLASH, EMPTY) + DateTime.Now.Ticks.ToString();

                await excelMapper.SaveAsync($"{path}\\{datetime}_Doanh thu theo khách hàng.xlsx", result, "Doanh thu theo khách hàng", xlsx: true);

                return CODE_SUCCESS.ToString();
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
        #endregion

        #region Revenue by product
        public IActionResult RevenueByProduct()
        {
            return View();
        }

        public async Task<string> GetRevenueByProduct(string startDate, string endDate)
        {
            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueByProductAsync(startDateConvert.ToString("yyyy-MM-dd"), endDateConvert.ToString("yyyy-MM-dd"));

            if (result is null || result.Equals("[]"))
            {
                return "NULL";
            }

            return result;
        }

        public async Task<string> ExportExcelByProduct(string startDate, string endDate)
        {

            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueExportExcelByProductAsync(startDateConvert.ToString("yyyy-MM-dd"), endDateConvert.ToString("yyyy-MM-dd"));

            if (result != null)
            {
                var excelMapper = new ExcelMapper();

                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                var datetime = DateTime.Now.Date.ToShortDateString().Replace(SLASH, EMPTY) + DateTime.Now.Ticks.ToString();

                await excelMapper.SaveAsync($"{path}\\{datetime}_Doanh thu theo sản phẩm.xlsx", result, "Doanh thu theo sản phẩm", xlsx: true);

                return CODE_SUCCESS.ToString();
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
        #endregion

        public async Task<IActionResult> RevenueByProvince()
        {
            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            return View();
        }

        public async Task<string> GetRevenueByProvince(string startDate, string endDate, string province)
        {
            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueByProvinceAsync(startDateConvert, endDateConvert, province);

            if (result is null || result.Equals("[]"))
            {
                return "NULL";
            }

            return result;
        }

        public async Task<string> ExportExcelByProvince(string startDate, string endDate, string province)
        {

            if (startDate is null || endDate is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            DateTime.TryParse(startDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime startDateConvert);
            DateTime.TryParse(endDate, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime endDateConvert);

            var result = await _orderService.GetRevenueExportExcelByProvinceAsync(startDateConvert, endDateConvert, province);

            if (result != null)
            {
                var excelMapper = new ExcelMapper();

                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                var datetime = DateTime.Now.Date.ToShortDateString().Replace(SLASH, EMPTY) + DateTime.Now.Ticks.ToString();

                await excelMapper.SaveAsync($"{path}\\{datetime}_Doanh thu theo tỉnh thành.xlsx", result, "Doanh thu theo tỉnh thành", xlsx: true);

                return CODE_SUCCESS.ToString();
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
    }
}
