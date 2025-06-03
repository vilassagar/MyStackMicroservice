// hooks/useAuth.tsx
import { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import authService, {
    AuthState,
    LoginRequest,
    SignupRequest,
    User
} from '../api/authService';

// Auth context type
interface AuthContextType extends AuthState {
    login: (credentials: LoginRequest) => Promise<void>;
    signup: (data: SignupRequest) => Promise<void>;
    logout: () => Promise<void>;
    requestPasswordReset: (email: string) => Promise<{ message: string }>;
    resetPassword: (token: string, password: string, confirmPassword: string) => Promise<{ message: string }>;
    error: string | null;
    clearError: () => void;
}

// Create context with default values
const AuthContext = createContext<AuthContextType>({
    user: null,
    token: null,
    isAuthenticated: false,
    isLoading: true,
    error: null,
    login: async () => { },
    signup: async () => { },
    logout: async () => { },
    requestPasswordReset: async () => ({ message: '' }),
    resetPassword: async () => ({ message: '' }),
    clearError: () => { },
});

// Auth provider props
interface AuthProviderProps {
    children: ReactNode;
}

// Auth provider component
export const AuthProvider = ({ children }: AuthProviderProps) => {
    // Local state to track auth state and errors
    const [authState, setAuthState] = useState<AuthState>({
        user: null,
        token: null,
        isAuthenticated: false,
        isLoading: true,
    });
    const [error, setError] = useState<string | null>(null);

    // Initialize auth state on component mount
    useEffect(() => {
        const initAuth = async () => {
            try {
                await authService.initializeAuth();

                // Update local state with service state
                setAuthState(authService.getAuthState());
            } catch (err) {
                setError('Failed to initialize authentication');
            }
        };

        initAuth();
    }, []);

    // Login function
    const login = async (credentials: LoginRequest) => {
        setError(null);

        try {
            await authService.login(credentials);
            setAuthState(authService.getAuthState());
        } catch (err: any) {
            setError(err.message || 'Login failed');
            throw err;
        }
    };

    // Signup function
    const signup = async (data: SignupRequest) => {
        setError(null);

        try {
            await authService.signup(data);
            setAuthState(authService.getAuthState());
        } catch (err: any) {
            setError(err.message || 'Signup failed');
            throw err;
        }
    };

    // Logout function
    const logout = async () => {
        setError(null);

        try {
            await authService.logout();
            setAuthState(authService.getAuthState());
        } catch (err: any) {
            setError(err.message || 'Logout failed');
        }
    };

    // Password reset request
    const requestPasswordReset = async (email: string) => {
        setError(null);

        try {
            return await authService.requestPasswordReset(email);
        } catch (err: any) {
            setError(err.message || 'Password reset request failed');
            throw err;
        }
    };

    // Reset password
    const resetPassword = async (token: string, password: string, confirmPassword: string) => {
        setError(null);

        try {
            return await authService.resetPassword(token, password, confirmPassword);
        } catch (err: any) {
            setError(err.message || 'Password reset failed');
            throw err;
        }
    };

    // Clear error
    const clearError = () => setError(null);

    // Context value
    const value = {
        ...authState,
        login,
        signup,
        logout,
        requestPasswordReset,
        resetPassword,
        error,
        clearError,
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

// Custom hook to use auth context
export const useAuth = () => {
    const context = useContext(AuthContext);

    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }

    return context;
};

export default useAuth;