using System;
using System.Collections.Generic;

namespace CSMSBE.Core.Helper
{
    public class Constant
    {
        public const string DisplayName = "CỤC PCCC C07";
        public const string LoginProvider = "QLHSC07";
        public const string Admin = "Admin";
        public const int DefaultPageSize = 20;
        public const int DefaultPageClassifiedSize = 10;
        public const int DefaultPageIndex = 1;
        public const int DefaultKeyword = 4;
        public const bool DefaultValue = false;
        public const string RegisterName = "REGISTERUSER";
        public const string SeeOnlyMineRecord = "SeeOnlyMineRecord";
        public const string SeeAllRecord = "SeeAllRecord";
        public const string DeviceType = "device";
        public const string TOKEN_CSMS = "Authorization";
        public const string FullName = "${FULLNAME}";
        public const string ActiveUrl = "${ACTIVE_URL}";
        public const string DeviceDefault = "DesktopDefault";
        public const string RecordPermission = "RecordPermission";
        public const int FileNameMaxLength = 100;
        public const int FileSizeMax = 2097152; //2MB
        public const string UserNameClaim = "userName"; //2MB
        public const string GEMBOX_KEY = "FREE-LIMITED-KEY"; //2MB
        public class UserType
        {
            public const string REGISTERUSER = "REGISTERUSER";
            public const string ADMINUSER = "ADMINUSER";
        }
        public class ResponseCode
        {
            public const string E00 = "Thành công";
            public const string E01 = "Da ton tai PageType";
        }

        public class Schema
        {
            public const string CMS = "cms";
            public const string SYS = "sys";
            public const string AUTHENTICATION = "authentication";
            public const string CSMS = "csms";
            public const string SecurityMatrixProject = "security_matrix_project";
        }
        public static class Module
        {
            public const string EmailTemplate = "Email Template";
        }

        public static class EmailType
        {
            public const int Register = 1;
            public const int ForgotPassWord = 1;
        }
        public class StatusCode
        {
            public const int _200 = 200;
            public const int _400 = 400;
        }

        public class Status
        {
            public const string ADD = "ADD";
            public const string UPDATE = "UPDATE";
            public const string DELETE = "DELETE";
            public const string NONE = "NONE";

        }

        public class EmailTemplate
        {
            public const string ContactToAdmin = "CONTACTADMIN";
            public const string ContactToUser = "CONTACTUSER";
            public const string ForgotPassword = "FORGOTPASSWORD";
            public const string ChangedPassword = "CHANGEDPASSWORD";
            public const string CreateUser = "CREATEUSER";
            public const string ACTIVEUSER = "ACTIVEUSER";
            public const string Reply = "REPLY";
            public const string ReplyContact = "REPLYCONTACT";
            public const string ConfirmAccount = "CONFIRMACCOUNT";
            public const string SendMessage = "SENDMESSAGE";
            public const string ReplyMessage = "REPLYMESSAGE";
        }
        public class PathName
        {
            public const string UserFolder = @"Uploads\account-image";
            public const string GeneralFolder = @"Uploads\general-folder";
            public const string Document = @"Uploads\document-content";
            public const string RecordsFolder= @"Uploads\records-content";
            public const string MsgExchangeFolder= @"Uploads\msg-exchange-folder";
            public const string DigitalFileFolder = @"Uploads\digital-file-folder";
        }
        public class DocumentType
        {
            public const string Folder = "Folder";
            public const string File = "File";
        }

        public class DefaultFields
        {
            public const string CreatedDate = "CreatedDate";
        }

        public class SortType
        {
            public const string desc = "desc";
        }

