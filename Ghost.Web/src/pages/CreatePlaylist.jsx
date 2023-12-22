import React, {useState} from "react";
import { createPlaylist } from "../services/playlists.service";
import { useNavigate } from "react-router";

export const CreatePlaylist = () => {
  const [name, setName] = useState('');
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    setLoading(true)

    try {
      await createPlaylist({ name })
    } finally {
      setLoading(false)
    }
  }
  return <>
    <h1>Create a new playlist</h1>
    <form onSubmit={handleSubmit}>
      <label htmlFor="name">Name</label>
      <input 
        type="text" 
        id="name" 
        value={name} 
        onChange={(e) => setName(e.target.value)} />
      <button type="submit">Create</button>
    </form>
  </>
}