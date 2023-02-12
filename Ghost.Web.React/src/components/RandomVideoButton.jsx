import React, { useState } from 'react'
import PropTypes from 'prop-types'
import axios from 'axios'
import ShuffleIcon from '@mui/icons-material/Shuffle';
import { CircularProgress, IconButton } from '@mui/material';
import { useNavigate, useSearchParams } from 'react-router-dom';
import watchStates from '../constants/watch-states';

export const RandomVideoButton = ({ fetchFn }) => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false)
    const search = searchParams.get('search') || ''
    const watchState = searchParams.get('watchState') || watchStates.all

    const findRandomVideo = async ({ search, watchState, setLoading }) => {
        setLoading(true)
        try {
            const video = await fetchFn({ search, watchState })
            //(await axios.get(`/media/random?search=${search}&watchState=${watchState}`)).data

            navigate(`/media/${video.id}/${video.title}`)
        } finally {
            setLoading(false)
        }
    }

    return <IconButton
        onClick={() => findRandomVideo({ search, watchState, setLoading })}>
        {!loading && <ShuffleIcon />}
        {loading && <CircularProgress />}
    </IconButton>
}

RandomVideoButton.propTypes = {
    fetchFn: PropTypes.func.isRequired
}