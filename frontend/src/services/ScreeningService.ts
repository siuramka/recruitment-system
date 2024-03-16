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
      const responseData: CvDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

type createScreeningParams = {
  applicationId: string;
  file: FormData;
};

export const createScreening = async ({
  applicationId,
  file,
}: createScreeningParams) => {
  try {
    const response = await api.post(
      `applications/${applicationId}/screening`,
      file,
      {
        headers: {
          "Content-Type": "application/pdf",
        },
      }
    );

    if (response.status === 201) {
      return {};
    }

    return null;
  } catch {
    return null;
  }
};
