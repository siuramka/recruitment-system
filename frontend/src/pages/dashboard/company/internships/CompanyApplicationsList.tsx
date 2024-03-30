import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getAllInternshipApplications } from "../../../../services/ApplicationService";
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
import { CreateInternshipDialog } from "./Dialog/CreateInternshipDialog";
import { InternshipDto } from "../../../../interfaces/Internship/InternshipDto";
import { ApplicationSheet } from "./Sheet/ApplicantionSheet";
import { Badge } from "@/components/ui/badge";
import { useDispatch } from "react-redux";
import { hideLoader, showLoader } from "@/features/GlobalLoaderSlice";
import NoDataAlert from "@/pages/not-found/NoDataAlert";

const CompanyApplicationsList = () => {
  const [applications, setApplications] = useState<ApplicationListItemDto[]>(
    []
  );
  const [internship, setInternsihp] = useState<InternshipDto>();
  const [refreshState, setRefreshState] = useState(false);
  const [noData, setNoData] = useState(false);
  const { internshipId } = useParams() as { internshipId: string };
  const dispatch = useDispatch();

  const handleRefresh = () => {
    setRefreshState((prevRefreshState) => !prevRefreshState);
  };

  const getData = async () => {
    var data = await getAllInternshipApplications({ internshipId });
    if (data?.length === 0) {
      setNoData(true);
      dispatch(hideLoader());
      return;
    }

    if (data) {
      setApplications(data);
      setInternsihp(data[0].internshipDto);
    }
    dispatch(hideLoader());
  };

  useEffect(() => {
    dispatch(showLoader());
    getData();
  }, [refreshState]);

  return (
    <div className="flex flex-col">
      <div className="space-y-0.5">
        <h2 className="text-2xl font-bold tracking-tight">Applications </h2>
      </div>
      <div>
        {noData ? (
          <NoDataAlert />
        ) : (
          <Table>
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
                    <ApplicationSheet
                      appId={app.id}
                      internshipId={internshipId}
                      handleRefresh={handleRefresh}
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </div>
    </div>
  );
};

export default CompanyApplicationsList;

// Table list of applicants
