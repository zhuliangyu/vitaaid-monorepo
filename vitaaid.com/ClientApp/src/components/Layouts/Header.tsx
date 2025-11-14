import React, { Fragment } from 'react';
import { useSelector, useDispatch } from 'react-redux';
// import Import from 'Import';
import { isMobileData, isMobileChanged } from 'redux/features/isMobileSlice';
import HeaderDesktop from 'components/Layouts/desktop/Header';
import HeaderMobile from 'components/Layouts/mobile/Header';
export const Header = () => {
  const dispatch = useDispatch();
  const isMobile = useSelector(isMobileData);

  var checkIfMobile = () => {
    const match = window.matchMedia(`(max-width: 767px)`).matches;
    dispatch(isMobileChanged(match));
    const doc = document.documentElement;
    doc.style.setProperty('--doc-height', `${window.innerHeight}px`);
  };

  React.useEffect(() => {
    checkIfMobile();
    window.addEventListener('resize', checkIfMobile);
    return () => {
      window.removeEventListener('resize', checkIfMobile);
    };
  }, []);

  return (
    <Fragment>
      {isMobile && <HeaderMobile />}
      {isMobile === false && <HeaderDesktop />}
    </Fragment>
  );
  // <Import
  //   mobile={() => import('components/Layouts/mobile/Header')}
  //   desktop={() => import('components/Layouts/desktop/Header')}
  // >
  //   {(ImportedComponent: any) => <ImportedComponent />}
  // </Import>
};
