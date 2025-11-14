/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { BrowserRouter, Routes, Route, useLocation } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { doRefreshToken, getTokenFromSession } from 'model/JwtToken';
import { blogCategoryChanged } from 'redux/features/BlogCategorySlice';
import {
  doRefreshShoppingCartToken,
  getShoppingCartTokenFromSession,
} from 'model/ShoppingCartToken';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { NotFoundPage } from './NotFoundPage';
import { HomePage } from './pages/HomePage';
import QualityTrakPage from './pages/QualityTrakPage';
import { ProductsPage } from './pages/ProductsPage';
import BlogPage from './pages/BlogPage';
import BlogArticlePage from './pages/BlogArticlePage';
import WebinarsPage from './pages/WebinarsPage';
import WebinarDetailPage from './pages/WebinarDetailPage';
import ProtocolsPage from './pages/ProtocolsPage';
import RegisterPage from './pages/RegisterPage';
import { ShoppingCartPage } from './pages/ShoppingCartPage';
import InfoforPatientsPage from './pages/InfoforPatientsPage';
import TechSupportPage from './pages/TechSupportPage';
import JoinOurMailingListPage from './pages/JoinOurMailingListPage';
import ContactUsPage from './pages/ContactUsPage';
import MedicalConsultancyTeamPage from './pages/MedicalConsultancyTeamPage';
import MedicalAdvisoryBoardPage from './pages/MedicalAdvisoryBoardPage';
import BuildingTrustThroughQualityPage from './pages/BuildingTrustThroughQualityPage';
import OurVisionPage from './pages/OurVisionPage';
import HowToPurchaseProductsPage from './pages/HowToPurchaseProductsPage';
import PrivacyPolicyPage from './pages/PrivacyPolicyPage';
import ShippingPolicyPage from './pages/ShippingPolicyPage';
import TermsNConditionsPage from './pages/TermsNConditionPage';
import { AccountPage } from './pages/AccountPage';
import { DosingCaculatorPage } from './pages/DosingCaculatorPage';
import { ResetPasswordPage } from './pages/ResetPasswordPage';
import { RedirectToUrl } from './components/RedirectToUrl';
import { getMemberFromSession } from 'model/Member';
import BingoPage from './pages/BingoPage';
import DrawPage from './pages/DrawPage';
import { LoginPage } from './pages/mobile/LoginPage';
import ForgotPasswordPage from './pages/mobile/ForgotPasswordPage';

function ScrollToTop() {
  const { pathname } = useLocation();

  React.useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  return null;
}

function App() {
  const token = getTokenFromSession();
  const dispatch = useDispatch();

  dispatch(blogCategoryChanged(''));

  if (token != null) {
    doRefreshToken(token, false);
  }
  const shoppingCartToken = getShoppingCartTokenFromSession();
  if (shoppingCartToken != null) {
    doRefreshShoppingCartToken(shoppingCartToken, false);
  }

  return (
    <BrowserRouter>
      <ScrollToTop />
      <div
        className="container-fluid app-main-body"
        css={css`
          padding-left: 0px;
          padding-right: 0px;
        `}
      >
        <Header />
        <div className="content-body">
          <Routes>
            <Route path="" element={<HomePage />} />
            <Route path="ca" element={<HomePage />} />
            <Route path="main/bmr_calculator.asp" element={<DosingCaculatorPage />} />
            <Route path="main/bmr_calculator_CA.asp" element={<DosingCaculatorPage />} />
            <Route path="main/*" element={<NotFoundPage redirect={true} />} />
            <Route path="usa/main/bmr_calculator.asp" element={<DosingCaculatorPage />} />
            <Route path="usa/main/bmr_calculator_US.asp" element={<DosingCaculatorPage />} />
            <Route path="usa/main/*" element={<NotFoundPage redirect={true} />} />
            {/* <Route path="qualitypage/:country" element={<QualityPage />} /> */}
            <Route path="qualitytrak/:country" element={<QualityTrakPage />} />
            <Route path="products" element={<ProductsPage />} />
            <Route path="cart" element={<ShoppingCartPage />} />
            <Route path="blog" element={<BlogPage />} />
            <Route path="blogarticle/:id" element={<BlogArticlePage />} />
            <Route path="webinars" element={<WebinarsPage />} />
            <Route path="webinardetail/:id" element={<WebinarDetailPage />} />
            <Route path="protocols" element={<ProtocolsPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="infoforpatients" element={<InfoforPatientsPage />} />
            <Route path="techsupport" element={<TechSupportPage />} />
            <Route path="joinourmailinglist" element={<JoinOurMailingListPage />} />
            {/*<Route path="profile" element={<ProfilePage />} />*/}
            <Route path="ourvision" element={<OurVisionPage />} />
            <Route path="medicaladvisoryboard" element={<MedicalAdvisoryBoardPage />} />
            <Route path="medicalconsultancyteam" element={<MedicalConsultancyTeamPage />} />
            <Route
              path="buildingtrustthroughquality"
              element={<BuildingTrustThroughQualityPage />}
            />
            <Route path="contactus" element={<ContactUsPage />} />
            <Route path="howtopurchaseproducts" element={<HowToPurchaseProductsPage />} />
            <Route path="privacypolicy" element={<PrivacyPolicyPage />} />
            <Route path="shippingpolicy" element={<ShippingPolicyPage />} />
            <Route path="termsnconditions" element={<TermsNConditionsPage />} />
            <Route path="account" element={<AccountPage />} />
            <Route path="bmr_calculator" element={<DosingCaculatorPage />} />
            <Route path="ResetPassword" element={<ResetPasswordPage />} />
            <Route path="bingo" element={<BingoPage />} />
            <Route path="draw" element={<DrawPage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="forgotpassword" element={<ForgotPasswordPage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
          <Footer />
        </div>
      </div>
    </BrowserRouter>
  );
}

export default App;
