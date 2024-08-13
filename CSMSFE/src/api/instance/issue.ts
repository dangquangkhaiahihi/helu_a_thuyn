import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { LookUpItem, PagedListContent } from "@/common/DTO/ApiResponse";
import { Issue } from "@/common/DTO/Issue/IssueDTO";
import { IssueCreateUpdateDTO } from "@/common/DTO/Issue/IssueCreateUpdateDTO";
import { IssueQueryFilter } from "@/common/DTO/Issue/IssueQueryFilter";

const service = new Service();

const IssueService = {
    Filter: async (pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: IssueQueryFilter | null) => {
        try {
            console.log("pageIndex", pageIndex);
            console.log("pageSize", pageSize);
            console.log("sortExpression", sortExpression);
            console.log("search", search);
            
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            search?.name && params.append("Name", search.name);
            search?.type && params.append("Type", search.type);
            search?.status && params.append("Status", search.status);
            search?.modelId && params.append("ModelId", search.modelId)
            search?.description && params.append("Desciption", search.description);
            search?.createdBy && params.append("createdBy", search.createdBy);
            search?.createdDate && params.append("createdDate", search.createdDate);
            search?.modifiedBy && params.append("modifiedBy", search.modifiedBy);
         

            return await service.get<PagedListContent<Issue>>(ApiEndpointCollection.IssueManagement.Filter, params);
        } catch ( err ) {
            throw err;
        }
    },

    Create: async (data: IssueCreateUpdateDTO) => {
        try {
            return await service.post<Issue>(ApiEndpointCollection.IssueManagement.Create, data);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data: IssueCreateUpdateDTO) => {
        try {
            
            return await service.put<Issue>(ApiEndpointCollection.IssueManagement.Update, data);
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            
            return await service.delete<any>(ApiEndpointCollection.IssueManagement.Delete.replace("{id}", id));
        } catch ( err ) {
            throw err;
        }
    },
    GetLookUpTypeIssue: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.IssueManagement.GetLookUpTypeIssue, params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default IssueService;