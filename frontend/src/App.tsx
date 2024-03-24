import "./App.css";
import { useDispatch, useSelector } from "react-redux";
import { selectUser } from "./features/AuthSlice";
import LayoutManager from "./layouts/LayoutManager";
import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/auth/LoginPage";
import InternshipsPage from "./pages/internship/InternshipsPage";
import { Toaster } from "./components/ui/toaster";
import ApplicationPage from "./pages/application/ApplicationPage";
import { COMPANY, SITE_USER } from "./interfaces/Auth/Roles";
import CompanyInternships from "./pages/dashboard/company/internships/CompanyInternships";
import CompanyApplicationsList from "./pages/dashboard/company/internships/CompanyApplicationsList";
import { UserApplicationsList } from "./pages/internship/UserApplicationsList";
import DecisionsList from "./pages/dashboard/decisions/DecisionsList";
import RegisterPage from "./pages/auth/RegisterPage";
import DecisionPage from "./pages/dashboard/decisions/DecisionPage";
import { LinearProgress } from "@mui/material";
import { selectIsLoading } from "./features/GlobalLoaderSlice";

function App() {
  const user = useSelector(selectUser);
  const isLoading = useSelector(selectIsLoading);

  return (
    <>
      {isLoading && (
        <div className="fixed top-0 z-[1000] w-full">
          <LinearProgress />
        </div>
      )}

      <Toaster />
      {user ? (
        <LayoutManager role={user.role}>
          <Routes>
            <>
              {(() => {
                switch (user.role) {
                  case SITE_USER:
                    return (
                      <>
                        {/* <Route path="*" element={<Navigate to="/error" replace />} />
                      <Route path="/error" element={<NotFoundPage />} /> */}
                        <Route
                          path="*"
                          element={<Navigate to="/internships" replace />}
                        />
                        <Route
                          path="/internships"
                          element={<InternshipsPage />}
                        />
                        <Route
                          path="/applications/:applicationId"
                          element={<ApplicationPage />}
                        />
                        <Route
                          path="/applications"
                          element={<UserApplicationsList />}
                        />
                      </>
                    );
                  case COMPANY:
                    return (
                      <>
                        <Route
                          path="*"
                          element={<Navigate to="/internships" replace />}
                        />
                        <Route
                          path="/internships"
                          element={<CompanyInternships />}
                        />
                        <Route path="/decisions" element={<DecisionsList />} />
                        <Route
                          path="/decisions/:applicationId"
                          element={<DecisionPage />}
                        />
                        <Route
                          path="/internships/:internshipId"
                          element={<CompanyApplicationsList />}
                        />
                      </>
                    );
                }
              })()}
            </>
          </Routes>
        </LayoutManager>
      ) : (
        <Routes>
          <Route path="*" element={<Navigate to="/sign-in" replace />} />
          <Route path="/sign-in" element={<LoginPage />} />
          <Route path="/sign-up" element={<RegisterPage />} />
        </Routes>
      )}
    </>
  );
}

export default App;
