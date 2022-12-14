import { createContext } from "react";

export const AuthenticationContext = createContext({
    userId: null,
    useranme: null,
    setUser: (user) => { }
})

export default AuthenticationContext