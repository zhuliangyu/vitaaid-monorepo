/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
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
import { order, orderSlice, orderChanged } from 'redux/features/shoppingcart/orderSlice';
import { OrderData, OrderItemData } from 'model/ShoppingCart';
import { resetOrderCoupon } from 'redux/features/shoppingcart/orderCouponSlice';
interface Props {
  isMobile: boolean;
}
export const OrderCompletion = ({ isMobile }: Props) => {
  const account = useSelector(accountData);
  let orderData = useSelector(order);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  return (
    <React.Fragment>
      <div className="content-main-body">
        <div className="order-completion">
          <div className="row">
            <div className="col-12 title">Order Completion</div>
          </div>
          {account && account.customerCode && orderData && (
            <Fragment>
              <p className="desc-text">
                Thank you for your order. An order confirmation will be sent to your email (email address) shortly.
                <br />
                The order No. of your purchase is : <span className="pono">{orderData.poNo}</span>
              </p>
              <button
                className="finish-btn"
                onClick={() => {
                  dispatch(orderChanged({} as OrderData));
                  // 当订单完成时，重置Coupon Code
                  // 这样用户可以再次输入新的Coupon Code
                  dispatch(resetOrderCoupon());
                  navigate('/');
                }}
              >
                FINISH
              </button>
            </Fragment>
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
