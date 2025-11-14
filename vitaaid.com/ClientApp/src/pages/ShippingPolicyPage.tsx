/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';

export default function ShippingPolicyPage() {
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Shipping Policy</title>
      </Helmet>
      <div className="content-main-body our-vision">
        <div className="row">
          <div className="col-12">
            <div className="header-block">Shipping</div>
            <div className="detail">
              <div className="section-title">
                Most orders are shipped within 48 hours on working days. During holiday season,
                order may be delayed.
              </div>
              <p></p>
              <div className="section-title">Free shipping requirements :</div>
              <ul>
                <li>
                  Orders over CDN $250 to BC, Alberta, Saskatchewan, Manitoba (1-2 Day Delivery)
                </li>
                <li>
                  Orders over CDN $350 to Ontario, Quebec, NS, NB, NF, and PEI (Next Day Delivery)
                </li>
                <li>
                  Orders USD $350 to Continental United States (Alaska and Hawaii, please inquire
                  for rate)
                </li>
              </ul>
              <div className="section-title">
                Orders below free shipping requirements are subject to the following fees :
              </div>
              <ul>
                <li>$12.50 CAD for shipments to areas within BC</li>
                <li>
                  $12.50 CAD for shipments (WITHOUT SynerClear<sup>®</sup>) to all provinces outside
                  BC in Canada
                </li>
                <li>
                  $16 CAD for shipments (WITH SynerClear<sup>®</sup>) to all other provinces outside
                  BC in Canada
                </li>
                <li>$20 USD for all US destinations.</li>
              </ul>
              <div className="section-title">
                Orders under $75 are subject to an additional $5 handling fee.
              </div>
              <p></p>
              <div className="section-title">
                The above shipping policy does not apply to certain remote areas; in which case,
                shipping cost will be by quote only.
              </div>
              <p></p>
              <div className="section-title">Other restrictions may apply.</div>
              <p></p>
              <div className="section-title">
                Errors or shortages in your order must be reported within 48hrs of receipt of goods.
                Please email info@vitaaid.com, or call us at 1-604-260-0696 for more information.
              </div>
              <p></p>
            </div>
            <div className="bottom-seperate-line"></div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
