/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';

export default function TechSupportPage() {
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Info for Patients</title>
      </Helmet>
      <div className="tech-support">
        <div className="banner">
          <div className="banner-block1" />
          <div className="banner-block2" />
          <div className="banner-block3" />
          <div className="banner-block4">
            <div className="icon1" />
            <div className="desc">
              <p>
                We have a team of staff physicians who use Vita Aid products regularly in their
                practices. They have a wealth of clinical pearls as well as industry knowledge of
                nutraceuticals, from raw material sourcing to analytical testing.
              </p>
            </div>
          </div>
          <div className="text1">
            Technical Support
            <br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;from&nbsp;
            <span className="text2">HCP</span>&nbsp;for<span className="text2">&nbsp;HCP</span>
          </div>
          <div className="banner-line1" />
          <div className="text3">
            <p>
              It is important that you feel supported and confident with using Vita Aid products in
              your practice.
            </p>
          </div>
        </div>
        <div className="body">
          <div className="row">
            <div className="col-12 title">Schedule Clinical Consultations </div>
          </div>
          <div className="row content">
            <div className="col-12">
              <div className="content-block">
                <div className="content-line1">
                  You can schedule a 15-minute or 30-minute virtual consultation in several ways :{' '}
                </div>
                <div className="content-detail">
                  <p>
                    1.Through your secure portal account
                    <br />
                    2.By emailing clinicalsupport@vitaaid.com
                    <br />
                    3.By following the prompts of the Chatbox
                    <br />
                    4.Or calling 1-800-490-1738
                    <br />
                    Prior to the virtual meeting, please briefly layout your questions and upload
                    them with your request to schedule a consultation.
                  </p>
                  <div className="content-bottom">
                    These consultations are always complimentary for our ordering practitioners.
                    <br />
                    Clinical consultations are typically scheduled 1-2 weeks out.(Schedule through
                    your secure portal account)
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
