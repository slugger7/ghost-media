import React from "react";
import PropTypes from 'prop-types'
import { Card, CardHeader } from "@mui/material";

export const PlaylistItem = ({ playlist, action, onClick, selected }) => {

  return <>
    <Card onClick={onClick} raised={selected} sx={{cursor: onClick ? 'pointer': null}}>
      <CardHeader 
        title={playlist.name} 
        action={action}
      />
    </Card>
  </>
}

PlaylistItem.propTypes = {
  playlist: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    createdAt: PropTypes.string.isRequired,
    playlistVideos: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.number.isRequired,
    })).isRequired
  }).isRequired,  
  action: PropTypes.node,
  onClick: PropTypes.func,
  selected: PropTypes.bool
}