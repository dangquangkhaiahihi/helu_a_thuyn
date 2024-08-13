export interface ModelSpeckleInfo {
    id: string;
    name: string;
    speckleModelId: string;
    speckleVersionInfos: {
        id: number;
        commitId: string;
        objectId: string;
        modifiedDate: string;
    }[];
}

export interface ModelSpeckleViewInfoDTO {
    speckleProjectId: string;
    speckleModelInfos: ModelSpeckleInfo[];
}

export interface ModelSpeckleViewInfoRequest {
    projectId: string;
    requestModelsInfo: string;
}