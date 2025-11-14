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
import { OrderHistoryData, getOrderHistory, getPatientOrderHistory } from 'model/Member';
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
import { LoadPanel } from 'devextreme-react/load-panel';
import { isMobileData } from 'redux/features/isMobileSlice';

export const PatientOrderHistory = () => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const isMobile = useSelector(isMobileData);
  const [orderHistories, setOrderHistories] = React.useState<OrderHistoryData[]>([]);
  const [currentPageOfOrders, setCurrentPageOfOrders] = React.useState<number>(0);
  const [totalPagesOfOrders, setTotalPagesOfOrders] = React.useState<number>(0);
  const [startPageOfNav, setStartPageOfNav] = React.useState<number>(0);
  const [loadingState, setLoadingState] = React.useState(1);
  var totalPgIdxs: number = isMobile ? 3 : 20;

  React.useEffect(() => {
    async function fetchData() {
      const data = await getPatientOrderHistory(account.customerCode);
      setOrderHistories(data);
      setLoadingState(0);
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
      setStartPageOfNav(1);
    } else {
      setTotalPagesOfOrders(1);
      setCurrentPageOfOrders(1);
      setStartPageOfNav(1);
    }
  }, [orderHistories]);

  return (
    <React.Fragment>
      <div className="content-main-body order-hisory">
        <div className="row">
          <div className="col-12 order-list">
            <div className="title">Patient Order History</div>
            <table className="order-info-table">
              <thead>
                <tr>
                  {isMobile === false && (
                    <Fragment>
                      <th
                        className="order-info-table-header"
                        css={css`
                          width: 10%;
                        `}
                      >
                        No.
                      </th>

                      <th
                        className="order-info-table-header"
                        css={css`
                          width: 10%;
                        `}
                      >
                        Status
                      </th>
                    </Fragment>
                  )}
                  <th
                    className="order-info-table-header"
                    css={css`
                      width: 10%;
                    `}
                  >
                    Patient
                  </th>
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
                          <Fragment>
                            <td>
                              <div className="space-wrap">
                                {`${order.index.toString().padStart(3, ' ')}`}.
                              </div>
                            </td>
                            <td className="order-status">{order.status}</td>
                          </Fragment>
                        )}
                        <td>{order.name}</td>
                        <td>
                          <div className="text-center">
                            <button
                              className="borderless-btn a-btn order-no"
                              onClick={() => {
                                dispatch(orderNoOfHistoryChanged(order.orderNo));
                                dispatch(patient_order_history_detail_page());
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
        <div className="row">
          <div className="col-12 page-navigate">
            {totalPagesOfOrders > 1 && (
              <Fragment>
                {isMobile === false && (
                  <button
                    className="borderless-btn"
                    onClick={() => {
                      setStartPageOfNav(1);
                      setCurrentPageOfOrders(1);
                    }}
                  >
                    <img
                      src="/img/first-idx.png"
                      srcSet="/img/first-idx@2x.png 2x, /img/first-idx@3x.png 3x"
                      alt=""
                    />
                  </button>
                )}
                <button
                  className="borderless-btn fist-last-idx"
                  onClick={() => {
                    const newCurrPageOfOrders = currentPageOfOrders - 1;
                    if (newCurrPageOfOrders < 1) return;
                    if (newCurrPageOfOrders < startPageOfNav) {
                      setStartPageOfNav(startPageOfNav - 1);
                    }
                    setCurrentPageOfOrders(newCurrPageOfOrders);
                  }}
                >
                  {isMobile && <div className="prev-idx-m" />}
                  {isMobile === false && (
                    <img
                      src="/img/prev-idx.png"
                      srcSet="/img/prev-idx@2x.png 2x, /img/prev-idx@3x.png 3x"
                      alt=""
                    />
                  )}
                </button>
                {startPageOfNav > 1 && <span className="more-page">...</span>}

                {[...Array(totalPgIdxs)].map((_, idx) => {
                  const pageIdx = startPageOfNav + idx;
                  if (pageIdx <= 0 || pageIdx > totalPagesOfOrders) return <Fragment />;
                  return (
                    <button
                      className={`nav-button borderless-btn ${
                        pageIdx === currentPageOfOrders ? 'selected-idx' : 'unselected-idx'
                      }`}
                      key={pageIdx}
                      onClick={() => {
                        setCurrentPageOfOrders(pageIdx);
                      }}
                    >
                      {pageIdx}
                    </button>
                  );
                })}
                {startPageOfNav + totalPgIdxs <= totalPagesOfOrders && (
                  <span className="more-page">...</span>
                )}
                <button
                  className="borderless-btn"
                  onClick={() => {
                    const newCurrPageOfOrders = currentPageOfOrders + 1;
                    if (newCurrPageOfOrders > totalPagesOfOrders) return;
                    if (newCurrPageOfOrders > startPageOfNav + totalPgIdxs - 1) {
                      if (startPageOfNav + totalPgIdxs + totalPgIdxs > totalPagesOfOrders)
                        setStartPageOfNav(totalPagesOfOrders - (totalPgIdxs - 1));
                      else setStartPageOfNav(startPageOfNav + totalPgIdxs);
                    }
                    setCurrentPageOfOrders(newCurrPageOfOrders);
                  }}
                >
                  {isMobile && <div className="next-idx-m" />}
                  {isMobile === false && (
                    <img
                      src="/img/next-idx.png"
                      srcSet="/img/next-idx@2x.png 2x, /img/next-idx@3x.png 3x"
                      alt=""
                    />
                  )}
                </button>
                {isMobile === false && (
                  <button
                    className="borderless-btn"
                    onClick={() => {
                      setStartPageOfNav(totalPagesOfOrders - (totalPgIdxs - 1));
                      setCurrentPageOfOrders(totalPagesOfOrders);
                    }}
                  >
                    <img
                      src="/img/last-idx.png"
                      srcSet="/img/last-idx@2x.png 2x, /img/last-idx@3x.png 3x"
                      alt=""
                    />
                  </button>
                )}
              </Fragment>
            )}
          </div>
        </div>
      </div>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        height={200}
        width={400}
        visible={loadingState === 1}
        message="Please wait, loading the order..."
      />
    </React.Fragment>
  );
};
