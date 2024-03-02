import { ApplicationCreateResponse } from "@/interfaces/Application/ApplicationCreateResponse";
import api from "./Api";
import { toast } from "@/components/ui/use-toast";
import { ApplicationDto } from "@/interfaces/Application/ApplicationDto";

type createApplicationParams = {
  internshipId: string;
};

export const createApplication = async ({
  internshipId,
}: createApplicationParams): Promise<ApplicationCreateResponse | undefined> => {
  try {
    const response = await api.post(`internships/${internshipId}/applications`);

    if (response.status === 201) {
      const responseData: ApplicationCreateResponse = response.data;
      return responseData;
    }

    return undefined;
  } catch (e) {
    toast({
      title: `${e.response.data}`,
      variant: "destructive",
    });
    return undefined;
  }
};

type getApplicationParams = {
  internshipId: string;
};

export const getApplication = async ({
  internshipId,
}: getApplicationParams) => {
  try {
    const response = await api.get(`internships/${internshipId}/application`);

    if (response.status === 200) {
      const responseData: ApplicationDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};