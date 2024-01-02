import React, { useCallback, useEffect, useState } from "react";
import PropTypes from "prop-types";
import { AuthenticationContext } from "./authentication.context";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

export const AuthenticationProvider = ({ children }) => {
  const [username, setUsername] = useState(localStorage.getItem("username"));
  const [token, setToken] = useState(localStorage.getItem("token"));

  const navigate = useNavigate();

  useEffect(() => {
    if (username) {
      localStorage.setItem("username", username);
    }
  }, [username]);

  useEffect(() => {
    if (token) {
      const user = jwtDecode(token);
      setUsername(user.name);
      localStorage.setItem("token", token);
    }
  }, [token]);

  const logout = useCallback(() => {
    setUsername(null);
    setToken(null);
    navigate("/login");
  }, [navigate]);

  return (
    <AuthenticationContext.Provider value={{ username, setToken, logout }}>
      {children}
    </AuthenticationContext.Provider>
  );
};

AuthenticationProvider.propTypes = {
  children: PropTypes.node,
};
