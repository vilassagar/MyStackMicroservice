const CalendarPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-teal-500 to-cyan-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Calendar</h1>
      <p className="text-teal-100">Manage your schedule and upcoming events.</p>
    </div>
    <div className={`p-6 rounded-xl shadow-lg ${
      darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
    }`}>
      <h3 className="text-lg font-semibold mb-4">Upcoming Events</h3>
      <div className="space-y-4">
        {[
          { title: 'Team Meeting', time: '10:00 AM', date: 'Today' },
          { title: 'Project Review', time: '2:00 PM', date: 'Tomorrow' },
          { title: 'Client Call', time: '11:00 AM', date: 'Friday' }
        ].map((event, index) => (
          <div key={index} className={`flex items-center justify-between p-4 rounded-lg ${
            darkMode ? 'bg-gray-800' : 'bg-gray-50'
          }`}>
            <div>
              <p className="font-medium">{event.title}</p>
              <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>{event.date}</p>
            </div>
            <p className={`text-sm font-medium ${darkMode ? 'text-teal-400' : 'text-teal-600'}`}>
              {event.time}
            </p>
          </div>
        ))}
      </div>
    </div>
  </div>
);

export default CalendarPage;