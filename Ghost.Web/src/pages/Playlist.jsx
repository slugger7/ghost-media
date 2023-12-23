import React from 'react'
import { useParams } from 'react-router-dom'
import { VideoView } from '../components/VideoView';
import { fetchVideosFromPlaylist } from '../services/playlists.service';

const fetchPlaylist = (playlistId) => async (params) => {
  return await fetchVideosFromPlaylist(playlistId, params);
}

export const Playlist = () => {
  const params = useParams();

  return (
    <VideoView fetchFn={fetchPlaylist(params.playlistId)}/>
  )
}