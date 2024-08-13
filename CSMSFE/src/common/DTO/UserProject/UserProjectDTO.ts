export interface UserProject {
    userId: string;
    userName: string;
    fullName: string;
    email: string;
    phone: string;

    image: string | null;
    roles: RoleUserProject[];

    isPending: boolean;
}

export interface RoleUserProject {
    id: string;
    name: string;
    code: string;
}