import Recat from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { ButtonLink } from './ButtonLink.jsx'
import { Card, CardActionArea, CardHeader, CardMedia, IconButton } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert';

export const VideoCard = ({ id, title }) => <Card>
  <CardHeader
    action={<IconButton><MoreVertIcon /></IconButton>}
    title={title} />
  <CardActionArea LinkComponent={Link} to={`/media/${id}`}>
    <CardMedia
      component="img"
      image={`${axios.defaults.baseURL}/media/${id}/thumbnail`}
      alt={title}
    />
  </CardActionArea>
</Card>

VideoCard.propTypes = {
  id: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired
}