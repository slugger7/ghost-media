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
import { TextEdit } from './TextEdit.jsx'
import { VideoMetaData } from './VideoMetaData.jsx'
import { VideoMenu } from './VideoMenu.jsx'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import { Chapters } from './Chapters.jsx'
import { generateVideoUrl } from '../services/video.service';

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const fetchGenres = async (id) => (await axios.get(`/genre/video/${id}`)).data
const fetchActors = async (id) => (await axios.get(`/actor/video/${id}`)).data
const updateGenres = async (id, genres) => (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) => (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) => (await axios.put(`/media/${id}/title`, { title })).data

export const Media = () => {
  const params = useParams()
  const navigate = useNavigate();
  const media = useAsync(fetchMedia, [params.id])
  const genresPage = useAsync(fetchGenres, [params.id])
  const actorsPage = useAsync(fetchActors, [params.id])
  const [menuAnchorEl, setMenuAnchorEl] = useState();
  const [chapter, setChapter] = useState();
  const videoSource = generateVideoUrl(params.id);

  const handleMenuClick = (event) => setMenuAnchorEl(event.target)
  const handleMenuClose = () => setMenuAnchorEl(null);

  return <>
    {media.loading && <Skeleton height="200px" width="100%" />}
    {!media.loading &&
      <Video
        chapter={chapter}
        source={videoSource}
        type={media.result.type}
        poster={`${axios.defaults.baseURL}/image/${media.result.thumbnail?.id}/${media.result.title}`}
      />}
    <Container sx={{ paddingX: 0 }}>
      <Paper sx={{ p: 2 }}>
        <Grid container spacing={1}>
          <Grid item xs={10} sm={11}>
            {media.loading && <Skeleton height="50px" width="100%" />}
            {!media.loading && <TextEdit text={media.result.title} update={async (title) => {
              const video = await updateTitle(params.id, title)
              media.set(mergeDeepRight(media, { result: { title: video.title } }))
            }} />}
          </Grid>
          <Grid item xs={2} sm={1}><IconButton
            onClick={handleMenuClick}
            id={`${params.id}-video-card-menu-button`}
            aria-controls={!!menuAnchorEl ? 'video-card-menu' : undefined}
            aria-haspopup={true}
            aria-expanded={!!menuAnchorEl}
          >
            <MoreVertIcon />
          </IconButton>
          </Grid>
        </Grid>
        {genresPage.loading && <ChipSkeleton />}
        {!genresPage.loading && <VideoGenres genres={genresPage.result} videoId={params.id}
          updateGenres={async (genres) => {
            const video = await updateGenres(params.id, genres)
            genresPage.set(mergeDeepRight(genres, { result: video.genres }));
          }}
        />}
        {actorsPage.loading && <ChipSkeleton />}
        {!actorsPage.loading && <VideoActors
          actors={actorsPage.result}
          videoId={params.id}
          updateActors={async (actors) => {
            const video = await updateActors(params.id, actors)
            actorsPage.set(mergeDeepRight(media, { result: video.actors }))
          }}
        />}
        {!media.loading && <Chapters
          chapters={media.result.chapters}
          setChapter={setChapter}
        />}
        {media.loading && <Grid container spacing={1} sx={{ py: 1 }}>
          <Grid item xs={12} sm={6} md={4} lg={3}><Skeleton height="400px" /></Grid>
          <Grid item xs={12} sm={6} md={4} lg={3}><Skeleton height="400px" /></Grid>
          <Grid item xs={12} sm={6} md={4} lg={3}><Skeleton height="400px" /></Grid>
          <Grid item xs={12} sm={6} md={4} lg={3}><Skeleton height="400px" /></Grid>
        </Grid>}
        {!media.loading && <VideoMetaData
          video={media.result}
        />}
        {media.loading && <Skeleton height="20px" width="100%" />}
      </Paper>
    </Container>

    {!media.loading && <VideoMenu
      source={videoSource}
      anchorEl={menuAnchorEl}
      handleClose={handleMenuClose}
      videoId={+params.id}
      title={media.result.title}
      removeVideo={() => navigate(-1)}
      setVideo={(video) => media.set(mergeDeepRight(media, { result: video }))}
    />}
  </>
}