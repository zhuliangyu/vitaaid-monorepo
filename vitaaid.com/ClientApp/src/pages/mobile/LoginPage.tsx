/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector } from 'react-redux';
import { LoginFormModule } from 'components/Layouts/common/LoginFormModule';
import { isMobileData } from 'redux/features/isMobileSlice';
import { NotFoundPage } from 'NotFoundPage';
export const LoginPage = () => {
  const isMobile = useSelector(isMobileData);
  return isMobile ? (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Login</title>
      </Helmet>
      <div className="content-main-body">
        <div className="login-m">
          <LoginFormModule />
        </div>
      </div>
    </React.Fragment>
  ) : (
    <NotFoundPage redirect={true} />
  );
};
