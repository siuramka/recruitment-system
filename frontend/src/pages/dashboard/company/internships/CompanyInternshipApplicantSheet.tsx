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

import * as React from "react";
import { Check, ChevronsUpDown } from "lucide-react";

import { cn } from "@/lib/utils";
import { useEffect, useState } from "react";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
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
  getApplicationSteps,
  getApplicationStepsById,
  updateApplicationStep,
} from "@/services/StepService";
import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";

type props = {
  appId: string;
  internshipId: string;
};

export function CompanyInternshipApplicantSheet({
  appId,
  internshipId,
}: props) {
  const [step, setStep] = useState("");
  const [allSteps, setAllSteps] = useState<ApplicationStepDto[]>([]);

  const getData = async () => {
    var dataSteps = await getApplicationStepsById({
      internshipId,
      applicationId: appId,
    });
    if (dataSteps) {
      setAllSteps(dataSteps);

      const activeStep = dataSteps.find((s) => s.isCurrentStep);
      setStep(activeStep!.stepType);
    }
  };

  const onStageChange = async (value: string) => {
    setStep(value);
    await updateApplicationStep({
      internshipId,
      applicationId: appId,
      stepType: value,
    });
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="outline">Open</Button>
      </SheetTrigger>
      <SheetContent className="sm:max-w-[1000px]">
        <SheetHeader>
          <SheetTitle>Application managment!</SheetTitle>
          <SheetDescription>
            Here you can find information about the application!
          </SheetDescription>
        </SheetHeader>
        <div className="pt-10">
          <Card className="w-[500px]">
            <CardHeader>
              <CardTitle>Update application stage</CardTitle>
            </CardHeader>
            <CardContent>
              <Select onValueChange={onStageChange} defaultValue={step}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Theme" />
                </SelectTrigger>
                <SelectContent>
                  {allSteps.map((s) => (
                    <SelectItem value={s.stepType}>{s.stepType}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </CardContent>
          </Card>
        </div>
        <SheetFooter>
          <SheetClose asChild>
            <Button type="submit">Save changes</Button>
          </SheetClose>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  );
}
