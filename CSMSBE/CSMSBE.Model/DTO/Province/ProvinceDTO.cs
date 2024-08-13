using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Model.DTO.District;

namespace CSMS.Model.DTO.Province
{
    public class ProvinceDTO
    {
        public int? Id {  get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public ICollection<DistrictDTO>? DistrictsDto { get; set; } = new List<DistrictDTO>();
    }
}
