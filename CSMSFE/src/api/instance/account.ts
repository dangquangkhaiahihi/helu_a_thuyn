import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { UserInfo } from "@/common/DTO/Account/UserInfoDTO";

const service = new Service();

interface ILoginInput {
    email: string;
    password: string;
    rememberMe: boolean;
    returnUrl: string;
    uUid: string;
}

interface ILoginOutput {
    token: string;
    returnUrl: string;
    refreshToken: string;
}

const AccountService = {
    GetMyInfo: async () => {
        try {
            return await service.get<UserInfo>(ApiEndpointCollection.Account.GetMyInfo);
        } catch ( err ) {
            throw err;
        }
    },
    Login: async (data: ILoginInput) => {
        try {
            return await service.post<ILoginOutput>(ApiEndpointCollection.Account.Login, data);
        } catch ( err ) {
            throw err;
        }
    },
}

export default AccountService;