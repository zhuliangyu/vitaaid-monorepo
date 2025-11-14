/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { useNavigate } from 'react-router-dom';

export default function FooterMobile() {
  let navigate = useNavigate();
  return (
    <div className="row vitaaid-footer-mobile">
      <div className="col-12 vitaaid-footer-content">
        <div className="footer-term-policy-block">
          <button className="a-btn footerTextStyle" onClick={() => navigate('/termsnconditions')}>
            Terms &amp; Conditions
          </button>
          <span className="footerTextStyle">&nbsp;&nbsp;|&nbsp;&nbsp;</span>
          <button
            className="a-btn footerTextStyle"
            css={css`
              margin-right: 8px;
            `}
            onClick={() => navigate('/privacypolicy')}
          >
            Privacy Policy
          </button>
          <img
            className="footer-social-media-icon img-btn"
            alt="facebook"
            src="/img/fb-smart-object.png"
            srcSet="/img/fb-smart-object@2x.png 2x, /img/fb-smart-object@3x.png 3x"
            onClick={() => {
              window.open('https://www.facebook.com/VitaAidProTherapeutics', '_blank');
            }}
          />
          <img
            className="footer-social-media-icon"
            alt="linkedin"
            src="/img/linkedin-smart-object.png"
            srcSet="/img/linkedin-smart-object@2x.png 2x, /img/linkedin-smart-object@3x.png 3x"
            onClick={() => {
              window.open(
                'https://www.linkedin.com/company/vita-aid-professional-therapeutics/about/',
                '_blank',
              );
            }}
          />
        </div>
        <div className="copyright-notices">
          <p className="text-center align-self-center footerTextStyle">
            Copyright &copy; {new Date().getFullYear()} vitaaid.com. All rights reserved.
          </p>
        </div>
      </div>
    </div>
  );
}
