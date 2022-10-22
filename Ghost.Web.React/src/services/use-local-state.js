import { useState } from 'react'

export default function useLocalState(key, initialState) {
  let localValue = sessionStorage.getItem(key)
  if (localValue === undefined || localValue === null) {
    localValue = initialState
  } else {
    try {
      localValue = JSON.parse(localValue)
    } catch (err) {
      localValue = initialState
    }
  }

  const [value, setValue] = useState(localValue)

  const setLocalValue = (newValue) => {
    sessionStorage.setItem(key, JSON.stringify(newValue))
    setValue(newValue)
  }

  return [value, setLocalValue]
}
