
import { Menu, X, Bell, User } from 'lucide-react';
import type { HeaderProps } from '../../types';
// Header Component
const AppHeader: React.FC<HeaderProps> = ({
  darkMode,
  activeItem,
  isSidebarOpen,
  onToggleSidebar,
  onToggleDarkMode,
  menuItems
}) => {
  return (
    <header className={`${
      darkMode ? 'bg-gray-900 border-gray-700' : 'bg-white border-gray-200'
    } border-b shadow-sm`}>
      {/* Top Bar */}
      <div className="flex items-center justify-between p-3.5">
        <div className="flex items-center space-x-4">
          {/* Mobile menu button */}
          <button
            onClick={onToggleSidebar}
            className={`md:hidden p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors ${
              darkMode ? 'text-gray-300' : 'text-gray-600'
            }`}
          >
            {isSidebarOpen ? <X size={20} /> : <Menu size={20} />}
          </button>
          
          <div>
            <h2 className={`text-xl font-semibold ${
              darkMode ? 'text-white' : 'text-gray-900'
            }`}>
              {menuItems.find(item => item.id === activeItem)?.label || 'Dashboard'}
            </h2>
            {/* <p className={`text-sm ${
              darkMode ? 'text-gray-400' : 'text-gray-500'
            }`}>
              Welcome back, John! Here's what's happening today.
            </p> */}
          </div>
        </div>

        <div className="flex items-center space-x-2">
          {/* Quick Actions */}
          <div className="hidden md:flex items-center space-x-2">
            <button className={`px-3 py-1.5 text-sm font-medium rounded-lg transition-colors ${
              darkMode 
                ? 'bg-blue-600 hover:bg-blue-700 text-white' 
                : 'bg-blue-600 hover:bg-blue-700 text-white'
            }`}>
              New Project
            </button>
            <button className={`px-3 py-1.5 text-sm font-medium rounded-lg transition-colors ${
              darkMode 
                ? 'border border-gray-600 text-gray-300 hover:bg-gray-800' 
                : 'border border-gray-300 text-gray-700 hover:bg-gray-50'
            }`}>
              Export
            </button>
          </div>

          {/* User Controls */}
          <div className="flex items-center space-x-2">
            <button
              onClick={onToggleDarkMode}
              className={`p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors ${
                darkMode ? 'text-gray-300' : 'text-gray-600'
              }`}
              title="Toggle theme"
            >
              {darkMode ? '☀️' : '🌙'}
            </button>
            
            <button className={`p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors relative ${
              darkMode ? 'text-gray-300' : 'text-gray-600'
            }`}>
              <Bell size={20} />
              <span className="absolute top-1 right-1 w-2 h-2 bg-red-500 rounded-full"></span>
            </button>

            {/* User Avatar Menu */}
            <div className="relative group">
              <button className="flex items-center space-x-2 p-1.5 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors">
                <div className="w-8 h-8 bg-gradient-to-r from-green-400 to-blue-500 rounded-full flex items-center justify-center">
                  <User size={16} className="text-white" />
                </div>
                <span className={`hidden md:block text-sm font-medium ${
                  darkMode ? 'text-white' : 'text-gray-900'
                }`}>
                  John D.
                </span>
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Breadcrumb Navigation */}
      <div className={`px-4 pb-3 border-t ${
        darkMode ? 'border-gray-700' : 'border-gray-100'
      }`}>
        <nav className="flex items-center space-x-2 text-sm">
          <span className={`${darkMode ? 'text-gray-400' : 'text-gray-500'}`}>Home</span>
          <span className={`${darkMode ? 'text-gray-600' : 'text-gray-400'}`}>›</span>
          <span className={`${darkMode ? 'text-gray-400' : 'text-gray-500'}`}>
            {menuItems.find(item => item.id === activeItem)?.label}
          </span>
          <span className={`${darkMode ? 'text-gray-600' : 'text-gray-400'}`}>›</span>
          <span className={`font-medium ${darkMode ? 'text-white' : 'text-gray-900'}`}>
            Overview
          </span>
        </nav>
      </div>
    </header>
  );
};

export default AppHeader;