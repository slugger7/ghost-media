import { useEffect, useState } from 'react'
import { useSearchParams } from 'react-router-dom'

const getValidSearchParam = (searchParam, initialState) => {
  if (searchParam !== null && searchParam !== null) {
    try {
      return JSON.parse(searchParam)
    } catch (err) {
      return searchParam
    }
  }
  return initialState
}

export default function useQueryState(key, initialState) {
  const [searchParams] = useSearchParams()
  const searchParam = searchParams.get(key)
  const localValue = getValidSearchParam(searchParam, initialState)

  const [value, setValue] = useState(localValue)

  useEffect(() => {
    const parsedParam = getValidSearchParam(searchParam, initialState);

    if (value && parsedParam !== value) {
      setValue(parsedParam)
    }
  }, [searchParam])

  return [value, setValue]
}
