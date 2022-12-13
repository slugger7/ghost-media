import React, { useContext, useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { Button, FormGroup, TextField } from '@mui/material'
import AuthenticationContext from '../context/authentication.context';

export const Login = () => {
    const {authenticated, setAuthenticated} = useContext(AuthenticationContext)
    const [password, setPassword] = useState('');

    const handlePasswordChange = (event) => setPassword(event.target.value)
    const handleLogin = () => {
        if (password === 'fanticness') {
            setAuthenticated(true);
        }
    }

    return <>
    {authenticated && <Navigate to="/" />}
    {!authenticated && <FormGroup>
        <TextField label="Password" variant='outlined' value={password} onChange={handlePasswordChange} />
        <Button onClick={handleLogin} variant='contained'>Login</Button>
    </FormGroup>}
  </>
}