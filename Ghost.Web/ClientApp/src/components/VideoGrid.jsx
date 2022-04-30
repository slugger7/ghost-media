import React from 'react'
import PropTypes from 'prop-types'
import { Container, Grid, Pagination, Stack, Paper } from '@mui/material'

import { VideoCard } from './VideoCard.jsx'
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx'
import { Search } from './Search';
import { NothingHere } from './NothingHere.jsx'

export const VideoGrid = ({ videosPage, page, count, onPageChange, setSearch, search }) => {
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
      {!videosPage.loading && videosPage.result?.content?.length === 0 && <Grid item xs={12}><NothingHere>Nothing here. Add a library and sync it to have videos appear here</NothingHere></Grid>}

      {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard video={video} />
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