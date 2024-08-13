using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.District;
using CSMS.Model.DTO.Province;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface ILocationService
    {
        Task SyncVNDataLocation();
        Task<IList<ProvinceDTO>> GetFullTreeLocation();
        Task<IList<LookupDTO>> GetLookUpProvince(IKeywordDto keywordDto);
        Task<IList<LookupDTO>> GetLookUpDistrict(IKeywordDto? keywordDto, int provinceId);
        Task<IList<LookupDTO>> GetLookupCommune(IKeywordDto? keywordDto, int districtId);
    }
}
