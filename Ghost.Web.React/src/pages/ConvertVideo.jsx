import React, { useContext } from "react";
import { useParams } from "react-router-dom";
import { Container, Grid } from "@mui/material";
import SelectedVideoContext from "../context/selectedVideos.context";
import { ConvertVideoSection } from "../components/ConvertVideoSection";

export const ConvertVideo = () => {
  const params = useParams();
  const { selectedVideos } = useContext(SelectedVideoContext)


  return <Container>
    <Grid container spacing={1}>
      {!selectedVideos && <ConvertVideoSection videoId={params.id} />}
      {selectedVideos?.map(video => <ConvertVideoSection videoId={video} key={video} />)}
    </Grid>
  </Container >
}