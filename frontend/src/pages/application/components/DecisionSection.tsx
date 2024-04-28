import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Terminal } from "lucide-react";

const DecisionSection = () => {
  return (
    <div className="flex justify-center">
      <div className="mt-6">
        <Alert variant="primary" className="py-10">
          <Terminal className="h-4 w-4" />
          <AlertTitle>Last step!</AlertTitle>
          <AlertDescription>
            Currently, we are reviewing your application. You will see the
            result as soon as the best candidate is selected and the final
            decision is made.
          </AlertDescription>
        </Alert>
      </div>
    </div>
  );
};

export default DecisionSection;
