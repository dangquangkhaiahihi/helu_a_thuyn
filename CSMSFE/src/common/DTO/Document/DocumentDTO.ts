import { NodeData } from "react-folder-tree";

export interface DocumentDTO extends NodeData {
    id?: number;
    parentId?: number;
    isFile?: boolean;
    urlPath?: string;
    size?: number;
    icon?: string;
    projectId?: string;
    fileExtension?: string;
    [key: string]: any;
}