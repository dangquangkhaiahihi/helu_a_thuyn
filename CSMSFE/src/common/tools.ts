import { CookiesKeysCollection } from "@/utils/configuration";
import { closeSnackbar, enqueueSnackbar } from "notistack";
import Cookies from "universal-cookie";

export function ConvertStringToNumberArray(stringData: string) {
    const result: number[] = [];
    stringData.split(',').map((dataNumber: string) => result.push(Number(dataNumber)))
    return result
}

export function GenerateDeviceId() {
    const cookies = new Cookies();
    var uid = cookies.get(CookiesKeysCollection.DEVICE_ID);
    if(uid == null)
    {
        const expireTime = new Date();
        const domainName = import.meta.env.VITE_BASE_DOMAIN;
        expireTime.setTime(expireTime.getTime() + 30 *24 * 60 * 60 * 1000);

        let options = { path: "/", domain: domainName, expires: expireTime };
        cookies.set(CookiesKeysCollection.DEVICE_ID, uniqueID(), options);
        return uniqueID();
    }
    else {
        return uid;
    }

    function uniqueID () {
        function chr4 () {
          return Math.random().toString(16).slice(-4);
        }

        return chr4() + chr4() +
          '-' + chr4() +
          '-' + chr4() +
          '-' + chr4() +
          '-' + chr4() + chr4() + chr4();
    }
}

export const isValidPhoneNumber = (value: string) => {
    const phoneNumberPattern = /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/;
    return phoneNumberPattern.test(value);
}

export const isValidEmail = (value: string) => {
    const emailPattern = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
    return emailPattern.test(value);
}

export const viewAlert = (message: string, severity: string) => {
    const snackbarId = enqueueSnackbar(message, {
        variant: 'muiSnackbar',
        alertProps: {
            severity,
            variant: 'filled',
        },
    });
    return snackbarId;
}

export const closeAlert = (snackbarId: string | number) => {
    closeSnackbar(snackbarId);
};