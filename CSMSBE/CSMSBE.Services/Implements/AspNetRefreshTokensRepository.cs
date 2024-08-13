using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.IdentityExtensions;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class AspNetRefreshTokensRepository : BaseRepository<AspNetRefreshTokens>, IAspNetRefreshTokensRepository
    {
        public AspNetRefreshTokensRepository(csms_dbContext context) : base(context)
        {
        }
    }
}
