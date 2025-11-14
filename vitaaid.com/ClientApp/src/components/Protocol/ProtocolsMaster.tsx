/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';
import { Header } from '../Layouts/Header';
import { Footer } from '../Layouts/Footer';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { ProtocolData, getProtocols } from 'model/Protocol';
import { ProtocolInfoInCategory } from './ProtocolInfoInCategory';
import { accountData } from '../../redux/features/account/accountSlice';
import { protocolID } from 'redux/features/protocolIDSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { visiblePractitionerOnlyMsgBoxChanged } from 'redux/features/visiblePractitionerOnlyMsgBoxSlice';
import { PageNav } from 'components/PageNav';

interface Props {
  isMobile: boolean;
}

export const ProtocolsMaster = ({ isMobile }: Props) => {
  let navigate = useNavigate();
  const [protocols, setProtocols] = React.useState<ProtocolData[]>([]);
  const [currentPageOfProtocols, setCurrentPageOfProtocols] = React.useState<number>(0);
  const [totalPagesOfProtocols, setTotalPagesOfProtocols] = React.useState<number>(0);
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const listSizePerPage = 8;

  React.useEffect(() => {
    async function fetchData() {
      const data = await getProtocols();
      setProtocols(data);
    }
    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setProtocols([]);
    };
  }, []);

  React.useEffect(() => {
    if (protocols) {
      setTotalPagesOfProtocols(Math.ceil(protocols.length / listSizePerPage));
      setCurrentPageOfProtocols(1);
    } else {
      setTotalPagesOfProtocols(1);
      setCurrentPageOfProtocols(1);
    }
  }, [protocols]);

  return (
    <React.Fragment>
      <div className="content-main-body protocols">
        <div className="row">
          <div className="col-12 protocol-list">
            <div className="title">Therapeutic Protocol</div>
            {isMobile && (
              <Fragment>
                {protocols &&
                  [...Array(listSizePerPage)].map((_, idx) => {
                    const artIdx = (currentPageOfProtocols - 1) * listSizePerPage + idx;
                    if (artIdx < 0 || artIdx >= protocols.length)
                      return <div key={`protocols-${idx}`} />;
                    const protocol = protocols[artIdx];
                    return (
                      <ProtocolInfoInCategory
                        key={protocol.id}
                        data={protocol}
                        showNote={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(true))}
                        isMobile={isMobile}
                      />
                    );
                  })}
              </Fragment>
            )}
            {isMobile === false && (
              <table className="protocol-info-table">
                <tbody>
                  {protocols &&
                    [...Array(8)].map((_, idx) => {
                      const artIdx = (currentPageOfProtocols - 1) * 8 + idx;
                      if (artIdx < 0 || artIdx >= protocols.length)
                        return (
                          <tr key={`protocols-${idx}`}>
                            <td />
                          </tr>
                        );
                      const protocol = protocols[artIdx];
                      return (
                        <ProtocolInfoInCategory
                          key={protocol.id}
                          data={protocol}
                          showNote={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(true))}
                          isMobile={isMobile}
                        />
                      );
                    })}
                </tbody>
              </table>
            )}
          </div>
        </div>
        {totalPagesOfProtocols > 1 && (
          <PageNav
            isMobile={isMobile}
            totalPages={totalPagesOfProtocols}
            currentPage={currentPageOfProtocols}
            currentPageChanged={(v) => setCurrentPageOfProtocols(v)}
          />
        )}
      </div>
    </React.Fragment>
  );
};
