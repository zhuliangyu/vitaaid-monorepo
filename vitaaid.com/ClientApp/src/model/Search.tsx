import { inMemoryToken } from './JwtToken';

export interface SearchResultData {
  content: string;
  identity: string;
  type: string;
}

export interface searchResults {
  products: [SearchResultData];
  blogs: [SearchResultData];
  webinars: [SearchResultData];
  protocols: [SearchResultData];
}

export const doSearch = async (
  keyword: string,
  country: string,
  memberType: number,
): Promise<searchResults> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let _Data: Promise<searchResults> = {} as Promise<searchResults>;
    await fetch(
      `/api/search?keyword=${keyword}&country=${country}&memberType=${memberType}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return {} as searchResults;
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as searchResults;
  }
};
