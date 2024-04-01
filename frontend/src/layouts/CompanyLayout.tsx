import CompanySidebar from "@/pages/dashboard/CompanySidebar";

interface LayoutProps {
  children: React.ReactNode;
}
const CompanyLayout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <CompanySidebar children={children} />
    </>
  );
};

export default CompanyLayout;
