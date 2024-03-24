import React, { useState } from "react";
import { Cell, Pie, PieChart } from "recharts";
import { FinalScoreDto } from "../../../../interfaces/FinalScore/FinalScoreDto";
import { Badge } from "@/components/ui/badge";
import {
  CardTitle,
  CardDescription,
  CardContent,
  CardHeader,
} from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import DetailedCalculationsDialog from "./DetailedCalculationsDialog";

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042"];

const DecisionPieChart = ({
  finalScoreDto,
}: {
  finalScoreDto: FinalScoreDto;
}) => {
  const [data, setData] = useState([
    {
      name: "X1X2 average",
      value: finalScoreDto.x1X2Average,
    },
    {
      name: "Correlation boost",
      value: finalScoreDto.correlationBoostValue,
    },
  ]);

  return (
    <>
      <CardHeader>
        <CardTitle>Total score breakdown</CardTitle>
        <CardDescription>Calculations of the total score</CardDescription>
      </CardHeader>
      <CardContent className="grid justify-center">
        <PieChart width={500} height={200}>
          <Pie
            data={data}
            dataKey="value"
            cx="50%"
            cy="50%"
            outerRadius={60}
            fill="#8884d8"
            label={({
              cx,
              cy,
              midAngle,
              innerRadius,
              outerRadius,
              value,
              index,
            }) => {
              const RADIAN = Math.PI / 180;
              const radius = 25 + innerRadius + (outerRadius - innerRadius);
              const x = cx + radius * Math.cos(-midAngle * RADIAN);
              const y = cy + radius * Math.sin(-midAngle * RADIAN);

              return (
                <text
                  x={x}
                  y={y}
                  fill="#8884d8"
                  textAnchor={x > cx ? "start" : "end"}
                  dominantBaseline="central"
                >
                  {data[index].name} {value}
                </text>
              );
            }}
          >
            {data.map((entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>
        </PieChart>
        <DetailedCalculationsDialog finalScoreDto={finalScoreDto} />
      </CardContent>
    </>
  );
};

export default DecisionPieChart;
