import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Check } from "lucide-react";
import ScoreAlert from "../components/ScoreAlert";
import { Button } from "@/components/ui/button";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  createAssessmentEvaluation,
  createInterviewEvaluation,
  getAssessmentEvaluation,
  getInterviewEvaluation,
} from "@/services/EvaluationService";
import { useEffect, useState } from "react";
import { Badge } from "@/components/ui/badge";
import { toast } from "@/components/ui/use-toast";

const formSchema = z.object({
  companyScore: z.coerce.number().int().min(1).max(5).default(5),
  content: z.string().min(20, {
    message: "Contact must be at least 20 characters.",
  }),
});

const AssessmentEvaluateCard = ({ assessmentId }: { assessmentId: string }) => {
  const [evaluation, setEvaluation] = useState<EvaluationDto>();
  const [hasCompanyEvaluated, setHasCompanyEvaluated] = useState(false);
  const [hasAiEvaluated, setHasAiEvaluated] = useState(false);

  const getData = async () => {
    const evaluation = await getAssessmentEvaluation({
      assessmentId,
    });
    if (evaluation) {
      setEvaluation(evaluation);
      setHasCompanyEvaluated(evaluation.companyScore !== 0);
      setHasAiEvaluated(evaluation.aiScore !== 0);
      updateDefaultValues({
        companyScore: evaluation.companyScore,
        content: evaluation.content,
      });
    }
  };

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  const updateDefaultValues = ({
    companyScore,
    content,
  }: {
    companyScore: number;
    content: string;
  }) => {
    form.setValue("companyScore", companyScore);
    form.setValue("content", content);
  };

  const handleSubmit = async (values: z.infer<typeof formSchema>) => {
    var evaluation = await createAssessmentEvaluation({
      interviewId: assessmentId,
      evaluation: values,
    });

    if (evaluation) {
      setEvaluation(evaluation);
      setHasCompanyEvaluated(true);
      setHasAiEvaluated(true);

      toast({
        title: "Evaluation submitted",
      });
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <Card className="mt-6">
      <CardHeader>
        <CardTitle>
          Evaluation{" "}
          {hasCompanyEvaluated && (
            <Badge>
              <Check className="w-4 h-4" /> Submitted
            </Badge>
          )}
        </CardTitle>
        <CardDescription>
          Evaluate how well the candidate did in this step
        </CardDescription>
      </CardHeader>
      <CardContent>
        {hasAiEvaluated && evaluation && (
          <ScoreAlert
            aiScore={evaluation.aiScore}
            message={
              "ChatGPT Large Language Model has given a score of how fit the interviewer was for the internship."
            }
          />
        )}

        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)}>
            <div className="pt-3">
              <FormField
                control={form.control}
                name="companyScore"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">Score</FormLabel>
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
            <div className="pt-3">
              <FormField
                control={form.control}
                name="content"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Evaluation text</FormLabel>
                    <FormControl>
                      <Textarea
                        className="w-[590px]"
                        placeholder="The candidate did alright in the assessment. They could improve on their problem-solving skills. The coding solution was not optimal, considering how long they took to solve it."
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            {!evaluation && (
              <div className="pt-3">
                <Button type="submit">Submit</Button>
              </div>
            )}
          </form>
        </Form>
      </CardContent>
    </Card>
  );
};

export default AssessmentEvaluateCard;
