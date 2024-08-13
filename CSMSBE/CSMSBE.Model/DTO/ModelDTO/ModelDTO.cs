﻿using CSMS.Entity.CSMS_Entity;
using CSMSBE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class ModelDTO : BaseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }       
        public int? Level { get; set; }
        public string? Description { get; set; }
        public string? SpeckleBranchId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? ParentId { get; set; }
        public bool? IsUpload { get; set; }
        public string? PreviewImg { get; set; }
        public string? ProjectID { get; set; }
        public string? ProjectName { set; get; }
        public VersionDTO? LatestVersion { get; set; }
    }
}
