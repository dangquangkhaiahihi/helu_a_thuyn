import {
  setConfiguration,
  ApiServerKey,
} from "@utils/configuration";
import AxiosInterceptor from "./api-config-interceptors";
import { ApiResponse } from "@/common/DTO/ApiResponse";

setConfiguration(
  ApiServerKey.APP_API_ROOT,
  import.meta.env.VITE_API_ENDPOINT
);

export default class Service {
  constructor() {}

  /**
   * Get Http Request
   * @param {any} action
   */
  async get<T>(action: string, params?: any): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.get(params ? action + "?" + params : action, {
        method: "GET",
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }
  /**
   * Post Http Request
   * @param {any} action
   * @param {any} params
   */
  postParams<T>(action: string, params: any, body: any): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.post(params ? action + "?" + params : action, {
        method: "POST",
        data: body,
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }

  /**
   * Get Http Request
   * @param {any} action
   */
  getBinary<T>(action: string, params?: any): Promise<T> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.get(params ? action + "?" + params : action, {
        method: "GET",
        responseType: "blob",
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }
  /**
   * Get Http Request
   * @param {any} action
   */
  postBinary<T>(action: string, params: any, body: any, callback: Function = () => {}): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.post(params ? action + "?" + params : action, {
        data: body,
        method: "POST",
        responseType: "blob",
        onDownloadProgress: (progressEvent: ProgressEvent) => {
          const { loaded, total } = progressEvent;
          let percentCompleted = Math.floor((loaded * 100) / total);
          callback(percentCompleted);
        },
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }

  /**
   * Post Http Request
   * @param {any} action
   * @param {any} params
   */
  post<T>(action: string, params: any): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.post(action, params)
        .then((response: any) => {
          console.log("AxiosInterceptor.post", response);
          
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }

  /**
   * Put Http Request
   * @param {any} action
   * @param {any} params
   */
  put<T>(action: string, params: any): Promise<ApiResponse<T>> {
      return new Promise((resolve, reject) => {
      AxiosInterceptor.put(action, params)
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }
  /**
   *Delete Http Request
   * @param {any} action
   * @param {any} params
   */
  delete<T>(action: string, params?: any): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.delete(params ? action + "?" + params : action, {
        method: "DELETE",
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }
  deleteMany<T>(action: string, params?: any): Promise<ApiResponse<T>> {
    return new Promise((resolve, reject) => {
      AxiosInterceptor.delete(action, {
        method: "DELETE",
        data : params
      })
        .then((response) => {
          if (response.data) {
            resolve(response.data);
          } else {
            reject(response);
          }
        })
        .catch((error) => {
          if (
            error.response &&
            error.response.data &&
            error.response.data.error
          ) {
            console.error("REST request error!", error.response.data.error);
            reject(error.response.data.error);
          } else reject(error);
        });
    });
  }

}
