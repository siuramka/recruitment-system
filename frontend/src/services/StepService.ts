import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";
import api from "./Api";
import { toast } from "@/components/ui/use-toast";
import { StepDto } from "@/interfaces/Step/StepDto";

export const getAvailableSteps = async () => {
  try {
    const response = await api.get(`steps`);

    if (response.status === 200) {
      const responseData: StepDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type getApplicationStepsParams = {
  applicationId: string;
};

export const getApplicationSteps = async ({
  applicationId,
}: getApplicationStepsParams) => {
  try {
    const response = await api.get(`application/${applicationId}/steps`);

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
      toast({
        title: "Successfully updated application step!",
      });
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
