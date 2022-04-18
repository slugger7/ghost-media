import React from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert';

export const VideoCard = ({ id, title }) => <Card sx={{ maxHeight: '400px' }}>
  <CardActionArea LinkComponent={Link} to={`/media/${id}`}>
    <CardMedia
      component="img"
      image={`${axios.defaults.baseURL}/media/${id}/thumbnail`}
      alt={title}
    />
    <CardHeader
      className="ghost-video-card-header"
      title={<Tooltip title={title}><Typography variant="h6" component="h6" noWrap={true}>{title}</Typography></Tooltip>}
      disableTypography={true}
      action={<IconButton>
        <MoreVertIcon />
      </IconButton>}
    />
  </CardActionArea>
</Card>

VideoCard.propTypes = {
  id: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired
}