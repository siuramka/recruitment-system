import { ApplicationCreateResponse } from "@/interfaces/Application/ApplicationCreateResponse";
import api from "./Api";
import { toast } from "@/components/ui/use-toast";
import { ApplicationDto } from "@/interfaces/Application/ApplicationDto";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";

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
      title: `Error`,
      variant: "destructive",
    });
    return undefined;
  }
};

type getApplicationParams = {
  applicationId: string;
};

export const getApplication = async ({
  applicationId,
}: getApplicationParams) => {
  try {
    const response = await api.get(`applications/${applicationId}`);

    if (response.status === 200) {
      const responseData: ApplicationListItemDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type getAllInternshipApplicationsParams = {
  internshipId: string;
};

export const getAllInternshipApplications = async ({
  internshipId,
}: getAllInternshipApplicationsParams) => {
  try {
    const response = await api.get(`internships/${internshipId}/applications`);

    if (response.status === 200) {
      const responseData: ApplicationListItemDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getAllUserApplications = async () => {
  try {
    const response = await api.get(`/applications`);

    if (response.status === 200) {
      const responseData: ApplicationListItemDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getAllDecisionApplications = async () => {
  try {
    const response = await api.get(`/applications/decisions`);

    if (response.status === 200) {
      const responseData: ApplicationListItemDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const applicationOffer = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.post(
      `applications/${applicationId}/decision/offer`
    );

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};

export const applicationReject = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.post(
      `applications/${applicationId}/decision/reject`
    );

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
