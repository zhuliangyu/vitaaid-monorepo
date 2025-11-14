import { inMemoryToken } from './JwtToken';
import { ProductData } from './Product';
export interface ProtocolData {
  id: number;
  author: string;
  issue: string;
  topic: string;
  blogContent: string;
  banner: string;
  tags: string;
  pdfFile: string;
  published: string;
  relatedProducts: ProductData[];
  dosingText: string;
}

export const getProtocols = async (): Promise<ProtocolData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch('/api/protocols', requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProtocolData[];
  }
};

export const getLatestProtocols = async (count: number): Promise<ProtocolData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/protocols/latest?count=${count}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProtocolData[];
  }
};

export const getProtocol = async (id: number, country: string): Promise<ProtocolData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/protocols/${id}?Country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return {} as ProtocolData;
  }
};
