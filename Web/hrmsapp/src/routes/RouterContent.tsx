import DashboardPage from '../pages/DashboardPage';
import AnalyticsPage from '../pages/AnalyticsPage';
import UsersPage from '../pages/UsersPage';
import MessagesPage from '../pages/MessagesPage';
import CalendarPage from '../pages/CalendarPage';
import DocumentsPage from '../pages/DocumentsPage';
import SettingsPage from '../pages/SettingsPage';

// Router-aware Main Content Component
 const RouterContent: React.FC<{ darkMode: boolean; currentPath: string }> = ({ darkMode, currentPath }) => {
  const getPageComponent = () => {
    switch (currentPath) {
      case '/dashboard':
        return <DashboardPage darkMode={darkMode} />;
      case '/analytics':
        return <AnalyticsPage darkMode={darkMode} />;
      case '/users':
        return <UsersPage darkMode={darkMode} />;
      case '/messages':
        return <MessagesPage darkMode={darkMode} />;
      case '/calendar':
        return <CalendarPage darkMode={darkMode} />;
      case '/documents':
        return <DocumentsPage darkMode={darkMode} />;
      case '/settings':
        return <SettingsPage darkMode={darkMode} />;
      default:
        return <DashboardPage darkMode={darkMode} />;
    }
  };

  return (
    <main className={`flex-1 p-6 overflow-y-auto ${
      darkMode ? 'bg-gray-800' : 'bg-gray-50'
    }`}>
      {getPageComponent()}
    </main>
  );
};

export default RouterContent;