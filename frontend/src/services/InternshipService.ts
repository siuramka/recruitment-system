import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import api from "./Api";
import { InternshipCreateStepDto } from "../interfaces/Step/InternshipCreateStepDto";
import { InternshipCreateDto } from "@/interfaces/Internship/InternshipCreateDto";

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

export const createInternship = async (
  internshipCreateDto: InternshipCreateDto
) => {
  try {
    const response = await api.post("internships", {
      ...internshipCreateDto,
    });

    if (response.status === 201) {
      const responseData: InternshipDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
