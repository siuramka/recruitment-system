import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Terminal } from "lucide-react";

export const NoDataAlert = () => {
  return (
    <div className="flex justify-center ">
      <div className="mt-6">
        <Alert variant="primary" className="w-[100vh]">
          <Terminal className="h-4 w-4" />
          <AlertTitle>No data exists yet!</AlertTitle>
          <AlertDescription>
            Results will show up here as soon as data is created!
          </AlertDescription>
        </Alert>
      </div>
    </div>
  );
};

export default NoDataAlert;
