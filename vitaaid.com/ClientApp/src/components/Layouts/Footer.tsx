import React, { Fragment } from 'react';
import { useSelector } from 'react-redux';
import FooterDesktop from 'components/Layouts/desktop/Footer';
import FooterMobile from 'components/Layouts/mobile/Footer';
import { isMobileData } from 'redux/features/isMobileSlice';

export const Footer = () => {
  const isMobile = useSelector(isMobileData);
  return (
    <Fragment>
      {isMobile && <FooterMobile />}
      {isMobile === false && <FooterDesktop />}
    </Fragment>
  );
  // return (
  //   <Import
  //     mobile={() => import('components/Layouts/mobile/Footer')}
  //     desktop={() => import('components/Layouts/desktop/Footer')}
  //   >
  //     {(ImportedComponent: any) => <ImportedComponent />}
  //   </Import>
  // );
};
