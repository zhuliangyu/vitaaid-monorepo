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
import { OrderData } from 'model/ShoppingCart';
import { getPatientOrderDetail } from 'model/Member';
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
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { OrderDetail } from 'components/OrderDetail';
import { LoadPanel } from 'devextreme-react/load-panel';
import { isMobileData } from 'redux/features/isMobileSlice';

export const PatientOrderHistoryDetail = () => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const orderNo = useSelector(orderNoOfHistory);
  const [orderData, setOrderData] = React.useState<OrderData>();
  const [loadingState, setLoadingState] = React.useState(1);
  const isMobile = useSelector(isMobileData);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getPatientOrderDetail(account.customerCode, orderNo);
      setOrderData(data);
      setLoadingState(0);
    }
    if (account && orderNo) fetchData();
  }, [orderNo]);

  return (
    <React.Fragment>
      <BreadCrumbsBlock
        subNodes={['Patient Order History', 'Patient Order Detail']}
        actions={[
          () => {
            dispatch(patient_order_history_page());
          },
          null,
        ]}
      />
      <div className="content-main-body order-hisory-detail">
        <div className="row">
          <div className="col-12">
            <div className="title">Patient Order Detail</div>
            {account && account.customerCode && orderData && (
              <React.Fragment>
                <div className="order-no-div">
                  Order No:<span className="pono-span"> {orderData.poNo}</span>
                </div>
                <OrderDetail account={account} orderData={orderData} isMobile={isMobile} />
                <div className="remark">
                  **Remark**: {orderData.comment ? orderData.comment : 'None.'}
                </div>
              </React.Fragment>
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
