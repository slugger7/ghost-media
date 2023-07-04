import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Modal, Paper, Typography, Button, Stack, Input, FormControlLabel, Checkbox, FormControl, FormLabel } from '@mui/material'
import CompareIcon from '@mui/icons-material/Compare';

const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    boxShadow: 24,
    p: 4
};

// Change convert to be a separate page flow
export const ConvertVideoModal = ({ title, open, onClose, onConfirm }) => {
    const [newTitle, setNewTitle] = useState(title);
    const [overwrite, setOverwrite] = useState(false)

    const handleTitleChange = (events) => {
        setNewTitle(events.target.value)
    }

    return <Modal
        open={open}
        onClose={onClose}
        aria-labelledby="ghost-convert-video-modal-title"
        aria-describedby="ghost-convert-video-modal-description"
    >
        <Paper sx={modalStyle}>
            <Typography id="ghost-convert-video-modal-title" variant="h6" component="h6">
                Convert video
            </Typography>
            <Typography id="ghost-convert-video-modal-description" sx={{ my: 2 }}>
                You are about to convert <strong>{title}</strong>. Please choose a new name for the converted video.
            </Typography>
            <Input value={newTitle} sx={{ width: '100%', mb: 1 }} onChange={handleTitleChange} />
            <FormControlLabel control={<Checkbox checked={overwrite} />} label="Overwrite existing" />
            <Stack direction="row" justifyContent="flex-end" spacing={1} >
                <Button variant="outlined" color="primary" onClick={onClose}>Cancel</Button>
                <Button
                    variant="contained"
                    onClick={() => onConfirm({ title: newTitle, overwrite })}
                    startIcon={<CompareIcon />}>Convert video</Button>
            </Stack>
        </Paper>
    </Modal>
}

ConvertVideoModal.propTypes = {
    title: PropTypes.string.isRequired,
    open: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    onConfirm: PropTypes.func.isRequired
}