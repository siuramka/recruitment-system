import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

import { z } from "zod";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { InterviewDto } from "@/interfaces/Interview/InterviewDto";
import { DateTimePicker } from "@/pages/components/DateTimePicker";
import { createInterview, getInterview } from "@/services/InterviewService";
import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InterviewCreateDto } from "@/interfaces/Interview/InterviewCreateDto";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Terminal } from "lucide-react";
import { getFormattedDate } from "@/services/DateService";
import { Textarea } from "@/components/ui/textarea";
type Props = {
  application: ApplicationListItemDto;
};

const formSchema = z.object({
  minutes: z.coerce.number().min(1).max(720).default(60),
  instructions: z.string().default(""),
});

const InterviewSheetItem = ({ application }: Props) => {
  const [date, setDate] = useState<Date>(new Date());
  const [interview, setInterview] = useState<InterviewDto>();

  const getData = async () => {
    const interviewData = await getInterview({ applicationId: application.id });
    if (interviewData) {
      setInterview(interviewData);
    }
  };

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    const createInterviewData: InterviewCreateDto = {
      startTime: date,
      minutesLength: values.minutes,
      instructions: values.instructions,
    };

    const createdInterview = await createInterview({
      applicationId: application.id,
      interviewCreateDto: createInterviewData,
    });

    if (createdInterview) {
      setInterview(createdInterview);
    }
  }

  useEffect(() => {
    getData();
  }, []);

  return (
    <Card className="mt-6">
      <CardHeader>
        <CardTitle>Interview step</CardTitle>
        <CardDescription>Actions for the Interview step</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
              <div>
                <FormField
                  control={form.control}
                  name="instructions"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Instructions</FormLabel>
                      <FormControl>
                        <Textarea
                          className="w-[590px]"
                          placeholder="Instructions for the interview. Please join this google meets link..."
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>{" "}
              <div className="flex">
                <FormField
                  control={form.control}
                  name="minutes"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Interview length in minutes</FormLabel>
                      <FormControl>
                        <Input
                          className="w-[300px]"
                          type="number"
                          placeholder="60"
                          defaultValue={60}
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <div className="ml-3">
                  <div className="text-sm font-medium mb-3">
                    Select interview date and time
                  </div>
                  <DateTimePicker date={date} setDate={setDate} />
                  <Button className="ml-3" type="submit" variant="secondary">
                    Submit
                  </Button>
                </div>
              </div>
            </form>
          </Form>
        </div>
        <div className="mt-10">
          {interview && (
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
          )}
        </div>
      </CardContent>
    </Card>
  );
};

export default InterviewSheetItem;
