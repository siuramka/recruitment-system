import { useEffect, useState } from "react";
import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import { getAllInternships } from "@/services/InternshipService";
import { Button } from "@/components/ui/button";
import { createApplication } from "@/services/ApplicationService";
import { useToast } from "@/components/ui/use-toast";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { useForm } from "react-hook-form";
import { Separator } from "@/components/ui/separator";
import { Badge } from "@/components/ui/badge";

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
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-4">
        <div className="lg:col-span-4">
          <div className="flex flex-col overflow-auto max-h-[75vh]">
            {internships.map((internship) => (
              <div
                key={internship.id}
                className={`cursor-pointer hover:shadow-lg hover:bg-gray-100 mb-4 p-4 rounded-md border border-gray-300
            flex flex-col items-start ${
              internship.id === activeInternship?.id
                ? "bg-blue-100 shadow-md border-blue-500"
                : "inactive-card"
            }`}
                onClick={() => setActiveInternship(internship)}
              >
                <h2 className="text-xl font-semibold mb-2">
                  {internship.name}
                </h2>
                <p className="text-gray-600 mb-1">{internship.address}</p>
                <div className="flex items-center gap-2">
                  <span
                    className={`text-sm ${
                      internship.isPaid ? "text-green-500" : "text-red-500"
                    }`}
                  >
                    {internship.isPaid ? "Paid" : "Unpaid"}
                  </span>
                  <span className="text-gray-400 text-xs">
                    {new Date(internship.createdAt).toLocaleDateString()}
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>
        <div className="lg:col-span-8 p-4 lg:p-8 border rounded-md min-h-[75vh] shadow-lg bg-white">
          <div className="flex justify-between items-center mb-6">
            <div className="font-bold text-2xl">
              {activeInternship?.name || "Select an Internship"}
            </div>
            <Button
              className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
              onClick={applyAction}
            >
              Apply
            </Button>
          </div>
          {activeInternship ? (
            <>
              <div className="text-lg mb-4">
                {activeInternship?.companyDto.name},{" "}
                {activeInternship?.companyDto.location}
              </div>
              <Separator className="my-3" />
              <div className="mb-4 font-semibold">About the Internship:</div>
              {activeInternship?.description
                .split("\n")
                .map((paragraph, index) => (
                  <p key={index} className="mb-2">
                    {paragraph}
                  </p>
                ))}
              <Separator className="my-3" />
              <div className="font-semibold my-3">Additional information:</div>
              <span>
                {activeInternship?.isPaid && <Badge>Paid position</Badge>}
                {activeInternship?.isRemote && <Badge>Remote work</Badge>}
              </span>
              <div className="mb-4 mt-3">
                Requirements: {activeInternship?.requirements}
              </div>
              <div className="mb-4">
                Nice to have skills: {activeInternship?.skills}
              </div>
              <div className="mb-4">
                Available spots: {activeInternship?.slotsAvailable}
              </div>
              <Separator className="my-3" />
              <div className="font-semibold">About the company:</div>
              <div className="ml-4">
                <div className="font-semibold mb-1">
                  {activeInternship?.companyDto.name}
                </div>
                <div className="text-gray-600 mb-1">
                  Contact: {activeInternship?.companyDto.phoneNumber}
                </div>
                <div className="mb-2">
                  <a
                    href={`mailto:${activeInternship?.contactEmail}`}
                    className="text-blue-500 underline mr-2"
                  >
                    Email us
                  </a>
                  <a
                    href={activeInternship?.companyDto.website}
                    target="_blank"
                    rel="noreferrer"
                    className="text-blue-500 underline"
                  >
                    Website
                  </a>
                </div>
              </div>
            </>
          ) : (
            <div className="text-center text-gray-500">
              No internship selected.
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default InternshipsPage;
