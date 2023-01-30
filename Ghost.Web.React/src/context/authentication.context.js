import { createContext } from "react";

export const AuthenticationContext = createContext({
    userId: null,
    useranme: null,
    token: null,
    setToken: (token) => { },
    logout: () => { }
})

export default AuthenticationContext