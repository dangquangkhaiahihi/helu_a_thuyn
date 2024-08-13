using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSMSBE.Services.BaseServices.Interfaces
{
    public interface IBaseService<T, TEntity, TEntityDTO, TEntityFilter> where TEntity : class where TEntityDTO : class where TEntityFilter : class
    {
        TEntityDTO Create(TEntityDTO dto);
        Task<TEntityDTO> CreateAsync(TEntityDTO dto);
        TEntityDTO GetById(T id);
        Task<TEntityDTO> GetByIdAsync(T id);
        IQueryable<TEntityDTO> GetAll(TEntityFilter filter);
        Task<IQueryable<TEntityDTO>> GetAllAsync(TEntityFilter filter);
        void Update(TEntityDTO dto);
        Task UpdateAsync(TEntityDTO dto);
        void Delete(T id);
        Task DeleteAsync(T id);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TEntityDto">DTO tương ứng với Entity</typeparam>
    /// <typeparam name="TListEntityDto">DTO dùng cho các trường hợp list, không có next object</typeparam>
    /// <typeparam name="TCreateDto">DTO Create</typeparam>
    /// <typeparam name="TUpdateDto">DTO Update</typeparam>
    /// <typeparam name="TFilterDto">DTO filter dữ liệu</typeparam>
    public interface IBaseService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto, TFilterDto> :
        IBaseNoFilterService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>
    {
        Task<IPagedList<TListEntityDto>> GetAll(TFilterDto filter);
        Task<ICollection<TListEntityDto>> GetAllNoPage(TFilterDto filter);
    }
}
