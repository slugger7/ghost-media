import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import {
  Avatar,
  Card,
  CardHeader,
  IconButton,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
} from '@mui/material'
import PersonIcon from '@mui/icons-material/Person'
import PersonAddAltIcon from '@mui/icons-material/PersonAddAlt'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { Box } from '@mui/system'

export const UserCard = ({ user, updateUser }) => {
  const [anchorEl, setAnchorEl] = useState(null)
  const [isSelected, setIsSelected] = useState(false)

  useEffect(() => {
    const userId = localStorage.getItem('userId')
    if (+userId === user.id) {
      setIsSelected(true)
    }
  }, [user.id])

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleChooseUser = () => {
    localStorage.setItem('username', user.username)
    localStorage.setItem('userId', user.id)
    handleMenuClose()

    updateUser()
  }

  return (
    <Box>
      <Card>
        <CardHeader
          avatar={
            <Avatar variant={isSelected ? 'rounded' : 'circular'}>
              <PersonIcon />
            </Avatar>
          }
          action={
            <IconButton
              id={`${user.id}-user-menu-button`}
              onClick={handleMenuClick}
              aria-controls={anchorEl ? 'user-menu' : undefined}
              aria-haspopup={true}
              aria-expanded={!!anchorEl}
            >
              <MoreVertIcon />
            </IconButton>
          }
          title={user.username}
          subheader={user.id}
        />
      </Card>
      <Menu
        id={`${user.id}-library-menu`}
        anchorEl={anchorEl}
        open={!!anchorEl}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleChooseUser}>
          <ListItemIcon>
            <PersonAddAltIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText>Choose user</ListItemText>
        </MenuItem>
      </Menu>
    </Box>
  )
}

UserCard.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.number.isRequired,
    username: PropTypes.string.isRequired,
  }),
  updateUser: PropTypes.func.isRequired,
}
