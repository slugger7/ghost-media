import React, { useContext, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { FormGroup, TextField, Container } from "@mui/material";
import LoadingButton from "@mui/lab/LoadingButton";
import LoginIcon from "@mui/icons-material/Login";
import AuthenticationContext from "../context/authentication.context";

export const Login = () => {
  const { setToken } = useContext(AuthenticationContext);
  const navigate = useNavigate()

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loggingIn, setLoggingIn] = useState(false);

  const handlePasswordChange = (event) => setPassword(event.target.value);
  const handleUsernameChange = (event) => setUsername(event.target.value);

  const handleLogin = async () => {
    setLoggingIn(true);
    try {
      const loginResult = (
        await axios.post("/user/login", { username, password })
      );

      const token = loginResult?.data.token

      if (token) {
        setToken(token);
        navigate("/")
      }
    } finally {
      setLoggingIn(false);
    }
  }

  const handleKeystroke = async (event) => {
    if (event.code === "Enter") {
      await handleLogin();
    }
  };

  return (
    <Container sx={{ mt: 1 }}>
      <FormGroup onKeyUp={handleKeystroke}>
        <TextField
          sx={{ mb: 1 }}
          label="Username"
          variant="outlined"
          value={username}
          type="text"
          onChange={handleUsernameChange}
        />
        <TextField
          sx={{ mb: 1 }}
          label="Password"
          variant="outlined"
          value={password}
          type="password"
          onChange={handlePasswordChange}
        />
        <LoadingButton
          onClick={handleLogin}
          variant="contained"
          loading={loggingIn}
          loadingPosition="start"
          startIcon={<LoginIcon />}
          disabled={loggingIn}
        >
          Login
        </LoadingButton>
      </FormGroup>
    </Container>
  );
};
