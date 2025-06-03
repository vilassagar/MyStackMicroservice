import { useState } from "react";
import { BrowserRouter } from "react-router-dom";
import type { MenuItem } from "./types";
import React from "react";
import AppSidebar from "./components/Layout/AppSidebar";
import AppLayout from "./components/Layout/AppLayout";
import RouterContent from "./routes/RouterContent";
import AppHeader from "./components/Layout/AppHeader";
import AppFooter from "./components/Layout/AppFooter";
import { BarChart3, Calendar, FileText, Home, Mail, Settings, Users } from "lucide-react";

const App: React.FC = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isCollapsed, setIsCollapsed] = useState(false);
    const [activeItem, setActiveItem] = useState('dashboard');
    const [darkMode, setDarkMode] = useState(false);
    const [currentPath, setCurrentPath] = useState('/dashboard');

    // Mock navigation hook (replace with actual useNavigate when using react-router-dom)
    // const navigate = useNavigate();
    // const location = useLocation();

    const menuItems: MenuItem[] = [
        {
            id: 'dashboard',
            label: 'Dashboard',
            icon: <Home size={20} />,
            path: '/dashboard'
        },
        {
            id: 'analytics',
            label: 'Analytics',
            icon: <BarChart3 size={20} />,
            badge: '2',
            path: '/analytics'
        },
        {
            id: 'users',
            label: 'Users',
            icon: <Users size={20} />,
            path: '/users'
        },
        {
            id: 'messages',
            label: 'Messages',
            icon: <Mail size={20} />,
            badge: '5',
            path: '/messages'
        },
        {
            id: 'calendar',
            label: 'Calendar',
            icon: <Calendar size={20} />,
            path: '/calendar'
        },
        {
            id: 'documents',
            label: 'Documents',
            icon: <FileText size={20} />,
            path: '/documents'
        },
        {
            id: 'settings',
            label: 'Settings',
            icon: <Settings size={20} />,
            path: '/settings'
        }
    ];

    const toggleSidebar = () => {
        setIsSidebarOpen(!isSidebarOpen);
    };

    const toggleCollapse = () => {
        setIsCollapsed(!isCollapsed);
    };

    const handleItemClick = (itemId: string, path: string) => {
        setActiveItem(itemId);
        setCurrentPath(path);
        // navigate(path); // Uncomment when using react-router-dom

        // Close mobile sidebar when item is clicked
        if (window.innerWidth < 768) {
            setIsSidebarOpen(false);
        }
    };

    const toggleDarkMode = () => {
        setDarkMode(!darkMode);
    };

    // Sync active item with current route (when using real router)
    React.useEffect(() => {
        const currentMenuItem = menuItems.find(item => item.path === currentPath);
        if (currentMenuItem) {
            setActiveItem(currentMenuItem.id);
        }
    }, [currentPath, menuItems]);

    return (
        <BrowserRouter>
            <AppLayout darkMode={darkMode} isCollapsed={isCollapsed}>
                <>
                    <AppSidebar
                        isOpen={isSidebarOpen}
                        isCollapsed={isCollapsed}
                        darkMode={darkMode}
                        activeItem={activeItem}
                        onToggleCollapse={toggleCollapse}
                        onItemClick={handleItemClick}
                    />

                    {/* Mobile Overlay */}
                    {isSidebarOpen && (
                        <div
                            className="fixed inset-0 bg-black/50 z-40 md:hidden"
                            onClick={toggleSidebar}
                        />
                    )}

                    {/* Main Content Area */}
                    <div className="flex-1 flex flex-col">
                        <AppHeader
                            darkMode={darkMode}
                            activeItem={activeItem}
                            isCollapsed={isCollapsed}
                            isSidebarOpen={isSidebarOpen}
                            onToggleSidebar={toggleSidebar}
                            onToggleDarkMode={toggleDarkMode}
                            menuItems={menuItems}
                            currentPath={currentPath}
                        />

                        {/* Router Content */}
                        <RouterContent darkMode={darkMode} currentPath={currentPath} />

                        {/* Uncomment below when using react-router-dom */}
                        {/* 
            <Routes>
              <Route path="/dashboard" element={<DashboardPage darkMode={darkMode} />} />
              <Route path="/analytics" element={<AnalyticsPage darkMode={darkMode} />} />
              <Route path="/users" element={<UsersPage darkMode={darkMode} />} />
              <Route path="/messages" element={<MessagesPage darkMode={darkMode} />} />
              <Route path="/calendar" element={<CalendarPage darkMode={darkMode} />} />
              <Route path="/documents" element={<DocumentsPage darkMode={darkMode} />} />
              <Route path="/settings" element={<SettingsPage darkMode={darkMode} />} />
              <Route path="/" element={<DashboardPage darkMode={darkMode} />} />
            </Routes>
            */}

                        <AppFooter darkMode={darkMode} />
                    </div>
                </>
            </AppLayout>
        </BrowserRouter>
    );
};

export default App;