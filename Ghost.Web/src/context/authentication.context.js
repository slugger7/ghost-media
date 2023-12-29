import { createContext } from "react";

export const AuthenticationContext = createContext({
    userId: null,
    useranme: null,
    token: null,
    // eslint-disable-next-line no-unused-vars
    setToken: (token) => { },
    logout: () => { }
})

export default AuthenticationContext