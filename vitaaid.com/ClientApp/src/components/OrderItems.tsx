/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { OrderData, OrderItemData } from 'model/ShoppingCart';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { AddressData, getAddressBook } from 'model/AddressBook';
import { MemberData } from 'model/Member';
import { order } from '../redux/features/shoppingcart/orderSlice';

interface Props {
  account: MemberData;
  orderData: OrderData;
  isReadOnly: boolean;
  isMobile: boolean;
}

const DesktopVersion = ({ account, orderData, isReadOnly, isMobile }: Props) => {
  const dispatch = useDispatch();
  return (
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
              padding-left: ${isReadOnly ? '30' : '5'}px;
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
        {Object.keys(orderData).length === 0 && (
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
                    {isReadOnly === false && (
                      <Fragment>
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
                      </Fragment>
                    )}
                    {`${idx + 1}.`}
                  </div>
                </td>
                <td>
                  <div className="table-cell">{x.code}</div>
                </td>
                <td>
                  <div className="table-cell" dangerouslySetInnerHTML={{ __html: x.name }}></div>
                </td>
                <td>
                  <div className="table-cell price-div">
                    <div
                      css={css`
                        text-align: end;
                      `}
                    >
                      {x.itemType !== 'CREDIT' && `$${x.price.toFixed(2)}`}
                    </div>
                  </div>
                </td>
                <td>
                  {x.itemType === 'CREDIT' && <div className="table-cell" />}
                  {x.itemType !== 'CREDIT' && (
                    <div className="table-cell">
                      {isReadOnly && (
                        <div
                          css={css`
                            text-align: center;
                          `}
                        >
                          {x.qty}
                        </div>
                      )}
                      {isReadOnly === false && (
                        <Fragment>
                          {x.discount < 100 && (
                            <input
                              className="qty-input"
                              type="text"
                              defaultValue={x.qty}
                              onChange={(e: React.FormEvent<HTMLInputElement>) => {
                                let newValue: number = parseInt(e.currentTarget.value, 10);
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
                        </Fragment>
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
  );
};

const MobileVersion = ({ account, orderData, isReadOnly, isMobile }: Props) => {
  const dispatch = useDispatch();
  return (
    <Fragment>
      {Object.keys(orderData).length === 0 && <div className="table-cell no-item">NO ITEM</div>}
      {orderData &&
        orderData.orderItems &&
        orderData.orderItems.map((x, idx) => {
          return (
            <div className={`row order-detail-row-m row-idx-${idx % 2}`}>
              <div className="col-12">
                <div className="detail-info-m">
                  <span className="item-title">No. : </span>
                  <span className="item-value">{`${idx + 1}.`}</span>
                  {isReadOnly === false && (
                    <Fragment>
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
                    </Fragment>
                  )}
                </div>
                <div className="detail-info-m">
                  <span className="item-title">Code. : </span>
                  <span className="item-value">{x.code}</span>
                </div>
                <div className="detail-info-m">
                  <span className="item-title">Product Name. : </span>
                  <div
                    className="item-value"
                    css={css`
                      display: contents;
                    `}
                    dangerouslySetInnerHTML={{ __html: x.name }}
                  ></div>
                </div>
                <div className="detail-info-m">
                  <span className="item-title">Unit Price : </span>
                  <span className="item-value">
                    {' '}
                    {x.itemType !== 'CREDIT' && `$${x.price.toFixed(2)}`}
                  </span>
                </div>
                {x.itemType !== 'CREDIT' && (
                  <div className="detail-info-m">
                    <span className="item-title">Qty. : </span>
                    {isReadOnly && <span className="item-value">{x.qty}</span>}
                    {isReadOnly === false && (
                      <Fragment>
                        {x.discount < 100 && (
                          <input
                            className="qty-input"
                            type="text"
                            defaultValue={x.qty}
                            onChange={(e: React.FormEvent<HTMLInputElement>) => {
                              let newValue: number = parseInt(e.currentTarget.value, 10);
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
                        {x.discount >= 100 && <span className="item-value">{x.qty}</span>}
                      </Fragment>
                    )}
                  </div>
                )}
                {x.discount > 0 && (
                  <div className="detail-info-m">
                    <span className="item-title">Discount. : </span>
                    <span className="item-value">
                      {x.discount > 0 && `${x.discount}%`}
                      {x.discount > 0 && x.discountName.length > 0 && ` (${x.discountName})`}
                    </span>
                  </div>
                )}
                <div className="detail-info-m">
                  <span className="item-title">Subtotal : </span>
                  <span className="item-value">
                    {x.amount < 0 && `($${(x.amount * -1).toFixed(2)})`}
                    {x.amount >= 0 && `$${x.amount.toFixed(2)}`}
                  </span>
                </div>
              </div>
            </div>
          );
        })}
    </Fragment>
  );
};
export const OrderItems = ({ account, orderData, isReadOnly, isMobile }: Props) => {
  return (
    <div className="order-items">
      {isMobile && (
        <MobileVersion
          account={account}
          orderData={orderData}
          isReadOnly={isReadOnly}
          isMobile={isMobile}
        />
      )}
      {isMobile === false && (
        <div className="row">
          {isReadOnly && (
            <div className="col-12">
              <DesktopVersion
                account={account}
                orderData={orderData}
                isReadOnly={isReadOnly}
                isMobile={isMobile}
              />
            </div>
          )}
          {isReadOnly === false && (
            <form className="col-12">
              <DesktopVersion
                account={account}
                orderData={orderData}
                isReadOnly={isReadOnly}
                isMobile={isMobile}
              />
            </form>
          )}
        </div>
      )}
    </div>
  );
};
