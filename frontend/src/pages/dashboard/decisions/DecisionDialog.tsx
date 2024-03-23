import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { DecisionDto } from "@/interfaces/Decision/DecisionDto";
import { DecisionScoreDto } from "@/interfaces/Decision/DecisionScoreDto";
import { getDecision } from "@/services/DecisionService";
import { useEffect, useState } from "react";

const DecisionDialog = ({
  application,
}: {
  application: ApplicationListItemDto;
}) => {
  const [decision, setDecision] = useState<DecisionScoreDto>();

  const getData = async () => {
    var decisionData = await getDecision({ applicationId: application.id });
    if (decisionData) {
      setDecision(decisionData);
    }
  };

  return (
    <>
      <Dialog>
        <DialogTrigger asChild>
          <Button variant="outline" onClick={() => getData()}>
            Open
          </Button>
        </DialogTrigger>
        <DialogContent className="max-w-[90%] ">
          <DialogHeader>
            <DialogTitle>
              {application.siteUserDto.firstName}{" "}
              {application.siteUserDto.lastName}
            </DialogTitle>
            <DialogDescription>
              Finalise your decision for the candidate.
              {decision?.decision.aiStagesReview}
            </DialogDescription>
          </DialogHeader>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default DecisionDialog;
