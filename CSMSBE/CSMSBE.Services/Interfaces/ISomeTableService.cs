using CSMSBE.Core.Interfaces;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.SomeTableDTO;


namespace CSMSBE.Services.Interfaces
{
    public interface ISomeTableService
    {
        SomeTableDTO CreateSomeTable(CreateSomeTableDTO dto);
        IQueryable<SomeTableDTO> GetLookupSomeTable();
        SomeTableDTO GetSomeTableById(int id);
        Task<IPagedList<SomeTableDTO>> FilterSomeTable(SomeTableFilterRequest filter);
        SomeTableDTO UpdateSomeTable(UpdateSomeTableDTO dto);
        bool RemoveSomeTable(int id);
    }
}
