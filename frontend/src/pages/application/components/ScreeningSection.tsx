import { useEffect, useState } from "react";
import {
  createScreening,
  downloadCv,
  getScreening,
} from "../../../services/ScreeningService";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { object, string, z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { toast } from "@/components/ui/use-toast";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";

export type ScreeningSectionParams = {
  application: ApplicationListItemDto;
};

const formSchema = z.object({
  file: z.custom((value) => {
    if (!(value instanceof FileList)) {
      throw new Error("Invalid file type");
    }
    const file = value[0];

    if (file.type !== "application/pdf") {
      throw new Error("Only PDF files are allowed");
    }

    return value;
  }),
});

const ScreeningSection = ({ application }: ScreeningSectionParams) => {
  const [hasUploadedCv, setHasUploadedCv] = useState<boolean>(false);
  const [screening, setScreening] = useState<CvDto>();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
  });

  const fileRef = form.register("file");

  const handleSubmit = async (values: z.infer<typeof formSchema>) => {
    const formData = new FormData();
    formData.append("cvFile", values.file[0]);

    const uploaded = await createScreening({
      applicationId: application.id,
      file: formData,
    });
    if (uploaded) {
      setHasUploadedCv(true);
    }
  };

  const getCvData = async () => {
    await downloadCv({ applicationId: application.id });
  };

  const getScreeningData = async () => {
    const screening = await getScreening({ applicationId: application.id });
    if (screening) {
      setScreening(screening);
    }
  };

  useEffect(() => {
    getScreeningData();
  }, [hasUploadedCv]);

  return (
    <>
      {hasUploadedCv || screening ? (
        <>
          We've received your application!
          <Button onClick={getCvData}>Download my CV</Button>
        </>
      ) : (
        <div>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(handleSubmit)}>
              <FormField
                control={form.control}
                name="file"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-start">
                      Upload your CV (.pdf)
                    </FormLabel>
                    <FormControl>
                      <div className="flex w-full max-w-sm items-center space-x-2">
                        <Input
                          id="file"
                          type="file"
                          accept=".pdf"
                          {...fileRef}
                        />
                        <Button type="submit">Upload</Button>
                      </div>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </form>
          </Form>
        </div>
      )}
    </>
  );
};

export default ScreeningSection;
