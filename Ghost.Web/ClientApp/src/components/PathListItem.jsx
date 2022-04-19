import React from 'react'
import { Avatar, IconButton, ListItem, ListItemAvatar, ListItemText } from "@mui/material"
import FolderIcon from "@mui/icons-material/Folder"
import DeleteIcon from "@mui/icons-material/Delete"
import PropTypes from 'prop-types'

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

PathListItem.propTypes = {
  path: PropTypes.string.isRequired,
  onRemove: PropTypes.func.isRequired
}