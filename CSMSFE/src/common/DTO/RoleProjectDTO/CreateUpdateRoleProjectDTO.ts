export interface CreateUpdateRoleProjectDTO {
    id: string;
    name: string;
    code: string,
    projectId: string,
    actionIds: string[]
}