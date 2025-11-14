/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';

import {
  AccountPageType,
  accountPageIdx,
  profile_page,
  address_book_page,
  order_history_page,
  order_history_detail_page,
  patient_order_history_page,
  patient_order_history_detail_page,
} from 'redux/features/account/accountPageSlice';
import { orderNoOfHistory } from 'redux/features/account/orderNoOfHistorySlice';
import { Profile } from 'components/Account/Profile';
import { OrderHistory } from 'components/Account/OrderHistory';
import { OrderHistoryDetail } from 'components/Account/OrderHistoryDetail';
import { PatientOrderHistory } from 'components/Account/PatientOrderHistory';
import { PatientOrderHistoryDetail } from 'components/Account/PatientOrderHistoryDetail';
import { AddressBook } from 'components/Account/AddressBook';
import { ChangePassword } from 'components/Account/ChangePassword';
export const AccountPage = () => {
  const currentAccountPageIdx = useSelector(accountPageIdx);
  const country = useSelector(selectedCountry);
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const navigate = useNavigate();

  React.useEffect(() => {
    if (account == null) navigate('/');
  }, [account]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Account</title>
      </Helmet>
      {currentAccountPageIdx === AccountPageType.profile_page && <Profile />}
      {currentAccountPageIdx === AccountPageType.address_book_page && <AddressBook />}
      {currentAccountPageIdx === AccountPageType.change_password_page && <ChangePassword />}
      {currentAccountPageIdx === AccountPageType.order_history_page && <OrderHistory />}
      {currentAccountPageIdx === AccountPageType.order_history_detail_page && (
        <OrderHistoryDetail />
      )}
      {currentAccountPageIdx === AccountPageType.patient_order_history_page && (
        <PatientOrderHistory />
      )}
      {currentAccountPageIdx == AccountPageType.patient_order_history_detail_page && (
        <PatientOrderHistoryDetail />
      )}
    </React.Fragment>
  );
};
