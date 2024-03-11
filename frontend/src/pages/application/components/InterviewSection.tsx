import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { InterviewDto } from "@/interfaces/Interview/InterviewDto";
import { getFormattedDate } from "@/services/DateService";
import { getInterview } from "@/services/InterviewService";
import { Terminal } from "lucide-react";
import { useEffect, useState } from "react";

type Props = {
  application: ApplicationListItemDto;
};
const InterviewSection = ({ application }: Props) => {
  const [interview, setInterview] = useState<InterviewDto>();

  const getData = async () => {
    const interviewData = await getInterview({ applicationId: application.id });
    if (interviewData) {
      setInterview(interviewData);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div className="flex">
      {interview ? (
        <div>
          <Alert variant="primary">
            <Terminal className="h-4 w-4" />
            <AlertTitle>Interview scheduled</AlertTitle>
            <AlertDescription>
              The interview has been scheduled at{" "}
              <span className="font-medium">
                {getFormattedDate(new Date(interview.startTime))} for{" "}
                {interview.minutesLength} minutes
              </span>{" "}
              You can also update it anytime in the interview step.
            </AlertDescription>
          </Alert>
          <Card>
            <CardHeader>
              <CardTitle>Follow the instructions</CardTitle>
            </CardHeader>
            <CardContent>{interview.instructions}</CardContent>
          </Card>
        </div>
      ) : (
        <Alert>
          <Terminal className="h-4 w-4" />
          <AlertTitle>Waiting for interview</AlertTitle>
          <AlertDescription>
            Your interview has not yet been scheduled. Please wait to receive
            the instructions.
          </AlertDescription>
        </Alert>
      )}
    </div>
  );
};

export default InterviewSection;
