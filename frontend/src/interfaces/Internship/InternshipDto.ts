import { CompanyDto } from "../Company/CompanyDto";
import { SettingDto } from "../Setting/SettingDto";

export interface InternshipDto {
  id: string;
  companyDto: CompanyDto;
  settingDto: SettingDto;
  name: string;
  contactEmail: string;
  address: string;
  description: string;
  requirements: string;
  isPaid: boolean;
  isRemote: boolean;
  createdAt: Date;
  startDate: Date;
  endDate: Date;
  slotsAvailable: number;
  takenSlots: number;
  skills: string;
}
