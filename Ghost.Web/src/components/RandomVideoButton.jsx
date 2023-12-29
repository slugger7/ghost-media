import React, { useState } from 'react'
import PropTypes from 'prop-types'
import ShuffleIcon from '@mui/icons-material/Shuffle';
import { CircularProgress, IconButton } from '@mui/material';
import { useNavigate, useSearchParams } from 'react-router-dom';
import watchStates from '../constants/watch-states';
import useLocalState from '../services/use-local-state';

const findRandomVideo = async ({ search, watchState, genres, setLoading, navigate, fetchFn }) => {
    setLoading(true)
    try {
        const video = await fetchFn({ search, watchState, genres })

        navigate(`/media/${video.id}/${video.title}`)
    } finally {
        setLoading(false)
    }
}

export const RandomVideoButton = ({ fetchFn }) => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false)
    const search = searchParams.get('search') || ''
    const watchState = searchParams.get('watchState') || watchStates.all
    const [genres] = useLocalState('genres', []);

    return <IconButton
        onClick={() => findRandomVideo({ search, watchState, genres, setLoading, navigate, fetchFn })}>
        {!loading && <ShuffleIcon />}
        {loading && <CircularProgress />}
    </IconButton>
}

RandomVideoButton.propTypes = {
    fetchFn: PropTypes.func.isRequired
}