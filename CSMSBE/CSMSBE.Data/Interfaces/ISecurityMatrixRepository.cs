using CSMS.Data.Repository;
using CSMS.Entity.SecurityMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface ISecurityMatrixRepository : IBaseRepository<SecurityMatrices>
    {
        Task<bool> CheckPermission(string roleName, string actionName, string screenName);

    }
}
