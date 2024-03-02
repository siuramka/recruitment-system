interface LayoutProps {
  children: React.ReactNode;
}
const CompanyLayout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <div className="container mx-auto">{children}</div>
    </>
  );
};

export default CompanyLayout;
