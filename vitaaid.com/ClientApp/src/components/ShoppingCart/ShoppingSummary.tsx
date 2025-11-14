/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { useSelector, useDispatch } from 'react-redux';
import {
  CartPageType,
  cartPageIdx,
  shopping_cart_page,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
} from 'redux/features/shoppingcart/cartPageSlice';
import { order, orderSlice, orderChanged } from 'redux/features/shoppingcart/orderSlice';
import {
  productFilterMethod,
  byCategory,
  byAlphabet,
  byKeyword,
  eFILTERMETHOD,
} from 'redux/features/product/productFilterMethodSlice';
import { productCode, productCodeChanged } from 'redux/features/product/productCodeSlice';
import {
  productCategory,
  productCategoryChanged,
} from 'redux/features/product/productCategorySlice';
import { AddressData, getAddressBook } from 'model/AddressBook';
import { OrderData, OrderItemData, buildOrder } from 'model/ShoppingCart';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { useEffect } from 'react';
import { OrderDetail } from 'components/OrderDetail';
interface Props {
  isMobile: boolean;
}
export const ShoppingSummary = ({ isMobile }: Props) => {
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  let cart = useSelector(shoppingCart);
  let orderData = useSelector(order);
  const dispatch = useDispatch();
  const [agreeTerm, setAgreeTerm] = React.useState<Boolean>(false);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      let billingAddID = 0;
      let shippingAddID = 0;
      const addressbook = await getAddressBook(account.customerCode);
      if (addressbook && addressbook.length > 0) {
        billingAddID =
          addressbook.filter((x) => x.defaultBillingAddress)[0]?.id ?? addressbook[0].id;
        shippingAddID = addressbook.filter((x) => x.defaultShippingAddress)[0]?.id ?? billingAddID;
      }

      const data = await buildOrder(
        account.customerCode,
        country,
        cart,
        billingAddID,
        shippingAddID,
      );
      dispatch(orderChanged(data));
    }
    if (cart && cart.length > 0) fetchData();
    else {
      dispatch(orderChanged({} as OrderData));
    }
  }, [cart]);

  return (
    <React.Fragment>
      <div className="content-main-body">
        <BreadCrumbsBlock
          subNodes={['Shopping Cart', 'Billing & Shipping', 'Shopping Summary']}
          actions={[
            () => {
              dispatch(shopping_cart_page());
            },
            () => {
              dispatch(billing_shipping_page());
            },
            null,
          ]}
        />
        <div className="shopping_cart_summary">
          <div className="row">
            <div className="col-12 title">Shopping Summary</div>
          </div>
          {account && account.customerCode && orderData && (
            <Fragment>
              <OrderDetail account={account} orderData={orderData} isMobile={isMobile} />
              <div className="row">
                <div className="col-12 cart-note">
                  <p className="note-header">
                    RESELLER POLICY (Check after you finish reading, and click NEXT)
                  </p>
                  <p className="note-detail">
                    Vita Aid. Professional Therapeutics provides health care professionals with high
                    quality nutraceutical products. To help maintain a healthy market environment
                    and to uphold our commitment to practitioners, Vita Aid has designed this
                    reseller and pricing policy. This policy serves to maintain a healthy
                    practitioner-patient relationship, protect practitioner's sales, ensure
                    sufficient practitioner profit margins, protect Vita Aid's brand image, and
                    expand Vita Aid's market share.
                  </p>
                  <p className="note-detail">
                    This policy applies to practitioners who own, manage, or operate online
                    websites, pharmacies, or clinics. The policy also applies to any advertisement
                    of Vita Aid products in any media including, but not limited to catalogs, direct
                    mails, newspapers, magazines, television, posters, flyers, inserts, coupons,
                    emails, internet, and etc.
                  </p>
                  <p className="note-detail">
                    The following section outlines the terms and conditions of distributing Vita Aid
                    products to consumers. These terms shall remain in effect until such time as a
                    revised version of the policy is implemented, at which point the revised policy
                    shall supersede and replace the current terms.
                  </p>
                  <p className="note-detail">
                    1.SELLING VITA AID PRODUCTS IN STORE PREMISE(S)
                    <ul>
                      <li>
                        a. All products from Vita Aid will ONLY be displayed behind the counter
                        (i.e., not directly accessible to consumers).
                      </li>
                      <li>
                        b. Since Vita Aid products are not designed for self-medication, they must
                        be prescribed or recommended by a licensed health care practitioner. The
                        practitioner can only sell Vita Aid products to consumers provided that:
                        <ul>
                          <li>
                            i. The consumer comes with a signed prescription from a licensed health
                            care practitioner, or
                          </li>
                          <li>
                            ii.The practitioner has an on-site licensed health care practitioner
                            that can provide consultation to patients.
                          </li>
                        </ul>
                      </li>
                    </ul>
                  </p>
                  <p className="note-detail">
                    2. SELLING VITA AID PRODUCTS ON-LINE
                    <ul>
                      <li>
                        a. The practitioner must have a sound screening system in place to prevent
                        consumers from obtaining Vita Aid products for self-medication.
                      </li>
                      <li>
                        b. All Vita Aid products can only be sold to consumers if it was a direct
                        referral from a health care practitioner or the consumer can provide proof
                        of prescription of his/her health care practitioner.
                      </li>
                      <li>
                        c. Retail Prices of all Vita Aid products need to be concealed on the
                        website and only available for members to view.
                      </li>
                      <li>
                        d. Selling Vita Aid products through 3rd party sites such as Amazon, EBay,
                        and etc. is not permitted and will be deemed to be violations of this
                        policy. Vita Aid reserves the right not to sell or supply any products to
                        any practitioner who is affiliated with a web site which violates this
                        policy.
                      </li>
                    </ul>
                  </p>
                  <p className="note-detail">
                    3. MANUFACTURER'S SUGGESTED RETAIL PRICE (MSRP) & MINIMUM ADVERTISED PRICE (MAP)
                    <ul>
                      <li>
                        a. The practitioner will not set the MAP of Vita Aid products for less than
                        85% of the MSRP.
                      </li>
                    </ul>
                  </p>
                  <p className="note-detail">
                    Failure to comply with this policy may result in temporary or permanent
                    suspension or termination of practitioner's account with Vita Aid.
                  </p>
                </div>
              </div>
              <div className="row">
                <div className="col-12 cart-note">
                  <p className="note-header">Shipping Terms (Canada & US)</p>
                  <p className="note-detail">
                    All orders to Canadian destinations are shipped via Canada Post (Expedited
                    Parcel). The time of arrival is within 2-12 business days depending on the
                    destinations; during holiday season, orders may be delayed. Ever since December
                    2003, all food products require substantial processing in order to get clearance
                    into United States (requiring clearance from the Food & Drug Agency and Prior
                    Notice with US Customs). Vita Aid, however, will provide the service of
                    completing Prior Notice for American customers free of charge.
                  </p>
                  <p className="note-detail">
                    It is also required by the US Custom that all imported foods into US needing to
                    undergo random inspections. If your package is selected to undergo inspection,
                    you will be charged an inspection fee. If the time of arrival is delayed, or if
                    the package is lost or damaged, due to the inspection, Canada Post is not
                    obliged to compensate the loss. Due to the fact that shipping is not part of
                    Vita Aid's service, Vita Aid cannot process a refund if a shipment is
                    mis-delivered caused by incorrect address information entered or lost during
                    transporting process o I agree to the above terms.
                  </p>
                </div>
              </div>
              <div className="row">
                <div className="col-12">
                  <label className="chkbox-agree-term">
                    <input
                      type="checkbox"
                      onClick={() => {
                        setAgreeTerm(!agreeTerm);
                      }}
                    />
                    I agree to the above terms.
                  </label>
                </div>
              </div>
              <div className="row">
                <div className="col-12 cart-action">
                  {orderData && orderData.orderItems && (
                    <Fragment>
                      <button
                        css={css`
                          margin-right: ${isMobile ? 14 : 0}px;
                        `}
                        onClick={() => {
                          dispatch(productCodeChanged(''));
                          dispatch(byCategory());
                          dispatch(productCategoryChanged(''));
                          navigate('/products');
                        }}
                      >
                        CONTINUE SHOPPING
                      </button>
                      {isMobile === false && (
                        <button
                          onClick={() => {
                            dispatch(shopping_cart_page());
                          }}
                        >
                          BACK TO SHOPPING CART
                        </button>
                      )}
                      <button
                        className={agreeTerm === true ? '' : 'btn-disable'}
                        onClick={() => {
                          dispatch(payment_method_page());
                        }}
                      >
                        NEXT
                      </button>
                    </Fragment>
                  )}
                  {Object.keys(orderData).length == 0 && (
                    <Fragment>
                      <button
                        onClick={() => {
                          dispatch(productCodeChanged(''));
                          dispatch(byCategory());
                          dispatch(productCategoryChanged(''));
                          navigate('/products');
                        }}
                      >
                        BACK TO PRODUCTS
                      </button>
                    </Fragment>
                  )}
                </div>
              </div>
            </Fragment>
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
