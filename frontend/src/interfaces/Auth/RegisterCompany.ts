import { RegisterUser } from "./RegisterUser";

export interface RegisterCompany {
  registerUserDto: RegisterUser;
  name: string;
  location: string;
  email: string;
  phoneNumber: string;
  website: string;
}
