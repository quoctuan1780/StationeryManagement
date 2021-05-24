using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> DeleteCommentByIdAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (!(comment is null))
            {
                comment.IsDeleted = true;

                _context.Update(comment);

                return await _context.SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<IList<Comment>> GetAllCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments
                        .Include(x => x.User)
                        .Where(x => x.ProductId == productId && x.IsDeleted == false)
                        .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<Comment> UpdateCommentAsync(int commentId, string content)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if(!(comment is null))
            {
                comment.Content = content;

                _context.Update(comment);

                await _context.SaveChangesAsync();

                return comment;
            }
            else
            {
                return null;
            }
        }
    }
}
