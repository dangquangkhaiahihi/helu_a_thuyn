export interface User {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    roleName: string | null;
    roleId: string | null;
    dateOfBirth: string;
    phoneNumber: string;
    address: string;
    gender: boolean;
    description: string;
    createdDate: string;
    createdBy: string;
    modifiedDate: string;
    modifiedBy: string;
    status: boolean;
    userType: string;
    avatar: string | null;
    screens: any[]; // Adjust type if necessary
    lastDateLogin: string | null;
    deviceStatus: string | null;
}
