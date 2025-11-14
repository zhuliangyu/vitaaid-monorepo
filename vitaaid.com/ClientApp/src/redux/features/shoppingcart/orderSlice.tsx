import { createSlice } from '@reduxjs/toolkit';
import { OrderData, OrderItemData } from 'model/ShoppingCart';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates,
//        by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"
export const orderSlice = createSlice({
  name: 'order',
  initialState: {
    value:
      sessionStorage.getItem('order') == null
        ? ({} as OrderData)
        : (JSON.parse(sessionStorage.getItem('order')!) as OrderData),
  },
  reducers: {
    orderChanged: (state, action) => {
      sessionStorage.setItem('order', JSON.stringify(action.payload));
      state.value = action.payload;
    },
  },
});

export const { orderChanged } = orderSlice.actions;
export const order = (state: any) =>
  state && state.order && state.order.value ? (state.order.value as OrderData) : ({} as OrderData);
export default orderSlice.reducer;
