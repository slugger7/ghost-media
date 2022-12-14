import React, { useCallback, useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { AuthenticationContext } from './authentication.context'

export const AuthenticationProvider = ({ children }) => {
    const userIdString = localStorage.getItem('userId');
    const [userId, setUserId] = useState(userIdString ? +userIdString : undefined);
    const [username, setUsername] = useState(localStorage.getItem('username'))

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

    const setUser = useCallback((user) => {
        setUserId(user.id);
        setUsername(user.username)
    })

    return <AuthenticationContext.Provider value={{ userId, username, setUser }}>
        {children}
    </AuthenticationContext.Provider>
}

AuthenticationProvider.propTypes = {
    children: PropTypes.node
}