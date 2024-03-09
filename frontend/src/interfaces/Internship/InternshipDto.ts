import { CompanyDto } from "../Company/CompanyDto";

export interface InternshipDto {
  id: string;
  companyDto: CompanyDto;
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
