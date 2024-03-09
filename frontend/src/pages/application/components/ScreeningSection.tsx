import { useEffect, useState } from "react";
import { downloadCv, getScreening } from "../../../services/CvService";
import { Button } from "@/components/ui/button";

export type ScreeningSectionParams = {
  internshipId: string;
};

const ScreeningSection = ({ internshipId }: ScreeningSectionParams) => {
  const [hasUploadedCv, setHasUploadedCv] = useState<boolean>(false);

  const getCvData = async () => {
    await downloadCv({ internshipId });
  };

  const getScreeningData = async () => {
    const hasUploadedCv = await getScreening({ internshipId });
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
