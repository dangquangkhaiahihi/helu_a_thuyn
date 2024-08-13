 using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.SecurityMatrix;
using CSMSBE.Services;

namespace CSMS.Model.DTO.IssueDTO
{
    public class IssueDTO : BaseModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Assignee { get; set; }
        public string? Image { get; set; }
        public bool? IsDelete { get; set; }
    }
}
