import { InternshipDto } from "../Internship/InternshipDto";
import { SiteUserDto } from "../SiteUser/SiteUserDto";

export interface ApplicationListItemDto {
  id: string;
  createdOn: string;
  companyScore: number;
  siteUserDto: SiteUserDto;
  stepName: string;
  internshipDto: InternshipDto;
}
