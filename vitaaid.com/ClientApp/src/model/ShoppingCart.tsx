import { inMemoryShoppingCartToken } from './ShoppingCartToken';
import { AddressData } from './AddressBook';

import {
  cartChanged,
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
} from 'redux/features/shoppingcart/shoppingCartSlice';

export interface OrderItemData {
  code: string;
  name: string;
  qty: number;
  price: number;
  discount: number;
  discountName: string;
  amount: number;
  itemType: string;
}
export interface OrderData {
  customerCode: string;
  poNo: string;
  orderDate: Date;
  currency: string;
  netSales: number;
  dAdjustmentDiscountPercentage: number;
  cartDiscountName: string;
  adjustment: number;
  shippingFee: number;
  extendedAreaSubcharge: number;
  shippingByQuote: boolean;
  subTotal: number;
  total: number;
  dTaxAmount: number;
  balanceDue: number;
  taxRate: number;
  taxTitle: string;
  dTotalByEarlyPayment: number;
  dExtraAdjustment: number;
  orderItems: OrderItemData[];
  comment: string;
  useAmountbyGiftCard: number;
  billingAddress: AddressData;
  shippingAddress: AddressData;
}

export interface CreditCardData {
  cardNo: string;
  expiryDate: string;
  cid: string;
  holder: string;
  phone: string;
  address: string;
}

export const buildOrder = async (
  customerCode: string,
  webSite: string,
  cart: ShoppingCartItem[],
  billingAddrID: number,
  shippingAddrID: number,
  couponCode?: string, // optional
): Promise<OrderData> => {

  let url = `${process.env.REACT_APP_SHOPPING_CART_URL}/api/ShoppingCart/buildOrder?customercode=${customerCode}&billingAddrID=${billingAddrID}&shippingAddrID=${shippingAddrID}&webSite=${webSite}`;
  if (couponCode && couponCode.length > 0) {
    url += `&couponCode=${couponCode}`;
  }
  
  const requestOptions = {
    method: 'PUT',
    headers: {
      ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(cart),
  };
  let data: Promise<OrderData> = {} as Promise<OrderData>;
  await fetch(
    url,
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
};

export const putOrder = async (
  memberID: number,
  customerCode: string,
  webSite: string,
  cart: ShoppingCartItem[],
  billingAddrID: number,
  shippingAddrID: number,
  dropShipping: boolean,
  paymentMethod: string,
  orderComment: string,
  creditCardData: CreditCardData,
  couponCode?: string, // optional
): Promise<OrderData> => {
  const requestOptions = {
    method: 'PUT',
    headers: {
      ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      Authorization: `Bearer ${inMemoryShoppingCartToken.access_token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      memberID,
      customerCode,
      billingAddrID,
      shippingAddrID,
      dropShipping,
      paymentMethod,
      orderComment,
      cart,
      creditCardData,
    }),
  };
  let data: Promise<OrderData> = {} as Promise<OrderData>;
  let url = `${process.env.REACT_APP_SHOPPING_CART_URL}/api/ShoppingCart/putOrder?webSite=${webSite}`;
  if (couponCode && couponCode.length > 0) {
    url += `&couponCode=${couponCode}`;
  }
  await fetch(
    url,
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
};
