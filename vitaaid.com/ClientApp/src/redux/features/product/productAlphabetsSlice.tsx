import { createSlice } from '@reduxjs/toolkit';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates,
//        by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"
export const productAlphabetsSlice = createSlice({
  name: 'productAlphabets',
  initialState: {
    value: [] as string[],
  },
  reducers: {
    productAlphabetsChanged: (state, action) => {
      state.value = action.payload;
    },
  },
});

export const { productAlphabetsChanged } = productAlphabetsSlice.actions;
export const productAlphabets = (state: any) => state.productAlphabets.value;
export default productAlphabetsSlice.reducer;
