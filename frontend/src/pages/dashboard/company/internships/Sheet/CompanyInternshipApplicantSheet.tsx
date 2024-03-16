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
  getApplicationStepsById,
  updateApplicationStep,
} from "@/services/StepService";
import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";
import { Badge } from "@/components/ui/badge";
import { getApplication } from "@/services/ApplicationService";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import ScreeningSheetItem from "./ScreeningSheetItem/ScreeningSheetItem";
import InterviewSheetItem from "./InterviewSheetItem/InterviewSheetItem";

type props = {
  appId: string;
  internshipId: string;
  handleRefresh: () => void;
};

export function CompanyInternshipApplicantSheet({
  appId,
  internshipId,
  handleRefresh,
}: props) {
  const [step, setStep] = useState("");
  const [allSteps, setAllSteps] = useState<ApplicationStepDto[]>([]);
  const [isUpdating, setIsUpdating] = useState(false);
  const [application, setApplication] = useState<ApplicationListItemDto>();

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
      setStep(activeStep!.stepType);
      setAllSteps(dataSteps);
    }
  };

  const onStageChange = async (value: string) => {
    setStep(value);
    setIsUpdating(true);
    var data = await updateApplicationStep({
      internshipId,
      applicationId: appId,
      stepType: value,
    });

    if (data) {
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
                <Select onValueChange={onStageChange} defaultValue={step}>
                  <SelectTrigger className="w-[180px]">
                    <SelectValue placeholder={step} />
                  </SelectTrigger>
                  <SelectContent>
                    {allSteps.map((s) => (
                      <SelectItem
                        key={s.positionAscending}
                        disabled={isUpdating}
                        value={s.stepType}
                      >
                        {s.stepType}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </CardContent>
            </Card>
          </div>
          <div>
            {(() => {
              switch (step) {
                case "Screening":
                  return <ScreeningSheetItem application={application!} />;
                case "Interview":
                  return <InterviewSheetItem application={application!} />;
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
