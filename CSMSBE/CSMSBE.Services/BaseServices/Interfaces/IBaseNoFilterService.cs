using CSMSBE.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSMSBE.Services.BaseServices.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TEntityDto">DTO tương ứng với Entity</typeparam>
    /// <typeparam name="TListEntityDto">DTO dùng cho các trường hợp list, không có next object</typeparam>
    /// <typeparam name="TCreateDto">DTO Create</typeparam>
    /// <typeparam name="TUpdateDto">DTO Update</typeparam>
    public interface IBaseNoFilterService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<TEntityDto>> GetAllAsync();
        Task<TEntityDto> GetById(T Id, params Expression<Func<TEntity, object>>[] includes);
        Task<ResponseItem<bool>> Create(TCreateDto obj);
        Task<ResponseItem<bool>> Update(TUpdateDto obj);
        Task<ResponseItem<bool>> Delete(T id);
        Task<ResponseItem<bool>> DeletePermanent(T id);
        Task<T> CreateReturnId(TCreateDto obj);
    }
}
