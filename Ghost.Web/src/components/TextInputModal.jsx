import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Button, FormGroup, Modal, Paper, TextField, Typography } from '@mui/material';
import LoadingButton from '@mui/lab/LoadingButton';
import { Stack } from '@mui/system';

const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    boxShadow: 24,
    p: 4
};

export const TextInputModal = ({ open, title, defaultValue, onSubmit, onCancel }) => {
    const [value, setValue] = useState(defaultValue);
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
        setLoading(true)
        try {
            await onSubmit(value);
        } finally {
            setLoading(false)
        }
    }

    return <Modal
        open={open}
        onClose={onCancel}
    >
        <Paper sx={modalStyle}>
            <Typography variant="h6" component="h6">{title}</Typography>
            <FormGroup sx={{ mb: 1 }}>
                <TextField variant='outlined' value={value} onChange={e => setValue(e.target.value)} />
            </FormGroup>
            <Stack direction="row" justifyContent='flex-end' spacing={1}>
                <Button variant="outlined" color="primary" onClick={onCancel} disabled={loading}>Cancel</Button>
                <LoadingButton
                    onClick={handleSubmit}
                    variant='contained'
                    loading={loading}
                >
                    Submit
                </LoadingButton>
            </Stack>
        </Paper>
    </Modal>
}

TextInputModal.propTypes = {
    open: PropTypes.bool.isRequired,
    title: PropTypes.string.isRequired,
    defaultValue: PropTypes.string,
    onSubmit: PropTypes.func.isRequired,
    onCancel: PropTypes.func.isRequired
}