import api from "./Api";
import { saveAs } from "file-saver";

type downloadCvParams = {
  internshipId: string;
};

export const downloadCv = async ({ internshipId }: downloadCvParams) => {
  try {
    const response = await api.get(
      `internships/${internshipId}/application/screening/cv`,
      {
        responseType: "blob",
      }
    );

    saveAs(response.data);
  } catch (error) {
    console.error(error);
  }
};

type getScreeningParams = {
  internshipId: string;
};

export const getScreening = async ({ internshipId }: getScreeningParams) => {
  try {
    const response = await api.get(
      `internships/${internshipId}/application/screening/cv`
    );

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
