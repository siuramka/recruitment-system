import { Badge } from "@/components/ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { getAllDecisionApplications } from "@/services/ApplicationService";
import { useEffect, useState } from "react";
import DecisionDialog from "./DecisionDialog";
import { useNavigate } from "react-router-dom";

const DecisionsList = () => {
  const [applications, setApplications] = useState<ApplicationListItemDto[]>(
    []
  );
  const navigate = useNavigate();

  const getData = async () => {
    var data = await getAllDecisionApplications();
    if (data) {
      setApplications(data);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  return (
    <>
      <div className="flex flex-col">
        <div className="space-y-0.5 mb-3">
          <h2 className="text-2xl font-bold tracking-tight">Decisions</h2>
          <p className="text-muted-foreground">
            Applications waiting for a final decision.
          </p>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">Candidate</TableHead>
              <TableHead className="w-[100px]">Position</TableHead>
              <TableHead className="w-[100px]">Email</TableHead>
              <TableHead className="w-[100px]">Step</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {applications.map((app) => (
              <TableRow
                key={app.id}
                className="hover:cursor-pointer"
                onClick={() => navigate(`/decisions/${app.id}`)}
              >
                <TableCell className="font-medium">
                  <div>
                    {app.siteUserDto.firstName} {app.siteUserDto.lastName}
                  </div>
                  <div>{app.siteUserDto.location}</div>
                </TableCell>
                <TableCell className="font-medium">
                  {app.internshipDto.name}
                </TableCell>
                <TableCell className="font-medium">
                  {app.siteUserDto.email}
                </TableCell>
                <TableCell>
                  <Badge variant="default">{app.stepName}</Badge>
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
    </>
  );
};

export default DecisionsList;
