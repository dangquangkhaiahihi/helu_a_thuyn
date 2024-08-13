using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.IdentityExtensions.IdentityMapping
{
    public class ExchangeRefreshTokenRequest
    {
        public string ReturnUrl { get; set; }
        public string RefreshToken { get; set; }
        public string uUid { get; set; }
    }
}
