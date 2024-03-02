import "./App.css";
import { useSelector } from "react-redux";
import { selectUser } from "./features/AuthSlice";
import LayoutManager from "./layouts/LayoutManager";
import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/auth/LoginPage";
import InternshipsPage from "./pages/internship/InternshipsPage";
import { Toaster } from "./components/ui/toaster";
import ApplicationPage from "./pages/application/ApplicationPage";
import NotFoundPage from "./pages/not-found/NotFoundPage";

function App() {
  const user = useSelector(selectUser);
  return (
    <>
      <Toaster />
      {user ? (
        <LayoutManager role={user.role}>
          <Routes>
            <Route path="*" element={<Navigate to="/error" replace />} />
            <Route path="/error" element={<NotFoundPage />} />
            <Route path="/internships" element={<InternshipsPage />} />
            <Route
              path="/internships/:internshipId/application"
              element={<ApplicationPage />}
            />
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
