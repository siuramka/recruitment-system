import { useEffect, useState } from "react";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { getApplication } from "@/services/ApplicationService";
import { getDecision } from "@/services/DecisionService";
import { useParams } from "react-router-dom";
import DecisionLineChart from "./components/DecisionLineChart";
import { Card, CardContent } from "@/components/ui/card";
import { DecisionScoreDto } from "@/interfaces/Decision/DecisionScoreDto";
import DecisionCombinedChart from "./components/DecisionCombinedChart";
import { Badge } from "@/components/ui/badge";
import DecisionPieChart from "./components/DecisionPieChart";
import { Separator } from "@/components/ui/separator";
import DecisionAlertDialog from "./components/DecisionAlertDialog";
import { useDispatch } from "react-redux";
import { hideLoader, showLoader } from "@/features/GlobalLoaderSlice";

const DecisionPage = () => {
  const [application, setApplication] = useState<ApplicationListItemDto>();
  const [decision, setDecision] = useState<DecisionScoreDto>();
  const { applicationId } = useParams() as { applicationId: string };

  const dispatch = useDispatch();

  const getDecisionData = async ({
    applicationId,
  }: {
    applicationId: string;
  }) => {
    var decisionData = await getDecision({ applicationId });
    if (decisionData) {
      setDecision(decisionData);
    }
    dispatch(hideLoader());
  };

  const getData = async () => {
    dispatch(showLoader());
    var data = await getApplication({ applicationId });
    if (data) {
      setApplication(data);
      getDecisionData({ applicationId });
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <>
      {application && decision && (
        <div>
          <div className="space-y-0.5 mb-3">
            <h2 className="text-2xl font-bold tracking-tight">
              Application decision
            </h2>
            <p className="text-muted-foreground">
              Make a final decision for cadidate{" "}
              <span className="font-medium text-black">
                {application.siteUserDto.firstName}{" "}
                {application.siteUserDto.lastName}
              </span>
            </p>
          </div>
          <div className="py-3">
            <Card>
              <CardContent className="grid grid-cols-4">
                <div className="space-y-0.5 mb-3 pt-8">
                  <h2 className="text-2xl font-medium tracking-tight">
                    Total score:
                    <span className="px-3">
                      <Badge variant="default" className="text-base">
                        {decision.finalScore.score} / 100
                      </Badge>
                    </span>
                  </h2>
                </div>
                <div className="pt-8">
                  <DecisionAlertDialog
                    applicationId={applicationId}
                    refetch={getData}
                  />
                </div>
              </CardContent>
            </Card>
          </div>
          <div className="grid grid-cols-2 gap-x-10">
            <Card>
              <div>
                <DecisionPieChart finalScoreDto={decision.finalScore} />
              </div>
              <Separator className="my-3" />
              <div>
                <DecisionLineChart applicationId={application.id} />
              </div>
              <Separator className="my-3" />
              <div>
                <DecisionCombinedChart applicationId={application.id} />
              </div>
            </Card>
          </div>
        </div>
      )}
    </>
  );
};

export default DecisionPage;
