import { mergeDeepRight } from 'ramda'

export const updateSearchParamsService =
  (setSearchParams, current) => (newSearchParams) =>
    setSearchParams(mergeDeepRight(current, newSearchParams))

export const getSearchParamsObject = (searchParams) => ({
  limit: searchParams.get('limit') ? parseInt(searchParams.get('limit')) : null,
  page: searchParams.get('page') ? parseInt(searchParams.get('page')) : null,
  search: searchParams.get('search')
    ? decodeURIComponent(searchParams.get('search'))
    : null,
  sortBy: searchParams.get('sortBy')
    ? decodeURIComponent(searchParams.get('sortBy'))
    : null,
  ascending:
    (searchParams.get('ascending') !== 'false' &&
      searchParams.get('ascending') !== 'true') ||
    searchParams.get('ascending') === 'true',
})
