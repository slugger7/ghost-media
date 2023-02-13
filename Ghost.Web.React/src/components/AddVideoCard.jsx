import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types';
import { Card, CardActionArea, Modal, Paper, Tooltip, Typography, Grid, Pagination, Stack, Button, Checkbox, FormControl, FormControlLabel } from '@mui/material'
import LoadingButton from '@mui/lab/LoadingButton';
import MergeIcon from '@mui/icons-material/Merge';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import { Search } from './Search';
import usePromise from '../services/use-promise';
import { fetchVideos, relateVideos } from '../services/video.service';
import { VideoCardSkeleton } from './VideoCardSkeleton';
import { VideoCard } from './VideoCard';
import watchStates from '../constants/watch-states';

const modalStyle = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  boxShadow: 24,
  p: 4,
  width: '90%'
};

export const AddVideoCard = ({ id, setVideos }) => {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [count, setCount] = useState(0)
  const [selectedVideo, setSelectedVideo] = useState(null);
  const [relateBothWays, setRelateBothWays] = useState(false)
  const [videosPage, error, loading] = usePromise(() => fetchVideos({
    limit: 4,
    search,
    page,
    watchState: watchStates.all.value
  }), [search, page])
  const [loadingConfirm, setLoadingConfirm] = useState(false)

  const onClose = () => {
    setSearch('')
    setPage(1)
    setSelectedVideo(null)
    setOpen(false)
  }

  const onConfirm = async () => {
    setLoadingConfirm(true)
    try {
      const relatedVideos = await relateVideos({ id, relateToId: selectedVideo.id });
      if (relateBothWays) {
        await relateVideos({ id: selectedVideo.id, relateToId: id });
      }
      setVideos(relatedVideos)
    } finally {
      setLoadingConfirm(false);
      onClose();
    }
  }

  useEffect(() => {
    if (!loading && !error) {
      setCount(Math.ceil(videosPage.total / 4) || 1)
    }
  }, [videosPage, error, loading])

  return (<>
    <Card sx={{
      maxHeight: '400px',
      height: '100%',
      display: 'flex',
      justifyContent: 'space-between',
      flexDirection: 'column'
    }}>
      <Tooltip title="Add new relation"><CardActionArea sx={{
        height: "100%",
        display: "flex",
        alignContent: 'center',
        justifyContent: 'center'
      }}
        onClick={() => setOpen(true)}>
        <AddCircleOutlineIcon sx={{ width: "50%", height: "50%", opacity: "50%" }} />
      </CardActionArea>
      </Tooltip>
    </Card>
    <Modal
      open={open}
      onClose={onClose}>
      <Paper sx={modalStyle} elevation={0}>
        <Typography variant="h3">Relate video to</Typography>
        <Search search={search} setSearch={setSearch} />
        {!loading && <>
          <Grid container spacing={2}>
            {videosPage.content.map(video =>
              <Grid key={video.id} item xs={12} sm={6} md={3} lg={3} xl={3}>
                <VideoCard
                  disabled={loadingConfirm}
                  key={video.id}
                  video={video}
                  onClick={setSelectedVideo}
                  selected={selectedVideo?.id === video.id}
                  disableActions={true}
                />
              </Grid>
            )}
          </Grid>
          <Pagination
            size='small'
            color='primary'
            page={page}
            defaultPage={1}
            count={count}
            onChange={(e, newPage) => setPage(newPage)}
          />
        </>}
        {loading && <Grid container spacing={2} sx={{ width: '100%' }}>
          <Grid item xs={12} sm={6} md={3} lg={3} xl={3}><VideoCardSkeleton /></Grid>
          <Grid item xs={12} sm={6} md={3} lg={3} xl={3}><VideoCardSkeleton /></Grid>
          <Grid item xs={12} sm={6} md={3} lg={3} xl={3}><VideoCardSkeleton /></Grid>
          <Grid item xs={12} sm={6} md={3} lg={3} xl={3}><VideoCardSkeleton /></Grid>
        </Grid>}
        <Stack direction="row" justifyContent="flex-end" spacing={1} >
          <FormControlLabel
            control={<Checkbox
              value={relateBothWays}
              onChange={(e) => {
                setRelateBothWays(e.target.checked)
              }} title='Relate Both Ways'>Relate both ways</Checkbox>}
            label="Relate both ways" />
          <Button variant="outlined" color="primary" onClick={onClose} disabled={loadingConfirm}>Cancel</Button>
          <LoadingButton
            variant="contained"
            color="primary"
            onClick={onConfirm}
            loading={loadingConfirm}
            startIcon={<MergeIcon />}>Relate</LoadingButton>
        </Stack>
      </Paper>
    </Modal>
  </>)
}

AddVideoCard.propTypes = {
  id: PropTypes.number.isRequired,
  setVideos: PropTypes.func.isRequired
} 