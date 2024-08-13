export interface IssueCreateUpdateDTO {
    id: number | string;
    name: string ;
    type: string;
    status: string;
    description: string;
    modelId: string;
    // createBy: string;
    // createDate: string;
    // modifiedBy:string;
    // modifiedDate: string;
}