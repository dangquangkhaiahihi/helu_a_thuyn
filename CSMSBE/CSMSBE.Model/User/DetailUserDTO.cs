﻿using CSMS.Model.SecurityMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.User
{
    public class DetailUserDTO
    {
        public string Id { set; get; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }        
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string? AvatarUrl { get; set; }
        public string LastDateLogin { get; set; }
        public string DeviceStatus { get; set; }
    }

    public class DetailUserCmsModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserCmsDto
    {
        public string Id { set; get; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

