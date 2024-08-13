import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import { SomeTableQueryFilter } from "@/common/DTO/SomeTable/SomeTableQueryFilter";
import { SomeTable } from "@/common/DTO/SomeTable/SomeTableDTO";
import { SomeTableCreateUpdateDTO } from "@/common/DTO/SomeTable/SomeTableCreateUpdateDTO";
import { PagedListContent } from "@/common/DTO/ApiResponse";

const service = new Service();

const SomeTableService = {
    Filter: async (pageIndex = DEFAULT_PAGE_INDEX, pageSize = DEFAULT_PAGE_SIZE, sortExpression = DEFAULT_SORT_EXPRESSION, search: SomeTableQueryFilter | null) => {
        try {
            console.log("pageIndex", pageIndex);
            console.log("pageSize", pageSize);
            console.log("sortExpression", sortExpression);
            console.log("search", search);
            
            const params = new URLSearchParams();
            params.append("PageIndex", pageIndex.toString());
            params.append("PageSize", pageSize.toString());
            params.append("Sorting", sortExpression);

            search?.NormalText && params.append("NormalText", search.NormalText);
            search?.PhoneNumber && params.append("PhoneNumber", search.PhoneNumber);
            search?.Email && params.append("Email", search.Email);
            search?.StartDate && params.append("StartDate", search.StartDate);
            search?.EndDate && params.append("EndDate", search.EndDate);
            search?.Status && params.append("Status", search.Status);
            search?.Type && params.append("Type", search.Type);

            return await service.get<PagedListContent<SomeTable>>(ApiEndpointCollection.SomeTableManagement.Filter, params);
        } catch ( err ) {
            throw err;
        }
    },

    Create: async (data: SomeTableCreateUpdateDTO) => {
        try {
            return await service.post<any>(ApiEndpointCollection.SomeTableManagement.Create, data);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data: SomeTableCreateUpdateDTO) => {
        try {
            
            return await service.put<SomeTable>(ApiEndpointCollection.SomeTableManagement.Update, data);
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            
            return await service.delete<any>(ApiEndpointCollection.SomeTableManagement.Delete.replace("{id}", id));
        } catch ( err ) {
            throw err;
        }
    },
}

export default SomeTableService;