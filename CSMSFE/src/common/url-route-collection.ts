
export const UrlRouteCollection = {
    Home: "/",
    Login: "/dang-nhap",
    ProjectManagement: "/du-an",
    ProjectDashboard: "/du-an/:projectId",

    //
    ProjectModelManagement: "/du-an/:projectId/mo-hinh",
    ProjectModelViewer: "/du-an/:projectId/mo-hinh/:modelId",
    //
    ProjectIssueManagement: "/du-an/:projectId/van-de",
    //
    ProjectIssueDetail: "/du-an/:projectId/van-de/:id",
    //
    ProjectDocumentManagement: "/du-an/:projectId/tai-lieu",
    //
    ProjectSetting: "/du-an/:projectId/cai-dat",
    //
    // ProjectUserSetting: "/du-an/:projectId/cai-dat/",

    // ADMIN
    UserManagement: "/quan-ly-tai-khoan",
    RoleManagement: "/quan-ly-chuc-vu",
}