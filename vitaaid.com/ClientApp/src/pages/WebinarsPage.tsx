/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { WebinarData, getWebinars } from 'model/Webinar';
import { WebinarInfoInCategory } from '../components/Webinar/WebinarInfoInCategory';
import { accountData } from '../redux/features/account/accountSlice';
import { visiblePractitionerOnlyMsgBoxChanged } from 'redux/features/visiblePractitionerOnlyMsgBoxSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { PageNav } from 'components/PageNav';

export default function WebinarsPage() {
  let navigate = useNavigate();

  const [webinars, setWebinars] = React.useState<WebinarData[]>([]);
  const [currentPageOfWebinars, setCurrentPageOfWebinars] = React.useState<number>(0);
  const [totalPagesOfWebinars, setTotalPagesOfWebinars] = React.useState<number>(0);
  const isMobile = useSelector(isMobileData);
  const listSizePerPage = 8;
  const dispatch = useDispatch();
  const account = useSelector(accountData);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getWebinars();
      setWebinars(data);
    }
    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setWebinars([]);
    };
  }, []);

  React.useEffect(() => {
    if (webinars) {
      setTotalPagesOfWebinars(Math.ceil(webinars.length / listSizePerPage));
      setCurrentPageOfWebinars(1);
    } else {
      setTotalPagesOfWebinars(1);
      setCurrentPageOfWebinars(1);
    }
  }, [webinars, isMobile]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Webinar</title>
      </Helmet>
      <div className="content-main-body webinars">
        <div className="row">
          <div className={`col-${isMobile ? '12' : '10'} webinar-list`}>
            <div className="title">Webinar</div>
            <table className="webinar-info-table">
              <tbody>
                {webinars &&
                  [...Array(listSizePerPage)].map((_, idx) => {
                    const artIdx = (currentPageOfWebinars - 1) * listSizePerPage + idx;
                    if (artIdx < 0 || artIdx >= webinars.length)
                      return (
                        <tr key={`webinars-${idx}`}>
                          <td />
                        </tr>
                      );
                    const art = webinars[artIdx];
                    return (
                      <WebinarInfoInCategory
                        key={art.id}
                        data={art}
                        showNote={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(true))}
                        isMobile={isMobile}
                      />
                    );
                  })}
              </tbody>
            </table>
          </div>
          {isMobile === false && (
            <div
              className="col-2"
              css={css`
                margin-top: 28px;
              `}
            />
          )}
        </div>
        {totalPagesOfWebinars > 1 && (
          <PageNav
            isMobile={isMobile}
            totalPages={totalPagesOfWebinars}
            currentPage={currentPageOfWebinars}
            currentPageChanged={(v) => setCurrentPageOfWebinars(v)}
          />
        )}
      </div>
    </React.Fragment>
  );
}
