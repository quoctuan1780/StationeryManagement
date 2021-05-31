using Entities.Models;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IWorkflowHistoryService
    {
        Task<WorkflowHistory> AddWorkflowHistoryAsync(WorkflowHistory workflowHistory);
    }
}
