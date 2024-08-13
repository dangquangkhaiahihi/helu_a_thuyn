using CSMS.Entity.CSMS_Entity;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.District;
using CSMS.Model.DTO.Province;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task InsertProvinces(IList<ProvinceDTO> dtos);
        Task CleanLocationData();
        Task InsertDistricts(IList<DistrictDTO> dtos);
        Task InsertCommunes(IList<CommuneDTO> dtos);
        IQueryable<Province> GetLookupProvince(IKeywordDto keywordDto);
        IQueryable<District> GetLookupDistrict(IKeywordDto keywordDto, int? provinceId);
        IQueryable<Commune> GetLookupCommune(IKeywordDto keywordDto, int? districtId);
        IQueryable<Province> GetAllProvince();
        IQueryable<District> GetAllDistrict();
        IQueryable<Commune> GetAllCommune();
        IQueryable<District> GetAllDistrictsByProvinceId(int id);
        IQueryable<Commune> GetAllCommunesByDistrictId(int id);
    }
}
