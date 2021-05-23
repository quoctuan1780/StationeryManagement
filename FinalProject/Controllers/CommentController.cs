using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Controllers
{
    [Authorize(Roles = ROLE_CUSTOMER)]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<string> AddParentComment(string userId, string content, int? productId)
        {
            var json = new JObject();

            if (userId is null && content is null && productId is null)
            {
                json.Add("Status", ERROR_CODE_NULL.ToString());

                return json.ToString();
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var comment = new Comment()
                {
                    Content = content,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    UserId = userId,
                    ProductId = productId.Value
                };

                var result = await _commentService.AddCommentAsync(comment);

                if (!(result is null))
                {
                    json.Add("Status", CODE_SUCCESS);
                    json.Add("CommentId", comment.CommentId);
                    
                    transaction.Complete();

                    return json.ToString();
                }
            }
            catch
            {
            }

            json.Add("Status", ERROR_CODE_SYSTEM.ToString());

            return json.ToString();
        }

        [HttpPost]
        public async Task<string> AddChildrentComment(int? commentId, string userId, string userName, string content, int? productId)
        {
            if (commentId is null || userId is null || userName is null || content is null || productId is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            try
            {
                var replyConment = new Comment()
                {
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Content = content,
                    ReplyCommentId = commentId.Value,
                    ProductId = productId.Value,
                    TagName = userName,
                    UserId = userId
                };

                var result = await _commentService.AddCommentAsync(replyConment);

                if (!(result is null))
                {
                    return CODE_SUCCESS.ToString();
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM.ToString();
        }

        [HttpPost]
        public async Task<string> AddChildrentReplyComment(int? commentId, string userId, string userName, string content, int? productId)
        {
            if (commentId is null || userId is null || userName is null || content is null || productId is null)
            {
                return ERROR_CODE_NULL.ToString();
            }
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(commentId.Value);

                if (!(comment is null))
                {
                    int commentIdReply = comment.CommentId;
                    if(!(comment.ReplyCommentId is null))
                    {
                        commentIdReply = comment.ReplyCommentId.Value;
                    }

                    var replyConment = new Comment()
                    {
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        Content = content,
                        ReplyCommentId = commentIdReply,
                        ProductId = productId.Value,
                        TagName = userName,
                        UserId = userId
                    };

                    var result = await _commentService.AddCommentAsync(replyConment);

                    if (!(result is null))
                    {
                        return CODE_SUCCESS.ToString();
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
    }
}
