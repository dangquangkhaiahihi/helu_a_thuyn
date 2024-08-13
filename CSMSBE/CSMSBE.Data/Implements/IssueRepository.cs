using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.CSMS_Entity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.Issues;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.Issue;
using CSMSBE.Core.Helper;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class IssueRepository :  BaseRepository<Issue>, IIssueRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<IssueRepository> _logger;
        private readonly IMapper _mapper;

        public IssueRepository(csms_dbContext context, ILogger<IssueRepository> logger, IMapper mapper) : base(context)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public Issue Create(CreateIssueDTO dto, string userId)
        {
            try
            {
                
                var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (user == null) throw new ArgumentException("User không tồn tại!");

                var model = _context.Models.Where(p => p.Id == dto.ModelId).FirstOrDefault();
                if (model == null) throw new ArgumentException("Model không tồn tại!");

                var project = _context.Projects.Find(model.ProjectID);
                if (project == null) throw new ArgumentException("Project không tồn tại!");
                var issueEntity = _mapper.Map<Issue>(dto);
                if (dto.Assignee != null)
                {
                    var userAssignee = _context.Users.Find(dto.Assignee);
                    if (userAssignee == null) throw new ArgumentException("Người được gán cho vấn đề không tồn tại!");
                    issueEntity.Assignee = userAssignee.UserName;
                }
                if (dto.File != null)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{project.Id}");
                    var saveResult = FileHelper.SaveFileAsync(dto.File, folderPath);
                    issueEntity.Image = $"uploads/project_{project.Id}/{dto.File.FileName}";
                }
                issueEntity.ProjectId = project.Id;
                issueEntity.SetDefaultValue(user.UserName);
                issueEntity.SetValueUpdate(user.UserName);
                issueEntity.UserId = userId;
                issueEntity.Status = "OPEN";
                var result = _context.Issues.Add(issueEntity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Issue Get(int id)
        {
            try
            {
                var result = _context.Issues
                  .Include(i => i.User)
                  .Include(i => i.Project)
                  .Include(i => i.Model)
                  .Include(i => i.Comments.Where(c => c.IsDelete == false))
                      .ThenInclude(c => c.User)
                  .FirstOrDefault(x => x.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public bool Remove(int id)
        {
            try
            {

                var entity = _context.Issues.FirstOrDefault(x => x.Id.Equals(id));
                if (entity == null)
                {
                    return false;
                }
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.Image);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                _context.Issues.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Issue Update(UpdateIssueDTO updateDto, string userId)
        {
            try
            {
                var entity = _context.Issues.Include(i => i.Project).FirstOrDefault(i => i.Id == updateDto.Id);

                var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();

                if (entity == null)
                {
                    throw new ArgumentException("Không tìm thấy bản ghi để cập nhật");
                }
                entity.UserId = userId;
                
                _context.Entry(entity).CurrentValues.SetValues(updateDto);

                entity.SetValueUpdate(user.UserName);
                if (updateDto.File != null)
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", $"project_{entity.Project.Id}");
                    var newFilePath = Path.Combine(folderPath, updateDto.File.FileName);
                    FileHelper.SaveFileAsync(updateDto.File, folderPath).Wait(); 

                    entity.Image = $"uploads/project_{entity.Project.Id}/{updateDto.File.FileName}";
                }
                _context.SaveChanges();

                return entity;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
