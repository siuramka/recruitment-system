"use client";

import { Button } from "@/components/ui/button";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
("use client");

import { useState } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  UpdateNextApplicationStep,
  getApplicationStepsById,
  updateApplicationStep,
} from "@/services/StepService";
import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";
import { Badge } from "@/components/ui/badge";
import { getApplication } from "@/services/ApplicationService";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import ScreeningSheetItem from "./ScreeningSheetItem/ScreeningSheetItem";
import InterviewSheetItem from "./InterviewSheetItem/InterviewSheetItem";
import AssessmentSheetItem from "./AssessmentSheetItem/AssessmentSheetItem";
import DecisionSheetItem from "./DecisionSheetItem/DecisionSheetItem";
import { Alert, AlertTitle } from "@/components/ui/alert";

type props = {
  appId: string;
  internshipId: string;
  handleRefresh: () => void;
};

export function ApplicationSheet({
  appId,
  internshipId,
  handleRefresh,
}: props) {
  const [step, setStep] = useState("");
  const [allSteps, setAllSteps] = useState<ApplicationStepDto[]>([]);
  const [isUpdating, setIsUpdating] = useState(false);
  const [application, setApplication] = useState<ApplicationListItemDto>();
  const [activeStepPosition, setActiveStepPosition] = useState(0);

  const getData = async () => {
    var applicationData = await getApplication({ applicationId: appId });
    if (applicationData) {
      setApplication(applicationData);
    }

    var dataSteps = await getApplicationStepsById({
      internshipId,
      applicationId: appId,
    });
    if (dataSteps) {
      const activeStep = dataSteps.find((s) => s.isCurrentStep);
      setActiveStepPosition(activeStep!.positionAscending);
      setStep(activeStep!.stepType);
      setAllSteps(dataSteps);
    }
  };

  const onStageChange = async (value: string) => {
    setStep(value);
    handleRefresh();

    var applicationData = await getApplication({ applicationId: appId });
    if (applicationData) {
      setApplication(applicationData);
    }

    setIsUpdating(false);
  };

  const handleNextStep = async () => {
    const nextStep = await UpdateNextApplicationStep({
      internshipId,
      applicationId: appId,
    });

    if (nextStep) {
      setActiveStepPosition(activeStepPosition + 1);
      setStep(nextStep.stepType);
      handleRefresh();
    }

    setIsUpdating(false);
  };

  return (
    <Sheet>
      <SheetTrigger onClick={getData} asChild>
        <Button variant="outline">Open</Button>
      </SheetTrigger>
      <SheetContent className="sm:max-w-[1000px] overflow-auto">
        <SheetHeader>
          <SheetTitle>Application managment!</SheetTitle>
          <SheetDescription>
            Here you can find information about the application!
          </SheetDescription>
        </SheetHeader>
        <div className="pt-10">
          <div className="flex justify-start">
            <Card>
              <CardHeader>
                <CardTitle>Current step</CardTitle>
                <CardDescription>View the current step</CardDescription>
              </CardHeader>
              <CardContent>
                <Badge className="py-3 px-6">{step}</Badge>
              </CardContent>
            </Card>
            <Card className="ml-6">
              <CardHeader>
                <CardTitle>Update application step</CardTitle>
                <CardDescription>
                  Actions for updating the application step
                </CardDescription>
              </CardHeader>
              <CardContent>
                <Select onValueChange={onStageChange} value={step}>
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder={step} />
                  </SelectTrigger>
                  <SelectContent>
                    {allSteps.map((s, index) => (
                      <SelectItem
                        key={s.positionAscending}
                        disabled={index > activeStepPosition}
                        value={s.stepType}
                      >
                        {s.stepType}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                {application?.stepName !== "Decision" &&
                  application?.stepName !== "Offer" &&
                  application?.stepName !== "Rejection" && (
                    <Button className="mt-4 w-full" onClick={handleNextStep}>
                      Next step
                    </Button>
                  )}
              </CardContent>
            </Card>
          </div>
          <div>
            {application &&
              (() => {
                switch (step) {
                  case "Screening":
                    return <ScreeningSheetItem application={application} />;
                  case "Interview":
                    return <InterviewSheetItem application={application} />;
                  case "Assessment":
                    return <AssessmentSheetItem application={application} />;
                  case "Decision":
                    return <DecisionSheetItem application={application} />;
                  default:
                    return (
                      <Alert variant="primary" className="my-4">
                        <AlertTitle>All steps finished!</AlertTitle>
                      </Alert>
                    );
                }
              })()}
          </div>
        </div>
        <SheetFooter>
          <SheetClose asChild>
            <Button variant="outline" type="submit" className="mt-4">
              Close
            </Button>
          </SheetClose>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  );
}
