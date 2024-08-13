using CSMS.Data.Repository;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.Issues;
using CSMS.Entity.SecurityMatrix;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IIssueRepository : IBaseRepository<Issue>
    {
        Issue Get(int id);
        Issue Create(CreateIssueDTO dto, string userId);
        Issue Update(UpdateIssueDTO dto, string userId);
        bool Remove(int id);
    }

}
