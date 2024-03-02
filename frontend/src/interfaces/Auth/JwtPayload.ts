export type JwtPayload = {
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  userId: string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
  exp: number;
};
