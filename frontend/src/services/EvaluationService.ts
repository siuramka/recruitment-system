import { EvaluationCreateDto } from "@/interfaces/Evaluation/EvaluationCreateDto";
import api from "./Api";

export const getAssessmentEvaluation = async ({
  assessmentId,
}: {
  assessmentId: string;
}) => {
  try {
    const response = await api.get(`assessment/${assessmentId}/evaluation`);

    if (response.status === 200) {
      const responseData: EvaluationDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getInterviewEvaluation = async ({
  interviewId,
}: {
  interviewId: string;
}) => {
  try {
    const response = await api.get(`interview/${interviewId}/evaluation`);

    if (response.status === 200) {
      const responseData: EvaluationDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getScreeningEvaluation = async ({
  screeningId,
}: {
  screeningId: string;
}) => {
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

export const createScreeningEvaluation = async ({
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

export const createInterviewEvaluation = async ({
  interviewId,
  evaluation,
}: {
  interviewId: string;
  evaluation: EvaluationCreateDto;
}) => {
  try {
    const response = await api.post(
      `interview/${interviewId}/evaluation`,
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

export const createAssessmentEvaluation = async ({
  interviewId,
  evaluation,
}: {
  interviewId: string;
  evaluation: EvaluationCreateDto;
}) => {
  try {
    const response = await api.post(
      `assessment/${interviewId}/evaluation`,
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
