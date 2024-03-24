import { GaugePointer } from "@/components/ui/gaugePointer";
import {
  GaugeContainer,
  GaugeReferenceArc,
  GaugeValueArc,
  useGaugeState,
} from "@mui/x-charts/Gauge";

const DecisionCorrelation = ({ score = 0 }: { score: number }) => {
  return (
    <div>
      <GaugeContainer
        height={100}
        startAngle={-110}
        endAngle={110}
        value={score}
      >
        <GaugeReferenceArc />
        <GaugeValueArc />
        <GaugePointer />
      </GaugeContainer>
    </div>
  );
};

export default DecisionCorrelation;
