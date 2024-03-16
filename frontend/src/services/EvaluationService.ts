import { EvaluationCreateDto } from "@/interfaces/Evaluation/EvaluationCreateDto";
import api from "./Api";

type getScreeningEvaluationProps = {
  screeningId: string;
};

export const getScreeningEvaluation = async ({
  screeningId,
}: getScreeningEvaluationProps) => {
  try {
    const response = await api.get(`screening/${screeningId}/evaluation`);

    if (response.status === 200) {
      const responseData: EvaluationDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const createScreeningCompanyEvaluation = async ({
  screeningId,
  evaluation,
}: {
  screeningId: string;
  evaluation: EvaluationCreateDto;
}) => {
  try {
    const response = await api.post(
      `screening/${screeningId}/evaluation`,
      evaluation
    );

    if (response.status === 200) {
      const responseData: EvaluationDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
