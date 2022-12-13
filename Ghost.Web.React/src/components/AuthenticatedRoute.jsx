import React, { useContext, useEffect } from 'react'
import { Navigate } from 'react-router-dom'
import PropTypes from 'prop-types'

import AuthenticationContext from '../context/authentication.context'

export const AuthenticatedRoute = ({children}) => {
    const { authenticated } = useContext(AuthenticationContext)
    
    return <>
        {authenticated ? children : <Navigate to='/login' />}
    </>
}

AuthenticatedRoute.propTypes = {
    children: PropTypes.node.isRequired
}