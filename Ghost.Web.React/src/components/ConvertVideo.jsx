import React, { useEffect, useState } from "react";
import { fetchMedia, convertVideo } from "../services/media.service";
import { useNavigate, useParams } from "react-router-dom";
import usePromise from "../services/use-promise";
import { Box, Button, Container, Grid, TextField, Typography } from "@mui/material";
import { mergeDeepLeft } from "ramda";

export const ConvertVideo = () => {
    const params = useParams();
    const navigate = useNavigate();
    const [video, , videoLoading] = usePromise(() => fetchMedia(params.id), [params.id])

    const [error, setError] = useState({})
    const [title, setTitle] = useState("");
    const [constantRateFactor, setConstantRateFactor] = useState(23);
    const [variableBitrate, setVariableBitrate] = useState();
    const [forcePixelFormat, setForcePixelFormat] = useState();
    const [height, setHeight] = useState(0);
    const [width, setWidth] = useState(0);

    useEffect(() => {
        if (video) {
            setTitle(video.title);
            setHeight(video.height);
            setWidth(video.width);
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
        setWidth(event.target.value)
    }

    const handleHeightChange = (event) => {
        setHeight(event.target.value)
    }

    const handleConvertClick = async () => {
        if (title === video.title) {
            setError(mergeDeepLeft({ title: "Title cannot be the same as current video title" }))
            return;
        }

        await convertVideo(params.id, {
            title, constantRateFactor, variableBitrate, forcePixelFormat, width, height
        });

        navigate(`/media/${video.id}/${video.title}`)
    }

    const handleCancelClick = () => {
        navigate(`/media/${video.id}/${video.title}`)
    }

    return <Container>
        {/*create skeleton while loading */}
        {!videoLoading && <><Typography variant="h3">Convert video: <strong>{video.title}</strong></Typography>
            <Grid container spacing={1}>
                <Grid item xs={12}>
                    <TextField label="Title" fullWidth value={title} onChange={handleTitleChange} error={!!error.title} helperText={error.title} />
                </Grid>
                <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Width" value={width} onChange={handleWidthChange} /></Grid>
                <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Height" value={height} onChange={handleHeightChange} /></Grid>
                <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Constant Rate Factor" value={constantRateFactor} onChange={handleConstantRateFactorChange} /></Grid>
                <Grid item xs={12} sm={6}><TextField fullWidth type="number" label="Variable Bitrate" value={variableBitrate} onChange={handleVariableBitrateChange} /></Grid>
                <Grid item xs={12}><TextField fullWidth label="Force Pixel Format" value={forcePixelFormat} onChange={handleForcePixelFormatChange} /></Grid>
                <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'end', gap: 1 }}>
                    <Button variant="outlined" onClick={handleCancelClick}>Cancel</Button>
                    <Button variant="contained" onClick={handleConvertClick}>Convert</Button>
                </Grid>
            </Grid>
        </>
        }
    </Container >
}