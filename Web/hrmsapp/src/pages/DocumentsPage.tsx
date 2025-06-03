import { FileText } from "lucide-react";

const DocumentsPage: React.FC<{ darkMode: boolean }> = ({ darkMode }) => (
  <div className={`max-w-4xl mx-auto ${darkMode ? 'text-white' : 'text-gray-900'}`}>
    <div className="bg-gradient-to-r from-indigo-500 to-purple-600 rounded-xl p-8 mb-8 text-white">
      <h1 className="text-3xl font-bold mb-2">Documents</h1>
      <p className="text-indigo-100">Organize and manage your files and documents.</p>
    </div>
    <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
      {[
        { name: 'Project Proposal.pdf', size: '2.4 MB', modified: '2 days ago' },
        { name: 'Budget Report.xlsx', size: '1.8 MB', modified: '1 week ago' },
        { name: 'Meeting Notes.docx', size: '523 KB', modified: '3 days ago' },
        { name: 'Design Assets.zip', size: '15.2 MB', modified: '5 days ago' },
        { name: 'User Manual.pdf', size: '4.1 MB', modified: '1 week ago' },
        { name: 'Analytics Data.csv', size: '890 KB', modified: '2 days ago' }
      ].map((doc, index) => (
        <div key={index} className={`p-4 rounded-xl shadow-lg ${
          darkMode ? 'bg-gray-900 border border-gray-700' : 'bg-white border border-gray-200'
        }`}>
          <div className="flex items-center mb-3">
            <FileText size={20} className="text-blue-500 mr-2" />
            <p className="font-medium truncate">{doc.name}</p>
          </div>
          <p className={`text-sm ${darkMode ? 'text-gray-400' : 'text-gray-600'}`}>{doc.size}</p>
          <p className={`text-xs ${darkMode ? 'text-gray-500' : 'text-gray-500'}`}>{doc.modified}</p>
        </div>
      ))}
    </div>
  </div>
);
export default DocumentsPage;