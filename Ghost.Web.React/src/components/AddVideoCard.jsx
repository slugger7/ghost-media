import React from 'react'
import { Card, CardActionArea, Tooltip } from '@mui/material'
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';


export const AddVideoCard = () => {


  return (<Card sx={{
    maxHeight: '400px',
    height: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    flexDirection: 'column'
  }}>
    <Tooltip title="Add new relation"><CardActionArea sx={{
      height: "100%",
      display: "flex",
      alignContent: 'center',
      justifyContent: 'center'
    }}>
      <AddCircleOutlineIcon sx={{ width: "50%", height: "50%", opacity: "50%" }} />
    </CardActionArea>
    </Tooltip>
  </Card>)
}

AddVideoCard.propTypes = {

} 