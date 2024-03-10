import { useEffect, useState } from "react";
import { downloadCv, getScreening } from "../../../services/CvService";
import { Button } from "@/components/ui/button";

export type ScreeningSectionParams = {
  applicaitonId: string;
};

const ScreeningSection = ({
  applicaitonId: applicationId,
}: ScreeningSectionParams) => {
  const [hasUploadedCv, setHasUploadedCv] = useState<boolean>(false);

  const getCvData = async () => {
    await downloadCv({ applicationId });
  };

  const getScreeningData = async () => {
    const hasUploadedCv = await getScreening({ applicationId });
    if (hasUploadedCv) {
      setHasUploadedCv(true);
    }
  };

  useEffect(() => {
    getScreeningData();
  });

  return (
    <>
      {hasUploadedCv ? (
        <>
          We've received your application!
          <Button onClick={getCvData}>Download my CV</Button>
        </>
      ) : (
        <>Please Upload your CV</>
      )}
    </>
  );
};

export default ScreeningSection;
