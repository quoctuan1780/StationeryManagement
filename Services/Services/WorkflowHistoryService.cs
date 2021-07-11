using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WorkflowHistoryService : IWorkflowHistoryService
    {
        private readonly ShopDbContext _context;

        public WorkflowHistoryService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<WorkflowHistory> AddWorkflowHistoryAsync(WorkflowHistory workflowHistory)
        {
            await _context.WorkflowHistories.AddAsync(workflowHistory);

            await _context.SaveChangesAsync();

            return workflowHistory;
        }

        public async Task<IList<WorkflowHistory>> GetWorkflowHistoriesAsync(string userId)
        {
            return await _context.WorkflowHistories.Where(x => x.CreatedBy == userId).ToListAsync();
        }
    }
}
