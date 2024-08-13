import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { UserProject } from "@/common/DTO/UserProject/UserProjectDTO";
import { UpdateUserRoleDTO } from "@/common/DTO/RoleUserProjectDTO/UpdateUserRoleDTO";

const service = new Service();

const PermissionUserProjectService = {
    GetUsersInProject: async (projectId: string = "") => {
        try {
            return await service.get<UserProject[]>(ApiEndpointCollection.PermissionUserProject.GetUsersInProject.replace("{projectId}", projectId));
        } catch ( err ) {
            throw err;
        }
    },

    UpdateUserRole: async (data: UpdateUserRoleDTO) => {
        try {
            return await service.post<any>(ApiEndpointCollection.PermissionUserProject.UpdateUserRole, data);
        } catch ( err ) {
            throw err;
        }
    },

    InviteUser: async (data: {userId: string, projectId: string}) => {
        try {
            return await service.post<any>(ApiEndpointCollection.PermissionUserProject.InviteUser, data);
        } catch ( err ) {
            throw err;
        }
    },

    RemoveUserFromProject: async (userId: string, projectId: string) => {
        const params = new URLSearchParams();
        params.append("userId", userId);
        params.append("projectId", projectId);
        
        try {
            return await service.delete<any>(ApiEndpointCollection.PermissionUserProject.RemoveUserFromProject, params);
        } catch (err) {
            throw err;
        }
    },

    AcceptOrRejectInvitation: async (data: {projectId: string, isApproved: boolean}) => {
        const params = new URLSearchParams();
        params.append("isApproved", data.isApproved.toString());

        try {
            return await service.post<any>(ApiEndpointCollection.PermissionUserProject.AcceptOrRejectInvitation
                .replace("{projectId}", data.projectId), params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default PermissionUserProjectService;