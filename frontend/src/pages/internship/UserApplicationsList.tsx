"use client";

("use client");

import { useEffect, useState } from "react";
import { ApplicationListItemDto } from "@/interfaces/Application/ApplicationListItemDto";
import { getAllUserApplications } from "@/services/ApplicationService";
import {
  TableCaption,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
  Table,
} from "@/components/ui/table";
import { CreateInternshipDialog } from "../dashboard/company/internships/Dialog/CreateInternshipDialog";
import { useNavigate } from "react-router-dom";

export function UserApplicationsList() {
  const [applications, setApplications] = useState<ApplicationListItemDto[]>(
    []
  );
  const navigate = useNavigate();

  const getData = async () => {
    const data = await getAllUserApplications();
    if (data) {
      setApplications(data);
    }
  };
  useEffect(() => {
    getData();
  }, []);

  return (
    <div className="flex flex-col">
      <div>
        <Table>
          <TableCaption>A list of applications you have applied.</TableCaption>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">Company</TableHead>
              <TableHead className="w-[100px]">Company Email</TableHead>
              <TableHead className="w-[100px]">Position</TableHead>
              <TableHead className="w-[100px]">Step</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {applications.map((app) => (
              <TableRow
                key={app.id}
                className="hover:cursor-pointer"
                onClick={() => navigate(`/applications/${app.id}`)}
              >
                <TableCell className="font-medium">
                  {app.internshipDto.companyDto.name}
                </TableCell>
                <TableCell className="font-medium">
                  {app.internshipDto.companyDto.email}
                </TableCell>
                <TableCell className="font-medium">
                  {app.internshipDto.name}
                </TableCell>
                <TableCell className="font-medium">{app.stepName}</TableCell>
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
}
