export interface MenuItem {
  id: string;
  label: string;
  icon: React.ReactNode;
  badge?: string;
  path: string; // Added path for routing
  submenu?: MenuItem[];
}

export interface SidebarProps {
  isOpen: boolean;
  isCollapsed: boolean;
  darkMode: boolean;
  activeItem: string;
  onToggleCollapse: () => void;
  onItemClick: (itemId: string, path: string) => void; // Updated to include path
}

export interface HeaderProps {
  darkMode: boolean;
  activeItem: string;
  isCollapsed: boolean;
  isSidebarOpen: boolean;
  onToggleSidebar: () => void;
  onToggleDarkMode: () => void;
  menuItems: MenuItem[];
  currentPath: string; // Added for breadcrumb routing
}

export interface FooterProps {
  darkMode: boolean;
}

export interface LayoutProps {
  children: React.ReactNode;
  darkMode: boolean;
  isCollapsed: boolean;
}