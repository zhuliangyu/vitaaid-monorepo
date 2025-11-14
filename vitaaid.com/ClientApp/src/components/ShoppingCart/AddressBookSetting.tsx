/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import { useForm, ValidateResult, Controller } from 'react-hook-form';
import { Popover } from 'devextreme-react/popover';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { dropShip, dropShipChanged } from 'redux/features/shoppingcart/dropShipSlice';
import { orderComment, orderCommentChanged } from 'redux/features/shoppingcart/orderCommentSlice';
import { addressBook, addressBookChanged } from 'redux/features/addressbook/addressBookSlice';

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
import { eOPSTATE, iStateData, iStateChanged } from 'redux/features/OPStateSlice';
import { sameAsBillingAddr } from 'redux/features/shoppingcart/sameAsBillingAddrSlice';

import {
  AddressData,
  getAddressBook,
  defaultAddressChange,
  updateAddress,
  removeAddress,
} from 'model/AddressBook';
import { OrderData, OrderItemData, buildOrder } from 'model/ShoppingCart';
import {
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  updateCartQty,
  removeCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { useEffect } from 'react';
import { useState } from 'react';
import { Country, State } from 'country-state-city';

interface AddressBlockProps {
  customerCode: string;
  address: AddressData;
  type: string;
  removable: boolean;
}

const AddressBlock = ({ customerCode, address, type, removable }: AddressBlockProps) => {
  let bSameAsBillingAddr = useSelector(sameAsBillingAddr);
  const addressBookState = useSelector(iStateData);
  const [submittedState, setSubmittedState] = React.useState(false);

  const [iState, setState] = useState<eOPSTATE>(
    Object.keys(address).length === 0 || address.id === 0 ? eOPSTATE.NEW : eOPSTATE.INIT,
  );
  const dispatch = useDispatch();
  const changeAddress = async (addressID: number) => {
    const data = await defaultAddressChange(customerCode, addressID, type, bSameAsBillingAddr);
    dispatch(addressBookChanged(data));
  };
  const reloadAddressBook = async () => {
    const data = await getAddressBook(customerCode);
    dispatch(addressBookChanged(data));
  };
  const changeAddressBtnClass =
    (type === 'billing' && address.defaultBillingAddress) ||
    (type === 'shipping' && address.defaultShippingAddress)
      ? 'default-address-button'
      : 'non-default-address-button';
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
    setError,
    control,
  } = useForm<AddressData>({ mode: 'onBlur' });

  const webSiteCountry = useSelector(selectedCountry);
  const [countryState, setCountryState] = React.useState<string>('');
  React.useEffect(() => {
    if (Object.keys(address).length > 0 && address.id > 0 && address.country.length > 0) {
      const tmpList = Country.getAllCountries().filter(
        (x) => x.name.toUpperCase() === address.country,
      );
      if (tmpList.length > 0) setCountryState(tmpList[0].isoCode);
      else setCountryState(webSiteCountry);
    } else setCountryState(webSiteCountry);
  }, []);
  React.useEffect(() => {
    if (addressBookState === eOPSTATE.INIT) setState(eOPSTATE.INIT);
  }, [addressBookState]);

  const submitForm = async (data: any) => {
    if (data.tel == undefined) data.tel = address.tel;
    var phone = data.tel.replace(/[()-+]/g, '');

    if (phone.length < 9 || phone.includes('_')) {
      setError('tel', {
        type: 'pattern',
        message: 'Wrong format!',
      });
      return;
    }
    setSubmittedState(true);
    address.addressPerson = data.addressPerson;
    address.addressName = data.addressName;
    address.address = data.address;
    address.city = data.city;
    address.postalCode = data.postalCode;
    address.province = data.province;
    address.country = Country.getCountryByCode(data.country)?.name.toUpperCase() ?? '';
    address.tel = data.tel;
    const newAddressBook = await updateAddress(customerCode, address);
    setSubmittedState(false);
    setState(eOPSTATE.INIT);
    dispatch(iStateChanged(eOPSTATE.INIT));
    dispatch(addressBookChanged(newAddressBook));
  };
  const onValidatePhone = async (phone: string): Promise<ValidateResult> => {
    phone = phone.replace(/[()-+]/g, '');

    if (phone.length >= 9) return true;
    setError('tel', {
      type: 'pattern',
      message: 'wrong telephone number format',
    });
    return 'wrong telephone number format';
  };
  const DeleteAddress = async () => {
    const newAddressBook = await removeAddress(customerCode, address.id);
    dispatch(addressBookChanged(newAddressBook));
    setState(eOPSTATE.INIT);
    dispatch(iStateChanged(eOPSTATE.INIT));
  };
  return (
    <div className="address-block">
      {(iState === eOPSTATE.INIT || iState === eOPSTATE.DELETE || submittedState == true) && (
        <Fragment key={address.id}>
          {address.addressPerson && <div>{address.addressPerson}</div>}
          {address.addressName && address.addressPerson !== address.addressName && (
            <div>{address.addressName}</div>
          )}
          <div>{address.address}</div>
          <div>
            {address.city}, {address.province}, {address.postalCode}
          </div>
          <div>{address.country}</div>
          <div>{address.tel}</div>
          {iState === eOPSTATE.DELETE && (
            <div className="delete-msg">
              <p>
                Please note: By deleting this address, you will not delete any pending orders being
                shipped to this address. To ensure uninterrupted fulfilment of future orders, please
                update any wishlists, subscribe-and-save settings, and periodical subscriptions
                using this address.
              </p>
            </div>
          )}
          <div className="action-block">
            {iState === eOPSTATE.DELETE && (
              <Fragment>
                <button
                  className="save-button"
                  onClick={() => {
                    DeleteAddress();
                  }}
                >
                  YES
                </button>
                <button
                  className="save-button"
                  onClick={() => {
                    setState(eOPSTATE.INIT);
                    dispatch(iStateChanged(eOPSTATE.INIT));
                  }}
                >
                  NO
                </button>
              </Fragment>
            )}
            {iState === eOPSTATE.INIT && (
              <Fragment>
                <button
                  className={
                    addressBookState === eOPSTATE.INIT
                      ? 'a-btn edit-remove-button'
                      : 'a-btn edit-remove-button btn-disable'
                  }
                  onClick={() => {
                    setState(eOPSTATE.DIRTY);
                    dispatch(iStateChanged(eOPSTATE.DIRTY));
                  }}
                >
                  Edit
                </button>
                <span>|</span>
                <button
                  className={
                    addressBookState === eOPSTATE.INIT && removable
                      ? 'a-btn edit-remove-button'
                      : 'a-btn edit-remove-button btn-disable'
                  }
                  onClick={() => {
                    setState(eOPSTATE.DELETE);
                    dispatch(iStateChanged(eOPSTATE.DELETE));
                  }}
                >
                  Remove
                </button>
                <button
                  className={
                    addressBookState === eOPSTATE.INIT
                      ? changeAddressBtnClass
                      : `${changeAddressBtnClass} btn-disable`
                  }
                  onClick={() => {
                    changeAddress(address.id);
                  }}
                >
                  {type === 'billing' ? 'Use this address' : 'Deliver to this address'}
                </button>
              </Fragment>
            )}
          </div>
        </Fragment>
      )}
      {(iState === eOPSTATE.NEW || iState === eOPSTATE.DIRTY) && submittedState == false && (
        <Fragment key={address.id}>
          <form className="edit-form edit-row" onSubmit={handleSubmit(submitForm)}>
            <div className="row">
              <div className="col-12">
                <div className="g-input">
                  <input
                    className="col-input"
                    id="addressPerson"
                    placeholder=" "
                    defaultValue={address.addressPerson}
                    {...register('addressPerson', { required: true })}
                  />
                  <label htmlFor="addressPerson">Name*</label>
                  {errors.addressPerson && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input">
                  <input
                    className="col-input"
                    type="tel"
                    id="telephone"
                    placeholder=" "
                    defaultValue={address.tel}
                    {...register('tel', {
                      required: true,
                      validate: onValidatePhone,
                    })}
                  />
                  <label htmlFor="telephone">Phone Number*</label>
                  {errors.tel && (
                    <div className="error-msg">
                      {errors.tel.type === 'required'
                        ? 'This field is required'
                        : errors.tel.message}
                    </div>
                  )}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12 g-input">
                <input
                  className="col-input colspan2"
                  id="addressName"
                  placeholder=" "
                  defaultValue={address.addressName}
                  {...register('addressName', { required: false })}
                />
                <label htmlFor="addressName">Clinic/Pharmacy(Optional)</label>
              </div>
            </div>
            <div className="row">
              <div className="col-12 g-input">
                <input
                  className={`col-input colspan2`}
                  id="address"
                  placeholder=" "
                  defaultValue={address.address}
                  {...register('address', { required: true })}
                />
                <label htmlFor="address">Address*</label>
                {errors.address && <div className="error-msg">This field is required</div>}
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input">
                  <input
                    className={`col-input`}
                    id="city"
                    placeholder=" "
                    defaultValue={address.city}
                    {...register('city', { required: true })}
                  />
                  <label htmlFor="city">City*</label>
                  {errors.city && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input">
                  <input
                    className={`col-input`}
                    id="zipCode"
                    placeholder=" "
                    defaultValue={address.postalCode}
                    {...register('postalCode', { required: true })}
                  />
                  <label htmlFor="zipCode">Zip/Postal Code*</label>
                  {errors.postalCode && <div className="error-msg">This field is required</div>}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input">
                  <select
                    key={`state-${countryState}`}
                    className={`col-input select-input`}
                    id="province"
                    placeholder=" "
                    defaultValue={address.province}
                    {...register('province', { required: true })}
                  >
                    <option value="" disabled hidden>
                      Select State/Province*
                    </option>
                    {countryState &&
                      State.getStatesOfCountry(
                        countryState !== '' ? countryState : webSiteCountry,
                      ).map((x) => {
                        console.log(
                          `refCountry=${countryState},defaultValue=${address.province}, value=${x.isoCode}`,
                        );
                        return (
                          <option
                            key={`code-${x.name}`}
                            value={`${
                              countryState === 'CA' || countryState === 'US' ? x.isoCode : x.name
                            }`}
                          >{`${x.name}`}</option>
                        );
                      })}
                  </select>
                  <label htmlFor="province">State/Province*</label>
                  {errors.province && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input">
                  <select
                    className={`col-input  select-input`}
                    id="country"
                    placeholder=" "
                    defaultValue={
                      address.country === ''
                        ? webSiteCountry === 'CA'
                          ? 'CA'
                          : 'US'
                        : Country.getAllCountries().filter(
                            (x) =>
                              x.name.localeCompare(address.country, undefined, {
                                sensitivity: 'base',
                              }) === 0,
                          )[0]?.isoCode
                    }
                    {...register('country', {
                      onChange: (e: any) => setCountryState(e.target.value),
                    })}
                  >
                    <option value="" disabled hidden>
                      Select country*
                    </option>
                    {Country.getAllCountries().map((x) => {
                      return (
                        <option key={`${x.isoCode}`} value={`${x.isoCode}`}>{`${x.name}`}</option>
                      );
                    })}
                  </select>
                  <label htmlFor="country">Country*</label>
                  {errors.country && <div className="error-msg">This field is required</div>}
                </div>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12"
                css={css`
                  text-align: end;
                `}
              >
                <button className="save-button" type="submit">
                  SAVE
                </button>
                <button
                  className="cancel-button"
                  onClick={() => {
                    setState(eOPSTATE.INIT);
                    dispatch(iStateChanged(eOPSTATE.INIT));
                    reloadAddressBook();
                  }}
                >
                  CANCEL
                </button>
              </div>
            </div>
          </form>
        </Fragment>
      )}
    </div>
  );
};

interface Props {
  customerCode: string;
  addressBook: AddressData[] | undefined;
  type: string;
}

export const AddressBookSetting = ({ customerCode, addressBook, type }: Props) => {
  const bRemovable = (addressBook?.length ?? 0) > 1;
  return (
    <Fragment>
      {addressBook && (
        <div className="addressbook-setting">
          {addressBook
            .filter((x) => x.id >= 0)
            .map((x) => {
              let address = { ...x };
              return (
                <div key={`div-${x.id}`} className="row">
                  <div className="col-12">
                    <AddressBlock
                      key={x.id}
                      customerCode={customerCode}
                      address={address}
                      type={type}
                      removable={bRemovable}
                    />
                  </div>
                </div>
              );
            })}
        </div>
      )}
    </Fragment>
  );
};
