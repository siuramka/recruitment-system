import {
  CardContent,
  CardHeader,
  Card,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import { LineStatisticsDto } from "@/interfaces/Statistics/LineStatisticsDto";
import { getLineStatistics } from "@/services/StatisticsService";
import { useEffect, useState } from "react";
import {
  Area,
  AreaChart,
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ReferenceLine,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

const DecisionLineChart = ({ applicationId }: { applicationId: string }) => {
  const [data, setData] = useState<LineStatisticsDto[]>([]);

  const getData = async () => {
    const lineData = await getLineStatistics({ applicationId });
    if (lineData) {
      setData(lineData);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div>
      <CardHeader>
        <CardTitle>Scores of each step</CardTitle>
        <CardDescription>
          Compare scores of other candidates for this position
        </CardDescription>
      </CardHeader>
      <CardContent className="flex justify-center">
        <AreaChart
          width={600}
          height={300}
          data={data}
          margin={{ top: 10, right: 30, left: 0, bottom: 0 }}
        >
          <defs>
            <linearGradient id="colorUv" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="#8884d8" stopOpacity={0.8} />
              <stop offset="95%" stopColor="#8884d8" stopOpacity={0} />
            </linearGradient>
            <linearGradient id="colorPv" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="#82ca9d" stopOpacity={0.8} />
              <stop offset="95%" stopColor="#82ca9d" stopOpacity={0} />
            </linearGradient>
          </defs>
          <XAxis dataKey="name" />
          <YAxis />
          <Legend />
          <CartesianGrid strokeDasharray="3 3" />

          <ReferenceLine
            y={5}
            stroke="#82ca9d"
            strokeDasharray="10 10"
            strokeWidth={1.5}
          />
          <Tooltip />
          <Area
            type="monotone"
            dataKey="ai"
            stroke="#8884d8"
            strokeWidth={1.5}
            fillOpacity={1}
            fill="url(#colorUv)"
          />
          <Area
            type="monotone"
            dataKey="company"
            stroke="#82ca9d"
            strokeWidth={1.5}
            fillOpacity={1}
            fill="url(#colorPv)"
          />
        </AreaChart>
      </CardContent>
    </div>
  );
};

export default DecisionLineChart;
