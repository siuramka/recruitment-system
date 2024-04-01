import { InterviewDto } from "@/interfaces/Interview/InterviewDto";
import api from "./Api";
import { InterviewCreateDto } from "@/interfaces/Interview/InterviewCreateDto";
import { toast } from "@/components/ui/use-toast";

type createInterviewParams = {
  applicationId: string;
  interviewCreateDto: InterviewCreateDto;
};

export const createInterview = async ({
  applicationId,
  interviewCreateDto,
}: createInterviewParams) => {
  try {
    const response = await api.post(`applications/${applicationId}/interview`, {
      ...interviewCreateDto,
    });

    if (response.status === 200) {
      const responseData: InterviewDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    toast({ title: "Interview already created!" });
    return null;
  }
};

type getInterviewParams = {
  applicationId: string;
};

export const getInterview = async ({ applicationId }: getInterviewParams) => {
  try {
    const response = await api.get(`applications/${applicationId}/interview`);

    if (response.status === 200) {
      const responseData: InterviewDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
