import { LinearProgress } from '@mui/material'
import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { useEffect } from 'react';

export const VideoProgress = ({ duration, current }) => {
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    setProgress(current / duration * 100000)
  }, [current, duration])

  return <LinearProgress value={progress} variant="determinate" />
}

VideoProgress.propTypes = {
  duration: PropTypes.number.isRequired,
  current: PropTypes.number.isRequired
}