        //Max length constant
        public class Maxlength
        {
            public const int PhoneNumber = 20;
            public const int Email = 50;
            public const int Title = 150;
            public const int Color = 7;
            public const int Name = 200;
            public const int UnitName = 50;
            public const int TaxCode = 20;
            public const int UserType = 15;
            public const int Description = 550;
            public const int MaxCodeLength = 95;
            public const int Default = 255;
            public const int Content = 9999;
        }
        public class ExternalLogin
        {
            public const string UrlReturn = "returnUrl";
            public const string GoogleService = "Google";
            public const string FacebookService = "Facebook";
            public const string UserIdClaim = "userId";
            public const string MessageExisted = "DuplicateUserName";
            public const string FullName = "fullName";
            public const string FirstName = "firstName";
            public const string LastName = "lastName";
            public const string Email = "email";
            public const string NameIdentifier = "nameIdentifier";
            public const string AuthenticationGooglePath = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
            public const string AuthenticationFacebookPath = "https://graph.facebook.com/me?access_token={0}&fields=email,first_name,last_name,name,gender,picture";

        }

        public class FileType
        {
            public const int UserTypeFile = 3;
            public const int DownloadImageDefaultId = 0;
            public const string ExtentionImage = ".jpg";
        }

        public class ActiveAccount
        {
            public const string Pc07LocalId = "pc07LocalId";
            public const string RecordPermission = "RecordPermission";
            public const string UserId = "userId";
            public const string UserName = "userName";
            public const string FullName = "fullName";
            public const string UserRole = "userRole";
            public const string Avatar = "avatar";
            public const string ExpireTime = "expireTime";
            public const string TelephoneNumber = "telephoneNumber";
            public const string Address = "address";
            public const string NameIdentified = "nameIdentified";
            public const string FirstName = "firstName";
            public const string LastName = "lastName";
            public const string MessageSuccedd = "Bạn đã active tài khoản thành công";
        }

        public class LimitedString
        {
            public const int Maximum = 50;
            public const int Minimum = 2;
        }
        public class AnalysisAddressMessage
        {
            public const string CreateBy = "Được tạo từ AnalysisAddress";
            public const string Url = "https://maps.googleapis.com/maps/api/geocode/json?{0}&key={1}";
        }

        public class APIName
        {
            public const string Login = "API/Login";
            public const string LoginExternal = "Login/";
        }

        public class StoreProceduce
        {
            public const string CoordinateClassified = "QLHSC07.getclassified_near_by_coordinate";
        }

        /*public class SizeImage
        {
            public static ImageSize[] ImageSize = {
                new ImageSize { Width = 376, Height = 400 },//List dự án tiêu biểu
                new ImageSize { Width = 576, Height = 324 },//Danh sách tin quy hoạch, tin dự án ở trang chủ, trang thông tin dự án, trang quy hoạch
                new ImageSize { Width = 117, Height = 73 }, //Tin nổi bật
                new ImageSize { Width = 120, Height = 90 }, //Danh sách tin quy hoạch, tin dự án ở trang chủ
                new ImageSize { Width = 176, Height = 112 },//Ảnh galary trang chi tiết dự án
                new ImageSize { Width = 270, Height = 155 }, //Danh sách  quy trình đầu tư dự án, Văn bản pháp lý ở trang chủ
                new ImageSize { Width = 120, Height = 75 }, //Ảnh nhỏ bạn cần biết ở trang chủ
                new ImageSize { Width = 376, Height = 226 },//Ảnh lớn bạn cần biết ở trang chủ
                new ImageSize { Width = 354, Height = 266 }, //list mua bán trang chủ, list trang mua bán, list post
                new ImageSize { Width = 776, Height = 0 }, //Ảnh ground trang chi tiết dự án
                new ImageSize { Width = 776, Height = 485 },//Ảnh galary trang chi tiết dự án
                new ImageSize { Width = 362, Height = 272 }, //Ảnh project ở trang chi tiết mua bán
                new ImageSize { Width = 0, Height = 342 }, //trang mua bán chi tiết
            };

        }*/

        public class PageName
        {
            public const string BUYNSALE = "BUYNSALE";
        }

        public static class FileExtentions
        {
            public static readonly string[] FileExtention = { "pdf", "xlsx" };
        }
        
        public static class AddressExtentions
        {
            public static readonly string[] CommuneExtention = { "xa", "phuong" };
            public static readonly string[] DistrictExtention = { "quan", "huyen", "thanh pho", "thi xa", "tp" };
        }
    }
}
