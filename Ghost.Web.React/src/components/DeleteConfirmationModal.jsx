import { Modal, Typography, Stack, Button, Paper } from '@mui/material'
import LoadingButton from '@mui/lab/LoadingButton';
import React from 'react'
import PropTypes from 'prop-types'
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';

const modalStyle = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  boxShadow: 24,
  p: 4
};

export const DeleteConfirmationModal = ({ title, open, onClose, onConfirm, loadingConfirm = false, ...props }) => <Modal
  open={open}
  onClose={onClose}
  aria-labelledby="ghost-delete-confirmation-modal-title"
  aria-describedby="ghost-delete-confirmation-modal-description"
  {...props}
>
  <Paper sx={modalStyle}>
    <Typography id="ghost-delete-confirmation-modal-title" variant="h6" component="h2">
      Permanently delete video?
    </Typography>
    <Typography id="ghost-delete-confirmation-modal-description" sx={{ my: 2 }}>
      You are about to permanently delete <strong>{title}</strong> from the library and your hard drive.
    </Typography>
    <Stack direction="row" justifyContent="flex-end" spacing={1} >
      <Button variant="outlined" color="primary" onClick={onClose} disabled={loadingConfirm}>Cancel</Button>
      <LoadingButton
        variant="contained"
        color="error"
        onClick={onConfirm}
        loading={loadingConfirm}
        startIcon={<DeleteForeverIcon />}>Delete</LoadingButton>
    </Stack>
  </Paper>
</Modal>

DeleteConfirmationModal.propTypes = {
  title: PropTypes.string.isRequired,
  open: PropTypes.bool.isRequired,
  loadingConfirm: PropTypes.bool,
  onClose: PropTypes.func.isRequired,
  onConfirm: PropTypes.func.isRequired,
}