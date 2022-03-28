import React, { useState } from 'react'
import { Modal, Box, Button, FormGroup, TextField } from '@mui/material'

export const AddFolderModal = ({ open, onClose, addFolder }) => {
  const [path, setPath] = useState('');

  const handlePathChange = (event) => setPath(event.target.value)

  const handleSubmit = () => {
    addFolder(path)
    setPath('')
  }

  return (<Modal
    open={open}
    onClose={onClose}>
      <Box sx={{
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        bgcolor: 'background.paper',
        boxShadow: 24,
        p: 4
      }}>
        <FormGroup>
          <TextField label="Path" variant='outlined' value={path} onChange={handlePathChange}/>
          <Button onClick={handleSubmit} variant='contained'>Add Folder</Button>
        </FormGroup>
      </Box>
    </Modal>)
}