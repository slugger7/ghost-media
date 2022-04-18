import React, { useEffect, useRef } from 'react'
import PropTypes from 'prop-types'

export const Video = ({ source, type, poster }) => {
  const videoRef = useRef()

  useEffect(() => videoRef.current?.load(), [source])

  return (<video controls ref={videoRef} poster={poster}>
    <source src={source} type={type} />
  </video>)
}

Video.propTypes = {
  source: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  poster: PropTypes.string
}