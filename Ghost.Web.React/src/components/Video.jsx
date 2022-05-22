import React, { useEffect, useRef, useState } from 'react'
import PropTypes from 'prop-types'
import axios from 'axios'

const keyFunctions = {
  "l": (currentTime) => currentTime + 30,
  "j": (currentTime) => currentTime - 30
}

export const Video = ({ source, type, poster, chapter }) => {
  const videoRef = useRef()

  useEffect(() => videoRef.current?.load(), [source])
  useEffect(() => {
    if (chapter) {
      videoRef.current.currentTime = chapter.timestamp / 1000;
    }
  }, [chapter])

  const handleKeystroke = (event) => {
    const fn = keyFunctions[event.key];
    if (fn) {
      videoRef.current.currentTime = fn(videoRef.current.currentTime);
    }
  }

  return (<video
    onKeyUp={handleKeystroke}
    className="ghost-video"
    autoPlay={!!chapter}
    controls={true}
    ref={videoRef}
    poster={chapter
      ? `${axios.defaults.baseURL}/image/${chapter.image.id}/${chapter.image.name}`
      : poster}
    playsInline={true}
    src={source}
    type={type} >
  </video >)
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
  })
}