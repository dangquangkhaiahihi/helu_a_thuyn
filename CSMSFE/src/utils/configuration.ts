import Cookies from "universal-cookie";
import { DecodeToken } from "./jwt-token-helper";

let configuration = new Map();

export const DefaultCordinate = "DefaultCordinate";

export const ApiServerKey = {
  APP_API_ROOT: "API_ROOT",
};

export const CookiesKeysCollection = {
  TOKEN_KEY : 'TOKEN',
  REFRESH_TOKEN_KEY : 'REFRESH_TOKEN',
  RETURN_URL: 'RETURN_URL',
  DEVICE_ID : 'DEVICE_ID',
  FCM_TOKEN : 'FCM_TOKEN', //Firebase Cloud Messaging Token
}

export const REGEX_PASSWORD = /^.*(?=.{8,})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/;
export const REGEX_LINK_YOUTUBE = /^.*(youtube\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;

export const TokenPrefix = "Bearer";

//--- Cookies
const cookies = new Cookies();
const dateExpires = new Date();
dateExpires.setTime(dateExpires.getTime() + 720 * 60 * 60 * 1000);

// TODO : Định nghĩa DTO mapper cho response auth từ admin
export const setCookiesUser = (res: any) => {
  const { token, refreshTokens, refreshToken } =
    res?.data?.content || res?.content;
  const expireTime = new Date();
  expireTime.setTime(expireTime.getTime() + 24 * 60 * 60 * 1000);
  const expiredTime = new Date();
  expiredTime.setTime(expiredTime.getTime() + 10 * 60 * 1000);

  const options = {
    path: "/",
    domain: import.meta.env.VITE_BASE_DOMAIN,
    expires: expireTime,
  };
  cookies.set("token", token, options);
  cookies.set("refreshToken", refreshTokens ?? refreshToken, options);
  // cookies.set("expiredTime", expiredTime);
};

export const setIsShowDialogConfirmRefresh = (
  isShow: boolean,
  options = { path: "/", domain: import.meta.env.VITE_BASE_DOMAIN }
) => {
  cookies.set("isShowDialog", isShow, options);
};

export const setLockScreen = (
  isLockScreen: boolean,
  options = { path: "/", domain: import.meta.env.VITE_BASE_DOMAIN }
) => {
  cookies.set("isLockScreen", isLockScreen, options);
};

export function setCookies(
  name: string,
  value: any,
  options = { path: "/", domain: import.meta.env.VITE_BASE_DOMAIN, expires: dateExpires }
) {
  cookies.set(name, value, options);
}

export function getCookies(name: string) {
  return cookies.get(name);
}

export function removeCookies(
  name: string,
  options = { path: "/", domain: import.meta.env.VITE_BASE_DOMAIN }
) {
  cookies.remove(name, options);
}

export function removeListCookies(nameList: [string]) {
  if (nameList instanceof Array) {
    nameList.map((name) => {
      cookies.remove(name, { path: "/", domain: import.meta.env.VITE_BASE_DOMAIN });
      cookies.remove(name, { path: "/", domain: window.location.host });
    });
  }
}

export function setConfiguration(name: string, value: any) {
  configuration = configuration.set(name, value);
}

export function getConfiguration(key: string) {
  if (!configuration.has(key)) {
    throw new Error("Undefined configuration key: " + key);
  }

  return configuration.get(key);
}

export function onRemoveTokens(tokens: []) {
  return Promise.resolve(onRemoveTokenKeys(tokens));
}

function onRemoveTokenKeys(tokens: []) {
  if (tokens && tokens.length > 0) {
    tokens.map((t) => localStorage.removeItem(t));
  }
}

export function getUserInfo() {
  let userInfoToken = getCookies(CookiesKeysCollection.TOKEN_KEY);
  let userInfo = DecodeToken(userInfoToken);
    if (userInfo) {
    return userInfo;
  }
  
  return null;
}

export const NotificationMessageType = {
  Success: "success",
  Warning: "warning",
  Error: "error",
};

function descendingComparator(a: any, b: any, orderBy: string) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

export function getComparator(order: string, orderBy: string) {
  return order === "desc"
    ? (a: any, b: any) => descendingComparator(a, b, orderBy)
    : (a: any, b: any) => -descendingComparator(a, b, orderBy);
}

export function stableSort(array: [any], comparator: Function) {
  const stabilizedThis = array.map((el, index) => [el, index]);
  stabilizedThis.sort((a, b) => {
    const order = comparator(a[0], b[0]);
    if (order !== 0) return order;
    return a[1] - b[1];
  });
  return stabilizedThis.map((el) => el[0]);
}

export function changeAlias(alias: string) {
  var str = alias;
  str = str.toLowerCase();
  str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
  str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
  str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
  str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
  str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
  str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
  str = str.replace(/đ/g, "d");
  str = str.replace(
    /~|`|!|@|#|\$|%|\^|&|\*|\(|\)|\+|=|{|\[|}|]|\\|\||:|;|'|"|,|<|>|\.|\?|\/|\\/g,
    ""
  );
  str = str.replace(/__|\\/g, "_");
  str = str.replace(/--|\\/g, "-");
  str = str.replace(/ + /g, "");
  str = str.trim();
  str = str.toUpperCase();
  return str;
}

export function compareValues(key: string, order = "asc") {
  return function innerSort(a: any, b: any) {
    if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
      return 0;
    }

    const varA = typeof a[key] === "string" ? a[key].toUpperCase() : a[key];
    const varB = typeof b[key] === "string" ? b[key].toUpperCase() : b[key];

    let comparison = 0;
    if (varA > varB) {
      comparison = 1;
    } else if (varA < varB) {
      comparison = -1;
    }
    return order === "desc" ? comparison * -1 : comparison;
  };
}

export function consoleLogTimeNow(message = "") {
  message && console.log(message);
  const dateTimeNow = new Date();
  console.log(
    dateTimeNow.getHours() * 60 * 60 +
      dateTimeNow.getMinutes() * 60 +
      dateTimeNow.getSeconds()
  );
}

export const MaxSizeImageUpload = "1048576";

export const mimeTypes = {
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.png': 'image/png',
  '.gif': 'image/gif',
  '.tiff': 'image/tiff',
  '.docx': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
  '.doc': 'application/msword',
  '.xls': 'application/vnd.ms-excel',
  '.xlsx': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
  '.pdf': 'application/pdf',
  '.txt': 'text/plain'
};