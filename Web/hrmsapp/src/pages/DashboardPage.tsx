// Page Components for Routing
const DashboardPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-blue-500 to-purple-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Dashboard Overview</h1>
      <p className="text-blue-100">Monitor your key metrics and performance indicators.</p>
    </div>
    <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      {[
        { title: 'Total Users', value: '24,563', change: '+12%' },
        { title: 'Revenue', value: '$45,280', change: '+8%' },
        { title: 'Orders', value: '1,247', change: '+23%' },
        { title: 'Conversion', value: '3.24%', change: '+0.4%' }
      ].map((stat, index) => (
        <div key={index} className={`p-6 rounded-xl shadow-lg ${
          darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
        }`}>
          <h3 className="text-sm font-medium text-gray-500 mb-2">{stat.title}</h3>
          <p className="text-2xl font-bold mb-1">{stat.value}</p>
          <p className="text-sm text-green-500">{stat.change}</p>
        </div>
      ))}
    </div>
  </div>
);
export default DashboardPage;