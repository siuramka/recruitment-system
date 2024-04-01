import { Button } from "@/components/ui/button";
import { toast } from "@/components/ui/use-toast";
import { removeUser } from "@/features/AuthSlice";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";

interface LayoutProps {
  children: React.ReactNode;
}

const SellerLayout: React.FC<LayoutProps> = ({ children }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(removeUser());
    toast({ title: "Logged out" });
  };

  return (
    <>
      <nav className="bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
        <div className="container mx-auto px-4 py-3 lg:px-8">
          <div className="flex items-center justify-between">
            <div className="flex items-center">
              <a href="" className="flex items-center">
                <span className="text-xl font-bold sm:text-2xl text-gray-800 dark:text-white">
                  Internship selection system
                </span>
              </a>
            </div>
            <div className="flex items-center space-x-4">
              <Button variant="ghost" onClick={() => navigate("/internships")}>
                Internships
              </Button>
              <Button variant="ghost" onClick={() => navigate("/applications")}>
                Applications
              </Button>
              <Button variant="ghost" onClick={handleLogout}>
                Logout
              </Button>
            </div>
          </div>
        </div>
      </nav>
      <div className="container mx-auto mt-8">{children}</div>
    </>
  );
};

export default SellerLayout;
