import { useState } from 'react'

export default function useLocalState(key, initialState) {
    let localValue = localStorage.getItem(key);

    if (localValue !== null && localValue !== undefined) {
        try {
            localValue = JSON.parse(localValue)
        } catch { }
    } else {
        localValue = initialState;
    }

    const [value, setValue] = useState(localValue);

    const setLocalValue = (newValue) => {
        localStorage.setItem(key, JSON.stringify(newValue));
        setValue(value)
    }

    return [value, setLocalValue];
}