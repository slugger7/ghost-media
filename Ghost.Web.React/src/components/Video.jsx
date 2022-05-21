import React, { useEffect, useRef } from 'react'
import PropTypes from 'prop-types'
import axios from 'axios'

export const Video = ({ source, type, poster, chapter }) => {
  const videoRef = useRef()

  useEffect(() => videoRef.current?.load(), [source])

  return (<video
    className="ghost-video"
    autoPlay={!!chapter}
    controls={true}
    ref={videoRef}
    poster={chapter
      ? `${axios.defaults.baseURL}/image/${chapter.image.id}/${chapter.image.name}`
      : poster}
    playsInline={true}
    src={`${source}${chapter ? `#t=${chapter?.timestamp / 1000}` : ''}`}
    type={type}>
  </video>)
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