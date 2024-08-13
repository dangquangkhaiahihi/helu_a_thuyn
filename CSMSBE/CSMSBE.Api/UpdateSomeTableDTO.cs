using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Core.Helper;

namespace CSMS.Model.DTO.SomeTableDTO
{
    public class UpdateSomeTableDTO
    {
        public int Id { get; set; }
        public string? NormalText { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public string? Type { get; set; }
        public void ValidateInput()
        {
            try
            {
                if (!ValidationHelper.IsValidPhoneNumber(this.PhoneNumber))
                {
                    throw new InvalidOperationException("Phone number is invalid");
                }
                if (!ValidationHelper.IsValidEmail(this.Email))
                {
                    throw new InvalidOperationException("Email is invalid");
                }
                if (this.EndDate != null && this.StartDate != null && this.EndDate < this.StartDate)
                {
                    throw new InvalidOperationException("Start date can not be higher or equal end date");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
