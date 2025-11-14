/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { addressBook, addressBookChanged } from 'redux/features/addressbook/addressBookSlice';
import { OrderHistoryData, getOrderHistory } from 'model/Member';
import { AddressData, newAddress, getAddressBook, defaultAddressChange } from 'model/AddressBook';
import {
  AccountPageType,
  accountPageIdx,
  profile_page,
  address_book_page,
  order_history_page,
  order_history_detail_page,
} from 'redux/features/account/accountPageSlice';
import { AddressBookSetting } from 'components/ShoppingCart/AddressBookSetting';
import {
  orderNoOfHistory,
  orderNoOfHistoryChanged,
} from 'redux/features/account/orderNoOfHistorySlice';

export const AddressBook = () => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  let addressBookData = useSelector(addressBook);

  const [orderHistories, setOrderHistories] = React.useState<OrderHistoryData[]>([]);
  const [currentPageOfOrders, setCurrentPageOfOrders] = React.useState<number>(0);
  const [totalPagesOfOrders, setTotalPagesOfOrders] = React.useState<number>(0);
  const [startPageOfNav, setStartPageOfNav] = React.useState<number>(0);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getAddressBook(account.customerCode);
      dispatch(addressBookChanged(data));
    }
    fetchData();
  }, []);

  return (
    <React.Fragment>
      <div className="content-main-body address-book">
        <div className="row">
          <div className="col-12">
            <div className="title">Address Book</div>
            <div className="">
              <AddressBookSetting
                key="billing-setting"
                customerCode={account.customerCode}
                addressBook={addressBookData}
                type="book"
              />
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
