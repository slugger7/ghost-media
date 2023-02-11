import { Box, Card, Skeleton } from '@mui/material'
import React from 'react'

export const VideoCardSkeleton = () => <Card sx={{
  maxHeight: '400px',
  height: '348px',
  display: 'flex',
  justifyContent: 'space-around',
  flexDirection: 'column'
}}>
  <Box sx={{ px: 1 }}>
    <Skeleton sx={{ width: "80%" }} />
    <Box sx={{ display: 'flex', justifyContent: 'left', gap: '10px' }}>
      <Skeleton width={50} />
      <Skeleton width={60} />
      <Skeleton width={40} />
      <Skeleton width={70} />
      <Skeleton width={30} />
    </Box>
  </Box>
  <Skeleton height={200} variant="rectangular" />
  <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between", px: 1 }}>
    <Skeleton sx={{ width: "30px", height: '30px' }} variant="circular" />
    <Skeleton sx={{ width: "30px", height: '30px' }} variant="circular" />
  </Box>
</Card>