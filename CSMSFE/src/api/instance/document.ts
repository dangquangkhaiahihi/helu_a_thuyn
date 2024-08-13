import { NodeData } from "react-folder-tree";
import ApiEndpointCollection from "../api-endpoint-collection";
import Service from "../api-service";
import DocumentUpdateDTO from "@/common/DTO/Document/DocumentUpdateDTO";
import { DocumentDTO } from "@/common/DTO/Document/DocumentDTO";
import { DocumentFolderCreateDTO } from "@/common/DTO/Document/DocumentFolderCreateDTO";
import { LookUpItem } from "@/common/DTO/ApiResponse";
import { DocumentUploadFileDTO } from "@/common/DTO/Document/DocumentUploadFileDTO";

const service = new Service();

const DocumentService = {
    GetDocumentTreeLevel1ByProjectId: async ( projectId: string ) => {
        try {
            const params = new URLSearchParams();
            params.append("projectId", projectId)
            return await service.get<DocumentDTO>(ApiEndpointCollection.Document.GetInit, params);
        } catch ( err ) {
            throw err;
        }
    },

    GetDocumentsByParentId: async ( parentId: number ) => {
        try {
            const params = new URLSearchParams();
            params.append("id", parentId.toString())
            return await service.get<DocumentDTO[]>(ApiEndpointCollection.Document.GetByParentId, params);
        } catch ( err ) {
            throw err;
        }
    },

    CreateFolder: async (data: DocumentFolderCreateDTO) => {
        try {
            const param = new FormData();
            param.append("isFile", false.toString());
            param.append("parentId", data.parentId.toString());
            param.append("projectId", data.projectId);

            param.append("name", data.name);
            
            return await service.post<DocumentDTO>(ApiEndpointCollection.Document.Create, param);
        } catch ( err ) {
            throw err;
        }
    },

    UploadFile: async (data: DocumentUploadFileDTO) => {
        try {
            const param = new FormData();
            param.append("isFile", true.toString());
            param.append("parentId", data.parentId.toString());
            param.append("projectId", data.projectId);

            param.append("file", data.file);
            
            return await service.post<DocumentDTO>(ApiEndpointCollection.Document.Create, param);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data: DocumentUpdateDTO) => {
        try {
            return await service.put<NodeData>(ApiEndpointCollection.Document.Update,data);
            
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: number[]) => {
        try {
            return await service.deleteMany<any>(ApiEndpointCollection.Document.Remove, id);
            
        } catch ( err ) {
            throw err;
        }
    },
    
    Download: async (id: string) => {
        try {
            return (await service.getBinary<BlobPart>(ApiEndpointCollection.Document.Download.replace("{id}", id)));
            
        } catch ( err ) {
            throw err;
        }
    },

    GetLookupFileExtensions: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.Document.GetLookUpFileExtension, params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default DocumentService;