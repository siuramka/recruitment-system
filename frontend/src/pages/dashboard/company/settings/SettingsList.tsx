import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Separator } from "@/components/ui/separator";
import { Slider } from "@/components/ui/slider";
import { SettingDto } from "@/interfaces/Setting/SettingDto";
import { getSettings } from "@/services/SettingService";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { updateSetitng } from "../../../../services/SettingService";
import { toast } from "@/components/ui/use-toast";

const formSchema = z.object({
  AiScoreWeight: z.coerce.number().int().min(0).max(100),
  CompanyScoreWeight: z.coerce.number().int().min(0).max(100),
  TotalScoreWeight: z.coerce.number().int().min(0).max(100),
});

const SettingsList = () => {
  const [settings, setSettings] = useState<SettingDto[]>([]);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  const updateValues = (values: z.infer<typeof formSchema>) => {
    form.setValue("AiScoreWeight", values.AiScoreWeight);
    form.setValue("CompanyScoreWeight", values.CompanyScoreWeight);
    form.setValue("TotalScoreWeight", values.TotalScoreWeight);
  };

  const getData = async () => {
    const settingsData = await getSettings();
    if (settingsData) {
      setSettings(settingsData);
      const values: z.infer<typeof formSchema> = {
        AiScoreWeight: parseFloat(
          settingsData.find((s) => s.name === "AiScoreWeight")?.value || "0"
        ),
        CompanyScoreWeight: parseFloat(
          settingsData.find((s) => s.name === "CompanyScoreWeight")?.value ||
            "0"
        ),
        TotalScoreWeight: parseFloat(
          settingsData.find((s) => s.name === "TotalScoreWeight")?.value || "0"
        ),
      };
      updateValues(values);
    }
  };

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    const newSettings = Object.entries(values).map(([name, value]) => ({
      name,
      value,
    }));

    await Promise.all(
      newSettings.map((setting) => {
        return updateSetitng({
          settingName: setting.name,
          value: setting.value.toString(),
        });
      })
    ).then(() => {
      toast({ title: "Settings updated" });
    });
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div>
      <div className="space-y-0.5">
        <h2 className="text-2xl font-bold tracking-tight">Settings</h2>
        <p className="text-muted-foreground">Manage settings of your company</p>
      </div>
      <Separator className="my-4" />
      <div className="grid grid-cols-5">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <FormField
              name="AiScoreWeight"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="flex justify-between">
                    <span>AiScore</span>
                    <span>{form.watch("AiScoreWeight")}</span>
                  </FormLabel>
                  <FormControl>
                    <div className="mt-3">
                      <Slider
                        value={[form.watch("AiScoreWeight")]}
                        onValueChange={(value) =>
                          form.setValue("AiScoreWeight", value[0])
                        }
                      />
                    </div>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              name="CompanyScoreWeight"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="flex justify-between">
                    <span>CompanyScore</span>
                    <span>{form.watch("CompanyScoreWeight")}</span>
                  </FormLabel>
                  <FormControl>
                    <div className="mt-3">
                      <Slider
                        value={[form.watch("CompanyScoreWeight")]}
                        onValueChange={(value) =>
                          form.setValue("CompanyScoreWeight", value[0])
                        }
                      />
                    </div>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              name="TotalScoreWeight"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="flex justify-between">
                    <span>TotalScore</span>
                    <span>{form.watch("TotalScoreWeight")}</span>
                  </FormLabel>
                  <FormControl>
                    <div className="mt-3">
                      <Slider
                        value={[form.watch("TotalScoreWeight")]}
                        onValueChange={(value) =>
                          form.setValue("TotalScoreWeight", value[0])
                        }
                      />
                    </div>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button className="w" type="submit">
              Update
            </Button>
          </form>
        </Form>
      </div>
    </div>
  );
};
export default SettingsList;
