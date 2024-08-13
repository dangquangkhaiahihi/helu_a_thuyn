using CSMS.Entity.Issues;
using CSMS.Model.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface ICommentRepository
    {
        Comment Create(CreateCommentDTO dto, string userId);
        Comment Update(UpdateCommentDTO dto, string userId);
        bool Remove(int id);
        Comment GetById(int id);
        Task<List<Comment>> GetIssueComments(int issueId);
    }
}
