import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { Container, Grid, IconButton, Paper, Skeleton } from '@mui/material'
import { mergeDeepRight } from 'ramda'
import MoreVertIcon from '@mui/icons-material/MoreVert'

import { Video } from './Video.jsx'
import { VideoGenres } from './VideoGenres.jsx'
import { VideoActors } from './VideoActors.jsx'
import { VideoTitle } from './VideoTitle.jsx'
import { VideoMetaData } from './VideoMetaData.jsx'
import { VideoMenu } from './VideoMenu.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const updateGenres = async (id, genres) => (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) => (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) => (await axios.put(`/media/${id}/title`, { title })).data

export const Media = () => {
  const params = useParams()
  const navigate = useNavigate();
  const media = useAsync(fetchMedia, [params.id])
  const [menuAnchorEl, setMenuAnchorEl] = useState();

  const handleMenuClick = (event) => setMenuAnchorEl(event.target)
  const handleMenuClose = () => setMenuAnchorEl(null);

  return <>
    {media.loading && <Skeleton height="200px" width="100%" />}
    {!media.loading && <>
      <Video
        source={`${axios.defaults.baseURL}/media/${params.id}`}
        type={media.result.type}
        poster={`${axios.defaults.baseURL}/image/${media.result.thumbnail?.id}`}
      />
      <Container>

        <Paper sx={{ p: 2 }}>
          <Grid container spacing={1}>
            <Grid item xs={10} sm={11}>
              <VideoTitle video={media.result} updateTitle={async (title) => {
                const video = await updateTitle(params.id, title)
                media.set(mergeDeepRight(media, { result: video }))
              }} />
            </Grid>
            <Grid item xs={2} sm={1}><IconButton
              onClick={handleMenuClick}
              id={`${media.result._id}-video-card-menu-button`}
              aria-controls={!!menuAnchorEl ? 'video-card-menu' : undefined}
              aria-haspopup={true}
              aria-expanded={!!menuAnchorEl}
            >
              <MoreVertIcon />
            </IconButton>
            </Grid>
          </Grid>
        </Paper>
        <VideoGenres genres={media.result.genres} videoId={params.id}
          updateGenres={async (genres) => {
            const video = await updateGenres(params.id, genres)
            media.set(mergeDeepRight(media, { result: video }))
          }}
        />
        <VideoActors
          actors={media.result.actors}
          videoId={params.id}
          updateActors={async (actors) => {
            const video = await updateActors(params.id, actors)
            media.set(mergeDeepRight(media, { result: video }))
          }}
        />
        <VideoMetaData
          video={media.result}
        />
      </Container>

      <VideoMenu
        anchorEl={menuAnchorEl}
        handleClose={handleMenuClose}
        videoId={media.result._id}
        title={media.result.title}
        removeVideo={() => navigate(-1)}
        setVideo={(video) => media.set(mergeDeepRight(media, { result: video }))}
      />
    </>}
  </>
}