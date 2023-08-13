import React from 'react'
import usePromise from '../services/use-promise'
import axios from 'axios'
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material'

export const Jobs = () => {
    const [jobs, error, loading] = usePromise(async () => (await axios.get('job')).data)

    // return <pre>{JSON.stringify(jobs, null, 2)}</pre>

    return <TableContainer component={Paper}>
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
            </TableBody>
        </Table>
    </TableContainer>
}