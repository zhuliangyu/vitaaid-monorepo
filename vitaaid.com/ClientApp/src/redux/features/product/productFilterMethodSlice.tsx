import { createSlice } from '@reduxjs/toolkit';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates,
//        by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"

export enum eFILTERMETHOD {
  CATEGORY,
  ALPHABET,
  KEYWORD,
}

export const productFilterMethodSlice = createSlice({
  name: 'productFilterMethod',
  initialState: {
    value:
      sessionStorage.getItem('productFilterMethod') == null
        ? eFILTERMETHOD.CATEGORY
        : parseInt(sessionStorage.getItem('productFilterMethod')!),
  },
  reducers: {
    byCategory: (state) => {
      state.value = eFILTERMETHOD.CATEGORY;
      sessionStorage.setItem('productFilterMethod', eFILTERMETHOD.CATEGORY.toString());
    },
    byAlphabet: (state) => {
      state.value = eFILTERMETHOD.ALPHABET;
      sessionStorage.setItem('productFilterMethod', eFILTERMETHOD.ALPHABET.toString());
    },
    byKeyword: (state) => {
      state.value = eFILTERMETHOD.KEYWORD;
      sessionStorage.setItem('productFilterMethod', eFILTERMETHOD.KEYWORD.toString());
    },
  },
});

export const { byCategory, byAlphabet, byKeyword } = productFilterMethodSlice.actions;
export const productFilterMethod = (state: any) => state.productFilterMethod.value;
export default productFilterMethodSlice.reducer;
