using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CSMS.Model.DTO.BaseFilterRequest
{
    public class PagedAndSortResultRequestDto : PagedResultRequestDto, ISortResultRequestDto
    {
        [DefaultValue("ModifiedDate desc")]
        public string Sorting { get; set; }
        public bool? IsDelete { get; set; }
    }
}
