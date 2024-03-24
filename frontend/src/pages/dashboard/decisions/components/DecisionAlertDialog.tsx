import { Button } from "@/components/ui/button";
import {
  AlertDialog,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { useState } from "react";
import {
  applicationOffer,
  applicationReject,
} from "@/services/ApplicationService";
import { useDispatch } from "react-redux";
import { showLoader } from "@/features/GlobalLoaderSlice";

const DecisionAlertDialog = ({
  applicationId,
  refetch,
}: {
  applicationId: string;
  refetch: () => void;
}) => {
  const [open, setOpen] = useState(false);
  const dispatch = useDispatch();

  const handleHire = async () => {
    dispatch(showLoader());

    await applicationOffer({ applicationId });
    setOpen(false);
  };

  const handleReject = async () => {
    dispatch(showLoader());

    await applicationReject({ applicationId });
    setOpen(false);
  };

  return (
    <AlertDialog open={open} onOpenChange={setOpen}>
      <AlertDialogTrigger>
        <Button variant="default">Make a decision</Button>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <Button variant="default" onClick={handleHire}>
            Offer position
          </Button>
          <Button variant="destructive" onClick={handleReject}>
            Reject candidate
          </Button>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
};

export default DecisionAlertDialog;
