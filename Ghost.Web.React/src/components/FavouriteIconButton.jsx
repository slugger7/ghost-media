import React, { useState } from 'react'
import { IconButton, Tooltip } from '@mui/material'
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import PropTypes from 'prop-types'

export const FavouriteIconButton = ({ state, update, id, toggleFn }) => {
  const [loading, setLoading] = useState(false);

  const handleToggleFavourite = async () => {
    if (loading) return;
    setLoading(true);
    try {
      const favourite = await toggleFn(id)
      update(favourite);
    } finally {
      setLoading(false);
    }
  }
  return <Tooltip title={state ? "Unfavourite" : "Favourite"}>
    <IconButton aria-label="add to favourites" onClick={handleToggleFavourite} disabled={loading}>
      {state ? <FavoriteIcon /> : <FavoriteBorderIcon />}
    </IconButton>
  </Tooltip>
}

FavouriteIconButton.propTypes = {
  state: PropTypes.bool.isRequired,
  update: PropTypes.func,
  id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
  toggleFn: PropTypes.func
}