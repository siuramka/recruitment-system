import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { downloadCv, getScreening } from "@/services/ScreeningService";
import { Terminal } from "lucide-react";
import { useEffect, useState } from "react";

type Props = {
  application: ApplicationListItemDto;
};

const ScreeningSheetItem = ({ application }: Props) => {
  const [hasScreening, setHasScreening] = useState(false);

  const getData = async () => {
    var screening = await getScreening({ applicationId: application.id });
    if (screening) {
      setHasScreening(true);
    }
  };

  const getCvData = async () => {
    await downloadCv({ applicationId: application.id });
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <Card className="mt-6">
      <CardHeader>
        <CardTitle>Screening step</CardTitle>
        <CardDescription>Actions for the Screening step</CardDescription>
      </CardHeader>
      <CardContent>
        {hasScreening ? (
          <div>
            <Button onClick={getCvData}>Download resume</Button>
          </div>
        ) : (
          <div>
            <Alert>
              <Terminal className="h-4 w-4" />
              <AlertTitle>Latest update regarding the application</AlertTitle>
              <AlertDescription>
                User has not yet submitted the application!
              </AlertDescription>
            </Alert>
          </div>
        )}
      </CardContent>
    </Card>
  );
};

export default ScreeningSheetItem;
