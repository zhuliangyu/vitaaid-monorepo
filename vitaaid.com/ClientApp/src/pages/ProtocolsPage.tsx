/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { ProtocolData, getProtocols } from 'model/Protocol';
import { ProtocolInfoInCategory } from '../components/Protocol/ProtocolInfoInCategory';
import { accountData } from '../redux/features/account/accountSlice';
import { protocolID, protocolIDChanged } from 'redux/features/protocolIDSlice';
import { ProtocolDetail } from 'components/Protocol/ProtocolDetail';
import { ProtocolsMaster } from 'components/Protocol/ProtocolsMaster';
import { forceUpdateData } from 'redux/features/forceUpdateSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { PageNav } from 'components/PageNav';

export default function ProtocolsPage() {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const isMobile = useSelector(isMobileData);
  const protocolDetailID = useSelector(protocolID);
  const useQuery = () => new URLSearchParams(useLocation().search);
  let query = useQuery();
  let pid = query.get('id');
  const requestUpdate = useSelector(forceUpdateData);

  React.useEffect(() => {
    if (pid) {
      if (!account) {
        navigate('/');
      } else {
        dispatch(protocolIDChanged(pid));
      }
    }
  }, [, requestUpdate]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Protocol</title>
      </Helmet>
      <div className="protocols-page">
        {protocolDetailID <= 0 && <ProtocolsMaster isMobile={isMobile} />}
        {protocolDetailID > 0 && <ProtocolDetail key={protocolDetailID} isMobile={isMobile} />}
      </div>
    </React.Fragment>
  );
}
