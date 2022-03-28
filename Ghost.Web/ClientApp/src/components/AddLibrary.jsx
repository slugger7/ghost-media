import React, { useState } from 'react'
import { 
  Box, 
  TextField, 
  Typography, 
  List,
  IconButton
} from '@mui/material'
import AddIcon from "@mui/icons-material/Add"
import { remove, append } from 'ramda'

import { FolderListItem } from './FolderListItem.jsx'
import { AddFolderModal } from './AddFolderModal.jsx'

export const AddLibrary = () => {
  const [chosenFolders, setChosenFolders] = useState(["here"]);
  const [addingFolder, setAddingFolder] = useState(false);

  const addFolder = (path) => {
    setChosenFolders(append(path))
    setAddingFolder(false)
  }

  const removeFolderAtIndex = (index) => () => setChosenFolders(remove(index, 1))

  return (<Box
    component="form"
    >
      <AddFolderModal open={addingFolder} addFolder={addFolder} onClose={() => setAddingFolder(false)}/>
      <Typography variant='h3' gutterBottom component='h3'>Add library</Typography>
      <TextField label="Name" variant="outlined" />
      <Typography variant='h4' component='h4'>Folders <IconButton
          onClick={() => setAddingFolder(true)}>
          <AddIcon />
        </IconButton>
      </Typography>
      <List dense>
        {chosenFolders?.map((chosenFolder, index) => <FolderListItem 
          key={index} path={chosenFolder} onRemove={removeFolderAtIndex(index)}
        />)}
      </List>
  </Box>)
}