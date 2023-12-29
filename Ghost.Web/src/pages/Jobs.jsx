import React, { useCallback } from 'react'
import usePromise from '../services/use-promise'
import axios from 'axios'
import { Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material'
import DeleteIcon from '@mui/icons-material/Delete';

const getJobs = async () => (await axios.get('job')).data

export const Jobs = () => {
    const [jobs, , loading, setJobs] = usePromise(getJobs)

    const clearCompleted = useCallback(async () => {
        await axios.delete('job/status/completed')
        const jobs = await getJobs();
        setJobs(jobs);
    }, [setJobs]);

    return <Box>
        <Box
            sx={{
                my: 1,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                flexWrap: 'wrap',
            }}
        >
            <Button onClick={clearCompleted}><DeleteIcon /> Clear completed</Button>
        </Box>
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Job name</TableCell>
                        <TableCell>Status</TableCell>
                        <TableCell>Created</TableCell>
                        <TableCell>Modified</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {!loading && jobs.map(job => <TableRow key={job.id}>
                        <TableCell>{job.threadName}</TableCell>
                        <TableCell>{job.status}</TableCell>
                        <TableCell>{job.created}</TableCell>
                        <TableCell>{job.modified}</TableCell>
                    </TableRow>)}
                    {!loading && jobs.length === 0 && <TableRow>
                        <TableCell colSpan={4}>No jobs at the moment</TableCell>
                    </TableRow>}
                </TableBody>
            </Table>
        </TableContainer>
    </Box>
}