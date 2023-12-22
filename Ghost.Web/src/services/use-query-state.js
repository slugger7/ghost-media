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
  const [searchParams, setSearchParams] = useSearchParams()
  const searchParam = searchParams.get(key)
  const localValue = getValidSearchParam(searchParam, initialState)

  const [value, setValue] = useState(localValue)

  useEffect(() => {
    const parsedParam = getValidSearchParam(searchParam, initialState);

    if (value && parsedParam !== value) {
      setValue(parsedParam)
    }
  }, [searchParam])

  const modifiedSet = (newValue, otherSearchParams) => {
    setValue(newValue);
    setSearchParams((currentSearchParams) => {
      currentSearchParams.set(key, newValue);
      if (otherSearchParams) {
        const keys = Object.keys(otherSearchParams);
        const values = Object.values(otherSearchParams);
        keys.forEach((key, index) => currentSearchParams.set(key, values[index]));
      }
      return currentSearchParams;
    });
  }

  return [value, modifiedSet]
}
