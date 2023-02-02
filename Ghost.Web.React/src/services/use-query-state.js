import { useState } from 'react'
import { useSearchParams } from 'react-router-dom'

export default function useQueryState(key, initialState, parse = true) {
  const [searchParams] = useSearchParams()
  const searchParam = searchParams.get(key)
  let localValue = localStorage.getItem(key)

  if (searchParam !== undefined && searchParam !== null) {
    localValue = searchParam
  }

  if (localValue === undefined || localValue === null) {
    localValue = initialState
  } else if (parse) {
    try {
      localValue = JSON.parse(localValue)
    } catch (err) {
      localValue = initialState
    }
  }

  const [value, setValue] = useState(localValue)

  const setLocalValue = (newValue) => {
    localStorage.setItem(key, JSON.stringify(newValue))
    setValue(newValue)
  }

  return [value, setLocalValue]
}
