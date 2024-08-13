using Serilog.Sinks.SystemConsole.Themes;

namespace CSMSBE.Core.Helper
{
    public class TableFieldNameHelper
    {
        public class Cms
        {
            public const string Province = "Province";
            public const string District = "District";
            public const string Commune = "Commune";
            public const string TypeProject = "TypeProject";
            
        }
        public class Sys
        {
            public const string Screen = "Screen";
            public const string Action = "Action";
            public const string SecurityMatrix = "SecurityMatrix";
            public const string UserLoginLog = "UserLoginLog";
        }
        public class CSMS
        {
            public const string ModelVersion = "ModelVersion";
            public const string SomeTable = "SomeTable";
            public const string UserProject = "UserProject";
            public const string Project = "Project";
            public const string Issue = "Issue";
            public const string Comment = "Comment";
            public const string Document = "Document";
            public const string FileExtensions = "FileExtensions";
        }
        public class IdentityExtentions
        {
            public const string LimitAccessPermission = "LimitAccessPermission";
            public const string AspNetRefreshTokens = "AspNetRefreshTokens";
            public const string AspNetRoles = "AspNetRoles";
            public const string AspNetUsers = "AspNetUsers";
            public const string AspNetUserRoles = "AspNetUserRoles";
            public const string AspNetUserTokens = "AspNetUserTokens";
            public const string AspNetUserLogins = "AspNetUserLogins";
            public const string AspNetUserClaims = "AspNetUserClaims";
            public const string AspNetRoleClaims = "AspNetRoleClaims";

        }
        public class SecurityMatrix_Project
        {
            public const string ProjectUserRoles = "ProjectUserRoles";
            public const string ActionProjects = "ActionProjects";
            public const string RoleProjects = "RoleProjects";
            public const string SecurityMatrixProjects = "SecurityMatrixProjects";
        }
    }
}
