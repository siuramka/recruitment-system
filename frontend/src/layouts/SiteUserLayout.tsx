interface LayoutProps {
  children: React.ReactNode;
}
const SellerLayout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <div className="container mx-auto">{children}</div>
    </>
  );
};

export default SellerLayout;
