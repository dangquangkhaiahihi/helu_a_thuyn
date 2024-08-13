import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { ProjectQueryFilter } from "@/common/DTO/Project/ProjectQueryFilter";
import { Project } from "@/common/DTO/Project/ProjectDTO";
import { ProjectCreateUpdateDTO } from "@/common/DTO/Project/ProjectCreateUpdateDTO";
import { LookUpItem, PagedListContent } from "@/common/DTO/ApiResponse";

const service = new Service();

const ProjectService = {
    Filter: async (pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: ProjectQueryFilter | null) => {
        try {
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            search?.Name && params.append("Name", search.Name);
            search?.ProjectRole && params.append("ProjectRole", search.ProjectRole);
            
            search?.Type && params.append("Type", search.Type);
            search?.CreatedDate && params.append("CreatedDate", search.CreatedDate);
            search?.ProvinceId && params.append("ProvinceId", search.ProvinceId);
            search?.DistrictId && params.append("DistrictId", search.DistrictId);
            search?.CommuneId && params.append("CommuneId", search.CommuneId);

            return await service.get<PagedListContent<Project>>(ApiEndpointCollection.ProjectManagement.Filter, params);
        } catch ( err ) {
            throw err;
        }
    },

    GetById: async (projectId: string) => {
        try {
            return await service.get<Project>(ApiEndpointCollection.ProjectManagement.GetById.replace("{id}", projectId));
        } catch ( err ) {
            throw err;
        }
    },

    Create: async (data: ProjectCreateUpdateDTO) => {
        try {
            return await service.post<Project>(ApiEndpointCollection.ProjectManagement.Create, data);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data: ProjectCreateUpdateDTO) => {
        try {
            
            return await service.put<Project>(ApiEndpointCollection.ProjectManagement.Update, data);
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            
            return await service.delete<any>(ApiEndpointCollection.ProjectManagement.Delete.replace("{id}", id));
        } catch ( err ) {
            throw err;
        }
    },

    GetLookUpTypeProject: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.ProjectManagement.GetLookUpTypeProject, params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default ProjectService;