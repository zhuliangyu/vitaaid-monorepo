/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { accountData } from 'redux/features/account/accountSlice';

import {
  CartPageType,
  cartPageIdx,
  shopping_cart_page,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
} from 'redux/features/shoppingcart/cartPageSlice';

import { ShoppingCartDetail } from 'components/ShoppingCart/ShoppingCartDetail';
import { BillingShippingDetail } from 'components/ShoppingCart/BillingShippingDetail';
import { ShoppingSummary } from 'components/ShoppingCart/ShoppingSummary';
import { PaymentMethod } from 'components/ShoppingCart/PaymentMethod';
import { OrderCompletion } from 'components/ShoppingCart/OrderCompletion';
import { useNavigate } from 'react-router-dom';
import { isMobileData } from 'redux/features/isMobileSlice';

export const ShoppingCartPage = () => {
  const currentCartPage = useSelector(cartPageIdx);
  const country = useSelector(selectedCountry);
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const navigate = useNavigate();
  const isMobile = useSelector(isMobileData);

  React.useEffect(() => {
    if (account == null) navigate('/');
  }, [account]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Shopping Cart</title>
      </Helmet>
      {currentCartPage === CartPageType.shopping_cart_page && (
        <ShoppingCartDetail isMobile={isMobile} />
      )}
      {currentCartPage === CartPageType.billing_shipping_page && (
        <BillingShippingDetail isMobile={isMobile} />
      )}
      {currentCartPage === CartPageType.shopping_summary_page && (
        <ShoppingSummary isMobile={isMobile} />
      )}
      {currentCartPage === CartPageType.payment_method_page && (
        <PaymentMethod isMobile={isMobile} />
      )}
      {currentCartPage === CartPageType.order_completion_page && (
        <OrderCompletion isMobile={isMobile} />
      )}
    </React.Fragment>
  );
};
