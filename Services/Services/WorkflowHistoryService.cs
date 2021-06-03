using Entities.Data;
using Entities.Models;
using Services.Interfacies;
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
    }
}
