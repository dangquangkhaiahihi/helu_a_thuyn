export interface UserCreateUpdateDTO {
  id?: string; // Optional for create operations
  fullName: string;
  userName: string;
  email: string;
  roleId: string;
  dateOfBirth: string;
  phoneNumber: string;
  address: string;
  gender: boolean;
  description: string;
  avatar?: File;
}