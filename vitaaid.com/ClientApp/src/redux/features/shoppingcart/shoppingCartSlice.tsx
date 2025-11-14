import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { useSelector, useDispatch } from 'react-redux';
export interface ShoppingCartItem {
  code: string;
  name: string;
  qty: number;
  price: number;
  dropShipPrice: number;
  size: string;
  image: string;
}

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates,
//        by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"
export const shoppingCartSlice = createSlice({
  name: 'shoppingCart',
  initialState: {
    value:
      sessionStorage.getItem('shoppingCart') == null
        ? ([] as ShoppingCartItem[])
        : (JSON.parse(sessionStorage.getItem('shoppingCart')!) as ShoppingCartItem[]),
  },
  reducers: {
    cartChanged: (state, action) => {
      sessionStorage.setItem('shoppingCart', JSON.stringify(action.payload));
      state.value = action.payload;
    },
    addCartItem: (state, action: PayloadAction<ShoppingCartItem>) => {
      if (!state.value || state.value.length == 0) state.value = [action.payload];
      else {
        const idx = state.value.findIndex((x) => x.code === action.payload.code);
        if (idx == -1) state.value = [...state.value, action.payload];
        else {
          state.value[idx].qty += action.payload.qty;
        }
      }
      sessionStorage.setItem('shoppingCart', JSON.stringify(state.value));
    },
    updateCartQty: (state, action: PayloadAction<ShoppingCartItem>) => {
      const idx = state.value.findIndex((x) => x.code === action.payload.code);
      if (idx == -1) return;
      state.value[idx].qty = action.payload.qty;
      sessionStorage.setItem('shoppingCart', JSON.stringify(state.value));
    },
    removeCartItem: (state, action) => {
      const updatedCart = [...state.value.filter((x) => x.code != action.payload)];
      if (updatedCart.length != state.value.length) {
        state.value = updatedCart;
        sessionStorage.setItem('shoppingCart', JSON.stringify(state.value));
      }
    },
  },
});

export const { cartChanged, addCartItem, updateCartQty, removeCartItem } =
  shoppingCartSlice.actions;
export const shoppingCart = (state: any) =>
  state && state.shoppingCart && state.shoppingCart.value
    ? (state.shoppingCart.value as ShoppingCartItem[])
    : ([] as ShoppingCartItem[]);
export default shoppingCartSlice.reducer;

//export const addToCart = (code: string, name: string, qty: number, price: number) => {
//  const cart = useSelector(shoppingCart);
//  const dispatch = useDispatch();

//  let item: ShoppingCartItem = {
//    code: code,
//    name: name,
//    qty: qty as number,
//    price: price,
//  };
//  if (cart.length === 0) dispatch(cartChanged([item]));
//  else {
//    const idx = cart.findIndex((x) => x.code === item.code);
//    if (idx == -1) dispatch(cartChanged([...cart, item]));
//    else {
//      item.qty += cart[idx].qty;
//      const updatedCart = [...cart];
//      updatedCart[idx] = item;
//      dispatch(cartChanged(updatedCart));
//    }
//  }
//};
/*
export const updateQty = (code: string, newQty: number) => {
  if (newQty <= 0) removeFromCart(code);
  else {
    const cart = useSelector(shoppingCart);

    const idx = cart.findIndex((x) => x.code === code);
    if (idx == -1) return;

    const updatedCart = [...cart];
    updatedCart[idx].qty = newQty;

    const dispatch = useDispatch();
    dispatch(cartChanged(updatedCart));
  }
};

export const removeFromCart = (code: string) => {
  const cart = useSelector(shoppingCart);
  const dispatch = useDispatch();

  const updatedCart = [...cart.filter((x) => x.code != code)];
  if (updatedCart.length != cart.length) {
    dispatch(cartChanged(updatedCart));
  }
};
*/
