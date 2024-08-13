namespace CSMS.Model.DTO.BaseFilterRequest.BaseModels
{
    public class EntityDto<T> : IEntityDto<T>
    {
        public T Id { get; set; }
    }
}
