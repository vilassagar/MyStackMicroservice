
import type { LayoutProps } from '../../types';
// Layout Component
const AppLayout: React.FC<LayoutProps> = ({ children, darkMode, isCollapsed }) => {
  return (
    <div className={`flex h-screen ${darkMode ? 'dark' : ''}`}>
      {children}
    </div>
  );
};

export default AppLayout;