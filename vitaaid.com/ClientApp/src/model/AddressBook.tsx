import { inMemoryToken } from './JwtToken';
import { inMemoryShoppingCartToken } from './ShoppingCartToken';
import { eOPSTATE } from 'redux/features/OPStateSlice';
export interface AddressData {
  id: number;
  defaultBillingAddress: boolean;
  defaultShippingAddress: boolean;
  addressPerson: string;
  addressName: string;
  address: string;
  city: string;
  province: string;
  postalCode: string;
  country: string;
  tel: string;
  fax: string;
  descOnInvoice: string;
}

export const newAddress = (): AddressData => {
  let newObj: AddressData = {
    id: 0,
    defaultBillingAddress: false,
    defaultShippingAddress: false,
    addressPerson: '',
    addressName: '',
    address: '',
    city: '',
    province: '',
    postalCode: '',
    country: '',
    tel: '',
    fax: '',
    descOnInvoice: '',
  };
  return newObj;
};

export const getAddressBook = async (customerCode: string): Promise<AddressData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
        'Content-Type': 'application/json',
      },
    };
    let _Data: Promise<AddressData[]> = {} as Promise<AddressData[]>;
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/AddressBook/${customerCode}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return [] as AddressData[];
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as Promise<AddressData[]>;
  }
};

export const defaultAddressChange = async (
  customerCode: string,
  addressID: number,
  type: string,
  bSameAsBillingAddr: boolean = false,
): Promise<AddressData[]> => {
  try {
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
        'Content-Type': 'application/json',
      },
    };
    let _Data: Promise<AddressData[]> = {} as Promise<AddressData[]>;
    await fetch(
      `${
        process.env.REACT_APP_SHOPPING_CART_URL
      }/api/AddressBook/defaultAddressChange/${customerCode}?addressID=${addressID}&type=${
        type == 'billing' && bSameAsBillingAddr ? 'both' : type
      }`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return [] as AddressData[];
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as Promise<AddressData[]>;
  }
};

export const updateAddress = async (
  customerCode: string,
  newAddress: AddressData,
): Promise<AddressData[]> => {
  try {
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newAddress),
    };
    let _Data: Promise<AddressData[]> = {} as Promise<AddressData[]>;
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/AddressBook/updateAddress/${customerCode}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return [] as AddressData[];
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as Promise<AddressData[]>;
  }
};

export const removeAddress = async (
  customerCode: string,
  addressID: number,
): Promise<AddressData[]> => {
  try {
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
        'Content-Type': 'application/json',
      },
    };
    let _Data: Promise<AddressData[]> = {} as Promise<AddressData[]>;
    await fetch(
      `${process.env.REACT_APP_SHOPPING_CART_URL}/api/AddressBook/removeAddress/${customerCode}?addressID=${addressID}`,
      requestOptions,
    )
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return [] as AddressData[];
        }
        _Data = response.json();
      })
      .catch((error) => {
        throw error;
      });
    return _Data;
  } catch (e) {
    return {} as Promise<AddressData[]>;
  }
};
