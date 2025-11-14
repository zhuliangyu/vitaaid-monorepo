/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';
import { Header } from '../Layouts/Header';
import { Footer } from '../Layouts/Footer';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { ProtocolData, getProtocol } from 'model/Protocol';
import { accountData } from 'redux/features/account/accountSlice';
import { protocolID, protocolIDChanged } from 'redux/features/protocolIDSlice';
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { ProtocolRelatedProductsList } from 'components/Protocol/ProtocolRelatedProductsList';
import { forceUpdateData } from 'redux/features/forceUpdateSlice';

interface Props {
  isMobile: boolean;
}
export const ProtocolDetail = ({ isMobile }: Props) => {
  let navigate = useNavigate();

  const [protocol, setProtocol] = React.useState<ProtocolData | undefined>();
  const dispatch = useDispatch();
  const protocolDetailID = useSelector(protocolID);
  const country = useSelector(selectedCountry);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getProtocol(protocolDetailID, country);
      setProtocol(data);
    }

    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setProtocol(undefined);
    };
  }, []);

  const BriefContent = () => {
    return (
      <Fragment>
        <div className="header-line1">
          <div className="protocol-topic">{protocol!.topic}</div>
          {isMobile === false && (
            <button
              className="a-btn borderless-btn protocol-download-btn"
              onClick={() => {
                const url = `${process.env.REACT_APP_PROTOCOL_DIR!}/${protocol!.pdfFile}`;
                //window.history.pushState({}, '', location.href);
                window.open(url, '_blank');
                //window.location.href = url;
              }}
            >
              <img className="download-img" src="/img/download.png" alt="" />
              Download
            </button>
          )}
        </div>
        <div className="protocol-author-date ">
          <span>Author(s) : {`${protocol!.author}`}</span>
          <br></br>
          <span>Date : {`${protocol!.issue}`}</span>
        </div>
        {isMobile && (
          <button
            className="a-btn borderless-btn protocol-download-btn"
            onClick={() => {
              const url = `${process.env.REACT_APP_PROTOCOL_DIR!}/${protocol!.pdfFile}`;
              //window.history.pushState({}, '', location.href);
              window.open(url, '_blank');
              //window.location.href = url;
            }}
          >
            <img className="download-img" src="/img/download.png" alt="" />
            Download
          </button>
        )}
      </Fragment>
    );
  };
  return (
    <React.Fragment>
      <div className="content-main-body">
        {protocol && (
          <React.Fragment>
            <BreadCrumbsBlock
              subNodes={['Therapeutic Protocol', `${protocol.topic}`]}
              actions={[
                () => {
                  dispatch(protocolIDChanged(0));
                },
                null,
              ]}
            />
            <div className="protocol-detail">
              <div className="row">
                <div className={`col-${isMobile ? '12' : '10'} col-protocol-body`}>
                  {isMobile && (
                    <Fragment>
                      {protocol!.banner && protocol!.banner.length > 0 && (
                        <div className="detail-banner-img-div-m">
                          <img
                            key={protocol!.id}
                            className="detail-banner-img-m"
                            alt={`${protocol!.banner}`}
                            src={`${process.env.REACT_APP_PROTOCOL_DIR!}${protocol!.banner}`}
                          ></img>
                        </div>
                      )}
                      <div className="brief-content">
                        <BriefContent />
                      </div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <table>
                      <tbody>
                        <tr>
                          <td className="td-banner-img">
                            {protocol!.banner && protocol!.banner.length > 0 && (
                              <div className="banner-img-div">
                                <img
                                  key={protocol!.id}
                                  className="banner-img"
                                  alt={`${protocol!.banner}`}
                                  src={`${process.env.REACT_APP_PROTOCOL_DIR!}${protocol!.banner}`}
                                ></img>
                              </div>
                            )}
                          </td>
                          <td className="brief-content">
                            <BriefContent />
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  )}
                  <div
                    className="protocol-content"
                    dangerouslySetInnerHTML={{ __html: protocol!.blogContent }}
                  ></div>
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
                {isMobile === false && (
                  <div className="col-2 protocol-related-resources">
                    {protocol?.relatedProducts && protocol.relatedProducts.length > 0 && (
                      <ProtocolRelatedProductsList products={protocol.relatedProducts} />
                    )}
                  </div>
                )}
              </div>
            </div>
          </React.Fragment>
        )}
      </div>
    </React.Fragment>
  );
};
