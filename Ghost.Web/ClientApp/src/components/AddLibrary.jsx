import React, { useState } from 'react'
import {
  Box,
  TextField,
  Typography,
  List,
  IconButton,
  Button,
  ListItem,
  Paper
} from '@mui/material'
import AddIcon from "@mui/icons-material/Add"
import { remove, append } from 'ramda'
import axios from 'axios'

import { PathListItem } from './PathListItem.jsx'
import { AddPathModal } from './AddPathModal.jsx'
import { useNavigate } from 'react-router-dom'

const createLibrary = async (name) => (await axios.post('library', { name })).data
const createPaths = async ({ _id, paths }) => (await axios.put(`library/${_id}/add-paths`, { paths }))

export const AddLibrary = () => {
  const navigate = useNavigate()
  const [libraryName, setLibraryName] = useState('test lib name');
  const [chosenPaths, setChosenPaths] = useState(["/home/slugger/dev/ghost-media/assets"]);
  const [addingPath, setAddingPath] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const addPath = (path) => {
    setChosenPaths(append(path))
    setAddingPath(false)
  }

  const removePathAtIndex = (index) => () => setChosenPaths(remove(index, 1))

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true)
    try {
      const newLibrary = await createLibrary(libraryName)
      await createPaths({ _id: newLibrary._id, paths: chosenPaths })
      navigate('/libraries')
    } finally {
      setSubmitting(false)
    }
  }

  return (<Box
    component="form"
    onSubmit={handleSubmit}
  >
    <AddPathModal open={addingPath} addPath={addPath} onClose={() => setAddingPath(false)} />
    <Typography variant='h3' gutterBottom component='h3'>Add library</Typography>
    <TextField label="Name" variant="outlined" value={libraryName} onChange={e => setLibraryName(e.target.value)} />
    <Typography variant='h4' component='h4'>Paths <IconButton
      onClick={() => setAddingPath(true)}>
      <AddIcon />
    </IconButton>
    </Typography>
    <Paper elevation={2}>
      <List dense={true}>
        {chosenPaths?.length === 0 && <ListItem>No folders</ListItem>}
        {chosenPaths?.map((chosenFolder, index) => <PathListItem
          key={index} path={chosenFolder} onRemove={removePathAtIndex(index)}
        />)}
      </List>
    </Paper>
    <Button variant='contained' type='submit'>Create Library</Button>
  </Box>)
}