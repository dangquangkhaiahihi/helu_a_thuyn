using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Models
{
    public class RootFile
    {
        public RootFile()
        {

        }
        public string FileNameRecordTemplate { get; set; }
        public string FileNameProjectTemplate { get; set; }
        public string NumberformatDateTime { get; set; }
        public string ProjectsApprovedTemplate { get; set; }
        public string StatisticsAndApprovalTemplate { get; set; }
        public string FileNameRecordResultProcessingTemplate { get; set; }
    }
}
