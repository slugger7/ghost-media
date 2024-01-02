import React, { useState } from "react";
import PropTypes from "prop-types";
import { Avatar, Card, CardHeader, IconButton, Menu } from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { Box } from "@mui/system";

export const UserCard = ({ user }) => {
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  return (
    <Box>
      <Card>
        <CardHeader
          avatar={
            <Avatar variant={"circular"}>
              <PersonIcon />
            </Avatar>
          }
          action={
            <IconButton
              id={`${user.id}-user-menu-button`}
              onClick={handleMenuClick}
              aria-controls={anchorEl ? "user-menu" : undefined}
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
        {/* <MenuItem onClick={handleChooseUser}>
          <ListItemIcon>
            <PersonAddAltIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText>Choose user</ListItemText>
        </MenuItem> */}
      </Menu>
    </Box>
  );
};

UserCard.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.number.isRequired,
    username: PropTypes.string.isRequired,
  }),
};
