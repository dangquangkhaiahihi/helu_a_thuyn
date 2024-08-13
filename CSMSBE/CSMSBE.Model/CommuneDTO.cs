using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Core.Helper;
using CSMS.Entity.CSMS_Entity;

namespace CSMS.Model
{
    public class CommuneDTO
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? DistrictId { get; set; }
    }
}
