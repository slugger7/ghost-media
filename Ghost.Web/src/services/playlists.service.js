import axios from "axios";

export const fetchPlaylists = async () => (await axios.get(`/playlist`)).data;

export const createPlaylist = async (playlist) => (await axios.post(`/playlist`, playlist)).data;

export const deletePlaylist = async (id) => (await axios.delete(`/playlist/${id}`)).data;

export const addVideosToPlaylist = async (playlistId, videoIds) => (await axios.post(`/playlist/${playlistId}/videos`, {videoIds})).data;