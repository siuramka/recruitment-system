import { Alert, AlertTitle, AlertDescription } from "@/components/ui/alert";
import { Badge } from "@/components/ui/badge";
import { Terminal } from "lucide-react";

type ScoreAlertProps = {
  aiScore: number;
  message: string;
};

const ScoreAlert = ({ aiScore, message }: ScoreAlertProps) => {
  return (
    <>
      <Alert variant="primary">
        <Terminal className="h-4 w-4" />
        <AlertTitle>OpenAI Score</AlertTitle>
        <AlertDescription>
          <div>
            <span className="font-medium">{message}</span>
          </div>
          <div className="mt-2">
            <Badge className="font-medium" variant={null}>
              {aiScore} out of 5
            </Badge>
          </div>
        </AlertDescription>
      </Alert>
    </>
  );
};

export default ScoreAlert;
