import { BarChart, Pen, Settings, TestTube } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface CompanySidebarProps {
  children: React.ReactNode;
}
const CompanySidebar = ({ children }: CompanySidebarProps) => {
  const navigate = useNavigate();

  return (
    <>
      <nav className="fixed top-0 z-50 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
        <div className="px-3 py-3 lg:px-5 lg:pl-3">
          <div className="flex items-center justify-between">
            <div className="flex items-center justify-start rtl:justify-end">
              <button
                data-drawer-target="logo-sidebar"
                data-drawer-toggle="logo-sidebar"
                aria-controls="logo-sidebar"
                type="button"
                className="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
              >
                <span className="sr-only">Open sidebar</span>
              </button>
              <a href="" className="flex ms-2 md:me-24">
                <span className="self-center text-xl font-semibold sm:text-2xl whitespace-nowrap dark:text-white">
                  Mariusoftas
                </span>
              </a>
            </div>
          </div>
        </div>
      </nav>

      <aside
        id="logo-sidebar"
        className="fixed top-0 left-0 z-40 w-64 h-screen pt-20 transition-transform -translate-x-full bg-white border-r border-gray-200 sm:translate-x-0 dark:bg-gray-800 dark:border-gray-700"
        aria-label="Sidebar"
      >
        <div className="h-full px-3 pb-4 overflow-y-auto bg-white dark:bg-gray-800">
          <ul className="space-y-2 font-medium">
            <li>
              <a
                onClick={() => navigate("/company/internships")}
                className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group hover:cursor-pointer"
              >
                <span className="ms-3 flex items-center">
                  <TestTube className="w-4 h-4 mr-2" />
                  Internships
                </span>
              </a>
            </li>
            <li>
              <a
                onClick={() => navigate("/company/decisions")}
                className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group hover:cursor-pointer"
              >
                <span className="ms-3 flex items-center">
                  <Pen className="w-4 h-4 mr-2" />
                  Decisions
                </span>
              </a>
            </li>
            <li>
              <a
                onClick={() => navigate("/company/statistics")}
                className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group hover:cursor-pointer"
              >
                <span className="ms-3 flex items-center">
                  <BarChart className="w-4 h-4 mr-2" />
                  Statistics
                </span>
              </a>
            </li>
            <li>
              <a
                onClick={() => navigate("/company/settings")}
                className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group hover:cursor-pointer"
              >
                <span className="ms-3 flex items-center">
                  <Settings className="w-4 h-4 mr-2" />
                  Settings
                </span>
              </a>
            </li>
          </ul>
        </div>
      </aside>
      <div className="pt-10 pl-64 ">
        <div className="space-y-6 p-10 pb-16 md:block">{children}</div>
      </div>
    </>
  );
};

export default CompanySidebar;
