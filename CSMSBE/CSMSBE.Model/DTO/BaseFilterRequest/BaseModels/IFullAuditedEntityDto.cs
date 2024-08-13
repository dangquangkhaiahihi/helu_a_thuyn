namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public interface IFullAuditedEntityDto<T> : IEntityDto<T>, IAuditedEntityDto
    {
    }
}
