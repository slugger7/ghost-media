import React, { useContext } from "react";
import { Navigate } from "react-router-dom";
import PropTypes from "prop-types";

import AuthenticationContext from "../context/authentication.context";

export const AuthenticatedRoute = ({ children }) => {
  const { token } = useContext(AuthenticationContext);

  return <>{token ? children : <Navigate to="/login" />}</>;
};

AuthenticatedRoute.propTypes = {
  children: PropTypes.node.isRequired,
};
