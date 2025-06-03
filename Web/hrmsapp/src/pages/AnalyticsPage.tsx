const AnalyticsPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-green-500 to-blue-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Analytics Dashboard</h1>
      <p className="text-green-100">Deep insights into your data and user behavior.</p>
    </div>
    <div className="grid md:grid-cols-2 gap-6">
      {[
        { title: 'Traffic Analytics', description: 'Monitor website traffic and user engagement' },
        { title: 'Conversion Funnel', description: 'Track user journey and conversion rates' },
        { title: 'Revenue Trends', description: 'Analyze revenue patterns over time' },
        { title: 'User Behavior', description: 'Understand how users interact with your app' }
      ].map((item, index) => (
        <div key={index} className={`p-6 rounded-xl shadow-lg ${
          darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
        }`}>
          <h3 className="text-lg font-semibold mb-2">{item.title}</h3>
          <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>
            {item.description}
          </p>
        </div>
      ))}
    </div>
  </div>
);
export default AnalyticsPage;