import { DecisionDto } from "@/interfaces/Decision/DecisionDto";
import api from "./Api";
import { DecisionCreateDto } from "@/interfaces/Decision/DecisionCreateDto";
import { DecisionScoreDto } from "@/interfaces/Decision/DecisionScoreDto";

export const getDecision = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.get(`applications/${applicationId}/decision`);

    if (response.status === 200) {
      const responseData: DecisionScoreDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const createDecision = async ({
  applicationId,
  decisionCreateDto,
}: {
  applicationId: string;
  decisionCreateDto: DecisionCreateDto;
}) => {
  try {
    const response = await api.post(
      `applications/${applicationId}/decision`,
      decisionCreateDto
    );

    if (response.status === 201) {
      const responseData: DecisionDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
