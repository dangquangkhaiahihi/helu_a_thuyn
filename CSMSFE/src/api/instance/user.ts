import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { UserQueryFilter } from "@/common/DTO/User/UserQueryFilter";
import { User } from "@/common/DTO/User/UserDTO";
import { UserCreateUpdateDTO } from "@/common/DTO/User/UserCreateUpdateDTO";
import { PagedListContent } from "@/common/DTO/ApiResponse";

const service = new Service();

const UserService = {
    Filter: async (pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: UserQueryFilter | null) => {
        try {
            const params = new URLSearchParams();
            params.append("Sorting", sortExpression);
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());

            search?.fullName && params.append("fullName", search.fullName);
            search?.email && params.append("email", search.email);
            search?.phoneNumber && params.append("phoneNumber", search.phoneNumber);

            return await service.get<PagedListContent<User>>(ApiEndpointCollection.UserManagement.Filter, params);
        } catch (err) {
            throw err;
        }
    },

    Create: async (data: UserCreateUpdateDTO) => {
        try {
            const param = new FormData();
            param.append("userName", data.userName);
            param.append("email", data.email);
            param.append("fullName", data.fullName);
            param.append("dateOfBirth", data.dateOfBirth);
            param.append("gender", data.gender.toString());
            param.append("address", data.address);
            param.append("phoneNumber", data.phoneNumber);
            param.append("roleId", data.roleId);
            data.avatar && param.append("avatar", data.avatar);

            return await service.post<User>(ApiEndpointCollection.UserManagement.Create, param);
        } catch (err) {
            throw err;
        }
    },

    Update: async (data: UserCreateUpdateDTO) => {
        try {
            const param = new FormData();
            data.id && param.append("id", data.id);
            param.append("userName", data.userName);
            param.append("email", data.email);
            param.append("fullName", data.fullName);
            param.append("dateOfBirth", data.dateOfBirth);
            param.append("gender", data.gender.toString());
            param.append("address", data.address);
            param.append("phoneNumber", data.phoneNumber);
            param.append("roleId", data.roleId);
            data.avatar && param.append("avatar", data.avatar);

            return await service.put<User>(ApiEndpointCollection.UserManagement.Update, param);
        } catch (err) {
            throw err;
        }
    },

    Active: async (userId: string) => {
        try {
            const id = typeof userId === 'string' ? userId : String(userId);
            const url = `${ApiEndpointCollection.UserManagement.Active}?id=${id}`;
            return await service.post<User>(url, null);
        } catch (err) {
            throw err;
        }
    },
    
    Deactive: async (userId: string) => {
        try {
            const id = typeof userId === 'string' ? userId : String(userId);
            const url = `${ApiEndpointCollection.UserManagement.DeActive}?id=${id}`;
            return await service.post<User>(url, null);
        } catch (err) {
            throw err;
        }
    },

    ResetPassword: async (userId: string) => {
        try {
            return await service.post(ApiEndpointCollection.UserManagement.ResetPassword, { userId });
        } catch (err) {
            throw err;
        }
    },

    GetLookup: async (keyword: string) => {
        try {
            const params = new URLSearchParams();
            params.append("Keyword", keyword);
            return await service.get<any>(`${ApiEndpointCollection.UserManagement.GetLookup}?${params.toString()}`);
        } catch (err) {
            throw err;
        }
    },

    GetUserDetail: async (userId: string) => {
        try {
            const id = typeof userId === 'string' ? userId : String(userId);
            const url = `${ApiEndpointCollection.UserManagement.Detail}?id=${id}`;
            return await service.get<User>(url);
        } catch (err) {
            throw err;
        }
    },
};

export default UserService;
