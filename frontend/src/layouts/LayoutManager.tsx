import { COMPANY, SITE_USER } from "@/interfaces/Auth/Roles";
import CompanyLayout from "./CompanyLayout";
import SiteUserLayout from "./SiteUserLayout";

interface LayoutChooserProps {
  role: string;
  children: React.ReactNode;
}

const LayoutManager: React.FC<LayoutChooserProps> = ({ role, children }) => {
  switch (role) {
    case SITE_USER:
      return <SiteUserLayout>{children}</SiteUserLayout>;
    case COMPANY:
      return <CompanyLayout>{children}</CompanyLayout>;
  }
};

export default LayoutManager;
