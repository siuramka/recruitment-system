import api from "./Api";
import { saveAs } from "file-saver";

type downloadCvParams = {
  applicationId: string;
};

export const downloadCv = async ({ applicationId }: downloadCvParams) => {
  try {
    const response = await api.get(
      `applications/${applicationId}/screening/cv`,
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
  applicationId: string;
};

export const getScreening = async ({ applicationId }: getScreeningParams) => {
  try {
    const response = await api.get(`applications/${applicationId}/screening`);

    if (response.status === 200) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
