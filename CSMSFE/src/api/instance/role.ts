import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { Role } from "@/common/DTO/Role/RoleDTO";
import { RoleCreateUpdateDTO } from "@/common/DTO/Role/RoleCreateUpdateDTO";
import { LookUpItem, PagedListContent } from "@/common/DTO/ApiResponse";
import { RoleQueryFilter } from "@/common/DTO/Role/RoleQueryFilter";
import { SecurityMatrixDTO } from "@/common/DTO/Role/SecurityMatrixDTO";
import { CreateSecurityMatrixDTO } from "@/common/DTO/Role/CreateSecurityMatrixDTO";

const service = new Service();

const RoleService = {
    Filter: async (pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: RoleQueryFilter | null) => {
        try {
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            // Thêm các tham số tìm kiếm nếu có
            search?.code && params.append("code", search.code);
            search?.name && params.append("name", search.name);

            return await service.get<PagedListContent<Role>>(ApiEndpointCollection.RoleManagement.Filter, params);
        } catch (err) {
            throw err;
        }
    },


    GetLookup: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.RoleManagement.GetLookup, params);
        } catch (err) {
            throw err;
        }
    },

    Create: async (data: RoleCreateUpdateDTO) => {
        try {
            return await service.post<any>(ApiEndpointCollection.RoleManagement.Create, data);
        } catch (err) {
            throw err;
        }
    },

    Update: async (data: RoleCreateUpdateDTO) => {
        try {
            return await service.put<Role>(ApiEndpointCollection.RoleManagement.Update, data);
        } catch (err) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            return await service.delete<any>(ApiEndpointCollection.RoleManagement.Delete.replace("{id}", id));
        } catch (err) {
            throw err;
        }
    },

    LookUpAction: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.PermissionLookUp.Action, params);
        } catch ( err ) {
            throw err;
        }
    },

    LookUpScreen: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.PermissionLookUp.Screen, params);
        } catch ( err ) {
            throw err;
        }
    },

    GetSecurityMatrixDetail: async ( roleId: string ) => {
        try {
            const params = new URLSearchParams();
            params.append("roleId", roleId);

            return await service.get<SecurityMatrixDTO[]>(ApiEndpointCollection.PermissionLookUp.GetSecurityMatrixDetail, params);
        } catch ( err ) {
            throw err;
        }
    },

    UpdateSecurityMatrix: async ( data: CreateSecurityMatrixDTO ) => {
        try {
            return await service.post<any>(ApiEndpointCollection.PermissionLookUp.UpdateSecurityMatrix, data);
        } catch ( err ) {
            throw err;
        }
    },
}

export default RoleService;
