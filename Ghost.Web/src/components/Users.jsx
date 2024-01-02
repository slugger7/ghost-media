import React from "react";
import { Box, Typography, Skeleton, Stack } from "@mui/material";
import axios from "axios";

import { UserCard } from "./UserCard";
import usePromise from "../services/use-promise";

const fetchUsers = async () => (await axios.get("user")).data;

export const Users = () => {
  const [usersPage, , loadingUsers] = usePromise(() => fetchUsers());
  return (
    <Box>
      <Typography variant="h4" component="h4">
        Users
      </Typography>
      {loadingUsers && <Skeleton height="90px" />}
      {!loadingUsers && (
        <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
          {usersPage?.content?.map((user) => (
            <UserCard key={user.id} user={user} />
          ))}
        </Stack>
      )}
    </Box>
  );
};
