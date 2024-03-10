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
import { DialogDemo } from "../../internship/DialogDemo";
import { useNavigate } from "react-router-dom";

const CompanyInternships = () => {
  const [internships, setInternships] = useState<InternshipDto[]>([]);
  const navigate = useNavigate();

  const getData = async () => {
    const internshipsData = await getAllInternships();
    if (internshipsData) {
      setInternships(internshipsData);
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
          <TableCaption>A list of your internships.</TableCaption>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">Name</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {internships.map((internship) => (
              <TableRow
                key={internship.id}
                onClick={() =>
                  navigate(`/company/internships/${internship.id}`)
                }
                className="hover:cursor-pointer"
              >
                <TableCell className="font-medium">{internship.name}</TableCell>
                <TableCell className="font-medium">
                  {internship.description}
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

export default CompanyInternships;