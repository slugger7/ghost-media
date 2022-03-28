import React from 'react'
import { Avatar, IconButton, ListItem, ListItemAvatar, ListItemText } from "@mui/material"
import FolderIcon from "@mui/icons-material/Folder"
import DeleteIcon from "@mui/icons-material/Delete"

export const PathListItem = ({ path, onRemove }) => (<ListItem 
  secondaryAction={<IconButton edge="end" aria-label="delete" onClick={onRemove}>
    <DeleteIcon />
  </IconButton>}>
  <ListItemAvatar>
    <Avatar>
      <FolderIcon />
    </Avatar>
  </ListItemAvatar>
  <ListItemText primary={path} />
</ListItem>)