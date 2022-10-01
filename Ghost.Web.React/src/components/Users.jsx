import React from 'react'
import { Box, Typography, Skeleton, Stack } from '@mui/material'
import axios from 'axios'

import { UserCard } from './UserCard'

const fetchUsers = async () => (await axios.get("user")).data

export const Users = () => {
  const usersPage = usePromise(() => fetchUsers())
  return <Box>
    <Typography variant="h4" component="h4">Users</Typography>
    {usersPage.loading && <Skeleton height="90px" />}
    {!usersPage.loading && <Stack direction="column" spacing={1} sx={{ mb: 1 }} >
      {usersPage.result?.content?.map(user => <UserCard key={user.id} user={user} refresh={usersPage.execute} />)}
    </Stack>}
  </Box>
}