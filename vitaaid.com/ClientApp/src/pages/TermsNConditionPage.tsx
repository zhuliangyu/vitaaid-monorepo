/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

import { Helmet } from 'react-helmet-async';

export default function TermsNConditionsPage() {
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Terms &amp; Conditions</title>
      </Helmet>
      <div className="content-main-body our-vision">
        <div className="row">
          <div className="col-12">
            <div className="header-block">Terms &amp; Conditions</div>
            <div className="detail">
              <div className="section-title">1. Introduction</div>
              <p>
                This Terms of Use agreement sets forth the agreement between Vita Aid (or "we") and
                each user ("you" or "user") governing the use by you of this site. By using this
                site, you signify your assent to this Terms of Use Agreement. If you do not agree to
                the terms and conditions contained in this Terms of Use Agreement, you may not
                access or otherwise use this site.
              </p>
              <div className="section-title">2. Not Healthcare Advice</div>
              <p>
                The products, information, services and other content provided on and through this
                site, including without limitation any products, information and services, are
                provided for informational purposes only to facilitate discussions with your
                physician or other healthcare professional (collectively, "Healthcare Professional")
                regarding treatment options.
              </p>
              <p>
                The information provided on this site, including without limitation information
                relating to medical and health conditions, products and treatments, is often
                provided in summary or aggregate form. It is not intended as a substitute for advice
                from your Healthcare Professional, or any information contained on or in any product
                label or packaging.
              </p>
              <p>
                You should not use the information or services on this site for diagnosis or
                treatment of any health issue or for prescription of any medication or other
                treatment. You should always speak with your Healthcare Professional, and carefully
                read all information provided by the manufacturer of a product and on or in any
                product label or packaging, before using any medication or nutritional, herbal or
                homeopathic product, before starting any diet or exercise program or before adopting
                any treatment for a health problem. Each person is different, and the way you react
                to a particular product may be significantly different from the way other people
                react to such product. You should also consult your physician or healthcare provider
                regarding any interactions between any medication you are currently taking and
                nutritional supplements.
              </p>
              <div className="section-title">3. Professional Use Only</div>
              <p>
                Due to the fact that Vita Aid product line is for professional use only, they are
                not recommended for patients to take without clinician's supervision.
                Self-medicating with our products can potentially cause unwanted side effects. For
                that reason, health care practitioners must NOT sell products to patients without
                providing consultation (eg. ONLINE shopping where patients can have access to the
                products at will). Vita Aid reserves the right to stop selling products to the
                practitioner if any of the above terms is violated.
              </p>
              <div className="section-title">4. Copyright and Trademarks</div>

              <p>
                All website design, text, graphics, and other content, and the selection and
                arrangement thereof, are the property of Vita Aid or its licensors, and are
                protected by Canadian and international copyright law. All rights to such materials
                are reserved to their respective copyright owners. Permission is granted to
                electronically copy and to print in hard copy portions of this website for the sole
                purpose of placing an order with Vita Aid or using this website to educate patients.
              </p>
            </div>
            <div className="bottom-seperate-line"></div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
