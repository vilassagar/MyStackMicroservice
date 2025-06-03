const MessagesPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-orange-500 to-red-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Messages</h1>
      <p className="text-orange-100">Stay connected with your team and customers.</p>
    </div>
    <div className="grid md:grid-cols-3 gap-6">
      <div className={`p-6 rounded-xl shadow-lg ${
        darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
      }`}>
        <h3 className="text-lg font-semibold mb-4">Inbox</h3>
        <p className="text-2xl font-bold text-blue-500 mb-2">12</p>
        <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>New messages</p>
      </div>
      <div className={`p-6 rounded-xl shadow-lg ${
        darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
      }`}>
        <h3 className="text-lg font-semibold mb-4">Sent</h3>
        <p className="text-2xl font-bold text-green-500 mb-2">48</p>
        <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>Messages sent</p>
      </div>
      <div className={`p-6 rounded-xl shadow-lg ${
        darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
      }`}>
        <h3 className="text-lg font-semibold mb-4">Archived</h3>
        <p className="text-2xl font-bold text-gray-500 mb-2">156</p>
        <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>Archived messages</p>
      </div>
    </div>
  </div>
);
export default MessagesPage;