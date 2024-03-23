import { useEffect, useState } from "react";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { getApplication } from "@/services/ApplicationService";
import { getDecision } from "@/services/DecisionService";
import { useParams } from "react-router-dom";
import DecisionLineChart from "./components/DecisionLineChart";
import { Card } from "@/components/ui/card";
import { DecisionScoreDto } from "@/interfaces/Decision/DecisionScoreDto";
import DecisionCombinedChart from "./components/DecisionCombinedChart";

const DecisionPage = () => {
  const [application, setApplication] = useState<ApplicationListItemDto>();
  const [decision, setDecision] = useState<DecisionScoreDto>();
  const { applicationId } = useParams() as { applicationId: string };

  const getDecisionData = async ({
    applicationId,
  }: {
    applicationId: string;
  }) => {
    var decisionData = await getDecision({ applicationId });
    if (decisionData) {
      setDecision(decisionData);
    }
  };

  const getData = async () => {
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
              {application.siteUserDto.firstName}{" "}
              {application.siteUserDto.lastName}
            </p>
          </div>
          <div className="grid grid-cols-2 gap-x-10">
            <Card>
              <div className="pb-3">
                <DecisionLineChart applicationId={application.id} />
              </div>
              <div className="pb-3">
                <DecisionCombinedChart applicationId={application.id} />
              </div>
            </Card>
            <Card></Card>
          </div>
        </div>
      )}
    </>
  );
};

export default DecisionPage;
//comapre with other candidates in the decition step for the same position
