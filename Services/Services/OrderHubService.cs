using Entities.Data;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Interfacies;
using System.Collections.Generic;

namespace Services.Services
{
    public class OrderHubService : IOrderHubService
    {
        private readonly IConfiguration _configuration;
        private readonly ShopDbContext _context;

        public OrderHubService(IConfiguration configuration, ShopDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public List<Order> GetOrders()
        {
            SqlDependency.Start(_context.Database.GetConnectionString());

            return null;
        }
    }
}
