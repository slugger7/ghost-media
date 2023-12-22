import { createContext } from "react";

export const SelectedVideoContext = createContext({
    selectedVideos: null,
    setSelectedVideos: (selectedVideos) => { }
})

export default SelectedVideoContext