import { createContext } from "react";

export const SelectedVideoContext = createContext({
    selectedVideos: null,
    // eslint-disable-next-line no-unused-vars
    setSelectedVideos: (selectedVideos) => { }
})

export default SelectedVideoContext