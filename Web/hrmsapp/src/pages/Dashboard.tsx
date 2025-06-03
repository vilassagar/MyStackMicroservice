
// Example Dashboard page for demonstration
// File: src/pages/Dashboard.tsx
import React from 'react';

const Dashboard: React.FC = () => {
    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-2xl font-semibold text-gray-800">Dashboard</h1>
                <button className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700">
                    Create New Project
                </button>
            </div>

            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
                {/* Stat Cards */}
                <div className="rounded-lg bg-white p-6 shadow">
                    <h3 className="text-lg font-medium text-gray-600">Total Projects</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-800">12</p>
                </div>
                <div className="rounded-lg bg-white p-6 shadow">
                    <h3 className="text-lg font-medium text-gray-600">Active Tasks</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-800">34</p>
                </div>
                <div className="rounded-lg bg-white p-6 shadow">
                    <h3 className="text-lg font-medium text-gray-600">Team Members</h3>
                    <p className="mt-2 text-3xl font-bold text-gray-800">8</p>
                </div>
            </div>

            <div className="rounded-lg bg-white p-6 shadow">
                <h2 className="mb-4 text-lg font-semibold text-gray-800">Recent Projects</h2>
                <div className="overflow-x-auto">
                    <table className="w-full">
                        <thead className="bg-gray-50 text-xs font-medium text-gray-500 uppercase">
                            <tr>
                                <th className="px-6 py-3 text-left">Project Name</th>
                                <th className="px-6 py-3 text-left">Status</th>
                                <th className="px-6 py-3 text-left">Team</th>
                                <th className="px-6 py-3 text-left">Due Date</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-200 bg-white">
                            {[
                                { name: 'Website Redesign', status: 'In Progress', team: 'Design', dueDate: 'May 25, 2025' },
                                { name: 'Mobile App', status: 'Planning', team: 'Development', dueDate: 'June 10, 2025' },
                                { name: 'Marketing Campaign', status: 'Completed', team: 'Marketing', dueDate: 'May 5, 2025' },
                            ].map((project, index) => (
                                <tr key={index}>
                                    <td className="px-6 py-4 whitespace-nowrap">{project.name}</td>
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        <span className={`px-2 py-1 text-xs font-semibold rounded-full ${project.status === 'Completed' ? 'bg-green-100 text-green-800' :
                                            project.status === 'In Progress' ? 'bg-blue-100 text-blue-800' :
                                                'bg-yellow-100 text-yellow-800'
                                            }`}>
                                            {project.status}
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap">{project.team}</td>
                                    <td className="px-6 py-4 whitespace-nowrap">{project.dueDate}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default Dashboard;