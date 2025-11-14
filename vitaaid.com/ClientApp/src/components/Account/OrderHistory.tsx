/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { OrderHistoryData, getOrderHistory } from 'model/Member';
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
import {
  orderNoOfHistory,
  orderNoOfHistoryChanged,
} from 'redux/features/account/orderNoOfHistorySlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { PageNav } from 'components/PageNav';

export const OrderHistory = () => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const isMobile = useSelector(isMobileData);
  const [orderHistories, setOrderHistories] = React.useState<OrderHistoryData[]>([]);
  const [currentPageOfOrders, setCurrentPageOfOrders] = React.useState<number>(0);
  const [totalPagesOfOrders, setTotalPagesOfOrders] = React.useState<number>(0);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getOrderHistory(account.customerCode);
      setOrderHistories(data);
    }
    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setOrderHistories([]);
    };
  }, []);

  React.useEffect(() => {
    if (orderHistories) {
      setTotalPagesOfOrders(Math.ceil(orderHistories.length / 20));
      setCurrentPageOfOrders(1);
    } else {
      setTotalPagesOfOrders(1);
      setCurrentPageOfOrders(1);
    }
  }, [orderHistories]);

  return (
    <React.Fragment>
      <div className="content-main-body order-hisory">
        <div className="row">
          <div className="col-12 order-list">
            <div className="title">Order History</div>
            <table className="order-info-table">
              <thead>
                <tr>
                  {isMobile === false && (
                    <th
                      className="order-info-table-header"
                      css={css`
                        width: 10%;
                      `}
                    >
                      No.
                    </th>
                  )}
                  <th
                    className="order-info-table-header"
                    css={css`
                      width: 10%;
                    `}
                  >
                    Status
                  </th>
                  {isMobile === false && (
                    <th
                      className="order-info-table-header"
                      css={css`
                        width: 10%;
                      `}
                    >
                      Shipping
                    </th>
                  )}
                  <th
                    className="order-info-table-header"
                    css={css`
                      width: 20%;
                    `}
                  >
                    <div className="text-center">Order No.</div>
                  </th>
                  <th
                    className="order-info-table-header"
                    css={css`
                      width: 10%;
                    `}
                  >
                    <div className="fixwidth-td text-center">Date</div>
                  </th>
                  {isMobile === false && (
                    <th
                      className="order-info-table-header"
                      css={css`
                        width: 30%;
                      `}
                    >
                      Payment Method
                    </th>
                  )}
                  <th
                    className="order-info-table-header"
                    css={css`
                      width: 10%;
                    `}
                  >
                    <div className="text-right">Amount</div>
                  </th>
                </tr>
              </thead>
              <tbody>
                {orderHistories &&
                  [...Array(20)].map((_, idx) => {
                    const orderIdx = (currentPageOfOrders - 1) * 20 + idx;
                    if (orderIdx < 0 || orderIdx >= orderHistories.length)
                      return (
                        <tr key={`orderHistories-${idx}`}>
                          <td />
                        </tr>
                      );
                    const order = orderHistories[orderIdx];
                    return (
                      <tr key={`orderHistories-${idx}`}>
                        {isMobile === false && (
                          <td>
                            <div className="space-wrap">
                              {`${order.index.toString().padStart(3, ' ')}`}.
                            </div>
                          </td>
                        )}
                        <td>{order.status}</td>
                        {isMobile === false && <td>{order.shippingMethod}</td>}
                        <td>
                          <div className="text-center">
                            <button
                              className="borderless-btn a-btn order-no"
                              onClick={() => {
                                dispatch(orderNoOfHistoryChanged(order.orderNo));
                                dispatch(order_history_detail_page());
                              }}
                            >
                              {order.orderNo}
                            </button>
                          </div>
                        </td>
                        <td>
                          <div className="fixwidth-td text-right">{order.orderDate}</div>
                        </td>
                        {isMobile === false && <td>{order.paymentMethod}</td>}
                        <td>
                          <div className="text-right">{`$${order.amount.toFixed(2)}`}</div>
                        </td>
                      </tr>
                    );
                  })}
              </tbody>
            </table>
          </div>
        </div>
        {totalPagesOfOrders > 1 && (
          <PageNav
            isMobile={isMobile}
            totalPages={totalPagesOfOrders}
            currentPage={currentPageOfOrders}
            currentPageChanged={(v) => setCurrentPageOfOrders(v)}
          />
        )}
      </div>
    </React.Fragment>
  );
};
