const ApiEndpointCollection = {
    Account: {
        Login: "Account/Login",
        Logout: "Account/Logout",
        RefreshToken: "Account/RefreshToken",
        GetMyInfo: "Account/GetMyInfo"
    },

    SomeTableManagement: {
        Filter: "SomeTable/Filter",
        GetById: "SomeTable/GetById/{id}",
        Create: "SomeTable/Create",
        Update: "SomeTable/Update",
        Delete: "SomeTable/Remove/{id}",
        GetLookup: "SomeTable/GetLookup"
    },

    ProjectManagement: {
        Filter: "Project/Filter",
        GetById: "Project/GetById/{id}",
        Create: "Project/Create",
        Update: "Project/Update",
        Delete: "Project/Remove/{id}",
        GetLookup: "Project/GetLookup",
        GetLookUpTypeProject: "Project/GetLookUpTypeProject"
    },
    
    IssueManagement: {
        Filter: "Issue/Filter",
        GetById: "Issue/GetById/{id}",
        Create: "Issue/Create",
        Update: "Issue/Update",
        Delete: "Issue/Remove/{id}",
        GetLookup: "Issue/GetLookup",
        GetLookUpTypeIssue: "Issue/GetLookUpTypeIssue"
    },


    LocationLookUp: {
        Province: "Location/GetLookUpProvince",
        District: "Location/GetLookUpDistrict",
        Commune: "Location/GetLookUpCommune",
    },
    Comment:{
        List: "Issue/GetById/{id}",
        Post: "Comment/Create",
        Update: "Comment/Update",
        Delete: "Comment/Remove/{id}"
    },
    Document:{
        GetInit: "Document/GetInit",
        Create: "Document/Create",
        Update: "Document/Update",
        Remove: "Document/Remove",
        Move: "Document/Move",
        Preview: "Document/Preview",
        GetByParentId: "Document/GetByParentId",
        Download: "Document/Download/{id}",
        GetLookUpFileExtension: "Document/GetLookUpFileExtension",
    },

    UserManagement: {
        Create: "User/Create",
        GetLookup: "User/GetLookup",
        Filter: "User/Filter",
        Detail: "User/GetUserDetail",
        Update: "User/Update",
        Active: "User/Active",
        DeActive: "User/DeActive",
        ResetPassword: "User/ResetPassword",
    },
    RoleManagement:{
        GetLookup: "Role/GetLookup",
        Filter: "Role/Filter",
        GetById: "Role/GetById/{id}",
        Create: "Role/Create",
        Update: "Role/Update",
        Delete: "Role/Remove/{id}",
    },
    PermissionLookUp: {
        GetSecurityMatrixDetail: "Permission/GetSecurityMatrixDetail",
        UpdateSecurityMatrix: "Permission/UpdateSecurityMatrix",
        Action: "Permission/GetLookupAction",
        Screen: "Permission/GetLookupScreen",
    },

    ModelManagement: {
        FilterModelByProjectId: "Model/FilterModelByProjectId/{id}",
        GetDirectChildren: "Model/GetDirectChildren/{id}",
        GetTreeModelByProjectId: "Model/GetTreeModelByProjectId",
        Create: "Model/Create",
        Update: "Model/Update",
        Delete: "Model/Remove/{id}",
        Move: "Model/Move",
        UploadFile: "Model/UploadFile",
        GetSpeckleModelsInfo: "Model/GetSpeckleModelsInfo",
        GetFullSpeckleModelsInfo: "Model/GetFullSpeckleModelsInfo"
    },

    PermissionProject: {
        FilterRoleByProjectId: "PermissionProject/FilterRoleByProjectId/{projectId}",
        CreateOrUpdateRole: "PermissionProject/CreateOrUpdateRole",
        RemoveRole: "PermissionProject/RemoveRole",
        GetLookupAction: "PermissionProject/GetLookupAction",
        GetLookupRoleByProject: "PermissionProject/GetLookupRoleByProject"
    },

    PermissionUserProject: {
        GetUsersInProject: "PermissionProject/GetUsersInProject/{projectId}",
        UpdateUserRole: "PermissionProject/UpdateUserRole",
        InviteUser: "PermissionProject/InviteUser",
        RemoveUserFromProject: "PermissionProject/RemoveUserFromProject",
        AcceptOrRejectInvitation: "PermissionProject/AcceptOrRejectInvitation/{projectId}"
    }
}

export default ApiEndpointCollection;