import React, { useState, useEffect, useRef, useCallback } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import axios from 'axios'
import { Container, Grid, IconButton, Skeleton, Box, Tooltip } from '@mui/material'
import { mergeDeepLeft } from 'ramda'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import CallSplitIcon from '@mui/icons-material/CallSplit';
import FirstPageIcon from '@mui/icons-material/FirstPage';
import LastPageIcon from '@mui/icons-material/LastPage';
import TheatersIcon from '@mui/icons-material/Theaters';

import { Video } from '../components/Video.jsx'
import { VideoGenres } from '../components/VideoGenres.jsx'
import { VideoActors } from '../components/VideoActors.jsx'
import { TextEdit } from '../components/TextEdit.jsx'
import { VideoMetaData } from '../components/VideoMetaData.jsx'
import { items, VideoMenu } from '../components/VideoMenu.jsx'
import { ChipSkeleton } from '../components/ChipSkeleton.jsx'
import { Chapters } from '../components/Chapters.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service.js'
import { FavouriteIconButton } from '../components/FavouriteIconButton.jsx'
import usePromise from '../services/use-promise.js'
import { ProgressIconButton } from '../components/ProgressIconButton.jsx'
import { MediaSection } from '../components/MediaSection.jsx'
import { VideoCard } from '../components/VideoCard.jsx'
import { AddVideoCard } from '../components/AddVideoCard.jsx'
import { TextInputModal } from '../components/TextInputModal.jsx'
import { fetchMedia } from '../services/media.service.js'

const fetchGenres = async (id) => (await axios.get(`/genre/video/${id}`)).data
const fetchActors = async (id) => (await axios.get(`/actor/video/${id}`)).data
const updateGenres = async (id, genres) =>
  (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) =>
  (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) =>
  (await axios.put(`/media/${id}/title`, { title })).data
const updateProgress = async (id, progress, reduceProgress = false) => {
  if (progress !== null && !isNaN(progress)) {
    await axios.put(`/media/${id}/progress`, {
      progress,
      reduceProgress,
    })
  }
}
const createSubVideo = async (id, name, start, end) => (await axios.post(`/media/${id}/sub-video`, {
  name,
  startMillis: parseInt(start * 1000),
  endMillis: parseInt(end * 1000)
})).data
const removeRelation = async (id, relatedTo) => (await axios.delete(`/media/${id}/relations/${relatedTo}`)).data

