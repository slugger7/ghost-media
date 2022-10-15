import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import axios from 'axios'
import {
  Container,
  Grid,
  IconButton,
  Paper,
  Skeleton,
  Box,
} from '@mui/material'
import { mergeDeepLeft } from 'ramda'
import MoreVertIcon from '@mui/icons-material/MoreVert'

import { Video } from './Video.jsx'
import { VideoGenres } from './VideoGenres.jsx'
import { VideoActors } from './VideoActors.jsx'
import { TextEdit } from './TextEdit.jsx'
import { VideoMetaData } from './VideoMetaData.jsx'
import { items, VideoMenu } from './VideoMenu.jsx'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import { Chapters } from './Chapters.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service'
import { FavouriteIconButton } from './FavouriteIconButton.jsx'
import usePromise from '../services/use-promise.js'
import { ProgressIconButton } from './ProgressIconButton.jsx'
import { MediaSection } from './MediaSection.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const fetchGenres = async (id) => (await axios.get(`/genre/video/${id}`)).data
const fetchActors = async (id) => (await axios.get(`/actor/video/${id}`)).data
const updateGenres = async (id, genres) =>
  (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) =>
  (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) =>
  (await axios.put(`/media/${id}/title`, { title })).data
const updateProgress = async (id, progress) => {
  if (progress !== null && !isNaN(progress)) {
    await axios.put(`/media/${id}/progress`, { progress })
  }
}

export const Media = () => {
  const params = useParams()
  const navigate = useNavigate()
  const [media, , loadingMedia, setMedia] = usePromise(
    () => fetchMedia(params.id),
    [params.id],
  )
  const [genres, , loadingGenres, setGenres] = usePromise(
    () => fetchGenres(params.id),
    [params.id],
  )
  const [actors, , loadingActors, setActors] = usePromise(
    () => fetchActors(params.id),
    [params.id],
  )
  const [menuAnchorEl, setMenuAnchorEl] = useState()
  const [chapter, setChapter] = useState()
  const videoSource = generateVideoUrl(params.id)
  const [progress, setProgress] = useState()

  const handleMenuClick = (event) => setMenuAnchorEl(event.target)
  const handleMenuClose = () => setMenuAnchorEl(null)
  const handleProgressUpdate = async (progress) => {
    setProgress(progress)
    await updateProgress(params.id, progress)
  }

  useEffect(() => {
    if (media?.progress !== undefined) {
      setProgress(media.progress)
    }
  }, [media?.progress])

  const updateMedia = (val) => {
    setMedia(mergeDeepLeft(val))
  }

  return (
    <>
      {loadingMedia && <Skeleton height="200px" width="100%" />}
      {!loadingMedia && (
        <Video
          chapter={chapter}
          source={videoSource}
          type={media.type}
          poster={
            media.thumbnail
              ? `${axios.defaults.baseURL}/image/${media.thumbnail.id}/${media.thumbnail.name}`
              : undefined
          }
          duration={media.runtime}
          currentProgress={media?.progress}
          progressUpdate={handleProgressUpdate}
        />
      )}
      <Container sx={{ paddingX: 0, mt: 1 }}>
        {!loadingMedia && (
          <MediaSection>
            <Box>
              <FavouriteIconButton
                id={params.id}
                state={media.favourite}
                toggleFn={toggleFavourite}
                update={(favourite) => updateMedia({ favourite })}
              />
              <ProgressIconButton
                update={handleProgressUpdate}
                progress={progress || 0}
                runtime={media?.runtime}
              />
            </Box>
            <IconButton
              sx={{ marginLeft: 'auto' }}
              onClick={handleMenuClick}
              id={`${params.id}-video-card-menu-button`}
              aria-controls={!!menuAnchorEl ? 'video-card-menu' : undefined}
              aria-haspopup={true}
              aria-expanded={!!menuAnchorEl}
            >
              <MoreVertIcon />
            </IconButton>
          </MediaSection>
        )}
        {loadingMedia && <Skeleton height="50px" width="100%" />}
        {!loadingMedia && (
          <MediaSection sx={{ justifyContent: '' }}>
            <TextEdit
              text={media.title}
              update={async (title) => {
                const video = await updateTitle(params.id, title)
                updateMedia({ title: video.title })
              }}
            />
          </MediaSection>
        )}
        {loadingGenres && <ChipSkeleton />}
        {!loadingGenres && (
          <MediaSection>
            <VideoGenres
              genres={genres}
              updateGenres={async (genres) => {
                const video = await updateGenres(params.id, genres)
                setGenres(video.genres)
              }}
            />
          </MediaSection>
        )}
        {loadingActors && <ChipSkeleton />}
        {!loadingActors && (
          <MediaSection>
            <VideoActors
              actors={actors}
              updateActors={async (actors) => {
                const video = await updateActors(params.id, actors)
                setActors(video.actors)
              }}
            />
          </MediaSection>
        )}
        {!loadingMedia && media.chapters.length > 0 && (
          <MediaSection>
            <Chapters chapters={media.chapters} setChapter={setChapter} />
          </MediaSection>
        )}
        {loadingMedia && (
          <MediaSection>
            <Grid container spacing={1} sx={{ py: 1 }}>
              <Grid item xs={12} sm={6} md={4} lg={3}>
                <Skeleton height="400px" />
              </Grid>
              <Grid item xs={12} sm={6} md={4} lg={3}>
                <Skeleton height="400px" />
              </Grid>
              <Grid item xs={12} sm={6} md={4} lg={3}>
                <Skeleton height="400px" />
              </Grid>
              <Grid item xs={12} sm={6} md={4} lg={3}>
                <Skeleton height="400px" />
              </Grid>
            </Grid>
          </MediaSection>
        )}
        {!loadingMedia && (
          <MediaSection>
            <VideoMetaData video={media} />
          </MediaSection>
        )}
        {loadingMedia && (
          <MediaSection>
            <Skeleton height="20px" width="100%" />
          </MediaSection>
        )}
      </Container>

      {!loadingMedia && (
        <VideoMenu
          source={videoSource}
          anchorEl={menuAnchorEl}
          handleClose={handleMenuClose}
          videoId={+params.id}
          title={media.title}
          removeVideo={() => navigate(-1)}
          setVideo={updateMedia}
          favourite={!!media.favourite}
          progress={progress}
          hideItems={[items.favourite, items.resetProgress]}
        />
      )}
    </>
  )
}
