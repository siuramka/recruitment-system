import { jwtDecode } from "jwt-decode";
import { User } from "./AuthSlice";
import { JwtPayload } from "@/interfaces/Auth/JwtPayload";

export const getUserFromTokens = (token: string, refreshToken: string) => {
  const decoded = jwtDecode<JwtPayload>(token);
  const user: User = {
    id: decoded.userId,
    role: decoded[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ],
    email:
      decoded[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
      ],
    exp: decoded.exp,
  };
  return user;
};

export const removeUserFromLocalStorage = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");
};

export const saveTokensToLocalStorage = (
  accessToken: string,
  refreshToken: string
) => {
  localStorage.setItem("token", accessToken);
  localStorage.setItem("refreshToken", refreshToken);
};

export const getUserFromLocalStorage = () => {
  const token = localStorage.getItem("token");
  const refreshToken = localStorage.getItem("refreshToken");
  if (!token || !refreshToken) return null;
  return getUserFromTokens(token, refreshToken);
};
