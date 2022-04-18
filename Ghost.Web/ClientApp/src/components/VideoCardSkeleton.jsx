import { Box, Skeleton } from '@mui/material'
import React from 'react'

export const VideoCardSkeleton = () => <Box>
  <Skeleton variant="rectangle" height="150px" />
  <Skeleton />
  <Skeleton />
</Box>