import { InternshipDto } from "@/interfaces/Internship/InternshipDto";
import { getInternship } from "@/services/InternshipService";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import NextIcon from "./components/NextIcon";
import { ApplicationStepDto } from "@/interfaces/Step/ApplicationStepDto";
import { getApplicationSteps } from "@/services/StepService";
import { getApplication } from "@/services/ApplicationService";
import ScreeningSection from "./components/ScreeningSection";
import InterviewSection from "./components/InterviewSection";
import OfferSection from "./components/OfferSection";

const ApplicationPage = () => {
  const { internshipId } = useParams() as { internshipId: string };
  const [internship, setInternship] = useState<InternshipDto>();
  const [steps, setSteps] = useState<ApplicationStepDto[]>([]);
  const navigate = useNavigate();

  const currentStep = steps.filter((stepItem) => stepItem.isCurrentStep)[0];

  const getInternshipData = async () => {
    const internshipData = await getInternship({ internshipId });
    if (internshipData) {
      setInternship(internshipData);
    } else {
      navigate("/error");
    }
  };

  const getApplicaiton = async () => {
    const application = await getApplication({ internshipId });
    if (!application) {
      navigate("/error");
    }
  };

  const getApplicationStepsData = async () => {
    const applicationStepsData = await getApplicationSteps({ internshipId });

    if (applicationStepsData) {
      setSteps(applicationStepsData);
    }
  };

  useEffect(() => {
    getInternshipData();
    getApplicaiton();
    getApplicationStepsData();
  }, []);

  return (
    <>
      <div className="pb-3 ">
        <ol className="flex items-center w-full p-3 shadow-sm space-x-2 text-sm font-medium text-center text-gray-500 bg-white border border-gray-200 rounded-lg  dark:text-gray-400 sm:text-base dark:bg-gray-800 dark:border-gray-700 sm:p-4 sm:space-x-4 rtl:space-x-reverse">
          {steps.map((stepItem) => (
            <>
              {stepItem.positionAscending <= currentStep.positionAscending ? (
                <>
                  <span key={stepItem.positionAscending}>
                    <li className="flex items-center text-blue-600 dark:text-blue-500">
                      <span className="flex items-center justify-center w-5 h-5 me-2 text-xs border border-blue-600 rounded-full shrink-0 dark:border-blue-500">
                        {stepItem.positionAscending + 1}
                      </span>
                      <span className="hidden sm:inline-flex sm:ms-2">
                        {stepItem.stepType}
                      </span>
                    </li>
                  </span>
                </>
              ) : (
                <span key={stepItem.positionAscending}>
                  <li className="flex items-center">
                    <span className="flex items-center justify-center w-5 h-5 me-2 text-xs border border-gray-500 rounded-full shrink-0 dark:border-gray-400">
                      {stepItem.positionAscending + 1}
                    </span>
                    <span className="hidden sm:inline-flex sm:ms-2">
                      {stepItem.stepType}
                    </span>
                  </li>
                </span>
              )}
              <NextIcon />
            </>
          ))}
        </ol>
      </div>
      <div className="border rounded-md min-h-svh shadow-lg w-full p-4">
        {currentStep && currentStep.stepType == "Screening" && (
          <ScreeningSection internshipId={internshipId} />
        )}
        {currentStep && currentStep.stepType == "Intreview" && (
          <InterviewSection internshipId={internshipId} />
        )}
        {currentStep && currentStep.stepType == "Offer" && <OfferSection />}
      </div>
    </>
  );
};

export default ApplicationPage;
