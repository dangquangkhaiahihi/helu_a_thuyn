using CSMS.Model.DTO.BaseFilterRequest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.FilterRequest
{
    public class UserFilterRequest : PagedAndSortResultRequestDto
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public string? RoleName { set; get; }
        public string? PhoneNumber { set; get; }
        public string? UserType { get; set; }

        public void ValidateInput()
        {
            try
            {
                if (this.DateOfBirth > DateTimeOffset.UtcNow)
                {
                    throw new InvalidOperationException("Date of birth cannot higher than date time now");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
