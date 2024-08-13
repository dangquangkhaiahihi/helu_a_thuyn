export interface Project {
    id: string;
    name: string;
    code: string;
    description: string;

    communeId: string;
    communeName: string;

    districtId: string;
    districtName: string;

    provinceId: string;
    provinceName: string;

    status: string;
    typeProjectId: string;
    typeProjectName: string;
    
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
}