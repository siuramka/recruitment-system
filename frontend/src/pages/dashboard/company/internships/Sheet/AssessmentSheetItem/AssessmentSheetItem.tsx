import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

import { z } from "zod";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { CalendarIcon, GraduationCap } from "lucide-react";
import { Textarea } from "@/components/ui/textarea";
import { AssessmentDto } from "@/interfaces/Assessment/AssessmentDto";
import { createAssessment, getAssessment } from "@/services/AssessmentService";
import { toast } from "@/components/ui/use-toast";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Calendar } from "@/components/ui/calendar";
import { format } from "date-fns";
import { cn } from "@/lib/utils";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import AssessmentEvaluateCard from "./AssessmentEvaluateCard";
type Props = {
  application: ApplicationListItemDto;
};

const formSchema = z.object({
  content: z.string().min(2),
  endTime: z.date().default(new Date()),
});

const AssessmentSheetItem = ({ application }: Props) => {
  const [assessment, setAssessment] = useState<AssessmentDto>();

  const updateDefaultValues = (values: z.infer<typeof formSchema>) => {
    form.setValue("content", values.content);
    form.setValue("endTime", values.endTime);
  };

  const getData = async () => {
    const assessmentData = await getAssessment({
      applicationId: application.id,
    });
    if (assessmentData) {
      setAssessment(assessmentData);
      updateDefaultValues({
        content: assessmentData.content,
        endTime: assessmentData.endTime,
      });
    }
  };

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    const createdAssessment = await createAssessment({
      applicationId: application.id,
      assessmentCreateDto: values,
    });

    if (createdAssessment) {
      setAssessment(createdAssessment);
      toast({ title: "Assessment created" });
    }
  }

  useEffect(() => {
    getData();
  }, []);

  return (
    <div>
      <Card className="mt-6">
        <CardHeader>
          <CardTitle>Assessment step</CardTitle>
          <CardDescription>Actions for the Assessment step</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex">
            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(onSubmit)}
                className="space-y-8"
              >
                <div>
                  <div className="mt-3">
                    <FormField
                      control={form.control}
                      name="content"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Instructions</FormLabel>
                          <FormControl>
                            <Textarea
                              className="w-[590px]"
                              placeholder="Please finish this assessment here http://google.com/assessment1 Good luck!"
                              {...field}
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="flex mt-3">
                    <FormField
                      control={form.control}
                      name="endTime"
                      render={({ field }) => (
                        <FormItem className="flex flex-col">
                          <FormLabel>Submit before date</FormLabel>
                          <Popover>
                            <PopoverTrigger asChild>
                              <FormControl>
                                <Button
                                  variant={"outline"}
                                  className={cn(
                                    "w-[300px] pl-3 text-left font-normal",
                                    !field.value && "text-muted-foreground"
                                  )}
                                >
                                  {field.value ? (
                                    format(field.value, "PPP")
                                  ) : (
                                    <span>Pick a date</span>
                                  )}
                                  <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                </Button>
                              </FormControl>
                            </PopoverTrigger>
                            <PopoverContent
                              className="w-auto p-0"
                              align="start"
                            >
                              <Calendar
                                mode="single"
                                selected={field.value}
                                onSelect={field.onChange}
                                disabled={(date) => date < new Date()}
                                initialFocus
                              />
                            </PopoverContent>
                          </Popover>
                          <FormDescription>
                            Date before which the assessment should be submitted
                          </FormDescription>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    {!assessment && (
                      <Button className="mt-5" type="submit" variant="default">
                        Submit
                      </Button>
                    )}
                  </div>
                </div>
                <div>
                  {assessment && (
                    <Alert variant="primary">
                      <GraduationCap className="h-4 w-4" />
                      <AlertTitle>Assesment instructions sent!</AlertTitle>
                    </Alert>
                  )}
                </div>
              </form>
            </Form>
          </div>
        </CardContent>
      </Card>
      {assessment && <AssessmentEvaluateCard assessmentId={assessment.id} />}
    </div>
  );
};

export default AssessmentSheetItem;
