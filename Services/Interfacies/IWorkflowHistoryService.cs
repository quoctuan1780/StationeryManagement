using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IWorkflowHistoryService
    {
        Task<WorkflowHistory> AddWorkflowHistoryAsync(WorkflowHistory workflowHistory);
        Task<IList<WorkflowHistory>> GetWorkflowHistoriesAsync(string userId);
    }
}
