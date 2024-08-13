using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.SecurityMatrix;
using CSMSBE.Core.Helper;
using CSMS.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class SecurityMatrixRepository : BaseRepository<SecurityMatrices>, ISecurityMatrixRepository
    {
        public SecurityMatrixRepository(csms_dbContext context) : base(context)
        {
        }

        public async Task<bool> CheckPermission(string roleName, string actionName, string screenName)
        {
            var role = _dbContext.Role.Where(r => r.Code == roleName).FirstOrDefault();
            var screen = _dbContext.Screens.Where(s => s.Code.ToUpper() == screenName).FirstOrDefault();
            var action = _dbContext.Actions.Where(a => a.Code.ToUpper() == actionName).FirstOrDefault();

            if (role == null || screen == null  || action == null) { return false; }

            var data = _dbContext.SecurityMatrix.
                Where(sm => sm.RoleId==role.Id 
                && sm.ScreenId == screen.Id 
                && sm.ActionId == action.Id);
            if (!data.Any())
            {
                return false;
            }
            else
            {
                return true;
            }          
        }
    }
}
