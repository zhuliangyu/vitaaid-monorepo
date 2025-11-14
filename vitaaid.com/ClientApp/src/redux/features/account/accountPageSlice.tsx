import { createSlice } from '@reduxjs/toolkit';

// action: an event that describes something that happened in the application
// reducer: an event listener(handler) which handles events based on the received action (event) type.
// Reducers must always follow some specific rules:
//   They should only calculate the new state value based on the state and action arguments
//   They are not allowed to modify the existing state. Instead, they must make immutable updates, by copying the existing state and making changes to the copied values.
//   They must not do any asynchronous logic, calculate random values, or cause other "side effects"

export enum AccountPageType {
  profile_page = 1,
  address_book_page,
  order_history_page,
  order_history_detail_page,
  patient_order_history_page,
  patient_order_history_detail_page,
  change_password_page,
}

export const accountPageSlice = createSlice({
  name: 'accountPage',
  initialState: {
    value: AccountPageType.profile_page,
  },
  reducers: {
    profile_page: (state) => {
      state.value = AccountPageType.profile_page;
    },
    address_book_page: (state) => {
      state.value = AccountPageType.address_book_page;
    },
    order_history_page: (state) => {
      state.value = AccountPageType.order_history_page;
    },
    order_history_detail_page: (state) => {
      state.value = AccountPageType.order_history_detail_page;
    },
    patient_order_history_page: (state) => {
      state.value = AccountPageType.patient_order_history_page;
    },
    patient_order_history_detail_page: (state) => {
      state.value = AccountPageType.patient_order_history_detail_page;
    },
    change_password_page: (state) => {
      state.value = AccountPageType.change_password_page;
    },
  },
});

export const {
  profile_page,
  address_book_page,
  order_history_page,
  order_history_detail_page,
  patient_order_history_page,
  patient_order_history_detail_page,
  change_password_page,
} = accountPageSlice.actions;

export const accountPageIdx = (state: any) => state.accountPage.value;
export default accountPageSlice.reducer;
