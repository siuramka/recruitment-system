import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getAllApplications } from "../../../../services/ApplicationService";
import {
  TableCaption,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
  Table,
} from "@/components/ui/table";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { DialogDemo } from "../../internship/DialogDemo";
import { InternshipDto } from "../../../../interfaces/Internship/InternshipDto";
import { CompanyInternshipApplicantSheet } from "./CompanyInternshipApplicantSheet";
import { Badge } from "@/components/ui/badge";

const CompanyInternshipView = () => {
  const [applications, setApplications] = useState<ApplicationListItemDto[]>(
    []
  );

  const [internship, setInternsihp] = useState<InternshipDto>();
  const { internshipId } = useParams() as { internshipId: string };

  const getData = async () => {
    var data = await getAllApplications({ internshipId });
    if (data) {
      setApplications(data);
      setInternsihp(data[0].internshipDto);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <div className="flex flex-col">
      <span className="flex justify-end">
        <DialogDemo />
      </span>
      <div>
        <Table>
          <TableCaption>A list of your apps.</TableCaption>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">Candidate</TableHead>
              <TableHead className="w-[100px]">Position</TableHead>
              <TableHead className="w-[100px]">Email</TableHead>
              <TableHead className="w-[100px]">Step</TableHead>
              <TableHead className="w-[100px]">Settings</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {applications.map((app) => (
              <TableRow key={app.id} className="hover:cursor-pointer">
                <TableCell className="font-medium">
                  <div>
                    {app.siteUserDto.firstName} {app.siteUserDto.lastName}
                  </div>
                  <div>{app.siteUserDto.location}</div>
                </TableCell>
                <TableCell className="font-medium">
                  {internship?.name}
                </TableCell>
                <TableCell className="font-medium">
                  {app.siteUserDto.email}
                </TableCell>
                <TableCell>
                  <Badge variant="default">{app.stepName}</Badge>
                </TableCell>
                <TableCell>
                  <CompanyInternshipApplicantSheet
                    appId={app.id}
                    internshipId={internshipId}
                  />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
          {/* <TableFooter>
            <TableRow>
              <TableCell colSpan={3}>Total</TableCell>
              <TableCell className="text-right">$2,500.00</TableCell>
            </TableRow>
          </TableFooter> */}
        </Table>
      </div>
    </div>
  );
};

export default CompanyInternshipView;

// Table list of applicants
