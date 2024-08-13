export interface Model {
    id: string;
    name: string;
    level: number;
    description?: string;
    speckleBranchId?: string;
    speckleBranchName?: string;
    type: "FOLDER" | "MODEL";
    projectID: string;
    projectName: string;
    status: string;
    parentId: string | null;
    isUpload?: boolean,
    
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;

    children?: Model[];
    path: number[];
}