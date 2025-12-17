/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, ReactNode as ReactNode, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useForm, Controller, Control } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { useSelector, useDispatch } from 'react-redux';
import { accountData } from 'redux/features/account/accountSlice';
import { OrderData, OrderItemData, CreditCardData, putOrder } from 'model/ShoppingCart';
import { AddressData } from 'model/AddressBook';
import { order, orderSlice, orderChanged } from 'redux/features/shoppingcart/orderSlice';
import { orderComment, orderCommentChanged } from 'redux/features/shoppingcart/orderCommentSlice';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
  cartChanged,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { addressBook } from 'redux/features/addressbook/addressBookSlice';
import {
  CartPageType,
  cartPageIdx,
  shopping_cart_page,
  billing_shipping_page,
  shopping_summary_page,
  payment_method_page,
  order_completion_page,
} from 'redux/features/shoppingcart/cartPageSlice';
import { dropShip } from 'redux/features/shoppingcart/dropShipSlice';
import {
  paymentMethod,
  pay_by_credit_card,
  pay_by_cheque,
} from 'redux/features/shoppingcart/paymentMethodSlice';
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import InputMask from 'react-input-mask';
import { LoadPanel } from 'devextreme-react/load-panel';
import { orderCoupon } from 'redux/features/shoppingcart/orderCouponSlice';
interface Props {
  isMobile: boolean;
}
export const PaymentMethod = ({ isMobile }: Props) => {
  const [submittedState, setSubmittedState] = React.useState(0);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  let cart = useSelector(shoppingCart);
  let orderData = useSelector(order);
  let addressBookData = useSelector(addressBook);
  let bDropShip = useSelector(dropShip);
  let paymentMethodData = useSelector(paymentMethod);
  let orderCommentData = useSelector(orderComment);
  let couponCode = useSelector(orderCoupon);
  window.scrollTo(0, 0);
  const {
    register: creditCardForm,
    handleSubmit: handleContactUsSubmit,
    watch,
    setError,
    control,
    formState: { errors: errorsCreditCard },
  } = useForm<CreditCardData>({ mode: 'onBlur' });
  const form = useRef(null);

  const isCreditCardValid = function (ccNum: string) {
    var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
    var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
    var amexpRegEx = /^(?:3[47][0-9]{13})$/;
    var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
    var isValid = false;

    if (visaRegEx.test(ccNum)) {
      isValid = true;
    } else if (mastercardRegEx.test(ccNum)) {
      isValid = true;
    } else if (amexpRegEx.test(ccNum)) {
      isValid = true;
    } else if (discovRegEx.test(ccNum)) {
      isValid = true;
    }
    return isValid;
  }

  // This method handles both credit card and cheque payment
  const submitCreditCardForm = async (data: CreditCardData) => {

    // paymentMethodData could be either 'Credit Card' or 'Check'
    if (paymentMethodData === 'Credit Card' ||
      paymentMethodData !== 'Credit Card'
    ) {

      // disable credit card verification
      // if (paymentMethodData === 'Credit Card') {
      //   var cardNo = data.cardNo.replace(/ /g, '');
      //   if (!isCreditCardValid(cardNo)) {
      //     setError('cardNo', {
      //       type: 'pattern',
      //       message: 'Credit card No. is not valid.',
      //     });
      //     return;
      //   }
      //   var splitted = data.expiryDate.split('/');
      //   var month: number = +splitted[0];
      //   var year: number = +splitted[1] + 2000;
      //   if (month > 12) {
      //     setError('expiryDate', {
      //       type: 'pattern',
      //       message: 'Wrong format on the expiry date!',
      //     });
      //     return;
      //   }

      //   var date = new Date();
      //   var currMonth: number = date.getMonth() + 1;
      //   var currYear: number = date.getFullYear();
      //   if (year < currYear || (year == currYear && month < currMonth)) {
      //     setError('expiryDate', {
      //       type: 'pattern',
      //       message: "The expiry date is before today's date",
      //     });
      //     return;
      //   }
      // }

      setSubmittedState(1);
      const rtnOrder = await putOrder(
        account.id,
        account.customerCode,
        country,
        cart,
        (addressBookData as AddressData[]).filter((x) => x.defaultBillingAddress)[0].id,
        (addressBookData as AddressData[]).filter((x) => x.defaultShippingAddress)[0].id,
        bDropShip,
        paymentMethodData,
        orderCommentData,
        data,
        couponCode,
      );

      setSubmittedState(0);
      setVisibleMsgBox(true);
      dispatch(orderChanged(rtnOrder));

      dispatch(cartChanged([] as ShoppingCartItem[]));
      dispatch(order_completion_page());
    }
  };
  return (
    <React.Fragment>
      <div className="content-main-body">
        <BreadCrumbsBlock
          subNodes={['Shopping Cart', 'Billing & Shipping', 'Shopping Summary', 'Payment Method']}
          actions={[
            () => {
              dispatch(shopping_cart_page());
            },
            () => {
              dispatch(billing_shipping_page());
            },
            () => {
              dispatch(shopping_summary_page());
            },
            null,
          ]}
        />
        <div className="row">
          <div className="col-12">
            <div className="payment-method">
              <div className="title">Payment Method</div>
              <div className="detail-block">
                <div
                  className="row"
                  css={css`
                    margin-left: 0px !important;
                    margin-right: 0px !important;
                  `}
                >
                  <form
                    ref={form}
                    className="col-12 credit-card-form"
                    onSubmit={handleContactUsSubmit(submitCreditCardForm)}
                  >
                    <div className="left-block">
                      <div className="row header">
                        <div className="col-12">
                          <label className="header-label">
                            <input
                              type="checkbox"
                              defaultChecked={paymentMethodData === 'Credit Card'}
                              checked={paymentMethodData === 'Credit Card'}
                              onChange={() => { }}
                              onClick={() => {
                                dispatch(pay_by_credit_card());
                              }}
                            />
                            <img className="payment-credit-img" alt="" src="/img/credit.png" />
                            {country === 'CA' && <span>Credit Card Payment (Only VISA/Master)</span>}
                            {country !== 'CA' && (
                              <span>Credit Card Payment (Only VISA/Master/AmericanExpress)</span>
                            )}
                          </label>

                        </div>
                      </div>

                      {/* Remove credit card input information */}

                      {/* <div>
                        <input
                          className={`credit-card-input full-col ${
                            submittedState === 1 ? 'input-disable' : ''
                          }`}
                          id="cardNo"
                          type="text"
                          {...creditCardForm('cardNo', {
                            required: paymentMethodData === 'Credit Card' ? true : false,
                          })}
                          placeholder="Credit Card No.*"
                        />
                        {paymentMethodData === 'Credit Card' && errorsCreditCard.cardNo && (
                          <div key="err-cardNo" className="error-msg">
                          {errorsCreditCard.cardNo.message}
                          </div>
                        )}
                      </div> */}

                      {/* <div className="line-2">
                        <div className="first-col">
                          <Controller
                            name="expiryDate"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                              <InputMask mask="99/99" value={value} onChange={onChange}>
                                <input
                                  type="text"
                                  id="expiryDate"
                                  name="expiryDate"
                                  placeholder="Expiry Date*    MM/YY*"
                                  className={`credit-card-input ${
                                    submittedState === 1 ? 'input-disable' : ''
                                  }`}
                                  required={paymentMethodData === 'Credit Card' ? true : false}
                                />
                              </InputMask>
                            )}
                          />
                          {paymentMethodData === 'Credit Card' && errorsCreditCard.expiryDate && (
                            <div key="err-expiryDate" className="error-msg">
                              {errorsCreditCard.expiryDate.type === 'required'
                                ? 'This field is required'
                                : errorsCreditCard.expiryDate.message}
                            </div>
                          )}
                        </div>
                        <div className="second-col ">
                          <input
                            className={`credit-card-input ${
                              submittedState === 1 ? 'input-disable' : ''
                            }`}
                            id="cid"
                            type="text"
                            {...creditCardForm('cid', {
                              required: paymentMethodData === 'Credit Card' ? true : false,
                            })}
                            placeholder="CID*"
                          />
                          {paymentMethodData === 'Credit Card' && errorsCreditCard.cid && (
                            <div key="err-cid" className="error-msg">
                              This field is required
                            </div>
                          )}
                        </div>
                      </div> */}

                      {/* <div>
                        <input
                          className={`credit-card-input full-col ${
                            submittedState === 1 ? 'input-disable' : ''
                          }`}
                          id="holder"
                          type="text"
                          {...creditCardForm('holder', {
                            required: paymentMethodData === 'Credit Card' ? true : false,
                          })}
                          placeholder="Card Holder*"
                        />
                        {paymentMethodData === 'Credit Card' && errorsCreditCard.holder && (
                          <div key="err-holder" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div> */}

                      {/* <div>
                        <input
                          className={`credit-card-input full-col ${
                            submittedState === 1 ? 'input-disable' : ''
                          }`}
                          id="address"
                          type="text"
                          {...creditCardForm('address', {
                            required: paymentMethodData === 'Credit Card' ? true : false,
                          })}
                          placeholder="Address*"
                        />
                        {paymentMethodData === 'Credit Card' && errorsCreditCard.address && (
                          <div key="err-address" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div> */}

                      {/* <div>
                        <input
                          className={`credit-card-input full-col ${
                            submittedState === 1 ? 'input-disable' : ''
                          }`}
                          id="phone"
                          type="text"
                          {...creditCardForm('phone', {
                            required: paymentMethodData === 'Credit Card' ? true : false,
                          })}
                          placeholder="Phone Number*"
                        />
                        {paymentMethodData === 'Credit Card' && errorsCreditCard.phone && (
                          <div key="err-phone" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div>  */}
                    </div>

                    <div className="right-block">
                      <div className="row header">
                        <div className="col-12">
                          <label className="header-label">
                            <input
                              type="checkbox"
                              defaultChecked={paymentMethodData === 'Check'}
                              checked={paymentMethodData === 'Check'}
                              onChange={() => { }}
                              onClick={() => {
                                dispatch(pay_by_cheque());
                              }}
                            />
                            <img className="payment-cheque-img" alt="" src="/img/cheque.png" />
                            Pay By Cheque
                          </label>
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-12">
                          <div className="check-desc">
                            <div>Please mail the cheque to the following address :</div>
                            <div>
                              #303-20285 Stewart Crescent, Maple Ridge,{isMobile ? <br /> : ' '}BC,
                              Canada V2X 8G1
                            </div>
                            <div>Payable to Vita Aid Professional Therapeutics Inc.</div>
                            <div>Payment term is 30 Days.</div>
                            {/* <div>Tel: 604-465-1688.</div> */}
                          </div>
                        </div>
                      </div>
                      <button
                        className={`${submittedState === 1 ? 'btn-disable submit-btn' : 'submit-btn'
                          }`}
                      >
                        NEXT
                      </button>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        height={200}
        minWidth={350}
        maxWidth={400}
        visible={submittedState === 1}
        message="The order will be submitted to the system. It may take up to 1- 2 minutes depending on the bandwidth of the transmission. Please wait for the next page to come up to confirm the completion of the order."
      />
    </React.Fragment>
  );
};
