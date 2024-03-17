import { SettingDto } from "@/interfaces/Setting/SettingDto";
import api from "./Api";

export const getSettings = async () => {
  try {
    const response = await api.get(`settings`);

    if (response.status === 200) {
      const responseData: SettingDto[] = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};

export const updateSetitng = async ({
  settingName,
  value,
}: {
  settingName: string;
  value: string;
}) => {
  try {
    const response = await api.put(`settings?settingName=${settingName}`, {
      value: value,
    });

    if (response.status === 200) {
      const responseData: SettingDto = response.data;
      return responseData;
    }

    return null;
  } catch {
    return null;
  }
};
