import React from 'react'
import PropTypes from 'prop-types'
import { Card, CardMedia, Grid, CardActionArea } from '@mui/material'
import axios from 'axios'

export const Chapters = ({ chapters, setChapter = () => { } }) => <Grid container spacing={1} sx={{ py: 1 }}>
  {chapters.map(chapter => <Grid key={chapter.id} item xs={12} sm={6} md={4} lg={3}>
    <Card sx={{ display: "inline-list-item", width: "100%" }}>
      <CardActionArea onClick={() => setChapter(chapter)}>
        <CardMedia
          component="img"
          sx={{ minHeight: "200px" }}
          image={`${axios.defaults.baseURL}/image/${chapter.image.id}/${chapter.image.name}`}
          alt={chapter.image.name}
        />
      </CardActionArea>
    </Card>
  </Grid>)}
</Grid>

Chapters.propTypes = {
  chapters: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      timestamp: PropTypes.number.isRequired,
      image: PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired
      }).isRequired
    }).isRequired
  ),
  setChapter: PropTypes.func
}