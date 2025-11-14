import { createSlice } from '@reduxjs/toolkit';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates, by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"
// state.value:
//  0: CA
//  1: USA
export const countrySlice = createSlice({
  name: 'country',
  initialState: {
    value: sessionStorage.getItem('country') == null ? 'CA' : sessionStorage.getItem('country'),
  },
  reducers: {
    ca: (state) => {
      sessionStorage.setItem('country', 'CA');
      state.value = 'CA';
    },
    usa: (state) => {
      sessionStorage.setItem('country', 'US');
      state.value = 'US';
    },
  },
});

export const { ca, usa } = countrySlice.actions;

// ref https://redux.js.org/tutorials/essentials/part-2-app-structure
// A thunk is a specific kind of Redux function that can contain asynchronous logic. Thunks are written using two functions:
//   An inside thunk function, which gets dispatch and getState as arguments
//   The outside creator function, which creates and returns the thunk function
// The function below is called a thunk and allows us to perform async logic.
// It can be dispatched like a regular action: `dispatch(incrementAsync(ca))`.
// This will call the thunk with the `dispatch` function as the first argument.
// Async code can then be executed and other actions can be dispatched
export const caAsync = () => (dispatch: any) => {
  setTimeout(() => {
    dispatch(ca());
  }, 1);
};
export const usaAsync = () => (dispatch: any) => {
  setTimeout(() => {
    dispatch(usa());
  }, 1);
};

export const selectedCountry = (state: any) => state.country.value;
export default countrySlice.reducer;
export const IsEqualSite = (site: number, target: string): boolean => {
  if (site == 255) return true;
  if (site == 1 && target == 'CA') return true;
  if (site == 2 && target == 'US') return true;
  return false;
};
export const ChangeCountrySite = (dispatch: any, site: number) => {
  if (site == 1) dispatch(ca());
  else if (site == 2) dispatch(usa());
};
