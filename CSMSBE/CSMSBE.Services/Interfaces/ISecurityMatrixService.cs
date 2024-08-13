using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.SecurityMatrix;
using CSMSBE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.User;
using CSMS.Model.DTO.SecurityMatrixDTO;
using ImageMagick;
using Microsoft.EntityFrameworkCore;

namespace CSMSBE.Services.Interfaces
{
    public interface ISecurityMatrixService
    {
        Task<IList<LookupDTO>> GetLookUpScreen(IKeywordDto keywordDto);
        Task<IList<LookupDTO>> GetLookUpAction(IKeywordDto keywordDto);
        Task<IPagedList<SecurityMatrixDTO>> FilterSM(SMFilterRequest filter);
        List<ScreenViewDTO> GetDetailSecurityMatrix(string Id);
        Task<bool> CreateSecurityMatrix(CreateSecurityMatrixDTO model);
        Task<bool> CheckPermission(string roleId, string actionName, string screenName);
        Task<ICollection<ScreenDTO>> GetListScreen(string userId);
        Task<bool> HasPermissionAsync(string roleId, string screen, string action);
    }
}
