import { Separator } from "@/components/ui/separator";

const StatisticsList = () => {
  return (
    <div>
      <div className="space-y-0.5">
        <h2 className="text-2xl font-bold tracking-tight">Statistics</h2>
        <p className="text-muted-foreground">
          Manage your account settings and set e-mail preferences.
        </p>
      </div>
      <Separator className="my-4" />
    </div>
  );
};

export default StatisticsList;
