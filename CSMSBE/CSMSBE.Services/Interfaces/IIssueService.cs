using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMS.Model.Issue;
using CSMS.Model.User;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface IIssueService
    {
        Task<IPagedList<IssueDTO>> FilterIssue(IssueFilterRequest filter);
        IssueDTO GetIssueById(int id);
        IssueDTO CreateIssue(CreateIssueDTO dto, string userId);
        IssueDTO UpdateIssue(UpdateIssueDTO dto, string userId);
        bool RemoveIssue(int id);
    }
}
