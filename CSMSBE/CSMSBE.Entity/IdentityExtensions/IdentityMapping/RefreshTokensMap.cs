using CSMS.Entity.IdentityExtentions.IdentityMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Entity.IdentityExtensions.IdentityMapping
{
    public class AspNetRefreshTokensMap : CSMSEntityTypeConfiguration<AspNetRefreshTokens>
    {
        public override void Configure(EntityTypeBuilder<AspNetRefreshTokens> builder)
        {
            builder.ToTable("AspNetRefreshTokens");
        }
    }
}
