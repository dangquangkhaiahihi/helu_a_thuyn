using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSMSBE.Core.Helper.Constant;

namespace CSMS.Model.Model
{
    public class UpdateModelDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public void ValidateInput() 
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("Name không được để trống");
            }
            if (string.IsNullOrEmpty(Description))
            {
                throw new InvalidOperationException("Description không được để trống");
            }
            if (string.IsNullOrEmpty(Status))
            {
                throw new InvalidOperationException("Status không được để trống");
            }
        }
    }
}
