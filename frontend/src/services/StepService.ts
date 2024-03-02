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
