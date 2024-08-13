export interface Issue {
    id: number | string;
    name: string ;
    type: string;
    status: string;
    description: string;
    createdDate: string;
    createdBy: string;
    modifiedBy: string;
    modifiedDate: string;
    // reporter: string;
    // endDate: string;
    // assignee:string;
    // image: string;
}