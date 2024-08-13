using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.SecurityMatrixDTO
{
    public class ScreenDTO
    {
        public int Id { set; get; }
        public int? ParentId { set; get; }
        [JsonIgnore]
        public ScreenDTO Parent { get; set; }

        public string Code { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public string Icon { set; get; }
        public int Order { get; set; }
        public ICollection<ScreenDTO> Childrent { get; set; }
    }
}
