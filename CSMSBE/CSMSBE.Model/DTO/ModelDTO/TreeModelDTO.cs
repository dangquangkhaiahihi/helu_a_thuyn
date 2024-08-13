using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Model.DTO.ModelDTO
{
    public class TreeModelDTO
    {
        [Required]
        public string[] Ids { get; set; }

        [Required]
        public bool IncludeUploaded { get; set; }
    }
}
