import { createContext } from "react";

export const AuthenticationContext = createContext({
    authenticated: false,
    setAuthenticated: (authenticated) => {}
})

export default AuthenticationContext