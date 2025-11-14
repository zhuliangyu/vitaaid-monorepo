/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';

export default function HowToPurchaseProductsPage() {
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - How to Purchase Vita Aid Products</title>
      </Helmet>
      <div className="content-main-body our-vision">
        <div className="row">
          <div className="col-12">
            <div className="header-block">How to purchase products</div>
            <div className="detail">
              <div className="section-title">Your Healthcare Practitioner Office.</div>
              <p>
                In many cases, you can simply purchase Vita Aid products with a prescription at your
                doctor’s clinic. Some products may be readily available, while others may require a
                special order.
              </p>
              <div className="section-title">Patient Drop Ship.</div>
              <p>
                Many Healthcare Provider offer ordering online on Vitaaid.com, which may be shipped
                directly to your home for added convenience. Ask doctor for more details.
              </p>
              <div className="section-title">Vitaaid.com with Patient Account.</div>
              <p>
                Ask your healthcare practitioner if s/he has a Vitaaid.com Physician Code. Register
                a patient account with the provided Physician Code and you will gain access to
                purchase Vitaaid products online at Vitaaid.com.
              </p>
            </div>
            <div className="bottom-seperate-line"></div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
