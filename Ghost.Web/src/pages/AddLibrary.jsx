import React, { useState, Fragment } from 'react'
import {
  Box,
  TextField,
  Typography,
  List,
  IconButton,
  Button,
  ListItem,
  Paper,
  Container,
  Divider,
  Stack
} from '@mui/material'
import AddIcon from "@mui/icons-material/Add"
import { remove, append } from 'ramda'
import axios from 'axios'

import { PathListItem } from '../components/PathListItem.jsx'
import { AddPathModal } from '../components/AddPathModal.jsx'
import { useNavigate } from 'react-router-dom'

const createLibrary = async (name) => (await axios.post('library', { name })).data
const createPaths = async ({ id, paths }) => (await axios.put(`library/${id}/add-paths`, { paths }))

export const AddLibrary = () => {
  const navigate = useNavigate()
  const [libraryName, setLibraryName] = useState();
  const [chosenPaths, setChosenPaths] = useState([]);
  const [addingPath, setAddingPath] = useState(false);

  const addPath = (path) => {
    setChosenPaths(append(path))
    setAddingPath(false)
  }

  const removePathAtIndex = (index) => () => setChosenPaths(remove(index, 1))

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newLibrary = await createLibrary(libraryName)
    await createPaths({ id: newLibrary.id, paths: chosenPaths })
    navigate('/libraries')
  }

  const handleCancel = () => {
    navigate('/libraries');
  }

  return (<Container><Box
    component="form"
    onSubmit={handleSubmit}
  >
    <AddPathModal open={addingPath} addPath={addPath} onClose={() => setAddingPath(false)} />

    <Typography variant='h3' gutterBottom component='h3'>Add library</Typography>
    <TextField sx={{ mb: 1 }} label="Name" variant="outlined" value={libraryName} onChange={e => setLibraryName(e.target.value)} />
    <Paper sx={{ p: 1, mb: 1 }}>
      <Typography variant='h4' component='h4'>Paths <IconButton
        onClick={() => setAddingPath(true)}>
        <AddIcon />
      </IconButton>
      </Typography>
      <List dense={true} sx={{ mb: 1 }}>
        {chosenPaths?.length === 0 && <ListItem>No folders</ListItem>}
        {chosenPaths?.map((chosenFolder, index) => <Fragment key={index}><PathListItem
          path={chosenFolder} onRemove={removePathAtIndex(index)}
        />
          {index + 1 !== chosenPaths.length && <Divider sx={{ m: 1 }} />}
        </Fragment>)}
      </List>
    </Paper>
    <Stack direction="row" spacing={1}>
      <Button variant='contained' type='submit'>Create Library</Button>
      <Button onClick={handleCancel}>Cancel</Button>
    </Stack>
  </Box>
  </Container>)
}