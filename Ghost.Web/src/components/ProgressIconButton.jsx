import React, { useState } from 'react'
import { IconButton, Tooltip } from '@mui/material'
import VisibilityIcon from '@mui/icons-material/Visibility'
import PropTypes from 'prop-types'

export const ProgressIconButton = ({ update, progress, runtime }) => {
  const [loading, setLoading] = useState(false)
  const watchPercentage = (progress * 1000) / runtime

  const handleProgressUpdate = async () => {
    if (loading) return
    setLoading(true)
    try {
      if (watchPercentage >= 0.9) {
        await update(0)
      } else {
        await update(runtime / 1000)
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <Tooltip title="Reset progress">
      <IconButton
        aria-label="reset progress"
        onClick={handleProgressUpdate}
        disabled={loading}
        color={watchPercentage >= 0.9 ? 'primary' : 'default'}
      >
        <VisibilityIcon />
      </IconButton>
    </Tooltip>
  )
}

ProgressIconButton.propTypes = {
  update: PropTypes.func,
  progress: PropTypes.number.isRequired,
  runtime: PropTypes.number.isRequired,
}
