import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Box } from '@mui/material'
import axios from 'axios'
import { VideoProgress } from './VideoProgress'
import { mergeDeepLeft } from 'ramda'

const keyFunctions = {
  KeyL: (currentTime) => currentTime + 30,
  KeyJ: (currentTime) => currentTime - 10,
}

const shiftedKeyFunctions = {
  KeyL: (currentTime) => currentTime + 5,
  KeyJ: (currentTime) => currentTime - 5,
}

export const Video = ({
  source,
  type,
  poster,
  chapter,
  duration,
  currentProgress,
  progressUpdate,
  videoRef,
  loseFocus,
  setStartMark,
  setEndMark,
  createSubVideo
}) => {
  const [currentTime, setCurrentTime] = useState()
  const [keysDown, setKeysDown] = useState({});

  useEffect(() => {
    videoRef?.current?.load()
    videoRef?.current?.focus()
    if (videoRef && videoRef.current) {
      videoRef.current.ontimeupdate = () => {
        setCurrentTime(videoRef?.current?.currentTime) // <-- this was the problem not any of the others (that I know of)
      }
    }
  }, [source, videoRef])

  useEffect(() => {
    if (videoRef && videoRef.current && currentProgress !== undefined) {
      videoRef.current.currentTime = currentProgress
      setCurrentTime(currentProgress)
    }
  }, [currentProgress, videoRef])

  useEffect(() => {
    if (chapter && videoRef && videoRef.current) {
      videoRef.current.currentTime = chapter.timestamp / 1000
      videoRef?.current.play()
    }
  }, [chapter, videoRef])

  useEffect(() => {
    progressUpdate(currentTime)
  }, [currentTime, progressUpdate])

  const handleKeyUp = (event) => {
    if ((keysDown.ControlLeft || keysDown.ControlRight) && event.code === "Enter") {
      createSubVideo()
      return
    }
    if (keysDown.ShiftLeft || keysDown.ShiftRight) {
      if (setEndMark && event.code === "KeyM") {
        setEndMark()
        return;
      }
      const fn = shiftedKeyFunctions[event.code]
      if (fn) {
        videoRef.current.currentTime = fn(videoRef.current.currentTime)
      }
    } else {
      if (setStartMark && event.code === "KeyM") {
        setStartMark()
        return;
      }
      const fn = keyFunctions[event.code]
      if (fn) {
        videoRef.current.currentTime = fn(videoRef.current.currentTime)
      } else {
        if (loseFocus && event.code === 'Escape') {
          loseFocus()
        }
      }
    }

    setKeysDown(mergeDeepLeft({ [event.code]: false }))
  }

  const handleKeyDown = (event) => {
    setKeysDown(mergeDeepLeft({ [event.code]: true }))
  }

  return (
    <Box>
      <video
        onKeyUp={handleKeyUp}
        onKeyDown={handleKeyDown}
        className="ghost-video"
        autoPlay={!!chapter}
        controls={true}
        ref={videoRef}
        poster={
          chapter
            ? `${axios.defaults.baseURL}/image/${chapter.image.id}/${chapter.image.name}`
            : poster
        }
        playsInline={false}
        src={source}
        type={type}
        onPlay={() => videoRef?.current.focus()}
      ></video>
      <VideoProgress duration={duration} current={currentTime} />
    </Box>
  )
}

Video.propTypes = {
  source: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  poster: PropTypes.string,
  chapter: PropTypes.shape({
    timestamp: PropTypes.number.isRequired,
    image: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
  }),
  duration: PropTypes.number.isRequired,
  progressUpdate: PropTypes.func.isRequired,
  currentProgress: PropTypes.number,
  videoRef: PropTypes.object.isRequired,
  loseFocus: PropTypes.func,
  setStartMark: PropTypes.func,
  setEndMark: PropTypes.func,
  createSubVideo: PropTypes.func
}
