import { InternshipDto } from "../Internship/InternshipDto";
import { SiteUserDto } from "../SiteUser/SiteUserDto";

export interface ApplicationListItemDto {
  id: string;
  createdOn: string;
  siteUserDto: SiteUserDto;
  stepName: string;
  internshipDto: InternshipDto;
}




