import { useEffect, useState } from "react";
import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import { getAllInternships } from "@/services/InternshipService";
import { Button } from "@/components/ui/button";
import { createApplication } from "@/services/ApplicationService";
import { useToast } from "@/components/ui/use-toast";
import { useNavigate } from "react-router-dom";

const InternshipsPage = () => {
  const [internships, setInternships] = useState<InternshipDto[]>([]);
  const [activeInternship, setActiveInternship] = useState<InternshipDto>();
  const navigate = useNavigate();
  const { toast } = useToast();

  const getData = async () => {
    const internshipsData = await getAllInternships();
    if (internshipsData) {
      setInternships(internshipsData);
      setActiveInternship(internshipsData[0]);
    }
  };

  const applyAction = async () => {
    if (activeInternship) {
      const createdApplication = await createApplication({
        internshipId: activeInternship?.id,
      });

      if (createdApplication) {
        toast({
          title: "Successfully applied to internship!",
          description: `${activeInternship.name} at ${activeInternship.companyDto.name}`,
        });
        navigate(`/internships/${activeInternship.id}/application`);
      }
    }
  };

  useEffect(() => {
    console.log("hello");
    getData();
  }, []);

  return (
    <div>
      <div className="grid grid-cols-12">
        <div className="col-span-4">
          <div className="flex flex-col overflow-auto max-h-svh">
            {internships.map((internship) => (
              <div
                key={internship.id}
                className={`cursor-pointer hover:shadow-lg hover:bg-gray-100 mb-4 p-4 rounded-md border border-gray-300
                flex flex-col items-start ${
                  internship.id === activeInternship?.id
                    ? "bg-gray-100 shadow-lg"
                    : "inactive-card"
                }`}
                onClick={() => setActiveInternship(internship)}
              >
                <h2 className="text-xl font-semibold mb-2">
                  {internship.name}
                </h2>
                <p className="text-gray-600 mb-2">{internship.address}</p>
                <p className="text-gray-600 mb-2">
                  Paid: {internship.isPaid.toString()}
                </p>
                <p className="text-gray-600">
                  {internship.createdAt.toString()}
                </p>
              </div>
            ))}
          </div>
        </div>
        <div className="col-span-8 ml-3 p-8 border rounded-md min-h-svh shadow-lg">
          <div className="flex items-start mb-4">
            <Button onClick={applyAction}>Apply</Button>
          </div>
          <div className="flex flex-col items-start">
            <div className="mb-4 font-bold text-2xl">
              {activeInternship?.name}
            </div>
            <div className="mb-4">
              {activeInternship?.companyDto.name},{" "}
              {activeInternship?.companyDto.location},{" "}
              {activeInternship?.startDate.toString()} -{" "}
              {activeInternship?.endDate.toString()}
            </div>
            <div className="mb-4">About the Internship:</div>
            <div>{activeInternship?.description}</div>
            <div>Paid: {activeInternship?.isPaid.toString()}</div>
            <div>Remote: {activeInternship?.isPaid.toString()}</div>
            <div>Requirements: {activeInternship?.requirements}</div>
            <div>Nice to have skills: {activeInternship?.skills}</div>
            <div>Available spots: {activeInternship?.slotsAvailable}</div>
          </div>
          <div>About the company:</div>
          <div className="flex flex-col items-start">
            <div>{activeInternship?.companyDto.name}</div>
            <div>{activeInternship?.companyDto.phoneNumber}</div>
            <div>{activeInternship?.contactEmail}</div>
            <div>{activeInternship?.companyDto.website}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default InternshipsPage;
