using AutoMapper;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.Issue;
using CSMSBE.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;
        public CommentService(ICommentRepository commentRepository, IMapper mapper, ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public CommentDTO CreateComment(CreateCommentDTO dto, string userId)
        {
            try
            {

                var result = _commentRepository.Create(dto, userId);
                return _mapper.Map<CommentDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public CommentDTO GetCommentById(int id)
        {
            try
            {
                var result = _commentRepository.GetById(id);
                return _mapper.Map<CommentDTO>(result);
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CommentDTO>> GetIssueComments(int issueId)
        {
            try
            {
                var result = await _commentRepository.GetIssueComments(issueId);
                return _mapper.Map<List<CommentDTO>>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveComment(int id)
        {
            try
            {
                var result = _commentRepository.Remove(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public CommentDTO UpdateComment(UpdateCommentDTO dto, string userId)
        {
            try
            {
                var result = _commentRepository.Update(dto, userId);
                return _mapper.Map<CommentDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
    
}
