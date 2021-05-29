using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ICommentService
    {
        Task<Comment> AddCommentAsync(Comment comment);

        Task<IList<Comment>> GetAllCommentsByProductIdAsync(int productId);

        Task<Comment> GetCommentByIdAsync(int commentId);

        Task<int> DeleteCommentByIdAsync(int commentId);

        Task<Comment> UpdateCommentAsync(int commentId, string content);
    }
}
