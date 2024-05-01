import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { FinalScoreDto } from "@/interfaces/FinalScore/FinalScoreDto";

const DetailedCalculationsDialog = ({
  finalScoreDto,
}: {
  finalScoreDto: FinalScoreDto;
}) => {
  return (
    <Dialog>
      <DialogTrigger>
        <Button variant={"outline"} className="mt-6 w-auto">
          Open score calculation breakdown
        </Button>
      </DialogTrigger>
      <DialogContent className="h-[90vh] overflow-auto">
        <DialogHeader>
          <DialogTitle>Detailed score calculations breakdown</DialogTitle>
          <DialogDescription>
            See simple representation of how the final score is calculated
          </DialogDescription>
        </DialogHeader>
        <div className="mt-5 space-y-8">
          <div className="p-4 bg-gray-50 rounded-lg">
            <h4 className="mb-1 font-semibold">AI Score (X1):</h4>
            <p className="text-sm">normalize = 20</p>
            <p className="text-sm">maxScore = 100</p>
            <p className="text-sm">
              Formula: aiScoreX1 = ({finalScoreDto.aiScoreX1} * {"20"}) * (
              {"AiWeight"} / {"100"})
            </p>
            <p className="text-sm">
              Value: aiScoreX1 = {finalScoreDto.aiScoreX1}
            </p>
          </div>

          <div className="p-4 bg-gray-50 rounded-lg">
            <h4 className="mb-1 font-semibold">Company Score (X2):</h4>
            {/* <p className="text-sm">
              companyScoreX2 = {finalScoreDto.companyScoreX2}
            </p> */}
            <p className="text-sm">
              Calculation: companyScoreX2 = ({"finalScoreDto.companyScoreX2"} *{" "}
              {"20"}) * ({"Company Score Weight"} / {"100"})
            </p>
          </div>

          <div className="p-4 bg-gray-50 rounded-lg">
            <h4 className="mb-1 font-semibold">X1X2 Average:</h4>
            <p className="text-sm">
              Formula: x1X2Average = {finalScoreDto.x1X2Average}
            </p>
            <p className="text-sm">
              Value: x1X2Average = ({finalScoreDto.aiScoreX1} +{" "}
              {finalScoreDto.companyScoreX2}) / 2
            </p>
          </div>

          <div className="p-4 bg-gray-50 rounded-lg">
            <h4 className="mb-1 font-semibold">Correlation Boost Modifier:</h4>
            <p className="text-sm">
              Formula: correlationBoostModifer ={" "}
              {finalScoreDto.correlationBoostModifer}
            </p>
            <p className="text-sm">
              Value: correlationBoostModifer = (1 + {finalScoreDto.correlation})
              * ({"Correlation Boost Weight"} / {"100"})
            </p>
          </div>

          <div className="p-4 bg-gray-50 rounded-lg">
            <h4 className="mb-1 font-semibold">Final Score:</h4>
            <p className="text-sm">
              Formula: finalScore = {finalScoreDto.score}
            </p>
            <p className="text-sm">
              Value: finalScore = {finalScoreDto.x1X2Average} *{" "}
              {finalScoreDto.correlationBoostModifer}
            </p>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default DetailedCalculationsDialog;
