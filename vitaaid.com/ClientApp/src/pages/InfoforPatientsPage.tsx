/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector } from 'react-redux';
import { Helmet } from 'react-helmet-async';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function InfoforPatientsPage() {
  const isMobile = useSelector(isMobileData);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Info for Patients</title>
      </Helmet>
      <div className="row img-fluid info-patients">
        <div className="banner">
          <div className="banner-block1" />
          <div className="banner-block2" />
          <div className="banner-block3" />
          <div className="banner-text-block">
            <div className="text1">
              Ready to <span className="text2">Experience</span>
              <br /> {isMobile && <Fragment>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;the&nbsp;</Fragment>}
              {isMobile === false && (
                <Fragment>
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;the&nbsp;
                </Fragment>
              )}
              <span className="text2">Vita Aid&#174;</span>&nbsp; Quality ?
            </div>
            <div className="banner-line1" />
            <div className="text3">
              {isMobile && (
                <p>
                  Vita Aid&#174; products are formulated using high-quality, concentrated
                  ingredients, without any unnecessary excipients, to achieve maximized therapeutic
                  effects. In addition, they are all verified Gluten-Free, Non-GMO, and Vegetarian.
                </p>
              )}
              {isMobile === false && (
                <p>
                  Vita Aid&#174; products are formulated using high-quality, concentrated
                  ingredients,
                  <br />
                  without any unnecessary excipients, to achieve maximized therapeutic effects.
                  <br />
                  In addition, they are all verified Gluten-Free, Non-GMO, and Vegetarian.
                </p>
              )}
            </div>
          </div>
        </div>
        <div className="content-main-body body">
          <div className="row">
            <div className="col-12 title">
              Vita Aid&#174; products are sold exclusively to healthcare professionals. There are
              two ways to purchase them:
            </div>
          </div>
          <div className="row content">
            {isMobile && (
              <Fragment>
                <div className="col-12">
                  <div className="info-header-m">
                    <div className="icon1" />
                    <div className="content-line1">
                      1. Directly from your
                      <br />
                      healthcare provider.
                    </div>
                  </div>
                  <div className="content-detail-1">
                    <p>
                      It is highly recommended that you purchase Vita Aid&#174; products through
                      your healthcare provider, who will provide advice and guidance. This will
                      ensure that you are using the products safely and effectively.
                    </p>
                  </div>
                </div>
                <div className="col-12">
                  <div className="info-header-m">
                    <div className="icon2" />
                    <div className="content-line1">
                      2. Patient fulfillment via
                      <br />
                      'Physician Code'
                    </div>
                  </div>
                  <div className="content-detail-2">
                    <p>
                      Your healthcare provider can share a 'Physician Code' with you. Once you
                      receive the code you will need to register with us here and complete a
                      registration form.
                    </p>
                    <p>
                      If your healthcare practitioner does not know what his/her Physician Code is,
                      please ask him/her to contact us to obtain the number.
                    </p>
                  </div>
                </div>
              </Fragment>
            )}
            {isMobile === false && (
              <Fragment>
                <div className="col-6">
                  <div className="icon1" />
                  <div className="content-block">
                    <div className="content-line1">1. Directly from your healthcare provider. </div>
                    <div className="content-detail-1">
                      <p>
                        It is highly recommended that you purchase Vita Aid&#174; products through
                        your healthcare provider, who will provide advice and guidance. This will
                        ensure that you are using the products safely and effectively.
                      </p>
                    </div>
                  </div>
                </div>
                <div className="col-6">
                  <div className="icon2" />
                  <div className="content-block">
                    <div className="content-line1">2. Patient fulfillment via 'Physician Code'</div>
                    <div className="content-detail-2">
                      <p>
                        Your healthcare provider can share a 'Physician Code' with you. Once you
                        receive the code you will need to register with us here and complete a
                        registration form.
                      </p>
                      <p>
                        If your healthcare practitioner does not know what his/her Physician Code
                        is, please ask him/her to contact us to obtain the number.
                      </p>
                    </div>
                  </div>
                </div>
              </Fragment>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
