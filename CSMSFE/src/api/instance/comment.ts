import Service from "../api-service";
import {Comment} from '../../common/DTO/Comment/CommentList'
import ApiEndpointCollection from "../api-endpoint-collection";
import CommentCreateDTO from "@/common/DTO/Comment/CommentCreateDTO";

const service = new Service();

const CommentService = {
    List: async (id:string) => {
        try {
            return await service.get<Comment[]>(ApiEndpointCollection.Comment.List.replace("{id}", id));
        } catch ( err ) {
            throw err;
        }
    },

    Create: async (data:CommentCreateDTO) => {
        try {
            return await service.post<Comment>(ApiEndpointCollection.Comment.Post,data);
        } catch ( err ) {
            throw err;
        }
    },

    Update: async (data:Comment) => {
        try {
            return await service.put<Comment>(ApiEndpointCollection.Comment.Update,data);
            
        } catch ( err ) {
            throw err;
        }
    },

    Delete: async (id: string) => {
        try {
            return await service.delete<any>(ApiEndpointCollection.Comment.Delete.replace("{id}", id));
            
        } catch ( err ) {
            throw err;
        }
    },
}

export default CommentService;