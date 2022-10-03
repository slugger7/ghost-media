import React, { useEffect, useRef, useState } from 'react'
import PropTypes from 'prop-types'
import { Box } from '@mui/material'
import axios from 'axios'
import { VideoProgress } from './VideoProgress'

const keyFunctions = {
  "KeyL": (currentTime) => currentTime + 30,
  "KeyJ": (currentTime) => currentTime - 10
}

export const Video = ({ 
  source,
  type,
  poster, 
  chapter, 
  duration, 
  currentProgress, 
  progressUpdate 
}) => {
  const videoRef = useRef()
  const [currentTime, setCurrentTime] = useState();

  useEffect(() => {
    videoRef.current?.load()
    videoRef.current?.focus()
    if (videoRef) {
      videoRef.current.ontimeupdate = () => {
        setCurrentTime(videoRef.current.currentTime);
      }
    }
  }, [source])

  useEffect(() => {
    if (currentProgress !== undefined) {
      videoRef.current.currentTime = currentProgress;
      setCurrentTime(currentProgress)
    }
  }, [currentProgress])

  useEffect(() => {
    if (chapter) {
      videoRef.current.currentTime = chapter.timestamp / 1000;
      videoRef.current.play();
    }
  }, [chapter])

  useEffect(() => {
    progressUpdate(currentTime)
  }, [currentTime]);

  const handleKeystroke = (event) => {
    const fn = keyFunctions[event.code];
    if (fn) {
      videoRef.current.currentTime = fn(videoRef.current.currentTime);
    }
  }

  return (<Box>
    <video
      onKeyUp={handleKeystroke}
      className="ghost-video"
      autoPlay={!!chapter}
      controls={true}
      ref={videoRef}
      poster={chapter
        ? `${axios.defaults.baseURL}/image/${chapter.image.id}/${chapter.image.name}`
        : poster}
      playsInline={false}
      src={source}
      type={type}
      onPlay={() => videoRef.current.focus()}>
    </video >
    <VideoProgress duration={duration} current={currentTime} />
  </Box>)
}

Video.propTypes = {
  source: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  poster: PropTypes.string,
  chapter: PropTypes.shape({
    timestamp: PropTypes.number.isRequired,
    image: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired
    }).isRequired
  }),
  duration: PropTypes.number.isRequired,
  progressUpdate: PropTypes.func.isRequired,
  currentProgress: PropTypes.number
}