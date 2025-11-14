import { inMemoryToken } from './JwtToken';
import { inMemoryShoppingCartToken } from './ShoppingCartToken';

export interface ProductData {
  id: number;
  oldID: number;
  category: string;
  category1: string;
  productName: string;
  function: string;
  productCode: string;
  tags: string;
  description: string;
  indications: string;
  combination: string;
  suggest: string;
  productSheet: string;
  image: string;
  timeStamp: Date;
  size: string;
  size2: string;
  caution: string;
  npn: string;
  featured: boolean;
  supplementFact: string;
  published: string;
  visibleSite: number;
  additionalInfo: string;
  oIngredients: IngredientData[];
  oProductImages: ProductImageData[];
  vitaaidCategory: number[];
  allergyCategory: number[];
  representativeImage: string;
  servingSize: number;
  servingUnit: string;
  sServingSize: string;
  servingsPerContainer: number;
}

export interface IngredientData {
  id: number;
  name: string;
  labelClaim: string;
  labelClaimUS: string;
  additionalInfo: string;
  groupNo: number;
  sequence: number;
}

export interface ProductImageData {
  id: number;
  frontImage: boolean;
  imageName: string;
  largeImageName: string;
  flag: string;
  width: number;
  height: number;
  largeWidth: number;
  largeHeight: number;
}

export interface WishData {
  customerCode: string;
  qty: number;
}

export const getAlphabetList = async (country: string): Promise<string[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch(`/api/Products/alphabetlist?country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as string[];
  }
};

export const getProducts = async (): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch('/api/Products', requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductData[];
  }
};

export const getProductsByKeyword = async (
  keyword: string,
  country: string,
): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch(`/api/Products?filter=${keyword}&siteFilter=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductData[];
  }
};

export const getProduct = async (code: string, country: string): Promise<ProductData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let _Data: Promise<ProductData> = {} as Promise<ProductData>;
    await fetch(`/api/Products/${code}?country=${country}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return {} as ProductData;
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as ProductData;
  }
};

export const getProductsByCategory = async (
  category: number,
  country: string,
): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch(`/api/Products/${category}/${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductData[];
  }
};

export const getProductsByAlphabet = async (
  ch: string,
  country: string,
): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch(`/api/Products/alphabet?ch=${ch}&country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductData[];
  }
};

export const getFeaturedProducts = async (country: string): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch(`/api/Products/featured?country=${country}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductData[];
  }
};

// export const getProductsByCategoryName = async (
//   category: string,
//   country: string,
// ): Promise<ProductData[]> => {
//   try {
//     const requestOptions = {
//       method: 'GET',
//       headers: {
//         ApiKey: `${process.env.REACT_APP_API_KEY!}`,
//         'Content-Type': 'application/x-www-form-urlencoded',
//         Authorization: `Bearer ${inMemoryToken.access_token}`,
//       },
//     };
//     var data = await fetch(
//       `/api/Products/${category}/${country}`,
//       requestOptions,
//     );
//     return data.json();
//   } catch (e) {
//     return [] as ProductData[];
//   }
// };

export const getRelatedProducts = async (code: string, country: string): Promise<ProductData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let _Data: Promise<ProductData[]> = {} as Promise<ProductData[]>;
    await fetch(`/api/Products/related/${code}?country=${country}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return [] as ProductData[];
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return [] as ProductData[];
  }
};

export const getProductImages = async (productcode: string): Promise<ProductImageData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
    };
    var data = await fetch(`/api/ProductImages?productcode=${productcode}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as ProductImageData[];
  }
};

export interface snp {
  stock: number;
  price: number;
  dropShipPrice: number;
}
export const getStockNPrice = async (
  productCode: string,
  customerCode: string,
  country: string,
): Promise<snp> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
      },
    };
    let data: Promise<snp> = {} as Promise<snp>;
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/Products/${productCode}/stocknprice?customercode=${customerCode}&country=${country}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          throw new Error('Bad response from server');
        }
        data = response.json();
      })
      .catch((error) => {
        throw error;
      });

    return data;
  } catch (e) {
    return { stock: 0, price: 0, dropShipPrice: 0 };
  }
};
export interface ElementalNutritionData {
  productCode: string;
  price: number;
  calories: number;
}

export const getNutrition = async (country: string): Promise<ElementalNutritionData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
      },
    };
    let data: Promise<ElementalNutritionData[]> = {} as Promise<ElementalNutritionData[]>;
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/Products/nutrition?country=${country}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          throw new Error('Bad response from server');
        }
        data = response.json();
      })
      .catch((error) => {
        throw error;
      });

    return data;
  } catch (e) {
    return {} as ElementalNutritionData[];
  }
};

export const putWishProduct = async (productCode: string, wishData: WishData) => {
  try {
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(wishData),
    };
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/Products/${productCode}/wishproduct`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return;
        }
      })
      .catch((error) => {
        throw error;
      });
    return;
  } catch (e) {
    return;
  }
};
