/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { OrderData, OrderItemData } from 'model/ShoppingCart';
import { AddressData, getAddressBook } from 'model/AddressBook';
import { MemberData } from 'model/Member';
import { order } from '../redux/features/shoppingcart/orderSlice';
import { OrderItems } from 'components/OrderItems';
interface AddressProps {
  address: AddressData;
  isMobile: boolean;
}
const AddressBlock = ({ address, isMobile }: AddressProps) => {
  if (address)
    return (
      <Fragment>
        {address.addressPerson && <div>{address.addressPerson}</div>}
        {address.addressName && address.addressPerson !== address.addressName && (
          <div>{address.addressName}</div>
        )}
        <div>{address.address}</div>
        <div>
          {address.city}, {address.province}, {address.postalCode}
        </div>
        <div>{address.country}</div>
        <div>Tel: {address.tel}</div>
      </Fragment>
    );
  else return <Fragment />;
};

interface Props {
  account: MemberData;
  orderData: OrderData;
  isMobile: boolean;
}
export const OrderDetail = ({ account, orderData, isMobile }: Props) => {
  function ToMoneyStr(m: number): String {
    if (m < 0) return `($${(m * -1).toFixed(2)})`;
    else return `$${m.toFixed(2)}`;
  }
  function ShippingFeeNotIncludeRemoteAreaSurcharge(oOrder: OrderData): number {
    if (oOrder.shippingByQuote) return 0;
    if (
      oOrder.shippingFee > 0 &&
      oOrder.extendedAreaSubcharge > 0 &&
      oOrder.shippingFee >= oOrder.extendedAreaSubcharge
    )
      return oOrder.shippingFee - oOrder.extendedAreaSubcharge;
    return oOrder.shippingFee;
  }
  function IncludeRemoteAreaSurcharge(oOrder: OrderData): boolean {
    return (
      oOrder.shippingFee > 0 &&
      oOrder.extendedAreaSubcharge > 0 &&
      oOrder.shippingFee >= oOrder.extendedAreaSubcharge
    );
  }
  return (
    <React.Fragment>
      <div className="order-detail">
        {isMobile === false && (
          <div className="row header">
            <div className="col-6 header-label">Billing</div>
            <div className="col-6 header-label">Shipping</div>
          </div>
        )}
        <div className="row address-summary">
          <div className={`${isMobile ? 'col-12' : 'col-6'} address`}>
            {isMobile && <div className="col-6 header-label">Billing</div>}
            <AddressBlock address={orderData.billingAddress} isMobile={isMobile} />
            {orderData.customerCode === account.customerCode && <div>{account.email}</div>}
          </div>
          {isMobile && <div className="gap-address-m"></div>}
          <div className={`${isMobile ? 'col-12' : 'col-6'} address`}>
            {isMobile && <div className="col-6 header-label">Shipping</div>}
            <AddressBlock address={orderData.shippingAddress} isMobile={isMobile} />
          </div>
        </div>
        {/* {isMobile && (
          <Fragment>
            {Object.keys(orderData).length === 0 && (
              <div className="table-cell no-item">NO ITEM</div>
            )}
            {orderData &&
              orderData.orderItems &&
              orderData.orderItems.map((x, idx) => {
                return (
                  <div className={`row order-detail-row-m row-idx-${idx % 2}`}>
                    <div className="col-12">
                      <div className="detail-info-m">
                        <span className="title">No. : </span>
                        <span className="value">{`${idx + 1}.`}</span>
                      </div>
                      <div className="detail-info-m">
                        <span className="title">Code. : </span>
                        <span className="value">{x.code}</span>
                      </div>
                      <div className="detail-info-m">
                        <span className="title">Product Name. : </span>
                        <div
                          className="value"
                          css={css`
                            display: contents;
                          `}
                          dangerouslySetInnerHTML={{ __html: x.name }}
                        ></div>
                      </div>
                      <div className="detail-info-m">
                        <span className="title">Unit Price : </span>
                        <span className="value">
                          {' '}
                          {x.itemType !== 'CREDIT' && `$${x.price.toFixed(2)}`}
                        </span>
                      </div>
                      {x.itemType !== 'CREDIT' && (
                        <div className="detail-info-m">
                          <span className="title">Qty. : </span>
                          <span className="value">{x.qty}</span>
                        </div>
                      )}
                      {x.discount > 0 && (
                        <div className="detail-info-m">
                          <span className="title">Discount. : </span>
                          <span className="value">
                            {x.discount > 0 && `${x.discount}%`}
                            {x.discount > 0 && x.discountName.length > 0 && ` (${x.discountName})`}
                          </span>
                        </div>
                      )}
                      <div className="detail-info-m">
                        <span className="title">Subtotal : </span>
                        <span className="value">
                          {x.amount < 0 && `($${(x.amount * -1).toFixed(2)})`}
                          {x.amount >= 0 && `$${x.amount.toFixed(2)}`}
                        </span>
                      </div>
                    </div>
                  </div>
                );
              })}
          </Fragment>
        )} */}
        <OrderItems account={account} orderData={orderData} isReadOnly={true} isMobile={isMobile} />
        {/* {isMobile === false && (
          <div className="row">
            <div className="col-12">
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
                        padding-left: 30px;
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
                            <div className="table-cell">{`${idx + 1}.`}</div>
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
                                <div
                                  css={css`
                                    text-align: center;
                                  `}
                                >
                                  {x.qty}
                                </div>
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
            </div>
          </div>
        )} */}
        <div className="row">
          <div className="col-12 cart-summary-1">
            <div> Net Sales : {ToMoneyStr(orderData?.netSales ?? 0.0)}</div>
            {orderData && orderData.dAdjustmentDiscountPercentage > 0 && (
              <div>
                {orderData.cartDiscountName && `${orderData.cartDiscountName}: `}
                {orderData.cartDiscountName.length === 0 &&
                  `${orderData.dAdjustmentDiscountPercentage}% OFF: `}
                {ToMoneyStr(orderData.adjustment * -1)}
              </div>
            )}
            <div>
              Shipping &amp; Handling :{' '}
              {orderData && orderData.shippingFee > 0
                ? ToMoneyStr(ShippingFeeNotIncludeRemoteAreaSurcharge(orderData))
                : orderData && orderData.shippingByQuote
                ? '(Shipping by quote)'
                : '$0.00'}
            </div>
            {orderData && IncludeRemoteAreaSurcharge(orderData) && (
              <div>Remote Area Surcharge : {ToMoneyStr(orderData.extendedAreaSubcharge)}</div>
            )}
            <div> Subtotal : {ToMoneyStr(orderData?.subTotal ?? 0.0)}</div>
          </div>
        </div>
        {orderData.taxTitle && orderData.dTaxAmount !== 0 && (
          <div className="row">
            <div className="col-12 cart-summary-tax">
              <div>
                {orderData.taxTitle} : {ToMoneyStr(orderData?.dTaxAmount ?? 0.0)}
              </div>
            </div>
          </div>
        )}
        <div className="row">
          <div className="col-12 cart-summary-total">
            <div>
              Total({orderData.currency}) : {ToMoneyStr(orderData?.total ?? 0.0)}
            </div>
          </div>
        </div>
        {orderData.balanceDue !== orderData.total && (
          <div className="row">
            <div className="col-12 cart-summary-total">
              {orderData?.useAmountbyGiftCard > 0 && (
                <div className="cart-credit">
                  Credit Applied : (${orderData?.useAmountbyGiftCard?.toFixed(2) ?? 0.0})
                </div>
              )}
              {orderData?.useAmountbyGiftCard < 0 && (
                <div className="cart-credit">
                  Credit : ${(orderData.useAmountbyGiftCard * -1).toFixed(2) ?? 0.0}
                </div>
              )}
              <div>Balance Due : {ToMoneyStr(orderData?.balanceDue ?? 0.0)}</div>
            </div>
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
