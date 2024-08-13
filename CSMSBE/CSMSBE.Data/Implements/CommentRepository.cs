using AutoMapper;
using CSMS.Data.Interfaces;
using CSMS.Data.Repository;
using CSMS.Entity;
using CSMS.Entity.Issues;
using CSMS.Model.Issue;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMS.Data.Implements
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly csms_dbContext _context;
        private readonly ILogger<CommentRepository> _logger;
        private readonly IMapper _mapper;

        public CommentRepository(csms_dbContext context, ILogger<CommentRepository> logger, IMapper mapper) : base(context)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public Comment GetById(int id)
        {
            try
            {
                var comment = _context.Comments.Include(x => x.Comments).FirstOrDefault(x => x.Id == id);
                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public Comment Create(CreateCommentDTO dto, string userId)
        {
            try
            {
                var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
                var issue = _context.Issues.Where(p => p.Id == dto.IssueId).FirstOrDefault();
                if (user == null) throw new ArgumentException("User không tồn tại!");
                if (issue == null) throw new ArgumentException("Vấn đề không tồn tại!");

                var commentEntity = _mapper.Map<Comment>(dto);
                if (dto.ParentId.HasValue)
                {
                    var parent = _context.Comments.Find(dto.ParentId);
                    if(parent == null)
                    {
                        commentEntity.ParentId = null;
                    }
                    else if (parent.ParentId.HasValue)
                    {
                        var rootParent = _context.Comments.Find(parent.Id);
                        commentEntity.ParentId = rootParent.Id;
                    }
                    else
                        commentEntity.ParentId = parent.Id;
                }
                else
                {
                    commentEntity.ParentId = null;
                }               
               
                commentEntity.SetDefaultValue(user.UserName);
                commentEntity.SetValueUpdate(user.UserName);
                commentEntity.UserId = userId;
                var result = _context.Comments.Add(commentEntity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<List<Comment>> GetIssueComments(int issueId)
        {
            try
            {
                var comments = await _context.Comments.Where(x => x.IssueId == issueId && !x.ParentId.HasValue).Include(x => x.Comments).ToListAsync();               
                return comments;
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
                var entity = _context.Comments.FirstOrDefault(x => x.Id.Equals(id));
                if (entity == null)
                {
                    return false;
                }
                entity.IsDelete = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Comment Update(UpdateCommentDTO dto, string userId)
        {
            try
            {
                var entity = _context.Comments.Find(dto.Id);
                var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (entity.UserId != user.Id)
                {
                    throw new ArgumentException("Không thể update do comment không phải của " + user.UserName);
                }
                if (entity == null)
                {
                    throw new ArgumentException("Không tìm thấy bản ghi để cập nhật");
                }

                // Update fields
                entity.UserId = userId;
                _context.Entry(entity).CurrentValues.SetValues(dto);

                entity.SetValueUpdate(user.UserName);

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
