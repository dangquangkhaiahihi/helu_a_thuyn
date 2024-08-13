export interface RoleProject {
    id: string;
    name: string;
    code: string;
    isDefault: string;

    projectId: string;
    projectName: string;

    actionIds: string[];

    createdDate: string;
    createdBy: string;
    modifiedBy: string;
    modifiedDate: string;
}

export interface RoleUserProject {
    id: string;
    name: string;
}