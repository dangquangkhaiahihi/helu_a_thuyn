using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Interfaces;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.District;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.DTO.Province;
using CSMSBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public LocationService(HttpClient httpClient, ILocationRepository locationRepository, IMapper mapper) { 
            _httpClient = httpClient;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        public async Task<IList<ProvinceDTO>> GetFullTreeLocation()
        {
            try
                {
                IList<ProvinceDTO> provincesDto = null;
                var provinces = await _locationRepository.GetAllProvince().ToListAsync();               
                if (provinces != null)
                {
                    provincesDto = await Task.Run(() => provinces.AsParallel().Select(dto => _mapper.Map<ProvinceDTO>(dto)).ToList());
                    foreach (var item in provincesDto)
                    {
                        var districts = await _locationRepository.GetAllDistrictsByProvinceId(item.Id.GetValueOrDefault()).ToListAsync();
                        if(districts != null)
                        {
                            var districtsDto = _mapper.Map<List<DistrictDTO>>(districts);
                            item.DistrictsDto = districtsDto;
                            foreach(var item1 in districtsDto)
                            {
                                var communes = await _locationRepository.GetAllCommunesByDistrictId(item1.Id.GetValueOrDefault()).ToListAsync();
                                if(communes != null)
                                {
                                    var communesDto = await Task.Run(() => provinces.AsParallel().Select(dto => _mapper.Map<CommuneDTO>(dto)).ToList());
                                    item1.CommunesDto = communesDto;
                                }
                            }
                        }
                    }
                }
                return provincesDto;
            }
            catch(Exception ex)
            {
                throw ex;
            }       
        }
        public async Task<IList<LookupDTO>> GetLookupCommune(IKeywordDto? keywordDto, int districtId)
        {
            try
            {
                var communes = await _locationRepository.GetLookupCommune(keywordDto, districtId).ToListAsync();
                if(communes != null)
                {
                    var lookupCommunesDto = await Task.Run(() => communes.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupCommunesDto;
                }
                return null;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<LookupDTO>> GetLookUpDistrict(IKeywordDto? keywordDto, int provinceId)
        {
            try
            {
                var districts = await _locationRepository.GetLookupDistrict(keywordDto, provinceId).ToListAsync();
                if (districts != null)
                {
                    var lookupDistrictsDto = await Task.Run(() => districts.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupDistrictsDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<LookupDTO>> GetLookUpProvince(IKeywordDto keywordDto)
        {
            try
            {
                var provinces = await _locationRepository.GetLookupProvince(keywordDto).ToListAsync();
                if (provinces != null)
                {
                    var lookupProvincesDto = await Task.Run(() => provinces.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                    return lookupProvincesDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<T> DeserializeAsync<T>(string json)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(json));
        }
        public async Task SyncVNDataLocation()
        {
            try
            {
                await _locationRepository.CleanLocationData();
                var provinceRes = await _httpClient.GetAsync("https://api.npoint.io/ac646cb54b295b9555be");
                var districtRes = await _httpClient.GetAsync("https://api.npoint.io/34608ea16bebc5cffd42");
                var communeRes = await _httpClient.GetAsync("https://api.npoint.io/dd278dc276e65c68cdf5");
                provinceRes.EnsureSuccessStatusCode();
                districtRes.EnsureSuccessStatusCode();
                communeRes.EnsureSuccessStatusCode();
                string provinceResBody = await provinceRes.Content.ReadAsStringAsync();
                string districtResBody = await districtRes.Content.ReadAsStringAsync();
                string communeResBody = await communeRes.Content.ReadAsStringAsync();
                Task<IList<ProvinceDTO>> provincesTask = DeserializeAsync<IList<ProvinceDTO>>(provinceResBody);
                Task<IList<DistrictDTO>> districtsTask = DeserializeAsync<IList<DistrictDTO>>(districtResBody);
                Task<IList<CommuneDTO>> communesTask = DeserializeAsync<IList<CommuneDTO>>(communeResBody);

                await Task.WhenAll(provincesTask, districtsTask, communesTask);
                IList<ProvinceDTO> provincesDTO = provincesTask?.Result;
                IList<DistrictDTO> districtsDTO = districtsTask?.Result;
                IList<CommuneDTO> communesDTO = communesTask?.Result;
                await _locationRepository.InsertProvinces(provincesDTO);
                await _locationRepository.InsertDistricts(districtsDTO);
                await _locationRepository.InsertCommunes(communesDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
