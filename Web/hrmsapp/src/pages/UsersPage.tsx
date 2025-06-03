const UsersPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-purple-500 to-pink-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">User Management</h1>
      <p className="text-purple-100">Manage users, roles, and permissions.</p>
    </div>
    <div className={`rounded-xl shadow-lg overflow-hidden ${
      darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
    }`}>
      <div className="p-6">
        <h3 className="text-lg font-semibold mb-4">Recent Users</h3>
        <div className="space-y-4">
          {[
            { name: 'John Doe', email: 'john@example.com', role: 'Admin', status: 'Active' },
            { name: 'Jane Smith', email: 'jane@example.com', role: 'User', status: 'Active' },
            { name: 'Mike Johnson', email: 'mike@example.com', role: 'Editor', status: 'Inactive' }
          ].map((user, index) => (
            <div key={index} className={`flex items-center justify-between p-4 rounded-lg ${
              darkMode ? 'bg-gray-800' : 'bg-gray-50'
            }`}>
              <div>
                <p className="font-medium">{user.name}</p>
                <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>{user.email}</p>
              </div>
              <div className="text-right">
                <p className="text-sm font-medium">{user.role}</p>
                <p className={`text-xs ${user.status === 'Active' ? 'text-green-500' : 'text-red-500'}`}>
                  {user.status}
                </p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  </div>
);
export default UsersPage;