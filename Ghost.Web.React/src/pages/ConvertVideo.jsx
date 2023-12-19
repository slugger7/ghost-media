import React, { useContext } from "react";
import { convertVideo } from "../services/media.service";
import { useNavigate, useParams } from "react-router-dom";
import { Button, Container, Grid, } from "@mui/material";
import { mergeDeepLeft } from "ramda";
import SelectedVideoContext from "../context/selectedVideos.context";
import { ConvertVideoSection } from "../components/ConvertVideoSection";

export const ConvertVideo = () => {
  const params = useParams();
  const { selectedVideos } = useContext(SelectedVideoContext)

  const navigate = useNavigate();

  const handleConvertClick = async () => {
    if (title === video.title) {
      setError(mergeDeepLeft({ title: "Title cannot be the same as current video title" }))
      return;
    }

    await convertVideo(params.id, {
      title, constantRateFactor, variableBitrate, forcePixelFormat, width, height
    });

    navigate(-1)
  }

  const handleCancelClick = () => {
    navigate(-1)
  }

  return <Container>
    <Grid container spacing={1}>
      {!selectedVideos && <ConvertVideoSection videoId={params.id} />}
      {selectedVideos?.map(video => <ConvertVideoSection videoId={video} key={video} />)}
      <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'end', gap: 1 }}>
        <Button variant="outlined" onClick={handleCancelClick}>Cancel</Button>
        <Button variant="contained" onClick={handleConvertClick}>Convert</Button>
      </Grid>
    </Grid>
  </Container >
}