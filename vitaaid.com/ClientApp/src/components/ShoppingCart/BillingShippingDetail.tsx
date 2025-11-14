/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useMemo, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { dropShip, dropShipChanged } from 'redux/features/shoppingcart/dropShipSlice';
import { orderComment, orderCommentChanged } from 'redux/features/shoppingcart/orderCommentSlice';
import { useSelector, useDispatch } from 'react-redux';
import { MessageBox } from 'components/MessageBox';

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
import { AddressData, newAddress, getAddressBook, defaultAddressChange } from 'model/AddressBook';
import { addressBook, addressBookChanged } from 'redux/features/addressbook/addressBookSlice';
import { OrderData, OrderItemData, buildOrder } from 'model/ShoppingCart';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import {
  sameAsBillingAddr,
  sameAsBillingAddrChanged,
} from 'redux/features/shoppingcart/sameAsBillingAddrSlice';
import { useEffect } from 'react';
import { useState } from 'react';
import { Popover, ToolbarItem } from 'devextreme-react/popover';
import { Popup } from 'devextreme-react/popup';
import { positionConfig } from 'devextreme/animation/position';
import { Button } from 'devextreme-react/text-box';

import ScrollView from 'devextreme-react/scroll-view';
import { AddressBookSetting } from 'components/ShoppingCart/AddressBookSetting';
import { eOPSTATE, iStateData, iStateChanged } from 'redux/features/OPStateSlice';
interface Props {
  address: AddressData | undefined;
  isMobile: boolean;
}

const AddressBlock = ({ address, isMobile }: Props) => {
  return (
    <Fragment>
      {address && (
        <Fragment>
          {address.addressPerson && (
            <div>
              {isMobile === false && <span className="address-block-label">Name : </span>}
              {address.addressPerson}
            </div>
          )}
          {address.addressName && (
            <div>
              {isMobile === false && (
                <span className="address-block-label">Clinic/Pharmacy : </span>
              )}
              {address.addressName}
            </div>
          )}
          <div>
            {isMobile === false && <span className="address-block-label">Address : </span>}
            {address.address}
          </div>
          {isMobile && (
            <div>
              {address.city}, {address.province}, {address.postalCode}
            </div>
          )}
          {isMobile === false && (
            <Fragment>
              <div>
                <span className="address-block-label">City : </span>
                {address.city}
              </div>
              <div>
                <span className="address-block-label">State/Province : </span>
                {address.province}
              </div>
              <div>
                <span className="address-block-label">Zip/Postal Code : </span>
                {address.postalCode}
              </div>
            </Fragment>
          )}

          <div>
            {isMobile === false && <span className="address-block-label">Country : </span>}
            {address.country}
          </div>
          <div>
            {isMobile === false && <span className="address-block-label">Phone Number : </span>}
            {address.tel}
          </div>
        </Fragment>
      )}
    </Fragment>
  );
};

interface BillingShippingDetailProps {
  isMobile: boolean;
}

