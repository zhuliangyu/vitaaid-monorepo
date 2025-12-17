import { configureStore } from '@reduxjs/toolkit';
import countryReducer from './features/country/countrySlice';
import blogCategoryReducer from './features/BlogCategorySlice';
import productCategoryReducer from './features/product/productCategorySlice';
import productFilterMethodReducer from './features/product/productFilterMethodSlice';
import productCodeReducer from './features/product/productCodeSlice';
import accountReducer from './features/account/accountSlice';
import accountPageReducer from './features/account/accountPageSlice';
import orderNoOfHistoryReducer from './features/account/orderNoOfHistorySlice';
import shoppingCartReducer from './features/shoppingcart/shoppingCartSlice';
import cartPageReducer from './features/shoppingcart/cartPageSlice';
import orderReducer from './features/shoppingcart/orderSlice';
import dropShipReducer from './features/shoppingcart/dropShipSlice';
import orderCommentReducer from './features/shoppingcart/orderCommentSlice';
import orderCouponReducer from './features/shoppingcart/orderCouponSlice';

import addressBookReducer from './features/addressbook/addressBookSlice';
import sameAsBillingAddrReducer from './features/shoppingcart/sameAsBillingAddrSlice';
import iStateReducer from './features/OPStateSlice';
import bCartBriefReducer from './features/shoppingcart/showCartBriefSlice';
import loginDlgReducer from './features/loginDlgSlice';
import urlAfterLoginReducer from './features/urlAfterLoginSlice';
import paymentMethodReducer from './features/shoppingcart/paymentMethodSlice';
import productAlphabetsReducer from './features/product/productAlphabetsSlice';
import protocolIDReducer from './features/protocolIDSlice';
import forceUpdateReducer from './features/forceUpdateSlice';
import memberTypeForURLAfterLoginReducer from './features/memberTypeForURLAfterLoginSlice';
import visiblePractitionerOnlyMsgBoxReducer from './features/visiblePractitionerOnlyMsgBoxSlice';
import requireLoginMessageReducer from './features/requireLoginMessageSlice';
import isMobileReducer from './features/isMobileSlice';
import forgotPasswordDlgReducer from './features/forgotPasswordDlgSlice';

export default configureStore({
  reducer: {
    country: countryReducer,
    blogCategory: blogCategoryReducer,
    productCategory: productCategoryReducer,
    productFilterMethod: productFilterMethodReducer,
    productCode: productCodeReducer,
    accountData: accountReducer,
    shoppingCart: shoppingCartReducer,
    order: orderReducer,
    cartPage: cartPageReducer,
    dropShip: dropShipReducer,
    orderComment: orderCommentReducer,
    orderCoupon: orderCouponReducer,
    addressBook: addressBookReducer,
    sameAsBillingAddr: sameAsBillingAddrReducer,
    iStateData: iStateReducer,
    bCartBrief: bCartBriefReducer,
    loginDlg: loginDlgReducer,
    urlAfterLogin: urlAfterLoginReducer,
    paymentMethod: paymentMethodReducer,
    productAlphabets: productAlphabetsReducer,
    orderNoOfHistory: orderNoOfHistoryReducer,
    accountPage: accountPageReducer,
    protocolID: protocolIDReducer,
    forceUpdate: forceUpdateReducer,
    memberTypeForURLAfterLogin: memberTypeForURLAfterLoginReducer,
    visiblePractitionerOnlyMsgBox: visiblePractitionerOnlyMsgBoxReducer,
    requireLoginMessage: requireLoginMessageReducer,
    isMobile: isMobileReducer,
    forgotPasswordDlg: forgotPasswordDlgReducer,
  },
});
