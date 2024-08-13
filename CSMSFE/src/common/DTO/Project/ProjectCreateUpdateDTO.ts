export interface ProjectCreateUpdateDTO {
    id?: number | string;
    name: string;
    code: string;
    description?: string;
    typeProjectId: string;

    communeId: string;
    districtId: string;
    provinceId: string;

    status: string;
}