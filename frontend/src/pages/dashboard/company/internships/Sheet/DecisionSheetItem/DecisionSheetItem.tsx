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
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { GraduationCap } from "lucide-react";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "@/components/ui/use-toast";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { createDecision, getDecision } from "@/services/DecisionService";
import { useDispatch } from "react-redux";
import { hideLoader, showLoader } from "@/features/GlobalLoaderSlice";
import { DecisionDto } from "@/interfaces/Decision/DecisionDto";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import { CircularProgress } from "@mui/material";

type Props = {
  application: ApplicationListItemDto;
};

const formSchema = z.object({
  companySummary: z.string().min(20, {
    message: "Summary must be at least 20 characters.",
  }),
  companyScore: z.coerce.number().int().min(1).max(5).default(5),
});

const DecisionSheetItem = ({ application }: Props) => {
  const [decision, setDecision] = useState<DecisionDto>();
  const [loading, setLoading] = useState<boolean>(false);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const getData = async () => {
    dispatch(showLoader());
    var decisionData = await getDecision({ applicationId: application.id });

    if (decisionData) {
      setDecision(decisionData.decision);
      form.setValue("companySummary", decisionData.decision.companySummary);
      form.setValue("companyScore", decisionData.decision.companyScore);
    }
    dispatch(hideLoader());
  };

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    dispatch(showLoader());
    setLoading(true);
    const response = await createDecision({
      applicationId: application.id,
      decisionCreateDto: values,
    });

    if (response) {
      toast({ title: "Decision created" });
      setDecision(response);
    }
    setLoading(false);
    dispatch(hideLoader());
  }

  useEffect(() => {
    getData();
  }, []);

  return (
    <div>
      <Card className="mt-6">
        <CardHeader>
          <CardTitle>Decision step</CardTitle>
          <CardDescription>Actions for the Decision step</CardDescription>
        </CardHeader>
        <CardContent>
          {loading && (
            <Alert variant="primary" className="my-3">
              <AlertTitle className="flex">
                Evaluating with ChatGPT <CircularProgress color="inherit" />
              </AlertTitle>
              <AlertDescription>This might take a while...</AlertDescription>
            </Alert>
          )}
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
                      name="companySummary"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Final review</FormLabel>
                          <FormControl>
                            <Textarea
                              className="w-[590px]"
                              placeholder="After considering all the factors, provide a final review of the candidate. 
                              Ai will evaluate the candidade based on the all steps and the final review."
                              {...field}
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="pt-3">
                    <FormField
                      control={form.control}
                      name="companyScore"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="flex items-start">
                            Score
                          </FormLabel>
                          <FormControl>
                            <div className="flex max-w-sm items-center space-x-2">
                              <Input
                                className="w-32"
                                type="number"
                                max={5}
                                min={1}
                                placeholder="5"
                                defaultValue={5}
                                {...field}
                              />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  {!decision && (
                    <Button className="mt-5" type="submit" variant="default">
                      Submit
                    </Button>
                  )}
                </div>
              </form>
            </Form>
          </div>
          <div>
            {decision && (
              <div className="w-[590px] mt-3">
                <div className="my-1">
                  <Alert variant="primary">
                    <GraduationCap className="h-4 w-4" />
                    <AlertTitle>
                      Congratulations! All steps finished!
                    </AlertTitle>
                  </Alert>
                </div>
                <div>
                  {application.stepName === "Decision" && (
                    <Button
                      className="my-3"
                      onClick={() => navigate(`/decisions/${application.id}`)}
                    >
                      Finalise
                    </Button>
                  )}
                </div>
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default DecisionSheetItem;
