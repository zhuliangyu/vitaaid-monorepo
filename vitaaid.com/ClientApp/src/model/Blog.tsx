import { inMemoryToken } from './JwtToken';
import { ProductData } from './Product';
export interface BlogData {
  id: number;
  category: number;
  sCategory: string;
  author: string;
  issue: string;
  reference: string;
  topic: string;
  blogContent: string;
  banner: string;
  tags: string;
  volume: number;
  no: number;
  thumb: string;
  published: string;
  relatedProducts: ProductData[];
}

export const getBlog = async (): Promise<BlogData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch('/api/blog', requestOptions);
    return data.json();
  } catch (e) {
    return [] as BlogData[];
  }
};

export const getLatestBlog = async (count: number): Promise<BlogData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/blog/latest?count=${count}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as BlogData[];
  }
};

export const getBlogByCategory = async (category: number): Promise<BlogData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/blog?category=${category}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as BlogData[];
  }
};

export const getBlogArticle = async (id: number, country: string): Promise<BlogData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/blog/${id}?Country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return {} as BlogData;
  }
};
