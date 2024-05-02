import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import {
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
  Form,
} from "@/components/ui/form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

import {
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { useState } from "react";
import { toast } from "@/components/ui/use-toast";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { ChevronDown, ChevronUp, X } from "lucide-react";
import { StepSelectItem } from "@/interfaces/Step/StepSelectItem";
import { createInternship } from "../../../../../services/InternshipService";
import { InternshipCreateDto } from "@/interfaces/Internship/InternshipCreateDto";
import { InternshipCreateStepDto } from "@/interfaces/Step/InternshipCreateStepDto";
import { Slider } from "@/components/ui/slider";
import { Separator } from "@/components/ui/separator";
import { SettingCreateDto } from "@/interfaces/Setting/SettingCreate";
import { useDispatch } from "react-redux";
import { hideLoader, showLoader } from "@/features/GlobalLoaderSlice";

const formSchema = z.object({
  name: z.string().min(2, {
    message: "Position name must be at least 2 characters.",
  }),
  contactEmail: z.string().min(2, {
    message: "Contact email must be at least 2 characters.",
  }),
  address: z.string().min(2, {
    message: "Address must be at least 2 characters.",
  }),
  description: z.string().min(2, {
    message: "Description must be at least 2 characters.",
  }),
  requirements: z.string().min(2, {
    message: "Requirements must be at least 2 characters.",
  }),
  skills: z.string().min(2, {
    message: "Requirements must be at least 2 characters.",
  }),
  isPaid: z.boolean().optional().default(false),
  isRemote: z.boolean().optional().default(false),
  slotsAvailable: z.coerce.number().min(1).default(1),
  aiScoreWeight: z.coerce.number().int().min(0).max(100),
  companyScoreWeight: z.coerce.number().int().min(0).max(100),
  totalScoreWeight: z.coerce.number().int().min(0).max(100),
});

const stepInitialItems: StepSelectItem[] = [
  { stepType: "Screening", positionAscending: 0, removable: true },
  { stepType: "Interview", positionAscending: 1, removable: true },
  { stepType: "Assessment", positionAscending: 2, removable: true },
  { stepType: "Decision", positionAscending: 3, removable: false },
  { stepType: "Offer", positionAscending: 4, removable: false },
  { stepType: "Rejection", positionAscending: 5, removable: false },
];

type props = {
  handleRefresh: () => void;
};

export function CreateInternshipDialog({ handleRefresh }: props) {
  const [activeTab, setActiveTab] = useState("internship");
  const dispatch = useDispatch();

  const [selectSteps, setSelectSteps] =
    useState<StepSelectItem[]>(stepInitialItems);

  const [savedData, setSavedDate] = useState<z.infer<typeof formSchema> | null>(
    null
  );

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    setSavedDate(values);
    toast({
      title: "Saved internship data",
      description: "You can update data anytime in the creation process!",
    });
    setActiveTab("settings");
  };

  const handleRemoveStep = (itemToRemove: StepSelectItem) => {
    if (selectSteps.length == 4 && itemToRemove.removable) {
      toast({
        title: "You must have at least 1 step",
      });
      return;
    }

    if (!itemToRemove.removable) {
      return;
    }

    const updatedItems = selectSteps.filter(
      (item) => item.stepType !== itemToRemove.stepType
    );

    const updatedItemsWithPositions = updatedItems.map((item, index) => ({
      ...item,
      position: index,
    }));

    setSelectSteps(updatedItemsWithPositions);
  };

  const handleUpStep = (itemToMove: StepSelectItem) => {
    const currentIndex = selectSteps.findIndex((item) => item === itemToMove);

    if (currentIndex > 0 && currentIndex < selectSteps.length - 2) {
      const updatedItems = [...selectSteps];
      [updatedItems[currentIndex], updatedItems[currentIndex - 1]] = [
        { ...updatedItems[currentIndex - 1], positionAscending: currentIndex },
        { ...updatedItems[currentIndex], positionAscending: currentIndex - 1 },
      ];

      setSelectSteps(updatedItems);
    }
  };

  const handleDownStep = (itemToMove: StepSelectItem) => {
    const currentIndex = selectSteps.findIndex((item) => item === itemToMove);
    if (currentIndex + 1 < selectSteps.length - 3) {
      const updatedItems = [...selectSteps];
      [updatedItems[currentIndex], updatedItems[currentIndex + 1]] = [
        { ...updatedItems[currentIndex + 1], positionAscending: currentIndex },
        { ...updatedItems[currentIndex], positionAscending: currentIndex + 1 },
      ];

      setSelectSteps(updatedItems);
    }
  };

  const handleSelectStepsPositions = (steps: any[]) =>
    steps.map((item: any, index: any) => ({
      ...item,
      positionAscending: index,
    }));

  const handleCreate = async () => {
    if (!savedData) {
      toast({ title: "Please save the internship data before creating." });
      return;
    }

    dispatch(showLoader());

    const steps = handleSelectStepsPositions(selectSteps);
    var internshipStepDtos = steps.map(
      (item) => item as InternshipCreateStepDto
    );

    const settingCreateDto: SettingCreateDto = {
      aiScoreWeight: savedData.aiScoreWeight,
      companyScoreWeight: savedData.companyScoreWeight,
      totalScoreWeight: savedData.totalScoreWeight,
    };

    const internshipCreateDto: InternshipCreateDto = {
      ...savedData,
      internshipStepDtos,
      settingCreateDto,
    };

    const createdInternship = await createInternship(internshipCreateDto);
    if (createdInternship) {
      handleRefresh();
    }

    dispatch(hideLoader());
  };

  const initializeSliders = () => {
    form.setValue("aiScoreWeight", 100);
    form.setValue("companyScoreWeight", 100);
    form.setValue("totalScoreWeight", 100);
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline" onClick={initializeSliders}>
          Create{" "}
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[1000px] flex justify-center overflow-auto h-5/6">
        <Tabs
          defaultValue="internship"
          value={activeTab}
          onValueChange={() => {
            activeTab === "internship"
              ? setActiveTab("settings")
              : setActiveTab("internship");
          }}
          className="w-[800px]"
        >
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="internship">Internship</TabsTrigger>
            <TabsTrigger value="settings">Settings</TabsTrigger>
          </TabsList>
          <TabsContent value="internship">
            <CardHeader>
              <CardTitle>Internship</CardTitle>
              <CardDescription>
                Enter information for the internship.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-2">
              <Form {...form}>
                <form
                  id="hook-form"
                  onSubmit={form.handleSubmit(onSubmit)}
                  className="space-y-4"
                >
                  <FormField
                    control={form.control}
                    name="name"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Position name
                        </FormLabel>
                        <FormControl>
                          <Input
                            {...field}
                            placeholder="Software Engineer Intern .NET"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="contactEmail"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Contact Email
                        </FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="hr@company.com" />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="address"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Address
                        </FormLabel>
                        <FormControl>
                          <Input
                            {...field}
                            placeholder="Jonavos g. 7, Kaunas"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="description"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Description
                        </FormLabel>
                        <FormControl>
                          <Textarea
                            placeholder="Description about the position"
                            className="min-h-[400px]"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="requirements"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Requirements
                        </FormLabel>
                        <FormControl>
                          <Textarea
                            {...field}
                            placeholder="Communication skills, Team Leading skills, 7 years of hiring experience"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="skills"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Nice To Have Skills
                        </FormLabel>
                        <FormControl>
                          <Textarea
                            {...field}
                            placeholder="Experience with Excel, Japanese language proficiency"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="slotsAvailable"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Slots available
                        </FormLabel>
                        <FormControl>
                          <Input
                            {...field}
                            type="number"
                            defaultValue={1}
                            min={1}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="isPaid"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">Paid</FormLabel>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="isRemote"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="flex items-start">
                          Remote
                        </FormLabel>
                        <FormControl>
                          <Switch
                            checked={field.value}
                            onCheckedChange={field.onChange}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <Separator className="my-4" />
                  <div className="grid grid-cols-3 gap-4">
                    <FormField
                      name="AiScoreWeight"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="flex justify-between">
                            <span>Ai Score Weight</span>
                            <span>{form.watch("aiScoreWeight")}</span>
                          </FormLabel>
                          <FormControl>
                            <div className="mt-3">
                              <Slider
                                value={[form.watch("aiScoreWeight")]}
                                onValueChange={(value: number[]) =>
                                  form.setValue("aiScoreWeight", value[0])
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
                            <span>Company Score Weight</span>
                            <span>{form.watch("companyScoreWeight")}</span>
                          </FormLabel>
                          <FormControl>
                            <div className="mt-3">
                              <Slider
                                value={[form.watch("companyScoreWeight")]}
                                onValueChange={(value: number[]) =>
                                  form.setValue("companyScoreWeight", value[0])
                                }
                              />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                    {/* <FormField
                      name="TotalScoreWeight"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="flex justify-between">
                            <span>Correlation Boost Weight</span>
                            <span>{form.watch("totalScoreWeight")}</span>
                          </FormLabel>
                          <FormControl>
                            <div className="mt-3">
                              <Slider
                                value={[form.watch("totalScoreWeight")]}
                                onValueChange={(value: number[]) =>
                                  form.setValue("totalScoreWeight", value[0])
                                }
                              />
                            </div>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    /> */}
                  </div>
                  <Button type="submit">Save</Button>
                </form>
              </Form>
            </CardContent>
          </TabsContent>
          <TabsContent value="settings">
            <CardHeader>
              <CardTitle>Settings</CardTitle>
              <CardDescription>
                Choose steps required for the recruiment process.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-2">
              <Button
                variant="outline"
                onClick={() => setSelectSteps(stepInitialItems)}
              >
                Reset
              </Button>
              <Table>
                <TableCaption>
                  A list of available recruitment steps.
                </TableCaption>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-[100px]">Step</TableHead>
                    <TableHead className="text-right">Action</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {selectSteps.map((item) => (
                    <TableRow>
                      <TableCell className="font-medium">
                        {item.stepType}
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex justify-end">
                          <Button
                            disabled={!item.removable}
                            variant="ghost"
                            className="mr-4"
                            size="icon"
                            onClick={() => handleUpStep(item)}
                          >
                            <ChevronUp className="h-4 w-4" />
                          </Button>
                          <Button
                            disabled={!item.removable}
                            variant="ghost"
                            className="mr-4 "
                            size="icon"
                            onClick={() => handleDownStep(item)}
                          >
                            <ChevronDown className="h-4 w-4" />
                          </Button>
                          <Button
                            disabled={!item.removable}
                            variant="ghost"
                            size="icon"
                            onClick={() => handleRemoveStep(item)}
                          >
                            <X className="h-4 w-4" />
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
            <CardFooter>
              <Button onClick={handleCreate}>Create Internship</Button>
            </CardFooter>
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  );
}
