using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.District;
using CSMS.Model.DTO.Province;
using CSMSBE.Core.Extensions;
using CSMSBE.Infrastructure.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Commune = CSMS.Entity.CSMS_Entity.Commune;
using District = CSMS.Entity.CSMS_Entity.District;
using Province = CSMS.Entity.CSMS_Entity.Province;

namespace CSMS.Data.Implements
{
    public class LocationRepository : ILocationRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<LocationRepository> _logger;
        private readonly IMapper _mapper;
        public LocationRepository(csms_dbContext context, ILogger<LocationRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public IQueryable<Commune> GetLookupCommune(IKeywordDto keywordDto, int? districtId)
        {
            try
            {
                IQueryable<Commune> query = null;
                if(String.IsNullOrEmpty(keywordDto.Keyword)) {
                    query = _context.Communes.Where(x => x.IsDelete == false).WhereIf(districtId.HasValue, x => x.DistrictId == districtId.GetValueOrDefault()); ;
                    return query;
                }
                query = _context.Communes.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()) && x.IsDelete == false)
                    .WhereIf(districtId.HasValue, x => x.DistrictId == districtId.GetValueOrDefault());
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Commune> GetAllCommunesByDistrictId(int id)
        {
            try
            {
                var query = _context.Communes.Where(x => x.DistrictId == id && (!x.IsDelete.HasValue || !x.IsDelete.Value));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<District> GetLookupDistrict(IKeywordDto keywordDto, int? provinceId)
        {
            try
            {
                IQueryable<District> query = null;
                if (String.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Districts.Where(x => x.IsDelete == false)
                        .WhereIf(provinceId.HasValue, x => x. ProvinceId== provinceId.GetValueOrDefault());
                    return query;
                }
                query = _context.Districts.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()) && x.IsDelete == false)
                    .WhereIf(provinceId.HasValue, x => x.ProvinceId == provinceId.GetValueOrDefault());
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<District> GetAllDistrictsByProvinceId(int id)
        {
            try
            {
                var query = _context.Districts.Where(x => x.ProvinceId == id && (!x.IsDelete.HasValue|| !x.IsDelete.Value));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Province> GetLookupProvince(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<Province> query = null;
                if (String.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Provinces.Where(x => x.IsDelete == false);
                    return query;
                }
                query = _context.Provinces.Where(x => x.Name.ToLower().Contains(keywordDto.Keyword.ToLower()) && x.IsDelete == false);
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task InsertCommunes(IList<CommuneDTO> dtos)
        {
            try
            {
                var entities = await Task.Run(() => dtos.AsParallel().Select(dto => _mapper.Map<Commune>(dto)).ToList());
                foreach (var item in entities)
                {
                    item.SetDefaultValue("ADMIN");
                }
                await _context.Communes.AddRangeAsync(entities?.ToArray());
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task InsertDistricts(IList<DistrictDTO> dtos)
        {
            try
            {
                var entities = await Task.Run(() => dtos.AsParallel().Select(dto => _mapper.Map<District>(dto)).ToList());
                foreach (var item in entities)
                {
                    item.SetDefaultValue("ADMIN");
                }
                await _context.Districts.AddRangeAsync(entities?.ToArray());
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task InsertProvinces(IList<ProvinceDTO> dtos)
        {
            try
            {              
                var entities = await Task.Run(() => dtos.AsParallel().Select(dto => _mapper.Map<Province>(dto)).ToList());
                foreach (var item in entities)
                {
                    item.SetDefaultValue("ADMIN");
                }
                await _context.Provinces.AddRangeAsync(entities?.ToArray());
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Province> GetAllProvince()
        {
            try
            {
                var query = _context.Provinces;
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<District> GetAllDistrict()
        {
            try
            {
                var query = _context.Districts;
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Commune> GetAllCommune()
        {
            try
            {
                var query = _context.Communes;
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CleanLocationData()
        {
            try
            {
                _context.Communes.RemoveRange(_context.Communes);
                _context.Districts.RemoveRange(_context.Districts);
                _context.Provinces.RemoveRange(_context.Provinces);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
