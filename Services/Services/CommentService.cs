using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ShopDbContext _context;

        public CommentService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            await _context.AddAsync(comment);

            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<IList<Comment>> GetAllCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments
                        .Include(x => x.User)
                        .Where(x => x.ProductId == productId)
                        .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }
    }
}
