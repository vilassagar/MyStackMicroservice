import { ChevronLeft, Search, User,LogOut, BarChart3, Home, Users, Mail, Calendar, FileText, Settings } from "lucide-react";
import type { SidebarProps,MenuItem } from '../../types';
// Sidebar Component

const AppSidebar: React.FC<SidebarProps> = ({
  isOpen,
  isCollapsed,
  darkMode,
  activeItem,
  onToggleCollapse,
  onItemClick
}) => {
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

  return (
    <div className={`
      fixed inset-y-0 left-0 z-50 
      ${isCollapsed ? 'w-16' : 'w-64'} 
      ${isOpen ? 'translate-x-0' : '-translate-x-full'}
      md:translate-x-0 md:static md:inset-0 md:flex-shrink-0
      transition-all duration-300 ease-in-out
      ${darkMode ? 'bg-gray-900 border-gray-700' : 'bg-white border-gray-200'}
      border-r shadow-lg md:shadow-none
    `}>
      {/* Sidebar Header */}
      <div className={`flex items-center ${isCollapsed ? 'justify-center' : 'justify-between'} p-4 border-b ${
        darkMode ? 'border-gray-700' : 'border-gray-200'
      }`}>
        {isCollapsed ? (
          <div className="relative group">
            <button
              onClick={onToggleCollapse}
              className="w-8 h-8 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg flex items-center justify-center hover:scale-105 transition-transform"
              title="Expand Sidebar"
            >
              <span className="text-white font-bold text-sm">A</span>
            </button>
            {/* Tooltip for expand */}
            <div className={`
              absolute left-full ml-2 px-3 py-2 rounded-lg text-sm font-medium
              opacity-0 invisible group-hover:opacity-100 group-hover:visible
              transition-all duration-200 z-50 whitespace-nowrap
              ${darkMode ? 'bg-gray-800 text-white border border-gray-700' : 'bg-white text-gray-900 border border-gray-200 shadow-lg'}
              top-1/2 transform -translate-y-1/2
            `}>
              Expand Sidebar
              {/* Arrow */}
              <div className={`absolute right-full top-1/2 transform -translate-y-1/2 w-0 h-0 border-y-4 border-r-4 border-transparent ${
                darkMode ? 'border-r-gray-800' : 'border-r-white'
              }`}></div>
            </div>
          </div>
        ) : (
          <div className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-sm">A</span>
            </div>
            <h1 className={`font-bold text-lg ${
              darkMode ? 'text-white' : 'text-gray-900'
            }`}>
              AppName
            </h1>
          </div>
        )}
        
        {/* Collapse button */}
        {!isCollapsed && (
          <button
            onClick={onToggleCollapse}
            className={`p-1.5 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors ${
              darkMode ? 'text-gray-300' : 'text-gray-600'
            }`}
            title="Collapse Sidebar"
          >
            <ChevronLeft size={16} />
          </button>
        )}
      </div>

      {/* Search Bar */}
      {!isCollapsed && (
        <div className="p-4">
          <div className={`relative ${
            darkMode ? 'text-gray-300' : 'text-gray-600'
          }`}>
            <Search size={18} className="absolute left-3 top-1/2 transform -translate-y-1/2" />
            <input
              type="text"
              placeholder="Search..."
              className={`w-full pl-10 pr-4 py-2 rounded-lg border transition-colors ${
                darkMode 
                  ? 'bg-gray-800 border-gray-700 text-white placeholder-gray-400 focus:border-blue-500' 
                  : 'bg-gray-50 border-gray-200 text-gray-900 placeholder-gray-500 focus:border-blue-500'
              } focus:outline-none focus:ring-2 focus:ring-blue-500/20`}
            />
          </div>
        </div>
      )}

      {/* Navigation Menu */}
      <nav className="flex-1 p-4 space-y-2">
        {menuItems.map((item) => (
          <div key={item.id} className="relative group">
            <button
              onClick={() => onItemClick(item.id, item.path)}
              className={`
                w-full flex items-center ${isCollapsed ? 'justify-center px-3 py-3' : 'space-x-3 px-3 py-2.5'} rounded-lg transition-all duration-200
                ${activeItem === item.id
                  ? darkMode 
                    ? 'bg-blue-600 text-white shadow-lg' 
                    : 'bg-blue-50 text-blue-600 border border-blue-200'
                  : darkMode
                    ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                    : 'text-gray-700 hover:bg-gray-100'
                }
              `}
            >
              <span className="flex-shrink-0 relative">
                {item.icon}
                {item.badge && isCollapsed && (
                  <span className="absolute -top-1 -right-1 w-2 h-2 bg-red-500 rounded-full"></span>
                )}
              </span>
              
              {!isCollapsed && (
                <>
                  <span className="font-medium flex-1 text-left">{item.label}</span>
                  {item.badge && (
                    <span className={`px-2 py-0.5 text-xs font-medium rounded-full ${
                      activeItem === item.id
                        ? 'bg-white text-blue-600'
                        : 'bg-red-500 text-white'
                    }`}>
                      {item.badge}
                    </span>
                  )}
                </>
              )}
            </button>
            
            {/* Tooltip for collapsed state */}
            {isCollapsed && (
              <div className={`
                absolute left-full ml-2 px-3 py-2 rounded-lg text-sm font-medium
                opacity-0 invisible group-hover:opacity-100 group-hover:visible
                transition-all duration-200 z-50 whitespace-nowrap
                ${darkMode ? 'bg-gray-800 text-white border border-gray-700' : 'bg-white text-gray-900 border border-gray-200 shadow-lg'}
                top-1/2 transform -translate-y-1/2
              `}>
                {item.label}
                {item.badge && (
                  <span className="ml-2 px-1.5 py-0.5 text-xs bg-red-500 text-white rounded-full">
                    {item.badge}
                  </span>
                )}
                {/* Arrow */}
                <div className={`absolute right-full top-1/2 transform -translate-y-1/2 w-0 h-0 border-y-4 border-r-4 border-transparent ${
                  darkMode ? 'border-r-gray-800' : 'border-r-white'
                }`}></div>
              </div>
            )}
          </div>
        ))}
      </nav>

      {/* User Profile Section */}
      <div className={`p-4 border-t ${
        darkMode ? 'border-gray-700' : 'border-gray-200'
      }`}>
        {!isCollapsed ? (
          <div className="space-y-2">
            <div className={`flex items-center space-x-3 p-2 rounded-lg ${
              darkMode ? 'bg-gray-800' : 'bg-gray-50'
            }`}>
              <div className="w-8 h-8 bg-gradient-to-r from-green-400 to-blue-500 rounded-full flex items-center justify-center">
                <User size={16} className="text-white" />
              </div>
              <div className="flex-1 min-w-0">
                <p className={`text-sm font-medium truncate ${
                  darkMode ? 'text-white' : 'text-gray-900'
                }`}>
                  John Doe
                </p>
                <p className={`text-xs truncate ${
                  darkMode ? 'text-gray-400' : 'text-gray-500'
                }`}>
                  john@example.com
                </p>
              </div>
            </div>
            
            <button className={`w-full flex items-center space-x-3 px-3 py-2 rounded-lg transition-colors ${
              darkMode 
                ? 'text-gray-300 hover:bg-gray-800 hover:text-white' 
                : 'text-gray-700 hover:bg-gray-100'
            }`}>
              <LogOut size={16} />
              <span className="text-sm">Logout</span>
            </button>
          </div>
        ) : (
          <div className="flex flex-col space-y-2 items-center">
            <div className="w-8 h-8 bg-gradient-to-r from-green-400 to-blue-500 rounded-full flex items-center justify-center">
              <User size={16} className="text-white" />
            </div>
            <button className={`p-2 rounded-lg transition-colors ${
              darkMode 
                ? 'text-gray-300 hover:bg-gray-800 hover:text-white' 
                : 'text-gray-700 hover:bg-gray-100'
            }`}>
              <LogOut size={16} />
            </button>
          </div>
        )}
      </div>
    </div>
  );
};


export default AppSidebar;