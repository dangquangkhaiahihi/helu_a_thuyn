export interface IssueQueryFilter {
    id: number | string;
    name: string ;
    type: string;
    status?: string;
    description: string;
    createdBy: string;
    createdDate: string; 
    modifiedBy: string;
    modifiedDate: string;
    // projectId:string;
    modelId:string;
        // reporter: string;
    // endDate: string;
    // assignee:string;
    // image: string;
}