import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { Container, Grid, IconButton, Paper, Skeleton, Box } from '@mui/material'
import { mergeDeepRight } from 'ramda'
import MoreVertIcon from '@mui/icons-material/MoreVert'

import { Video } from './Video.jsx'
import { VideoGenres } from './VideoGenres.jsx'
import { VideoActors } from './VideoActors.jsx'
import { TextEdit } from './TextEdit.jsx'
import { VideoMetaData } from './VideoMetaData.jsx'
import { items, VideoMenu } from './VideoMenu.jsx'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import { Chapters } from './Chapters.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service';
import { FavouriteIconButton } from './FavouriteIconButton.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const fetchGenres = async (id) => (await axios.get(`/genre/video/${id}`)).data
const fetchActors = async (id) => (await axios.get(`/actor/video/${id}`)).data
const updateGenres = async (id, genres) => (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) => (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) => (await axios.put(`/media/${id}/title`, { title })).data
const updateProgress = async (id, progress) => {
  if (progress !== null && !isNaN(progress)) {
    (await axios.put(`/media/${id}/progress`, { progress }))
  }
}

export const Media = () => {
  const params = useParams()
  const navigate = useNavigate();
  const media = useAsync(fetchMedia, [params.id])
  const genresPage = useAsync(fetchGenres, [params.id])
  const actorsPage = useAsync(fetchActors, [params.id])
  const [menuAnchorEl, setMenuAnchorEl] = useState();
  const [chapter, setChapter] = useState();
  const videoSource = generateVideoUrl(params.id);
  const [progress, setProgress] = useState();

  const handleMenuClick = (event) => setMenuAnchorEl(event.target)
  const handleMenuClose = () => setMenuAnchorEl(null);
  const handleProgressUpdate = (progress) => {
    setProgress(progress);
    updateProgress(params.id, progress)
  }

  useEffect(() => {
    if (media.result?.progress !== undefined) {
      setProgress(media.result.progress);
    }
  }, [media.result])

  const updateMedia = (val) => {
    media.set(mergeDeepRight(media, { result: val }));
  }

  return <>
    {media.loading && <Skeleton height="200px" width="100%" />}
    {!media.loading &&
      <Video
        chapter={chapter}
        source={videoSource}
        type={media.result.type}
        poster={media.result.thumbnail ? `${axios.defaults.baseURL}/image/${media.result.thumbnail.id}/${media.result.thumbnail.name}` : undefined}
        duration={media.result.runtime}
        currentProgress={media.result.progress}
        progressUpdate={handleProgressUpdate}
      />}
    <Container sx={{ paddingX: 0 }}>
      <Paper sx={{ p: 2 }}>
        {!media.loading && <Box sx={{ display: 'flex', flexDirection: 'row' }}>
          <FavouriteIconButton
            id={params.id}
            state={media.result.favourite}
            toggleFn={toggleFavourite}
            update={favourite => updateMedia({ favourite })} />
          < IconButton
            sx={{ marginLeft: 'auto' }}
            onClick={handleMenuClick}
            id={`${params.id}-video-card-menu-button`}
            aria-controls={!!menuAnchorEl ? 'video-card-menu' : undefined}
            aria-haspopup={true}
            aria-expanded={!!menuAnchorEl}
          >
            <MoreVertIcon />
          </IconButton>
        </Box>}
        {media.loading && <Skeleton height="50px" width="100%" />}
        {!media.loading && <TextEdit text={media.result.title} update={async (title) => {
          const video = await updateTitle(params.id, title)
          updateMedia({ title: video.title })
        }} />}
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

    {
      !media.loading && <VideoMenu
        source={videoSource}
        anchorEl={menuAnchorEl}
        handleClose={handleMenuClose}
        videoId={+params.id}
        title={media.result.title}
        removeVideo={() => navigate(-1)}
        setVideo={updateMedia}
        favourite={!!media.result.favourite}
        progress={progress}
        hideItems={[items.favourite]}
      />
    }
  </>
}