export const BillingShippingDetail = ({ isMobile }: BillingShippingDetailProps) => {
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  let cart = useSelector(shoppingCart);
  let orderData = useSelector(order);
  let bDropShip = useSelector(dropShip);
  let orderCommentData = useSelector(orderComment);
  let addressBookData = useSelector(addressBook);
  let bSameAsBillingAddr = useSelector(sameAsBillingAddr);
  const addressBookState = useSelector(iStateData);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [billingAddress, setBillingAddress] = React.useState<AddressData>();
  const [shippingAddress, setShippingAddress] = React.useState<AddressData>();
  const [showAddressChangePopover, setAddressChangePopover] = React.useState<boolean>(false);
  const [changeType, setChangeType] = React.useState<string>('');
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);

  const AddNewAddress_Click = () => {
    let newObj: AddressData = newAddress();
    let newAddressBook = [newObj, ...addressBookData];
    dispatch(iStateChanged(eOPSTATE.NEW));
    dispatch(addressBookChanged(newAddressBook));
  };
  let scrollViewOfAddressBook = useRef(null);
  const addNewAddressOption = {
    text: 'ADD NEW ADDRESS',
    onClick: () => {
      AddNewAddress_Click();
      // let newObj: AddressData = newAddress();
      // let newAddressBook = [newObj, ...addressBookData];
      // dispatch(iStateChanged(eOPSTATE.NEW));
      // dispatch(addressBookChanged(newAddressBook));
    },
  };
  const destop_ver_positionOfPopover: positionConfig = {
    my: 'center',
    at: 'center',
    of: window,
  };
  const mobile_ver_positionOfPopover: positionConfig = {
    my: { x: 'left', y: 'top' },
    at: { x: 'left', y: 'top' },
    of: window,
  };

  useEffect(() => {
    async function fetchData() {
      const data = await getAddressBook(account.customerCode);
      if (
        data &&
        ((data as AddressData[]).length == 1 ||
          ((data as AddressData[]).length > 1 &&
            (data as AddressData[]).filter((x) => x.defaultBillingAddress)[0]
              .defaultShippingAddress))
      )
        dispatch(sameAsBillingAddrChanged(true));
      else dispatch(sameAsBillingAddrChanged(false));

      dispatch(addressBookChanged(data));
    }
    if (cart && cart.length > 0) {
      cart
        .filter((x) => x.qty <= 0)
        .map((x) => {
          dispatch(removeCartItem(x.code));
        });
      fetchData();
    } else {
      dispatch(orderChanged({} as OrderData));
    }
  }, []);
  useEffect(() => {
    const billing =
      (addressBookData as AddressData[]).filter((x) => x.defaultBillingAddress)[0] ??
      addressBookData[0];
    const shipping =
      (addressBookData as AddressData[]).filter((x) => x.defaultShippingAddress)[0] ?? billing;
    setBillingAddress(billing);
    setShippingAddress(shipping);
  }, [addressBookData]);

  useEffect(() => {
    if (
      billingAddress &&
      shippingAddress &&
      billingAddress.country === 'CANADA' &&
      shippingAddress.country !== 'CANADA'
    )
      setVisibleMsgBox(true);
  }, [billingAddress, shippingAddress]);
  return (
    <React.Fragment>
      <div className="content-main-body">
        <BreadCrumbsBlock
          subNodes={['Shopping Cart', 'Billing & Shipping']}
          actions={[
            () => {
              dispatch(shopping_cart_page());
            },
            null,
          ]}
        />

        <div className="billing_shipping_detail">
          <div className="row">
            <div className="col-12 title">Billing &amp; Shipping</div>
          </div>
          {account && account.customerCode && orderData && (
            <Fragment>
              <div className="row header">
                <div className="col-12">
                  <label className="header-label">
                    <input
                      type="checkbox"
                      defaultChecked={bDropShip}
                      onClick={() => {
                        dispatch(dropShipChanged(!bDropShip));
                      }}
                    />
                    CHECK if this is a DROP SHIP order
                  </label>
                  <div className="desc-drop-ship-order">
                    (ie. shipped to patient ; invoice will be mailed separately.)
                  </div>
                  {/* <h4 css={css`color: red;`}>Note: Drop Shipping Service is temporarily ceased. Kindly confirm your shipping address.</h4> */}
                </div>
              </div>
              <div id="billing-part" className="row address-table-header">
                <div className="col-6 address-table-header-col">
                  Billing
                  <button
                    className="change-button a-btn"
                    onClick={() => {
                      setChangeType('billing');
                      setAddressChangePopover(!showAddressChangePopover);
                    }}
                  >
                    Change
                  </button>
                </div>
                {isMobile === false && (
                  <div id="shipping-part" className="col-6 address-table-header-col">
                    <div className="shipping-block">
                      Shipping
                      <button
                        className={
                          bSameAsBillingAddr
                            ? 'change-button a-btn btn-disable'
                            : 'change-button a-btn'
                        }
                        onClick={() => {
                          setChangeType('shipping');
                          setAddressChangePopover(!showAddressChangePopover);
                        }}
                      >
                        Change
                      </button>
                      <div className="chk-sames-div">
                        <input
                          className="checkbox"
                          id="chk-sames"
                          type="checkbox"
                          checked={bSameAsBillingAddr}
                          onChange={(e: any) => {
                            async function DoChange(addressID: number) {
                              const data = await defaultAddressChange(
                                account.customerCode,
                                addressID,
                                'shipping',
                              );
                              dispatch(addressBookChanged(data));
                            }
                            if (
                              bSameAsBillingAddr == false &&
                              shippingAddress!.id !== billingAddress!.id
                            ) {
                              DoChange(billingAddress!.id);
                            } else {
                              dispatch(sameAsBillingAddrChanged(!bSameAsBillingAddr));
                            }
                            //dispatch(sameAsBillingAddrChanged(!bSameAsBillingAddr));
                          }}
                        />
                        <label htmlFor="chk-sames">Same as Billing Address</label>
                      </div>
                    </div>
                  </div>
                )}
              </div>
              <div className="row address-summary">
                <div className={`${isMobile ? 'col-12' : 'col-6'} address billing-address-part`}>
                  <AddressBlock address={billingAddress} isMobile={isMobile} />
                  <div>
                    {isMobile === false && (
                      <span className="address-block-label">Email Address : </span>
                    )}
                    {account.email}
                  </div>
                </div>
                {isMobile && (
                  <div id="shipping-part" className="col-12 address-table-header-col">
                    <div className="shipping-block">
                      Shipping
                      <button
                        className={
                          bSameAsBillingAddr
                            ? 'change-button a-btn btn-disable'
                            : 'change-button a-btn'
                        }
                        onClick={() => {
                          setChangeType('shipping');
                          setAddressChangePopover(!showAddressChangePopover);
                        }}
                      >
                        Change
                      </button>
                      <div className="chk-sames-div">
                        <input
                          className="checkbox"
                          id="chk-sames"
                          type="checkbox"
                          checked={bSameAsBillingAddr}
                          onChange={(e: any) => {
                            async function DoChange(addressID: number) {
                              const data = await defaultAddressChange(
                                account.customerCode,
                                addressID,
                                'shipping',
                              );
                              dispatch(addressBookChanged(data));
                            }
                            if (
                              bSameAsBillingAddr == false &&
                              shippingAddress!.id !== billingAddress!.id
                            ) {
                              DoChange(billingAddress!.id);
                            } else {
                              dispatch(sameAsBillingAddrChanged(!bSameAsBillingAddr));
                            }
                            //dispatch(sameAsBillingAddrChanged(!bSameAsBillingAddr));
                          }}
                        />
                        <label htmlFor="chk-sames">Same as Billing Address</label>
                      </div>
                    </div>
                  </div>
                )}
                <div className={`${isMobile ? 'col-12' : 'col-6'} address`}>
                  <div className="shipping-block">
                    <AddressBlock address={shippingAddress} isMobile={isMobile} />
                  </div>
                </div>
              </div>
              <div className="row order-comment-block">
                <div className="col-12">
                  <p className="order-comment-label">
                    If you have any remarks that you would like to add to the order (eg. shipping
                    instructions), please enter them on below :
                  </p>
                  <textarea
                    className="order-comment-textarea"
                    name="orderComment"
                    defaultValue={orderCommentData}
                    onChange={(e: any) => {
                      dispatch(orderCommentChanged(e.target.value?.trim() ?? ''));
                    }}
                  />
                  {/*  <p className="restriction-text">*/}
                  {/*    *Restrictions may apply to remote area shipping ; in which case, we will contact*/}
                  {/*    you.*/}
                  {/*  </p>*/}
                </div>
              </div>
              <div className="row">
                <div className="col-12 cart-action">
                  {orderData && orderData.orderItems && (
                    <button
                      onClick={() => {
                        dispatch(shopping_summary_page());
                      }}
                    >
                      NEXT
                    </button>
                  )}
                </div>
              </div>
            </Fragment>
          )}
        </div>
      </div>
      {account && (
        <Fragment>
          {isMobile === false && (
            <Popover
              position={destop_ver_positionOfPopover}
              shading={true}
              shadingColor="rgba(0, 0, 0, 0.5)"
              showTitle={true}
              showCloseButton={true}
              visible={showAddressChangePopover}
              closeOnOutsideClick={true}
              onHiding={() => {
                dispatch(iStateChanged(eOPSTATE.INIT));
                setAddressChangePopover(false);
              }}
              title="Address book"
              className="address-popover"
            >
              <ScrollView
                id="scrollViewOfAddressBook"
                ref={scrollViewOfAddressBook}
                width="100%"
                height="100%"
              >
                <div className="container-fluid address-change-popover">
                  <AddressBookSetting
                    key="billing-setting"
                    customerCode={account.customerCode}
                    addressBook={addressBookData}
                    type={changeType}
                  />
                </div>
              </ScrollView>
              <ToolbarItem
                widget="dxButton"
                toolbar="bottom"
                location="after"
                disabled={addressBookState !== eOPSTATE.INIT}
                options={addNewAddressOption}
              ></ToolbarItem>
            </Popover>
          )}
          {isMobile && (
            <Popup
              position={mobile_ver_positionOfPopover}
              visible={showAddressChangePopover}
              showCloseButton={false}
              showTitle={false}
              closeOnOutsideClick={true}
              onHiding={() => {
                dispatch(iStateChanged(eOPSTATE.INIT));
                setAddressChangePopover(false);
              }}
              className="address-popup"
            >
              <div className="address-book-popup-body">
                <div className="close-img-block">
                  <img
                    className="close-img"
                    alt="close"
                    src="/img/x-m-object.png"
                    srcSet="/img/x-m-object@2x.png 2x, /img/x-m-object@3x.png 3x"
                    onClick={() => {
                      dispatch(iStateChanged(eOPSTATE.INIT));
                      setAddressChangePopover(false);
                    }}
                  ></img>
                </div>
                <div className="address-book-title">Address Book</div>

                <ScrollView
                  id="scrollViewOfAddressBook"
                  ref={scrollViewOfAddressBook}
                  width="100%"
                  height={`${window.innerHeight - 138}px`}
                >
                  <div className="container-fluid address-change-popover">
                    <AddressBookSetting
                      key="billing-setting"
                      customerCode={account.customerCode}
                      addressBook={addressBookData}
                      type={changeType}
                    />
                  </div>
                </ScrollView>
                <div className="add-btn-div">
                  <button
                    className="add-new-address"
                    onClick={() => AddNewAddress_Click()}
                    disabled={addressBookState !== eOPSTATE.INIT}
                  >
                    ADD NEW ADDRESS
                  </button>
                </div>
              </div>
            </Popup>
          )}
          <MessageBox
            Title="Note"
            Message="Please be reminded that changing the shipping address to destinations outside Canada will result in the USD prices being applied to the order."
            Type="INFO"
            IsVisible={visibleMsgBox}
            onVisibleChange={() => setVisibleMsgBox(false)}
          />
        </Fragment>
      )}
    </React.Fragment>
  );
};
