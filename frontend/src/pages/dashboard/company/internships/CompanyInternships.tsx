import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import { getAllInternships } from "@/services/InternshipService";
import { useEffect, useState } from "react";
import { CreateInternshipDialog } from "./Dialog/CreateInternshipDialog";
import { useNavigate } from "react-router-dom";
import { hideLoader, showLoader } from "@/features/GlobalLoaderSlice";
import { useDispatch } from "react-redux";
import NoDataAlert from "@/pages/not-found/NoDataAlert";

const CompanyInternships = () => {
  const [internships, setInternships] = useState<InternshipDto[]>([]);
  const [refreshState, setRefreshState] = useState(false);
  const [noData, setNoData] = useState(false);
  const dispatch = useDispatch();

  const navigate = useNavigate();

  const handleRefresh = () => {
    setRefreshState((prevRefreshState) => !prevRefreshState);
  };

  const getData = async () => {
    dispatch(showLoader());
    const internshipsData = await getAllInternships();
    if (internshipsData?.length === 0) {
      setNoData(true);
      dispatch(hideLoader());
      return;
    }
    if (internshipsData) {
      setInternships(internshipsData);
      dispatch(hideLoader());
    }
  };

  useEffect(() => {
    getData();
  }, [refreshState]);

  return (
    <div className="flex flex-col">
      <div className="space-y-0.5">
        <h2 className="text-2xl font-bold tracking-tight">Internships</h2>
        <p className="text-muted-foreground">
          Information about internships at your company
        </p>
      </div>
      <span className="flex justify-end">
        <CreateInternshipDialog handleRefresh={handleRefresh} />
      </span>
      <div>
        {noData ? (
          <NoDataAlert />
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-[100px]">Name</TableHead>
                <TableHead className="w-[100px]">Created At</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {internships.map((internship) => (
                <TableRow
                  key={internship.id}
                  onClick={() => navigate(`/internships/${internship.id}`)}
                  className="hover:cursor-pointer"
                >
                  <TableCell className="font-medium">
                    {internship.name}
                  </TableCell>
                  <TableCell className="font-medium">
                    {new Date(internship.createdAt).toDateString()}
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

export default CompanyInternships;
