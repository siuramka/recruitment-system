import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { AssessmentDto } from "@/interfaces/Assessment/AssessmentDto";
import { getAssessment } from "@/services/AssessmentService";
import { getFormattedDate } from "@/services/DateService";
import { Terminal } from "lucide-react";
import { useEffect, useState } from "react";

type Props = {
  application: ApplicationListItemDto;
};
const AssessmentSection = ({ application }: Props) => {
  const [assessment, setAssessment] = useState<AssessmentDto>();

  const getData = async () => {
    const assessmentdata = await getAssessment({
      applicationId: application.id,
    });
    if (assessmentdata) {
      setAssessment(assessmentdata);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div className="flex">
      {assessment ? (
        <div>
          <Alert variant="primary" className="my-3">
            <Terminal className="h-4 w-4" />
            <AlertTitle>Complete the assessments</AlertTitle>
            <AlertDescription>
              Please finish these assessments. You have time until{" "}
              <span className="font-medium">
                {getFormattedDate(new Date(assessment.endTime))}
              </span>
            </AlertDescription>
          </Alert>
          <CardHeader>
            <CardTitle>Follow the instructions</CardTitle>
          </CardHeader>
          <CardContent>{assessment.content}</CardContent>
        </div>
      ) : (
        <Alert>
          <Terminal className="h-4 w-4" />
          <AlertTitle>Waiting for assessment</AlertTitle>
          <AlertDescription>
            Your assessment has not been created yet. Please wait for the
            instructions for you to complete the assessment.
          </AlertDescription>
        </Alert>
      )}
    </div>
  );
};

export default AssessmentSection;
