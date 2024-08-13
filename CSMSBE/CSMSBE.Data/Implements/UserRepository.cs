using CSMS.Data.Interfaces;
using CSMS.Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.ProjectDTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CSMS.Model.User;
using ImageMagick;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static CSMSBE.Core.Helper.Constant;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CSMS.Model.DTO.BaseFilterRequest;

namespace CSMS.Data.Implements
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;
        private readonly csms_dbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserRepository(csms_dbContext context, ILogger<UserRepository> logger, IMapper mapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager) : base(context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IQueryable<User> GetLookupUser(IKeywordDto keywordDto)
        {
            try
            {
                IQueryable<User> query = null;
                if (string.IsNullOrEmpty(keywordDto.Keyword))
                {
                    query = _context.Users;
                    return query;
                }
                query = _context.Users.Where(x => x.Email.ToLower().Contains(keywordDto.Keyword.ToLower()) 
                                                    || x.FullName.ToLower().Contains(keywordDto.Keyword.ToLower()));
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> Create(CreateUserDTO dto)
        {
            try
            {
                var userEntity = _mapper.Map<User>(dto);
                userEntity.Id = Guid.NewGuid().ToString();
                userEntity.EmailConfirmed = true;
                userEntity.PhoneNumberConfirmed = true;
                userEntity.CreatedDate = DateTimeOffset.Now;
                userEntity.ModifiedDate = DateTimeOffset.Now;
                userEntity.CreatedBy = "ADMIN";
                userEntity.ModifiedBy = "ADMIN";
                userEntity.Status = true;
                userEntity.UserType = UserType.REGISTERUSER;
                string password = "123@123Aa";
                var addUser = await _userManager.CreateAsync(userEntity, password);
                if (!addUser.Succeeded)
                {
                    throw new Exception("Failed to create user.");
                }

                // Add role
                if (!string.IsNullOrWhiteSpace(dto.RoleId))
                {
                    var role = _roleManager.FindByIdAsync(dto.RoleId).Result.NormalizedName;
                    await _userManager.AddToRoleAsync(userEntity, role);
                }

                // Add default claim
                await _userManager.AddClaimAsync(userEntity, new Claim(RecordPermission, SeeOnlyMineRecord));

                var mappedEntity = _mapper.Map<User>(userEntity);
                var result = _context.Users.Add(mappedEntity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public List<UserRole> GetListUserRole(string id)
        {
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == id).ToList();
            return userRoles;
        }

        public Role GetRoleDetail(string id)
        {
            var role = _context.Roles.Find(GetListUserRole(id).FirstOrDefault().RoleId);
            return role;
        }

        public User UpdateUser(UpdateUserDTO updateDto)
        {
            try
            {
                var entity = _context.Users.Find(updateDto.Id);
                if (entity == null)
                {
                    throw new ArgumentException("Không tìm thấy user để cập nhật");
                }
                entity.ModifiedDate = DateTimeOffset.Now;
                // Update fields of the User entity
                _mapper.Map(updateDto, entity);
                _context.SaveChanges();

                // Find current UserRole for the updated User
                var currentUserRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == entity.Id);
                if (currentUserRole == null)
                {
                    throw new ArgumentException("Không tìm thấy role để cập nhật");
                }

                // Delete current UserRole
                _context.UserRoles.Remove(currentUserRole);
                _context.SaveChanges();

                // Add new UserRole with updated RoleId
                var newUserRole = new UserRole
                {
                    UserId = entity.Id,
                    RoleId = updateDto.RoleId  // Assign new RoleId here
                };
                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> UploadAvatar(string avatarUrl, string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
                if (user == null) return false;
                user.AvatarUrl = avatarUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
