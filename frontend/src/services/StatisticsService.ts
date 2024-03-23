import { LineStatisticsDto } from "@/interfaces/Statistics/LineStatisticsDto";
import { CombinedStatisticsDto } from "@/interfaces/Statistics/CombinedStatisticsDto";
import api from "./Api";

export const getLineStatistics = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.get(
      `applications/${applicationId}/statistics/line`
    );

    if (response.status === 200) {
      const responseData: LineStatisticsDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const getCombinedStatistics = async ({
  applicationId,
}: {
  applicationId: string;
}) => {
  try {
    const response = await api.get(
      `applications/${applicationId}/statistics/combined`
    );

    if (response.status === 200) {
      const responseData: CombinedStatisticsDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
