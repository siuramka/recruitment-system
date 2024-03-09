import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";
import api from "./Api";

type getApplicationStepsParams = {
  internshipId: string;
};

export const getApplicationSteps = async ({
  internshipId,
}: getApplicationStepsParams) => {
  try {
    const response = await api.get(
      `internships/${internshipId}/application/steps`
    );

    if (response.status === 200) {
      const responseData: ApplicationStepDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type getApplicationByIdParams = {
  internshipId: string;
  applicationId: string;
};

export const getApplicationStepsById = async ({
  internshipId,
  applicationId,
}: getApplicationByIdParams) => {
  try {
    const response = await api.get(
      `internships/${internshipId}/application/${applicationId}/steps`
    );

    if (response.status === 200) {
      const responseData: ApplicationStepDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type updateApplicationStepParams = {
  internshipId: string;
  applicationId: string;
  stepType: string;
};
export const updateApplicationStep = async ({
  internshipId,
  applicationId,
  stepType,
}: updateApplicationStepParams) => {
  try {
    const response = await api.put(
      `internships/${internshipId}/application/${applicationId}/steps`,
      { stepType }
    );

    if (response.status === 200) {
      const responseData = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
