import { InternshipCreateStepDto } from "../Step/InternshipCreateStepDto";
import { SettingCreateDto } from "../Setting/SettingCreate";

export interface InternshipCreateDto {
  name: string;
  contactEmail: string;
  address: string;
  description: string;
  requirements: string;
  isPaid: boolean;
  isRemote: boolean;
  slotsAvailable: number;
  skills: string;
  internshipStepDtos: InternshipCreateStepDto[];
  settingCreateDto: SettingCreateDto;
}
