import React from 'react'
import { Stack, Skeleton } from '@mui/material'

export const ChipSkeleton = () => <Stack direction="row" spacing={1}>
  <Skeleton height="30px" width="100px" />
  <Skeleton height="30px" width="120px" />
  <Skeleton height="30px" width="90px" />
</Stack>