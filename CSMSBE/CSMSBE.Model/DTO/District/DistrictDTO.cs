using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.District
{
    public class DistrictDTO
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? ProvinceId { get; set; }
        public ICollection<CommuneDTO>? CommunesDto { get; set; } = new List<CommuneDTO>();      
    }
}
