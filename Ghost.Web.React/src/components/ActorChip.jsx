import React from 'react'
import PropTypes from 'prop-types'
import { Chip } from '@mui/material'
import { Link } from 'react-router-dom'

export const ActorChip = ({ actor }) => (
  <Chip
    sx={{ m: 0.5 }}
    label={`${actor.name} ${actor.videoCount}`}
    variant="outlined"
    color={actor.favourite ? 'success' : 'primary'}
    component={Link}
    to={`/actors/${actor.id}/${encodeURIComponent(actor.name.toLowerCase())}`}
    clickable
  />
)

ActorChip.propTypes = {
  actor: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    videoCount: PropTypes.number.isRequired,
    favourite: PropTypes.bool.isRequired,
  }).isRequired,
}
