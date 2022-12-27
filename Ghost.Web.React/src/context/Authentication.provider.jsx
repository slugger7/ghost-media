import React, { useCallback, useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { AuthenticationContext } from './authentication.context'
import jwtDecode from 'jwt-decode';

export const AuthenticationProvider = ({ children }) => {
    const userIdString = localStorage.getItem('userId');
    const [userId, setUserId] = useState(userIdString ? +userIdString : undefined);
    const [username, setUsername] = useState(localStorage.getItem('username'))
    const [token, setToken] = useState(localStorage.getItem('token'))

    useEffect(() => {
        if (username) {
            localStorage.setItem('username', username)
        }
    }, [username])

    useEffect(() => {
        if (userId) {
            localStorage.setItem('userId', userId);
        }
    }, [userId])

    useEffect(() => {
        if (token) {
            const user = jwtDecode(token);
            setUserId(user.primarysid)
            setUsername(user.name)
            localStorage.setItem('token', token)
        }
    }, [token])

    const setUser = useCallback((user) => {
        setUserId(user.id);
        setUsername(user.username)
    })

    return <AuthenticationContext.Provider value={{ userId, username, setToken }}>
        {children}
    </AuthenticationContext.Provider>
}

AuthenticationProvider.propTypes = {
    children: PropTypes.node
}