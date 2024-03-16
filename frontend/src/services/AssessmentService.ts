import { AssessmentDto } from "@/interfaces/Assessment/AssessmentDto";
import api from "./Api";
import { AssessmentCreateDto } from "../interfaces/Assessment/AssessmentCreateDto";

export const createAssessment = async ({
  applicationId,
  assessmentCreateDto,
}: {
  applicationId: string;
  assessmentCreateDto: AssessmentCreateDto;
}) => {
  try {
    const response = await api.post(
      `applications/${applicationId}/assessment`,
      assessmentCreateDto
    );

    if (response.status === 200) {
      const responseData: AssessmentDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getAssessment = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.get(`applications/${applicationId}/assessment`);

    if (response.status === 200) {
      const responseData: AssessmentDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
