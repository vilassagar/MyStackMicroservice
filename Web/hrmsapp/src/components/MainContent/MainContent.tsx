import type { MenuItem } from '../../types';
// Main Content Component
const MainContent: React.FC<{ darkMode: boolean; activeItem: string; menuItems: MenuItem[] }> = ({ 
  darkMode, 
  activeItem, 
  menuItems 
}) => {
  return (
    <main className={`flex-1 p-6 overflow-y-auto ${
      darkMode ? 'bg-gray-800' : 'bg-gray-50'
    }`}>
      <div className={`max-w-4xl mx-auto ${
        darkMode ? 'text-white' : 'text-gray-900'
      }`}>
        <div className={`bg-gradient-to-r from-blue-500 to-purple-600 rounded-xl p-8 mb-8 text-white`}>
          <h1 className="text-3xl font-bold mb-2">
            Welcome to {menuItems.find(item => item.id === activeItem)?.label}
          </h1>
          <p className="text-blue-100">
            This is a responsive sidebar component with dark mode support, mobile-friendly design, and smooth animations.
          </p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
          {[1, 2, 3, 4, 5, 6,7,8,9].map((item) => (
            <div key={item} className={`p-6 rounded-xl shadow-lg ${
              darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
            }`}>
              <h3 className="text-lg font-semibold mb-2">Card {item}</h3>
              <p className={`text-sm ${
                darkMode ? 'text-gray-400' : 'text-gray-600'
              }`}>
                Sample content for demonstration purposes. The sidebar is fully responsive and works great on all devices.
              </p>
            </div>
          ))}
        </div>
      </div>
    </main>
  );
};

export default MainContent;