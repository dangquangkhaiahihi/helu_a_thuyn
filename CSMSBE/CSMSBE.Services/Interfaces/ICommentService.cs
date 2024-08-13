using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface ICommentService
    {
        CommentDTO CreateComment(CreateCommentDTO dto, string userId);
        CommentDTO UpdateComment(UpdateCommentDTO dto, string userId);
        bool RemoveComment(int id);
        CommentDTO GetCommentById(int id);
        Task<List<CommentDTO>> GetIssueComments(int issueId);
    }
}
