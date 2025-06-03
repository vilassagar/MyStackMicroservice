import React from 'react';
import type { FooterProps } from '../../types';
// Footer Component
const AppFooter: React.FC<FooterProps> = ({ darkMode }) => {
  return (
    <footer className={`${
      darkMode ? 'bg-gray-900 border-gray-700' : 'bg-white border-gray-200'
    } border-t mt-auto`}>
      {/* Main Footer Content */}
      {/* <div className="px-6 py-8">
        <div className="max-w-4xl mx-auto">
          <div className="grid md:grid-cols-4 gap-8">
          
            <div className="md:col-span-1">
              <div className="flex items-center space-x-2 mb-4">
                <div className="w-8 h-8 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg flex items-center justify-center">
                  <span className="text-white font-bold text-sm">A</span>
                </div>
                <span className={`font-bold text-lg ${
                  darkMode ? 'text-white' : 'text-gray-900'
                }`}>
                  AppName
                </span>
              </div>
              <p className={`text-sm mb-4 ${
                darkMode ? 'text-gray-400' : 'text-gray-600'
              }`}>
                Building modern web applications with cutting-edge technology and exceptional user experience.
              </p>
              <div className="flex space-x-3">
                {['📧', '🐦', '📱', '💼'].map((icon, index) => (
                  <button
                    key={index}
                    className={`w-8 h-8 rounded-lg flex items-center justify-center transition-colors ${
                      darkMode 
                        ? 'bg-gray-800 hover:bg-gray-700 text-gray-300' 
                        : 'bg-gray-100 hover:bg-gray-200 text-gray-600'
                    }`}
                  >
                    {icon}
                  </button>
                ))}
              </div>
            </div>

  
            <div>
              <h3 className={`font-semibold mb-4 ${
                darkMode ? 'text-white' : 'text-gray-900'
              }`}>
                Quick Links
              </h3>
              <ul className="space-y-2">
                {['Dashboard', 'Analytics', 'Reports', 'Settings'].map((link) => (
                  <li key={link}>
                    <button className={`text-sm hover:underline transition-colors ${
                      darkMode ? 'text-gray-400 hover:text-white' : 'text-gray-600 hover:text-gray-900'
                    }`}>
                      {link}
                    </button>
                  </li>
                ))}
              </ul>
            </div>

  
            <div>
              <h3 className={`font-semibold mb-4 ${
                darkMode ? 'text-white' : 'text-gray-900'
              }`}>
                Resources
              </h3>
              <ul className="space-y-2">
                {['Documentation', 'API Reference', 'Tutorials', 'Community'].map((link) => (
                  <li key={link}>
                    <button className={`text-sm hover:underline transition-colors ${
                      darkMode ? 'text-gray-400 hover:text-white' : 'text-gray-600 hover:text-gray-900'
                    }`}>
                      {link}
                    </button>
                  </li>
                ))}
              </ul>
            </div>

       
            <div>
              <h3 className={`font-semibold mb-4 ${
                darkMode ? 'text-white' : 'text-gray-900'
              }`}>
                Support
              </h3>
              <ul className="space-y-2">
                {['Help Center', 'Contact Us', 'Status Page', 'Bug Reports'].map((link) => (
                  <li key={link}>
                    <button className={`text-sm hover:underline transition-colors ${
                      darkMode ? 'text-gray-400 hover:text-white' : 'text-gray-600 hover:text-gray-900'
                    }`}>
                      {link}
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </div>
      </div> */}

      {/* Footer Bottom */}
      <div className={`px-6 py-4 border-t ${
        darkMode ? 'border-gray-700 bg-gray-800' : 'border-gray-200 bg-gray-50'
      }`}>
        <div className="max-w-4xl mx-auto flex flex-col md:flex-row justify-between items-center space-y-2 md:space-y-0">
          <div className="flex items-center space-x-4">
            <p className={`text-sm ${
              darkMode ? 'text-gray-400' : 'text-gray-600'
            }`}>
              © 2024 AppName. All rights reserved.
            </p>
            <div className="hidden md:flex items-center space-x-4">
              {['Privacy Policy', 'Terms of Service', 'Cookie Policy'].map((link, index) => (
                <React.Fragment key={link}>
                  <button className={`text-sm hover:underline transition-colors ${
                    darkMode ? 'text-gray-400 hover:text-white' : 'text-gray-600 hover:text-gray-900'
                  }`}>
                    {link}
                  </button>
                  {index < 2 && (
                    <span className={`${darkMode ? 'text-gray-600' : 'text-gray-400'}`}>•</span>
                  )}
                </React.Fragment>
              ))}
            </div>
          </div>
          
          <div className="flex items-center space-x-4">
            <div className={`flex items-center space-x-2 text-sm ${
              darkMode ? 'text-gray-400' : 'text-gray-600'
            }`}>
              <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
              <span>All systems operational</span>
            </div>
            <select className={`text-sm rounded px-2 py-1 border ${
              darkMode 
                ? 'bg-gray-700 border-gray-600 text-gray-300' 
                : 'bg-white border-gray-300 text-gray-700'
            }`}>
              <option>English</option>
              <option>Spanish</option>
              <option>French</option>
            </select>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default AppFooter;