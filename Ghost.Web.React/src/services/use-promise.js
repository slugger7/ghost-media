import React, { useEffect, useState } from "react"

export default function usePromise(f, deps) {
  const [result, setResult] = useState()
  const [error, setError] = useState()
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    let subscribed = true
    setLoading(true)

    f()
      .then(r => {
        if (subscribed) {
          setLoading(false)
          setResult(r)
        }
      })
      .catch(e => {
        if (subscribed) {
          setLoading(false)
          setError(e)
        }
      })

    return () => {
      subscribed = false
    }
  }, deps || [])

  return {result, error, loading}
}