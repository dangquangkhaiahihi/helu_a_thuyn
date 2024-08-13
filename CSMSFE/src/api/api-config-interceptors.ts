import axios, { AxiosInstance, AxiosResponse, InternalAxiosRequestConfig, AxiosHeaders } from "axios";
import {
  TokenPrefix,
  getCookies,
  removeCookies,
  setCookiesUser,
  setConfiguration,
  getConfiguration,
  ApiServerKey,
  CookiesKeysCollection,
} from "@utils/configuration";
import { GenerateDeviceId } from "@/common/tools";
import ApiEndpointCollection from "./api-endpoint-collection";
import { UrlRouteCollection } from "@/common/url-route-collection";

const DomainAdminSide = import.meta.env.VITE_BASE_DOMAIN;

setConfiguration(
  ApiServerKey.APP_API_ROOT,
  import.meta.env.VITE_API_ENDPOINT
);

// Extend AxiosRequestConfig to include _retry property
interface ConfigWithRetry extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

class AxiosInterceptor {
  private axiosInstance: AxiosInstance;
  private apiRoot: string;
  private baseDomain: string;
  private isHandlerEnabled: boolean = true;

  constructor() {
    this.apiRoot = getConfiguration(ApiServerKey.APP_API_ROOT);
    this.baseDomain = DomainAdminSide;
    this.isHandlerEnabled = true;
    this.axiosInstance = axios.create({
      baseURL: this.apiRoot,
      responseType: "json",
    });

    this.initializeInterceptors();
  }

  private initializeInterceptors() {
    this.axiosInstance.interceptors.request.use(
      (request: ConfigWithRetry) => this.requestHandler(request),
      (error) => this.errorHandler(error)
    );

    this.axiosInstance.interceptors.response.use(
      (response: AxiosResponse) => this.successHandler(response),
      async (error) => await this.errorResponseHandler(error)
    );
  }

  private requestHandler(request: ConfigWithRetry) {
    if (this.isHandlerEnabled) {
      if (!request.headers) {
        request.headers = new AxiosHeaders(); // Initialize headers as an instance of AxiosHeaders
      }
      if (request.data instanceof FormData) {
        request.headers.set("Content-Type", "multipart/form-data");
      } else {
        request.headers.set("Content-Type", "application/json; charset=utf-8");
      }
      request.headers.set("Accept", "application/json, text/javascript, */*; q=0.01");
      request.headers.set("Access-Control-Allow-Origin", "*");

      const token = getCookies(CookiesKeysCollection.TOKEN_KEY);
      if (token) {
        request.headers.set("Authorization", `${TokenPrefix} ${token}`);
      }
    }

    return request;
  }

  private successHandler(response: AxiosResponse) {
    // Do something with the successful response if needed
    return response;
  }

  private errorHandler(error: any) {
    // Handle request error if needed
    return Promise.reject(error);
  }

  private async errorResponseHandler(error: any) {
    if (error.response && error.response.status === 401) {
      const config = error.config as ConfigWithRetry; // Cast to include _retry
      if (!config._retry) {
        config._retry = true;
        try {
          await this.refreshToken();
          config.headers.set("Authorization", "Bearer " + getCookies(CookiesKeysCollection.TOKEN_KEY));
          return this.axiosInstance(config);
        } catch (err) {
          return Promise.reject(err);
        }
      }
    }
    return Promise.reject({
      ...(error.response
        ? error.response.data
        : {
            errorType: "Unhandled Exception",
            errorMessage: error,
          }),
    });
  }

  public async refreshToken() {
    try {
      const uUid = GenerateDeviceId();
      const response = await axios
        .create({
          baseURL: this.apiRoot,
        })
        .post(ApiEndpointCollection.Account.RefreshToken , {
          returnUrl: this.baseDomain,
          refreshToken: getCookies(CookiesKeysCollection.REFRESH_TOKEN_KEY),
          uUid: uUid,
        });
      removeCookies("isShowDialog");
      setCookiesUser(response);
    } catch (error) {
      removeCookies("isShowDialog");
      removeCookies("isLockScreen");
      removeCookies(CookiesKeysCollection.TOKEN_KEY);
      removeCookies(CookiesKeysCollection.REFRESH_TOKEN_KEY);
      removeCookies(CookiesKeysCollection.RETURN_URL);
      window.location.replace(this.baseDomain + UrlRouteCollection.Login);
    }
  }

  public getInstance(): AxiosInstance {
    return this.axiosInstance;
  }
}

export default new AxiosInterceptor().getInstance();
