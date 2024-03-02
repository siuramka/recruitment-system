import axios from "axios";
import { refresh } from "./AuthService";
import {
  removeUserFromLocalStorage,
  saveTokensToLocalStorage,
} from "../features/AuthSliceActions";
import { AppDispatch, store } from "../app/store";
import { removeUser } from "../features/AuthSlice";

const baseURL = "/api/";

const api = axios.create({
  baseURL,
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;
    const dispatch: AppDispatch = store.dispatch;

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const token = localStorage.getItem("token");
        const refreshToken = localStorage.getItem("refreshToken");
        if (token && refreshToken) {
          const newToken = await refresh({
            accessToken: token,
            refreshToken: refreshToken,
          });

          if (newToken) {
            saveTokensToLocalStorage(
              newToken.accessToken,
              newToken.refreshToken
            );
          } else {
            removeUserFromLocalStorage();
            dispatch(removeUser());
          }

          originalRequest.headers.Authorization = `Bearer ${newToken}`;
        }
        return api(originalRequest);
      } catch (refreshError) {
        throw refreshError;
      }
    }

    return Promise.reject(error);
  }
);

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;
