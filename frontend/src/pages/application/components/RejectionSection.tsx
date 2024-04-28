import { Alert, AlertTitle, AlertDescription } from "@/components/ui/alert";
const RejectionSection = () => {
  return (
    <>
      <div>
        <Alert variant={"destructive"}>
          <AlertTitle>Unfortunate!</AlertTitle>
          <AlertDescription>
            Your applicaiton was denied and the company will not be contacting
            you.
          </AlertDescription>
        </Alert>
      </div>
    </>
  );
};

export default RejectionSection;
