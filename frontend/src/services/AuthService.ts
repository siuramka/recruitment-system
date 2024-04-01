import { RegisterUser } from "@/interfaces/Auth/RegisterUser";
import api from "./Api";
import { RegisterCompany } from "@/interfaces/Auth/RegisterCompany";

type refreshParams = {
  accessToken: string;
  refreshToken: string;
};

export const refresh = async ({ accessToken, refreshToken }: refreshParams) => {
  try {
    const response = await api.post("auth/refresh", {
      accessToken,
      refreshToken,
    });

    if (response.status === 200) {
      const responseData: LoginResponse = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type authParams = {
  email: string;
  password: string;
};

export const login = async ({ email, password }: authParams) => {
  try {
    const response = await api.post("auth/login", { email, password });

    if (response.status === 200) {
      const responseData: LoginResponse = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const registerUser = async ({
  registerUser,
}: {
  registerUser: RegisterUser;
}) => {
  console.log(registerUser);
  try {
    const response = await api.post("auth/register/user", { ...registerUser });

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};

export const registerCompany = async ({
  registerComapny,
}: {
  registerComapny: RegisterCompany;
}) => {
  try {
    const response = await api.post("auth/register/company", {
      ...registerComapny,
    });

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
