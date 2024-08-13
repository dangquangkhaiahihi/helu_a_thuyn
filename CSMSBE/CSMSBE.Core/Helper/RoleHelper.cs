using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core.Helper
{
    public class RoleHelper
    {
        public const string Admin = "ADMIN";
        public const string RegisterUser = "REGISTERUSER";

        public class RoleProjects
        {
            public const string ViewerCode = "VIEWER";
            public const string ViewerName = "Người xem";

            public const string ProjectOwnerCode = "PROJECT_OWNER";
            public const string ProjectOwnerName = "Chủ sở hữu";

            public const string BIMEngineerCode = "BIM_ENGINEER";
            public const string BIMEngineerName = "Kỹ sư BIM";

            public const string ReviewerCode = "REVIEWER";
            public const string ReviewerName = "Người phê duyệt";

            public const string ProjectManagerCode = "PROJECT_MANAGER";
            public const string ProjectManagerName = "Quản lý";
        }
        public class ActionProjects
        {
            public const string EditResource = "EDIT_RESOURCE";
            public const string EditProject = "EDIT_PROJECT";
            public const string Invite = "INVITE";
            public const string ViewAllResource = "VIEW_ALL_RESOURCE";
            public const string ReviewResource = "REVIEW_RESOURCE";
            public const string ViewResource = "VIEW_RESOURCE";
        }
        public class Action
        {
            public const string FullPermission = "FULL_PERMISSION";
            public const string View = "VIEW";
            public const string Create = "CREATE";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
            public const string Download = "DOWNLOAD";
        }
        public class Screen
        {
            public const string UserManagement = "USER_MANAGEMENT";
            public const string RoleManagement = "ROLE_MANAGEMENT";
            public const string EmailTemplate = "EMAIL_TEMPLATE";
            public const string ListUsers = "LIST_USERS";
            public const string SecurityMatrix = "SECURITY_MATRIX";
            public const string Records = "RECORDS";
            public const string Project = "PROJECT";
            public const string CreateProject = "CREATERECORDS";
            public const string Report = "REPORT";
            public const string MessageHasSend = "MESSAGEHASSEND";
            public const string MessageHasReceive = "MESSAGEHASRECEIVE";
            public const string SendMessage = "SENDMESSAGE";
            public const string District = "DISTRICT";
            public const string Province = "PROVINCE";
            public const string Commune = "COMMUNE";
            public const string RecordsStatus = "RECORDSSTATUS";
            public const string ProjectStatus = "PROJECTSTATUS";
            public const string ProjectGroup = "PROJECTGROUP";
            public const string PC07Local = "PC07LOCAL";
            public const string RecordsResultProcessing = "RECORDSRESULTPROCESSING";
            public const string Investor = "INVESTOR";
            public const string AttachFileType = "ATTACHFILETYPE";
            public const string ConstructionType = "CONSTRUCTIONTYPE";
            public const string ImportData = "IMPORTDATA";

        }
    }
}
