import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { useSearchParams } from 'react-router-dom';
import { VideoGrid } from './VideoGrid.jsx';

const fetchVideos = async (page, limit) => (await axios.get(`media?page=${page - 1}&limit=${limit}`)).data

export const Home = () => {
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(1)
  const videosPage = useAsync(fetchVideos, [page, limit])
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(parseInt(searchParams.get("limit")) || 48)
    setPage(parseInt(searchParams.get("page")) || 1)
  }, [searchParams])


  return (<VideoGrid
    videosPage={videosPage}
    onPageChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
    page={page}
    count={Math.ceil(total / limit) || 1}
  />)
}
