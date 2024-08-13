import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { ModelQueryFilter } from "@/common/DTO/Model/ModelQueryFilter";
import { Model } from "@/common/DTO/Model/ModelDTO";
import { PagedListContent } from "@/common/DTO/ApiResponse";
import { ModelUpdateDTO } from "@/common/DTO/Model/ModelUpdateDTO";
import { ModelMoveDTO } from "@/common/DTO/Model/ModelMoveDTO";
import { ModelCreateDTO } from "@/common/DTO/Model/ModelCreateDTO";
import { ModelUploadDTO } from "@/common/DTO/Model/ModelUploadDTO";
import { ModelSpeckleViewInfoDTO, ModelSpeckleViewInfoRequest } from "@/common/DTO/Model/ModelSpeckleViewInfoDTO";

const service = new Service();

const ModelService = {
    FilterModelByProjectId: async (projectId: string = "",pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: ModelQueryFilter | null) => {
        try {
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            search?.Name && params.append("Name", search.Name);
            search?.CreatedBy && params.append("?.CreatedBy", search.CreatedBy);
            search?.Status && params.append("?.Status", search.Status);

            return await service.get<PagedListContent<Model>>(ApiEndpointCollection.ModelManagement.FilterModelByProjectId.replace("{id}", projectId), params);
        } catch ( err ) {
            throw err;
        }
    },

    GetDirectChildren: async ( parentId: string = "", search: ModelQueryFilter | null) => {
        try {
            const params = new URLSearchParams();
            search?.Name && params.append("Name", search.Name);
            search?.CreatedBy && params.append("CreatedBy", search.CreatedBy);
            search?.Status && params.append("Status", search.Status);
            params.append("Sorting", DEFAULT_SORT_EXPRESSION);

            return await service.get<PagedListContent<Model>>(ApiEndpointCollection.ModelManagement.GetDirectChildren.replace("{id}", parentId), params);
        } catch ( err ) {
            throw err;
        }
    },
    
    GetTreeModelByProjectId: async ( projectId: string, includeUploaded: boolean = true ) => {
        try {
            const params = new URLSearchParams();
            typeof includeUploaded === "boolean" && includeUploaded && params.append("includeUploaded", includeUploaded.toString());
            projectId && params.append("projectId", projectId);
            return await service.get<Model[]>(ApiEndpointCollection.ModelManagement.GetTreeModelByProjectId, params);
        } catch ( err ) {
            throw err;
        }
    },

    Create: async (data: ModelCreateDTO) => {
        try {
            return await service.post<Model>(ApiEndpointCollection.ModelManagement.Create, data);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data: ModelUpdateDTO) => {
        try {
            
            return await service.put<Model>(ApiEndpointCollection.ModelManagement.Update, data);
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            
            return await service.delete<any>(ApiEndpointCollection.ModelManagement.Delete.replace("{id}", id));
        } catch ( err ) {
            throw err;
        }
    },

    Move: async (data: ModelMoveDTO) => {
        try {
            return await service.post<any>(ApiEndpointCollection.ModelManagement.Move, data);
        } catch ( err ) {
            throw err;
        }
    },

    UploadFile: async (data: ModelUploadDTO) => {
        const params = new FormData();
        params.append("ModelId", data.modelId);
        params.append("File", data.file);
        
        try {
            return await service.post<any>(ApiEndpointCollection.ModelManagement.UploadFile, params);
        } catch ( err ) {
            throw err;
        }
    },

    GetSpeckleModelsInfo: async ( data: ModelSpeckleViewInfoRequest ) => {
        try {
            const params = new URLSearchParams();
            params.append("projectId", data.projectId);
            params.append("requestModelsInfo", data.requestModelsInfo);
            return await service.get<ModelSpeckleViewInfoDTO>(ApiEndpointCollection.ModelManagement.GetSpeckleModelsInfo, params);
        } catch ( err ) {
            throw err;
        }
    },

    GetFullSpeckleModelsInfo: async (projectId: string = "") => {
        try {
            return await service.get<ModelSpeckleViewInfoDTO>(ApiEndpointCollection.ModelManagement.GetSpeckleModelsInfo.replace("{projectId}", projectId));
        } catch ( err ) {
            throw err;
        }
    },
}

export default ModelService;