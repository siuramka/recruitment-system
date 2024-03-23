import {
  CardHeader,
  CardTitle,
  CardContent,
  CardDescription,
} from "@/components/ui/card";
import { CombinedStatisticsDto } from "@/interfaces/Statistics/CombinedStatisticsDto";
import { getCombinedStatistics } from "@/services/StatisticsService";
import { useEffect, useState } from "react";
import {
  ComposedChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  Area,
  Bar,
  Line,
  ReferenceLine,
} from "recharts";

const data = [
  {
    name: "Interview",
    aiAverage: 3.5,
    companyAverage: 4.8,
    candidateAiScore: 5,
    candidateCompanyScore: 4,
  },
  {
    name: "Screening",
    aiAverage: 1.5,
    companyAverage: 2.8,
    candidateAiScore: 5,
    candidateCompanyScore: 5,
  },
];

const DecisionCombinedChart = ({
  applicationId,
}: {
  applicationId: string;
}) => {
  const [data, setData] = useState<CombinedStatisticsDto[]>([]);

  const getData = async () => {
    const combinedData = await getCombinedStatistics({ applicationId });
    if (combinedData) {
      setData(combinedData);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div>
      <CardHeader>
        <CardTitle>Comparison</CardTitle>
        <CardDescription>
          Compare scores of other candidates for this position
        </CardDescription>
      </CardHeader>
      <CardContent className="flex justify-center">
        <ComposedChart width={600} height={300} data={data}>
          <CartesianGrid stroke="#f5f5f5" />
          <XAxis dataKey="step" />
          <YAxis />
          <Tooltip />
          <Legend />
          <Bar dataKey="aiAverage" barSize={20} fill="hsl(221.2 33.2% 53.3%)" />
          <Bar
            dataKey="companyAverage"
            barSize={20}
            fill="hsl(221.2 83.2% 53.3%)"
          />
          <ReferenceLine
            y={5}
            stroke="hsl(221.2 83.2% 53.3%)"
            strokeDasharray="10 10"
            strokeWidth={1.5}
          />
          <Line
            type="monotone"
            dataKey="candidateCompanyScore"
            stroke="hsl(346.8 77.2% 49.8%)"
            strokeWidth={2}
          />
          <Line
            type="monotone"
            dataKey="candidateAiScore"
            stroke="hsl(296.8 87.2% 49.8%)"
            strokeWidth={2}
          />
        </ComposedChart>
      </CardContent>
    </div>
  );
};

export default DecisionCombinedChart;
