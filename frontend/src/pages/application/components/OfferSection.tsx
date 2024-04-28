import { AlertTitle, Alert, AlertDescription } from "@/components/ui/alert";

const OfferSection = () => {
  return (
    <div>
      <Alert variant={"primary"}>
        <AlertTitle>Congratulations!</AlertTitle>
        <AlertDescription>
          Your application was successful! The company will contact you shortly!
        </AlertDescription>
      </Alert>
    </div>
  );
};

export default OfferSection;
