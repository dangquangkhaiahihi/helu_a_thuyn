
using CSMS.Entity.IdentityExtensions;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface IAspNetRefreshTokensRepository : IBaseRepository<AspNetRefreshTokens>
    {
    }
}
