 const SettingsPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-gray-600 to-gray-800 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Settings</h1>
      <p className="text-gray-200">Configure your application preferences.</p>
    </div>
    <div className="grid md:grid-cols-2 gap-6">
      {[
        { title: 'Profile Settings', description: 'Update your personal information and preferences' },
        { title: 'Security', description: 'Manage passwords, 2FA, and security settings' },
        { title: 'Notifications', description: 'Control email and push notification preferences' },
        { title: 'Privacy', description: 'Manage data privacy and sharing settings' }
      ].map((setting, index) => (
        <div key={index} className={`p-6 rounded-xl shadow-lg ${
          darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
        }`}>
          <h3 className="text-lg font-semibold mb-2">{setting.title}</h3>
          <p className={`text-sm mb-4 ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>
            {setting.description}
          </p>
          <button className="text-blue-500 hover:text-blue-600 text-sm font-medium">
            Configure →
          </button>
        </div>
      ))}
    </div>
  </div>
);
export default SettingsPage;