export const Media = () => {
  const params = useParams()
  const navigate = useNavigate()
  const videoRef = useRef()
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
  const [refocusFn, setRefocusFn] = useState(() => { })

  const [editMode, setEditMode] = useState(false)
  const [startMarker, setStartMarker] = useState(null);
  const [endMarker, setEndMarker] = useState(null);
  const [subVideoNameModalOpen, setSubVideoNameModalOpen] = useState(false);

  useEffect(() => {
    if (media?.progress !== undefined) {
      setProgress(media.progress)
    }
  }, [media?.progress])

  useEffect(() => {
    if (!editMode) {
      setStartMarker(null);
      setEndMarker(null);
    }
  }, [editMode])

  const handleMenuClick = useCallback((event) => setMenuAnchorEl(event.target), [])
  const handleMenuClose = useCallback(() => setMenuAnchorEl(null), [])
  const handleProgressUpdate = useCallback(async (progress) => {
    setProgress(progress)
    await updateProgress(params.id, progress)
  }, [params.id])

  const handleProgressStatusUpdate = useCallback(async (progress) => {
    setProgress(progress)
    await updateProgress(params.id, progress, true)
  }, [params.id])

  const updateMedia = useCallback((val) => {
    setMedia(mergeDeepLeft(val))
  }, [setMedia])

  const focusVideo = useCallback((fn) => {
    videoRef?.current?.focus()
    setRefocusFn(() => fn)
  }, [videoRef])

  const handleRelationRemoval = useCallback((id, relatedTo) => async () => {
    const relatedVideos = await removeRelation(id, relatedTo)

    updateMedia({ relatedVideos })
  }, [updateMedia])

  const handleStartMarkerClick = useCallback(() => {
    if (editMode) {
      setStartMarker(progress);
    }
  }, [editMode, progress])

  const handleEndMarkerClick = useCallback(() => {
    if (editMode) {
      setEndMarker(progress);
    }
  }, [editMode, progress])

  const handleSubVideoClick = useCallback(() => {
    if (editMode) {
      setSubVideoNameModalOpen(true);
    }
  }, [editMode])

  const handleSubVideoSubmit = useCallback(async (newVideoName) => {
    await createSubVideo(media.id, newVideoName, startMarker, endMarker);

    const updatedVideo = await fetchMedia(media.id);
    updateMedia({ relatedVideos: updatedVideo.relatedVideos })
    setSubVideoNameModalOpen(false)
  }, [media?.id, startMarker, endMarker, updateMedia])

  return (
    <Box>
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
          videoRef={videoRef}
          loseFocus={refocusFn}
          setStartMark={handleStartMarkerClick}
          setEndMark={handleEndMarkerClick}
          createSubVideo={handleSubVideoClick}
        />
      )}
      <Container sx={{ paddingX: 0, mt: 1 }}>
        {!loadingMedia && (
          <MediaSection>
            {!editMode && <Box>
              <FavouriteIconButton
                id={params.id}
                state={media.favourite}
                toggleFn={toggleFavourite}
                update={(favourite) => updateMedia({ favourite })}
              />
              <ProgressIconButton
                update={handleProgressStatusUpdate}
                progress={progress || 0}
                runtime={media?.runtime}
              />
            </Box>}
            {editMode && <Box>
              <IconButton onClick={handleStartMarkerClick}>
                <FirstPageIcon color={startMarker !== null ? "primary" : "inherit"} />
              </IconButton>
              <IconButton onClick={handleEndMarkerClick}>
                <LastPageIcon color={endMarker !== null ? "primary" : "inherit"} />
              </IconButton>
            </Box>}
            <Box>
              {editMode
                && startMarker !== null
                && endMarker !== null
                && <IconButton
                  onClick={handleSubVideoClick}
                >
                  <TheatersIcon />
                </IconButton>}
              <IconButton
                sx={{ marginLeft: 'auto' }}
                onClick={handleMenuClick}
                id={`${params.id}-video-card-menu-button`}
                aria-controls={menuAnchorEl ? 'video-card-menu' : undefined}
                aria-haspopup={true}
                aria-expanded={!!menuAnchorEl}
              >
                <MoreVertIcon />
              </IconButton>
            </Box>
          </MediaSection>
        )}
        {loadingMedia && <Skeleton height="50px" width="100%" />}
        {!loadingMedia && (
          <MediaSection>
            <TextEdit
              loseFocus={focusVideo}
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
              loseFocus={focusVideo}
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
              loseFocus={focusVideo}
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
          <Grid container spacing={2}>
            {media.relatedVideos.map(video =>
              <Grid key={video.id} item xs={12} sm={6} md={4} lg={4} xl={4}>
                <VideoCard
                  key={video.id}
                  video={video}
                  overrideLeftAction={<Tooltip title="Remove relationship"><IconButton onClick={handleRelationRemoval(media.id, video.id)}><CallSplitIcon /></IconButton></Tooltip>} />
              </Grid>
            )}
            <Grid item xs={12} sm={6} md={4} lg={4} xl={4}>
              <AddVideoCard id={media.id} setVideos={(videos) => { updateMedia({ relatedVideos: videos }) }} />
            </Grid>
          </Grid>
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
          removeVideo={() => {
            const historyLength = history.length;
            if (historyLength === 1) {
              navigate("/")
            } else {
              navigate(-1);
            }
          }}
          setVideo={updateMedia}
          favourite={!!media.favourite}
          progress={progress}
          hideItems={[
            items.favourite, 
            items.resetProgress, 
            items.toggleSelected, 
            items.removeFromPlaylist]}
          editing={editMode}
          setEditing={setEditMode}
        />
      )}
      {!loadingMedia && <TextInputModal
        open={subVideoNameModalOpen}
        title="Please name your new sub video"
        defaultValue={media?.title}
        onSubmit={handleSubVideoSubmit}
        onCancel={() => setSubVideoNameModalOpen(false)}
      />}
    </Box>
  )
}
