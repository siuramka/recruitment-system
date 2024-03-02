import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import api from "./Api";

export const getAllInternships = async () => {
  try {
    const response = await api.get("internships");

    if (response.status === 200) {
      const responseData: InternshipDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type getInternshipParams = {
  internshipId: string;
};

export const getInternship = async ({ internshipId }: getInternshipParams) => {
  try {
    const response = await api.get(`internships/${internshipId}`);

    if (response.status === 200) {
      const responseData: InternshipDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
