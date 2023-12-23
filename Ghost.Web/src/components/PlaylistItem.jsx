import React from "react";
import PropTypes from 'prop-types'
import { Card, CardHeader } from "@mui/material";

export const PlaylistItem = ({ playlist, action, selected, component, to, ...props}) => {

  return <>
    <Card raised={selected} sx={{cursor: props.onClick ? 'pointer': null}} {...props}>
      <CardHeader 
        component={component}
        to={to}
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
  selected: PropTypes.bool,
  component: PropTypes.elementType,
  to: PropTypes.string,
}