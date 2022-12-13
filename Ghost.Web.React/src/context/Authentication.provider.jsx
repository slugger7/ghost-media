import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { AuthenticationContext } from './authentication.context'

export const AuthenticationProvider = ({children}) => {
    const [authenticated, setAuthenticated] = useState(localStorage.getItem('authenticated').toLocaleLowerCase() === "true");

    useEffect(() => {
        localStorage.setItem('authenticated', authenticated)
    }, [authenticated])

    return <AuthenticationContext.Provider value={{authenticated, setAuthenticated}}>
        {children}
    </AuthenticationContext.Provider>
}

AuthenticationProvider.propTypes = {
    children: PropTypes.node
}