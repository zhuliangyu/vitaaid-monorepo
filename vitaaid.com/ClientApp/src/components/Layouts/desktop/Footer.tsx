/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { useNavigate } from 'react-router-dom';

export default function FooterDesktop() {
  let navigate = useNavigate();
  return (
    <div className="row vitaaid-footer">
      <div className="col-12 vitaaid-footer-content">
        <table className="footer-table">
          <thead>
            <tr>
              <th
                className="footerCategory"
                css={css`
                  width: 30.1%;
                `}
              >
                POLICY
              </th>
              <th
                className="footerCategory"
                css={css`
                  width: 30.1%;
                `}
              >
                CUSTOMER SERVICE
              </th>
              <th
                className="footerCategory"
                css={css`
                  width: 30.1%;
                `}
              >
                ABOUT
              </th>
              <th
                className="footerCategory"
                css={css`
                  width: 9.7%;
                `}
              >
                COMMUNITY
              </th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <button
                  className="a-btn footerTextStyle tr-1"
                  onClick={() => navigate('/termsnconditions')}
                >
                  Terms &amp; Conditions
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle tr-1"
                  onClick={() => {
                    navigate('/joinourmailinglist');
                  }}
                >
                  Join Our Mailing List
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle tr-1"
                  onClick={() => {
                    navigate('/ourvision');
                  }}
                >
                  Our Vision
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle tr-1"
                  onClick={() => {
                    window.open('https://www.facebook.com/VitaAidProTherapeutics', '_blank');
                  }}
                >
                  FaceBook
                </button>
              </td>
            </tr>
            <tr>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => navigate('/shippingpolicy')}
                >
                  Shipping
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => navigate('/howtopurchaseproducts')}
                >
                  How to Purchase Products
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => {
                    navigate('/medicaladvisoryboard');
                  }}
                >
                  Medical Advisory Board
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => {
                    window.open(
                      'https://www.linkedin.com/company/vita-aid-professional-therapeutics/about/',
                      '_blank',
                    );
                  }}
                >
                  LinkedIn
                </button>
              </td>
            </tr>
            <tr>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => navigate('/privacypolicy')}
                >
                  Privacy Policy
                </button>
              </td>
              {/*<td>*/}
              {/*  <button className="a-btn footerTextStyle" onClick={() => {}}>*/}
              {/*    Site Feedback*/}
              {/*  </button>*/}
              {/*</td>*/}
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => {
                    navigate('/contactus');
                  }}
                >
                  Contact Us
                </button>
              </td>
              <td>
                <button
                  className="a-btn footerTextStyle"
                  onClick={() => {
                    navigate('/medicalconsultancyteam');
                  }}
                >
                  Medical Consultancy Team
                </button>
              </td>
              <td />
            </tr>
            {/*  <tr>*/}
            {/*    <td />*/}
            {/*    <td />*/}
            {/*    <td>*/}
            {/*      <button*/}
            {/*        className="a-btn footerTextStyle"*/}
            {/*        onClick={() => {*/}
            {/*          navigate('/contactus');*/}
            {/*        }}*/}
            {/*      >*/}
            {/*        Contact Us*/}
            {/*      </button>*/}
            {/*    </td>*/}
            {/*    <td />*/}
            {/*  </tr>*/}
          </tbody>
        </table>
        <div className="copyright-notices">
          <p className="text-center align-self-center footerTextStyle">
            Copyright &copy; {new Date().getFullYear()} vitaaid.com. All rights reserved.
          </p>
        </div>
      </div>
    </div>
  );
}
