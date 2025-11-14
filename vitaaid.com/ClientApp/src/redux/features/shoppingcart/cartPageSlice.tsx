import { createSlice } from '@reduxjs/toolkit';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates, by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"

export enum CartPageType {
  shopping_cart_page = 1,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
}

export const cartPageSlice = createSlice({
  name: 'cartPage',
  initialState: {
    value: CartPageType.shopping_cart_page,
  },
  reducers: {
    shopping_cart_page: (state) => {
      state.value = CartPageType.shopping_cart_page;
    },
    billing_shipping_page: (state) => {
      state.value = CartPageType.billing_shipping_page;
    },
    shopping_summary_page: (state) => {
      state.value = CartPageType.shopping_summary_page;
    },
    payment_method_page: (state) => {
      state.value = CartPageType.payment_method_page;
    },
    order_completion_page: (state) => {
      state.value = CartPageType.order_completion_page;
    },
  },
});

export const {
  shopping_cart_page,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
} = cartPageSlice.actions;

export const cartPageIdx = (state: any) => state.cartPage.value;
export default cartPageSlice.reducer;
