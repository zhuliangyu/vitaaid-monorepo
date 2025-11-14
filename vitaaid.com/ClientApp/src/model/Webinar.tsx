import { inMemoryToken } from './JwtToken';
import { ProductData } from './Product';
export interface WebinarData {
  id: number;
  author: string;
  issue: string;
  reference: string;
  topic: string;
  webinarContent: string;
  videoLink: string;
  tags: string;
  thumbnail: string;
  published: string;
  relatedProducts: ProductData[];
}

export const getWebinars = async (): Promise<WebinarData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch('/api/webinars', requestOptions);
    return data.json();
  } catch (e) {
    return [] as WebinarData[];
  }
};

export const getLatestWebinars = async (count: number): Promise<WebinarData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/webinars/latest?count=${count}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as WebinarData[];
  }
};

export const getWebinar = async (id: number, country: string): Promise<WebinarData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/webinars/${id}?Country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return {} as WebinarData;
  }
};
