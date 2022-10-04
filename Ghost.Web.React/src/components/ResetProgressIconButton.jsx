import React, { useState } from 'react'
import { IconButton, Tooltip } from '@mui/material'
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff'
import PropTypes from 'prop-types'

export const ResetProgressIconButton = ({ resetFn }) => {
  const [loading, setLoading] = useState(false)

  const handleResetProgress = async () => {
    if (loading) return
    setLoading(true)
    try {
      await resetFn()
    } finally {
      setLoading(false)
    }
  }

  return (
    <Tooltip title="Reset progress">
      <IconButton
        aria-label="reset progress"
        onClick={handleResetProgress}
        disabled={loading}
      >
        <VisibilityOffIcon fontSize="small" />
      </IconButton>
    </Tooltip>
  )
}

ResetProgressIconButton.propTypes = {
  resetFn: PropTypes.func,
}
