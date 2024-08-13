using CSMS.Entity;
using CSMSBE.Core.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class CreateUserDTO
    {
        
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string RoleId { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string PhoneNumber { get; set; }
        public void ValidateInput()
        {
            try
            {
                if (!ValidationHelper.IsValidPhoneNumber(PhoneNumber))
                {
                    throw new InvalidOperationException("Phone number is invalid");
                }
                if (!ValidationHelper.IsValidEmail(Email))
                {
                    throw new InvalidOperationException("Email is invalid");
                }
                if (DateOfBirth != null && DateOfBirth > DateTimeOffset.UtcNow)
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
