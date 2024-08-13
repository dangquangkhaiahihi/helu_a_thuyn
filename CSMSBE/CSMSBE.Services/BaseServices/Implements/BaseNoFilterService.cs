using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CSMSBE.Services.BaseServices.Interfaces;
using CSMSBE.Core;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity;
using CSMS.Model.DTO.BaseFilterRequest.BaseModels;
using CSMS.Data.Repository;
using CSMSBE.Model.Repository;

namespace CSMSBE.Services.BaseServices.Implements
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TEntityDto">DTO tương ứng với Entity, có thể dùng cho detail hoặc list</typeparam>
    /// <typeparam name="TListEntityDto">DTO dùng cho các trường hợp list, không có next object</typeparam>
    /// <typeparam name="TCreateDto">DTO Create</typeparam>
    /// <typeparam name="TUpdateDto">DTO Update</typeparam>
    public class BaseNoFilterService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto> : 
        IBaseNoFilterService<T, TEntity,TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>
        where TEntityDto : EntityDto<T>
        where TListEntityDto : EntityDto<T>
                                      where TCreateDto : class
                                      where TUpdateDto : EntityDto<T>
                                      where TEntity : BaseFullAuditedEntity<T>
    {
        protected readonly IRepository<TEntity,T> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseNoFilterService<T, TEntity,TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>> _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly UserManager<User> _userManager;
        //protected readonly IAppMemoryCache<T, TEntity> _memoryCache;
        //private readonly bool _useCache;
        public BaseNoFilterService(IRepository<TEntity,T> repository, IMapper mapper,
            ILogger<BaseNoFilterService<T, TEntity, TEntityDto, TListEntityDto, TCreateDto, TUpdateDto>> logger, 
            IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
            //,IAppMemoryCache<T, TEntity> memoryCache, bool useCache=false)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            //_memoryCache = memoryCache;
            //_useCache = useCache;
        }

        public async virtual Task<IQueryable<TEntity>> QueryFilter()
        {
            IQueryable<TEntity> result;
            //if (_useCache)
            //    result = (await _memoryCache.GetAll()).AsQueryable();
            //else
                result = _repository.GetAll().OrderByDescending(x => x.ModifiedDate);

            return result;
        }

        #region Get
        public async virtual Task<ICollection<TEntityDto>> GetAllAsync()
        {
            try
            {
                var query = (await QueryFilter());
                var response = _mapper.Map<ICollection<TEntityDto>>(query.ToList());
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async virtual Task<TEntityDto> GetById(T id, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                TEntity result;
                //if (_useCache)
                //    result = await _memoryCache.GetById(id, includes);
                //else
                    result = _repository.Find(x => x.Id.Equals(id), includes);
                var response = _mapper.Map<TEntityDto>(result);
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
            
        }

        #endregion

        #region Update
        public async virtual Task<ResponseItem<bool>> Create(TCreateDto obj)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(obj);
                var user = GetCurrentUserName();
                if (user==null)
                {
                    entity.CreatedBy = "Administrator";
                    entity.ModifiedBy = "Administrator";
                }    
                else
                {
                    entity.CreatedBy = user;
                    entity.ModifiedBy = user;
                }
                entity.ModifiedDate = DateTime.Now;
                entity.CreatedDate = DateTime.Now;
                var response = _repository.InsertAsync(entity);
                _unitOfWork.Complete();

                //if (_useCache)
                //    await _memoryCache.AddItemToEntry(entity);
                
                return new ResponseItem<bool>
                {
                    Result = true
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = ex.InnerException.ToString(),
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = false
                };
            }
        }
        public async virtual Task<T> CreateReturnId(TCreateDto obj)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(obj);
                var user = GetCurrentUserName();
                if (user==null)
                {
                    entity.CreatedBy = "Administrator";
                    entity.ModifiedBy = "Administrator";
                }    
                else
                {
                    entity.CreatedBy = user;
                    entity.ModifiedBy = user;
                }
                entity.ModifiedDate = DateTime.Now;
                entity.CreatedDate = DateTime.Now;
                var response = await _repository.InsertAsync(entity);
                await _unitOfWork.CompleteAsync();

                //if (_useCache)
                //    await _memoryCache.AddItemToEntry(entity);

                return (T)response.Id;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return default;
            }
        }

        public async virtual Task<ResponseItem<bool>> Update(TUpdateDto obj)
        {
            try
            {
                var entity = _repository.GetById(obj.Id);
                if (entity == null)
                    return new ResponseItem<bool>
                    {
                        Err = 1,
                        ResponseError = new ResponseErrorData
                        {
                            ErrorMessage = StringMessage.ErrorMessages.ItemNotFound,
                            ErrorType = ErrorTypeConstant.RecordNotFound
                        },
                        Result = false
                    };
                entity = _mapper.Map(obj, entity);
                var user = GetCurrentUserName();
                if (user == null)
                    entity.ModifiedBy = "Administrator";
                else
                    entity.ModifiedBy = user;
                entity.ModifiedDate = DateTime.Now;
                var rs = _repository.Update(entity);
                _unitOfWork.Complete();
                //if (_useCache)
                //    await _memoryCache.UpdateItemToEntry(rs);
                return new ResponseItem<bool>
                {
                    Result = true
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = ex.InnerException.ToString(),
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = false
                };
            }
        }

        public async Task<ResponseItem<bool>> Restore(T id)
        {
            try
            {
                var entity = _repository.Find(x=>x.Id.Equals(id));
                if (entity == null)
                {
                    return new ResponseItem<bool>
                    {
                        Err = 1,
                        Result = false,
                        ResponseError = new ResponseErrorData
                        {
                            ErrorMessage = StringMessage.ErrorMessages.ItemNotFound,
                            ErrorType = ErrorTypeConstant.RecordNotFound
                        }
                    };
                }
                var userName = GetCurrentUserName();
                entity.IsDelete = false;
                entity.ModifiedBy = string.IsNullOrEmpty(userName) ? "Administator" : userName;
                entity.ModifiedDate = DateTime.Now;
                _repository.Update(entity);
                _unitOfWork.Complete();

                return new ResponseItem<bool>
                {
                    Result = true
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = e.InnerException.ToString(),
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = false
                };
            }
        }

        public async virtual Task<ResponseItem<bool>> Delete(T id)
        {
            try
            {
                var entity = _repository.Find(x => x.Id.Equals(id));
                if (entity == null)
                {
                    return new ResponseItem<bool>
                    {
                        Err = 1,
                        Result = false,
                        ResponseError = new ResponseErrorData
                        {
                            ErrorMessage = StringMessage.ErrorMessages.ItemNotFound,
                            ErrorType = ErrorTypeConstant.RecordNotFound
                        }
                    };
                }
                var userName = GetCurrentUserName();
                entity.IsDelete = true;
                entity.ModifiedBy = string.IsNullOrEmpty(userName) ? "Administator" : userName;
                entity.ModifiedDate = DateTime.Now;
                _repository.Update(entity);
                _unitOfWork.Complete();

                return new ResponseItem<bool>
                {
                    Result = true
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = e.InnerException.ToString(),
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = false
                };
            }
        }

        public async virtual Task<ResponseItem<bool>> DeletePermanent(T id)
        {
            try
            {
                var result = _repository.Delete(id);
                _unitOfWork.Complete();

                //if (_useCache)
                //    await _memoryCache.DeleteItemFromEntry(id);

                return new ResponseItem<bool>
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = ex.InnerException.ToString(),
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = false
                };
            }
        }
        #endregion

        #region Helper Method
        protected string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type.Equals("userId"))?.Value;
            return userId;
        }

        protected string GetCurrentUserName()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return null;
            }
            else
            {
                var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                return user?.UserName;
            }
        }

        protected ResponseItem<bool> ReturnResult(Exception ex = null, bool result=true)
        {
            if (ex == null)
            {
                return new ResponseItem<bool>
                {
                    Result = result
                };
            }
            else
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseItem<bool>
                {
                    Err = 1,
                    ResponseError = new ResponseErrorData
                    {
                        ErrorMessage = StringMessage.ErrorMessages.ErrorProcess,
                        ErrorType = ErrorTypeConstant.ErrorProcess
                    },
                    Result = result
                };
            }
        }

        #endregion
    }
}
