import { createContext } from "react";

export const AuthenticationContext = createContext({
    userId: null,
    useranme: null,
    token: null,
    setToken: (token) => { }
})

export default AuthenticationContext