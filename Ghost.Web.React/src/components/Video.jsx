import React, { useEffect, useRef } from 'react'
import PropTypes from 'prop-types'

export const Video = ({ source, type, poster }) => {
  const videoRef = useRef()

  useEffect(() => videoRef.current?.load(), [source])

  return (<video
    className="ghost-video"
    controls={true}
    ref={videoRef}
    poster={poster}
    playsInline={true}
    src={source}
    type={type}>
  </video>)
}

Video.propTypes = {
  source: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  poster: PropTypes.string
}