import React, { useState, useEffect } from "react";
import PropTypes from 'prop-types'
import { fetchMedia } from "../services/media.service";
import usePromise from "../services/use-promise";

import { Grid, IconButton, TextField, Tooltip, Typography } from "@mui/material";

import LinkIcon from '@mui/icons-material/Link';
import LinkOffIcon from '@mui/icons-material/LinkOff';

export const ConvertVideoSection = ({ videoId }) => {
  const [video, , videoLoading] = usePromise(() => fetchMedia(videoId), [videoId])

  const [error, setError] = useState({})
  const [title, setTitle] = useState("");
  const [constantRateFactor, setConstantRateFactor] = useState(23);
  const [variableBitrate, setVariableBitrate] = useState();
  const [forcePixelFormat, setForcePixelFormat] = useState('yuv420p');
  const [height, setHeight] = useState(0);
  const [width, setWidth] = useState(0);
  const [originalAspectRatio, setOriginalAspectRatio] = useState(true);
  const [aspectRatio, setAspectRatio] = useState(0)

  useEffect(() => {
    if (video) {
      setTitle(video.title);
      setHeight(video.height);
      setWidth(video.width);
      setAspectRatio(height / width)
    }
  }, [video])

  const handleTitleChange = (event) => {
    setTitle(event.target.value);
    if (event.target.value !== video.title) {
      setError(mergeDeepLeft({ title: undefined }))
    }
  }

  const handleConstantRateFactorChange = (event) => {
    setConstantRateFactor(event.target.value);
  }

  const handleVariableBitrateChange = (event) => {
    setVariableBitrate(event.target.value);
  }

  const handleForcePixelFormatChange = (event) => {
    setForcePixelFormat(event.target.value)
  }

  const handleWidthChange = (event) => {
    const value = event.target.value
    setWidth(value);
    if (originalAspectRatio) {
      const ratio = video.height / video.width;
      setHeight(Math.round(ratio * value))
    }
  }

  const handleHeightChange = (event) => {
    const value = event.target.value;
    setHeight(value)
    if (originalAspectRatio) {
      const ratio = video.width / video.height;
      setWidth(Math.round(ratio * value));
    }
  }

  const handleToggleOriginalAspectRatio = () => {
    setOriginalAspectRatio(!originalAspectRatio)
  }

  return <>
    {/*create skeleton while loading */}
    {!videoLoading && <>
      <Grid item xs={12}><Typography variant="h3" sx={{ textOverflow: "ellipsis", whiteSpace: "nowrap", overflow: "hidden" }}>Convert video: <strong>{video.title}</strong></Typography></Grid>
      <Grid item xs={12}>
        <TextField label="Title" fullWidth value={title} onChange={handleTitleChange} error={!!error.title} helperText={error.title} />
      </Grid>
      <Grid item xs={12} sm={5}><TextField fullWidth type="number" label="Width" value={width} onChange={handleWidthChange} /></Grid>
      <Grid item xs={12} sm={2} sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
        <Tooltip title={originalAspectRatio ? "Keep the original aspect ration" : "Disregard original aspect ratio"}
          sx={{ height: 'original' }}>
          <IconButton aria-label="keep-aspect-ratio" onClick={handleToggleOriginalAspectRatio}>
            {originalAspectRatio && <LinkIcon color="primary" />}
            {!originalAspectRatio && <LinkOffIcon />}
          </IconButton>
        </Tooltip>
      </Grid>
      <Grid item xs={12} sm={5}><TextField fullWidth type="number" label="Height" value={height} onChange={handleHeightChange} /></Grid>
      <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Constant Rate Factor" value={constantRateFactor} onChange={handleConstantRateFactorChange} /></Grid>
      <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Variable Bitrate" value={variableBitrate} onChange={handleVariableBitrateChange} /></Grid>
      <Grid item xs={12}><TextField fullWidth label="Force Pixel Format" value={forcePixelFormat} onChange={handleForcePixelFormatChange} /></Grid></>
    }
  </>
}

ConvertVideoSection.propTypes = {
  videoId: PropTypes.string.isRequired
}