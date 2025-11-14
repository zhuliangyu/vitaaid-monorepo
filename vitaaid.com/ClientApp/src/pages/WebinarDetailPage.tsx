/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { WebinarData, getWebinar } from 'model/Webinar';
import { accountData } from 'redux/features/account/accountSlice';
import { WebinarRelatedProductsList } from 'components/Webinar/WebinarRelatedProductsList';
import { forceUpdateData } from 'redux/features/forceUpdateSlice';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function WebinarDetailPage() {
  const [webinar, setWebinar] = React.useState<WebinarData | undefined>();
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  const requestUpdate = useSelector(forceUpdateData);
  const isMobile = useSelector(isMobileData);
  const { id } = useParams();
  let navigate = useNavigate();

  React.useEffect(() => {
    async function fetchData() {
      const data = await getWebinar(parseInt(id!, 10), country);
      setWebinar(data);
    }
    if (id === undefined) navigate('/webinar');
    else if (account) fetchData();
    else navigate('/');
    // Specify how to clean up after this effect:
    return () => {
      setWebinar(undefined);
    };
  }, [, requestUpdate]);

  return (
    <Fragment>
      <Helmet>
        <title>Vita Aid - Webinar</title>
      </Helmet>
      <div className="content-main-body">
        {webinar && (
          <BreadCrumbs subNodes={['Webinar', `${webinar.topic}`]} hrefs={['/webinars', '']} />
        )}
        <div className="webinar-detail">
          <div className="row">
            <div className={`col-${isMobile ? '12' : '10'} col-webinar-body`}>
              {webinar && (
                <div>
                  <div className="title" dangerouslySetInnerHTML={{ __html: webinar.topic }} />
                  <div className="issue-vol-no-block">
                    <span>{`Speaker : ${webinar.author}`}</span>
                    <div className="sep-space">{` `}</div>
                    {isMobile && <br />}
                    <span>{`Broadcase date : ${webinar.issue}`}</span>
                  </div>
                  <div
                    className="webinar-content"
                    dangerouslySetInnerHTML={{ __html: webinar.webinarContent }}
                  ></div>
                  {webinar.videoLink && (
                    <div className="webinar-video-block">
                      <iframe
                        title="webinar-video"
                        className="webinar-video"
                        src={`${webinar.videoLink}`}
                      ></iframe>
                    </div>
                  )}
                  <div className="notice">
                    <div>† This presentation is for educational purpose only.</div>
                    <p>
                      The entire contents are not intended to be a substitute for professional
                      medical advice, diagnosis, or treatment. Always seek the advice of your
                      physician or other qualified health provider with any questions you may have
                      regarding a medical condition.
                    </p>
                    <p>
                      Never disregard professional medical advice or delay in seeking it because of
                      something you have read in this presentation.
                    </p>
                    <p>
                      These statements made in this presentation have not been evaluated by the Food
                      and Drug Administration and are not intended to be used to diagnose, treat, or
                      prevent any diseases.
                    </p>
                    <p>
                      The entire Content is the opinion of the author, and does not represent the
                      opinion of any other party.
                    </p>
                  </div>
                </div>
              )}
            </div>
            {isMobile === false && (
              <div className="col-2 webinar-related-resources">
                {webinar?.relatedProducts && webinar.relatedProducts.length > 0 && (
                  <WebinarRelatedProductsList
                    key="webinarRelatedProductList"
                    products={webinar.relatedProducts}
                  />
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </Fragment>
  );
}
