/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { OrderData, OrderItemData, buildOrder } from 'model/ShoppingCart';
import { order, orderSlice, orderChanged } from 'redux/features/shoppingcart/orderSlice';

import {
  CartPageType,
  cartPageIdx,
  shopping_cart_page,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
} from 'redux/features/shoppingcart/cartPageSlice';
import {
  productCategory,
  productCategoryChanged,
} from 'redux/features/product/productCategorySlice';
import {
  productFilterMethod,
  byCategory,
  byAlphabet,
  byKeyword,
  eFILTERMETHOD,
} from 'redux/features/product/productFilterMethodSlice';
import { productCode, productCodeChanged } from 'redux/features/product/productCodeSlice';
import { useEffect } from 'react';
import { OrderItems } from 'components/OrderItems';

import { freemem } from 'os';

interface Props {
  isMobile: boolean;
}

export const ShoppingCartDetail = ({ isMobile }: Props) => {
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  let cart = useSelector(shoppingCart);
  let orderData = useSelector(order);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      const data = await buildOrder(account.customerCode, country, cart, 0, 0);
      dispatch(orderChanged(data));
    }
    if (account && cart && cart.length > 0) fetchData();
    else {
      dispatch(orderChanged({} as OrderData));
    }
  }, [cart]);
  return (
    <React.Fragment>
      <div className="content-main-body">
        <div className="shopping_cart_detail">
          <div className="row">
            <div className="col-12 title">Shopping Cart</div>
          </div>
          {account && account.customerCode && orderData && (
            <Fragment>
              <div className="row">
                <div className="col-12 header">
                  <div className="header-label">
                    Account No. : <span className="header-value">{account.customerCode}</span>
                  </div>
                  <div className="header-label">
                    Currency. : <span className="header-value">{orderData.currency}</span>
                  </div>
                </div>
              </div>
              <OrderItems
                account={account}
                orderData={orderData}
                isReadOnly={false}
                isMobile={isMobile}
              />

              {/* <div className="row">
                <form className="col-12">
                  {isMobile === false && (
                    <table className="cart-detail-table">
                      <thead className="table-header">
                        <tr>
                          <th
                            css={css`
                              width: 10%;
                            `}
                          >
                            No.
                          </th>
                          <th
                            css={css`
                              width: 10%;
                            `}
                          >
                            Code
                          </th>
                          <th
                            css={css`
                              width: 35%;
                            `}
                          >
                            Product Name
                          </th>
                          <th
                            css={css`
                              width: 15%;
                            `}
                          >
                            Unit Price
                          </th>
                          <th
                            css={css`
                              width: 8%;
                              padding-left: 5px;
                            `}
                          >
                            Qty
                          </th>
                          <th
                            css={css`
                              width: 12%;
                            `}
                          >
                            Discount
                          </th>
                          <th
                            css={css`
                              width: 10%;
                              text-align: end;
                            `}
                          >
                            Subtotal
                          </th>
                        </tr>
                      </thead>
                      <tbody className="table-body">
                        {Object.keys(orderData).length == 0 && (
                          <tr>
                            <td>
                              <div className="table-cell no-item">NO ITEM</div>
                            </td>
                            <td />
                            <td />
                            <td />
                            <td />
                            <td />
                            <td />
                          </tr>
                        )}
                        {orderData &&
                          orderData.orderItems &&
                          orderData.orderItems.map((x, idx) => {
                            return (
                              <tr key={x.code}>
                                <td>
                                  <div className="table-cell">
                                    {x.discount < 100 && x.itemType !== 'CREDIT' && (
                                      <button
                                        className="borderless-btn remove-button"
                                        onClick={() => {
                                          dispatch(removeCartItem(x.code));
                                        }}
                                      >
                                        x
                                      </button>
                                    )}
                                    {(x.discount >= 100 || x.itemType === 'CREDIT') && (
                                      <button
                                        className="borderless-btn remove-button"
                                        onClick={() => {}}
                                      ></button>
                                    )}
                                    {`${idx + 1}.`}
                                  </div>
                                </td>
                                <td>
                                  <div className="table-cell">{x.code}</div>
                                </td>
                                <td>
                                  <div
                                    className="table-cell"
                                    dangerouslySetInnerHTML={{ __html: x.name }}
                                  ></div>
                                </td>
                                <td>
                                  <div className="table-cell">
                                    {x.itemType !== 'CREDIT' && `$${x.price.toFixed(2)}`}
                                  </div>
                                </td>
                                <td>
                                  {x.itemType === 'CREDIT' && <div className="table-cell" />}
                                  {x.itemType !== 'CREDIT' && (
                                    <div className="table-cell">
                                      {x.discount < 100 && (
                                        <input
                                          className="qty-input"
                                          type="text"
                                          defaultValue={x.qty}
                                          onChange={(e: React.FormEvent<HTMLInputElement>) => {
                                            let newValue: number = parseInt(
                                              e.currentTarget.value,
                                              10,
                                            );
                                            if (isNaN(newValue)) newValue = 0;
                                            const item: ShoppingCartItem = {
                                              code: x.code,
                                              name: x.name,
                                              qty: newValue,
                                              price: x.price,
                                              dropShipPrice: x.price,
                                              size: '',
                                              image: '',
                                            };
                                            dispatch(updateCartQty(item));
                                          }}
                                        ></input>
                                      )}
                                      {x.discount >= 100 && (
                                        <div
                                          css={css`
                                            text-align: center;
                                          `}
                                        >
                                          {x.qty}
                                        </div>
                                      )}
                                    </div>
                                  )}
                                </td>
                                <td>
                                  <div
                                    className="table-cell"
                                    css={css`
                                      text-align: center;
                                    `}
                                  >
                                    {x.discount > 0 && `${x.discount}%`}
                                    {x.discount > 0 && x.discountName.length > 0 && (
                                      <span className="item-discount-name">{` (${x.discountName})`}</span>
                                    )}
                                  </div>
                                </td>
                                <td>
                                  <div
                                    className="table-cell"
                                    css={css`
                                      text-align: end;
                                    `}
                                  >
                                    {x.amount < 0 && `($${(x.amount * -1).toFixed(2)})`}
                                    {x.amount >= 0 && `$${x.amount.toFixed(2)}`}
                                  </div>
                                </td>
                              </tr>
                            );
                          })}
                      </tbody>
                    </table>
                  )}
                </form>
              </div> */}
              <div className="row">
                <div className="col-12 cart-summary">
                  {orderData && orderData.dAdjustmentDiscountPercentage > 0 && (
                    <Fragment>
                      <div> Netsales: ${orderData?.netSales?.toFixed(2) ?? 0.0}</div>
                      <div>
                        {orderData.cartDiscountName && `${orderData.cartDiscountName}: `}
                        {orderData.cartDiscountName.length === 0 &&
                          `${orderData.dAdjustmentDiscountPercentage}% OFF: `}
                        {`($${orderData.adjustment.toFixed(2)})`}
                      </div>
                    </Fragment>
                  )}
                  <div> Subtotal: ${orderData?.subTotal?.toFixed(2) ?? 0.0}</div>
                </div>
              </div>
              <div className="row">
                <div className="col-12 cart-action">
                  {orderData && orderData.orderItems && (
                    <Fragment>
                      <button
                        onClick={() => {
                          dispatch(productCodeChanged(''));
                          dispatch(byCategory());
                          dispatch(productCategoryChanged(''));
                          navigate('/products');
                        }}
                      >
                        CONTINUE SHOPPING
                      </button>
                      <button
                        onClick={() => {
                          dispatch(billing_shipping_page());
                        }}
                      >
                        CHECKOUT
                      </button>
                    </Fragment>
                  )}
                  {Object.keys(orderData).length == 0 && (
                    <Fragment>
                      <button
                        onClick={() => {
                          dispatch(productCodeChanged(''));
                          dispatch(byCategory());
                          dispatch(productCategoryChanged(''));
                          navigate('/products');
                        }}
                      >
                        BACK TO PRODUCTS
                      </button>
                    </Fragment>
                  )}
                </div>
              </div>
            </Fragment>
          )}
          <div className="row">
            <div className="col-12 cart-note">
              <p>
                Most orders are shipped within 48 hours on working days. During holiday season,
                orders may be delayed.
              </p>
              <p>
                Free Shipping Requirements:
                <ul>
                  <li>
                    Orders over CDN $250 to BC, Alberta, Saskatchewan, Manitoba (1-2 Day Delivery)
                  </li>
                  <li>
                    Orders over CDN $350 to Ontario, Quebec, NS, NB, NF, and PEI (Next Day Delivery)
                  </li>
                  <li>
                    Orders USD $350 to Continental United States (Alaska and Hawaii, please inquire for rate)
                  </li>
                </ul>
              </p>
              <p>
                Orders below free shipping requirements are subject to the following fees:
                <ul>
                  <li>$15 CAD for shipments to areas within BC</li>
                  <li>
                    $20 CAD for shipments to all provinces outside BC in Canada
                  </li>
                  <li>
                    $25 USD for Continental United States (Alaska and Hawaii, please inquire for rate)
                  </li>
                  {/* <li>$20 USD for all US destinations.</li> */}
                </ul>
              </p>
              <p>Orders under $100 are subject to an additional $15 handling fee.</p>
              <p>
                The above shipping policy does not apply to certain remote areas; in which case, a remote area surcharge (actual rate incurred by the carrier) may apply.
              </p>
              <p>Other restrictions may apply.</p>
              <p>
                Errors or shortages in your order must be reported within 48hrs of receipt of goods.
                Please email info@vitaaid.com , or call us at 1-604-260-0696 for more information.
              </p>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
