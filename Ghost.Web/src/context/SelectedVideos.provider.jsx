import React, { useState } from "react"
import PropTypes from 'prop-types'
import { SelectedVideoContext } from "./selectedVideos.context";

export const SelectedVideosProvider = ({ children }) => {
    const [selectedVideos, setSelectedVideos] = useState(null);

    return <SelectedVideoContext.Provider value={{ selectedVideos, setSelectedVideos }}>
        {children}
    </SelectedVideoContext.Provider>
}

SelectedVideosProvider.propTypes = {
    children: PropTypes.node
}