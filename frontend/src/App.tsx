import "./App.css";
import { useSelector } from "react-redux";
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

function App() {
  const user = useSelector(selectUser);

  return (
    <>
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
                          element={
                            <Navigate to="/company/internships" replace />
                          }
                        />
                        <Route
                          path="/company/internships"
                          element={<CompanyInternships />}
                        />
                        <Route
                          path="/company/internships/:internshipId"
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
        </Routes>
      )}
    </>
  );
}

export default App;
