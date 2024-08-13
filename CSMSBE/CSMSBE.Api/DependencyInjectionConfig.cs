using Microsoft.Extensions.DependencyInjection;
using CSMSBE.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSBE.Services.Interfaces;

using CSMSBE.Services.Implements;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Services;
using CSMS.Data.Interfaces;
using CSMS.Data.Implements;
using CSMSBE.Core.Helper;
using Microsoft.AspNetCore.Identity;
using CSMS.Entity.IdentityAccess;
using CSMS.Entity.IdentityExtensions;
using System.IdentityModel.Tokens.Jwt;
using CSMS.Data.Repository;
using Data.Implements;
using Data.Interfaces;
using CSMSBE.Model.Repository;

namespace CSMSBE.Api
{
    public static class DependencyInjectionConfig
    {
        public static void Configuration(this IServiceCollection services)
        {   //unit of work
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddTransient<IAspNetRefreshTokensRepository, AspNetRefreshTokensRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));


            //log history
            services.AddTransient<IWebHelper, WebHelper>();
            services.AddScoped<ILogHistoryRepository, LogHistoryRepository>();
            services.AddScoped<ILogHistoryService, LogHistoryService>();
            // User
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserManager<User>>();
            services.AddScoped<SignInManager<User>>();
            //SecurityMatrix
            services.AddScoped<ISecurityMatrixService, SecurityMatrixService>();
            services.AddScoped<ISecurityMatrixRepository, SecurityMatrixRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IScreenRepository, ScreenRepository>();               
            /*services.AddScoped<IRepository<UserLoginLog, int>>();*/
            //Role
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            //Project
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();
            //TypeProject
            services.AddScoped<ITypeProjectRepository, TypeProjectRepository>();
            //Location
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationService, LocationService>();
            //SomeTable
            services.AddScoped<ISomeTableRepository, SomeTableRepository>();
            services.AddScoped<ISomeTableService, SomeTableService>();
            //Document
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IFileExtensionsRepository, FileExtensionsRepository>();
        }
    }
}
