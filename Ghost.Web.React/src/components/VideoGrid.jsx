import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Container, Grid, Pagination, Stack } from '@mui/material'
import { remove } from 'ramda'

import { VideoCard } from './VideoCard.jsx'
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx'
import { Search } from './Search';
import { NothingHere } from './NothingHere.jsx'

const removeVideo = ({ index, setVideos }) => () =>
  setVideos(remove(index, 1))

export const VideoGrid = ({ videosPage, page, count, onPageChange, setSearch, search }) => {
  const [videos, setVideos] = useState([]);

  useEffect(() => {
    if (videosPage && !videosPage.loading && videosPage.result) {
      setVideos(videosPage.result.content);
    }
  }, [videosPage]);

  const paginationComponent = <>
    {count > 1 && page && <Pagination
      color="primary"
      page={page}
      defaultPage={1}
      count={count}
      showFirstButton showLastButton
      onChange={onPageChange}
    />}
  </>

  return <>
    <Container sx={{ p: 1 }}>
      <Stack direction="row" spacing={1}>
        <Search search={search} setSearch={setSearch} />
        {paginationComponent}
      </Stack>
    </Container>
    <Grid container spacing={2}>
      {videosPage.loading && <Grid item xs={12} sm={6} md={4} lg={3} xl={2}><VideoCardSkeleton /></Grid>}
      {!videosPage.loading && videos.length === 0 && <Grid item xs={12}><NothingHere>Nothing here. Add a library and sync it to have videos appear here</NothingHere></Grid>}

      {!videosPage.loading && videos.map((video, index) => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard video={video} remove={removeVideo({ index, setVideos })} />
      </Grid>)}
    </Grid>
    <Container sx={{ p: 1 }}>
      {paginationComponent}
    </Container>
  </>
}

VideoGrid.propTypes = {
  videosPage: PropTypes.shape({
    loading: PropTypes.bool.isRequired,
    error: PropTypes.object,
    result: PropTypes.object
  }),
  page: PropTypes.number,
  count: PropTypes.number.isRequired,
  onPageChange: PropTypes.func.isRequired,
  search: PropTypes.string.isRequired,
  setSearch: PropTypes.func.isRequired
}