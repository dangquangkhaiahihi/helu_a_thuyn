import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { LookUpItem, PagedListContent } from "@/common/DTO/ApiResponse";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from "@/common/default-config";
import { RoleProject } from "@/common/DTO/RoleProjectDTO/RoleProjectDTO";
import { CreateUpdateRoleProjectDTO } from "@/common/DTO/RoleProjectDTO/CreateUpdateRoleProjectDTO";

const service = new Service();

const PermissionProjectService = {
    FilterRoleByProjectId: async (projectId: string = "", pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, 
        search: {
            name: string,
            code: string
        } | null) => {
        try {
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            search?.name && params.append("name", search.name);
            search?.code && params.append("code", search.code);

            return await service.get<PagedListContent<RoleProject>>(ApiEndpointCollection.PermissionProject.FilterRoleByProjectId.replace("{projectId}", projectId), params);
        } catch ( err ) {
            throw err;
        }
    },

    CreateOrUpdateRole: async (data: CreateUpdateRoleProjectDTO) => {
        try {
            return await service.post<any>(ApiEndpointCollection.PermissionProject.CreateOrUpdateRole, data);
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (roleId: string) => {
        const params = new URLSearchParams();
        params.append("roleId", roleId);
        
        try {
            return await service.delete<any>(ApiEndpointCollection.PermissionProject.RemoveRole, params);
        } catch (err) {
            throw err;
        }
    },

    LookupRoleByProject: async (data: { Keyword?: string; projectId: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);
            params.append("projectId", data.projectId);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.PermissionProject.GetLookupRoleByProject, params);
        } catch ( err ) {
            throw err;
        }
    },

    LookupAction: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.PermissionProject.GetLookupAction, params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default PermissionProjectService;