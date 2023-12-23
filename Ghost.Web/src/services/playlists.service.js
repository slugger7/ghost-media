import axios from "axios";
import { constructVideoParams } from "./video.service";

export const fetchPlaylists = async () => (await axios.get(`/playlist`)).data;

export const createPlaylist = async (playlist) => (await axios.post(`/playlist`, playlist)).data;

export const deletePlaylist = async (id) => (await axios.delete(`/playlist/${id}`)).data;

export const addVideosToPlaylist = async (playlistId, videoIds) => (await axios.post(`/playlist/${playlistId}/videos`, {videoIds})).data;

export const fetchVideosFromPlaylist = async (playlistId, params) => (await axios.get(`/playlist/${playlistId}/videos?${constructVideoParams(params)}`)).data;