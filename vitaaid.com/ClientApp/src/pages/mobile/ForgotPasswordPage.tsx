/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector } from 'react-redux';
import { ForgotPasswordModule } from 'components/Layouts/common/ForgotPasswordModule';
import { isMobileData } from 'redux/features/isMobileSlice';
import { NotFoundPage } from 'NotFoundPage';
export default function ForgotPasswordPage() {
  const isMobile = useSelector(isMobileData);
  return isMobile ? (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Forgot Password</title>
      </Helmet>
      <div className="content-main-body">
        <div className="forgot-password-m forgot-pwd-popover">
          <ForgotPasswordModule />
        </div>
      </div>
    </React.Fragment>
  ) : (
    <NotFoundPage redirect={true} />
  );
}
