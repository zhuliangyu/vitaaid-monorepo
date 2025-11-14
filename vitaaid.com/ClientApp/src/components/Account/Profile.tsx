/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { Header } from 'components/Layouts/Header';
import { Footer } from 'components/Layouts/Footer';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { Country, State } from 'country-state-city';
import { MemberData, updateMember } from 'model/Member';
import { accountChanged, accountData } from 'redux/features/account/accountSlice';
import { validateEmail, validatePhysicianCode } from 'model/Registration';
import { eUNITTYPE, getUnitTypes, UnitTypeData } from 'model/UnitType';
import { isMobileData } from 'redux/features/isMobileSlice';
import {
  useForm,
  UseFormRegister,
  UseFormHandleSubmit,
  UseFormWatch,
  UseFormSetValue,
  FieldErrors,
  ValidateResult,
  UseFormSetError,
  Controller,
  Control,
} from 'react-hook-form';
import { MessageBox } from 'components/MessageBox';

export const Profile = () => {
  const webSiteCountry = useSelector(selectedCountry);
  const navigate = useNavigate();
  const account = useSelector(accountData);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const dispatch = useDispatch();
  const isMobile = useSelector(isMobileData);
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
    setError,
    control,
  } = useForm<MemberData>({ mode: 'onBlur' });
  const [submitted, setSubmitted] = React.useState(false);

  const form = useRef(null);
  React.useEffect(() => {}, []);
  const watchCountry = watch('country', webSiteCountry === 'CA' ? 'CA' : 'US');

  const onValidatePhysicianCode = async (physicianCode: string | null): Promise<ValidateResult> => {
    if (account.memberType !== 2) return true; // PATIENT = 2
    const isValid = await validatePhysicianCode(physicianCode);
    if (isValid === true) return true;
    setError('pat_pcode', {
      type: 'validate',
      message: "Couldn't find your physician code.",
    });
    return "Couldn't find your physician code.";
  };
  const onValidateEmail = async (email: string): Promise<ValidateResult> => {
    const isValid = await validateEmail(email, account.id);
    if (isValid === true) return true;
    setError('email', {
      type: 'validate',
      message: 'That email is taken.',
    });
    return 'That email is taken.';
  };
  const onValidatePhone = async (phone: string): Promise<ValidateResult> => {
    phone = phone.replace(/[()-+]/g, '');

    if (phone.length >= 9) return true;
    setError('telephone', {
      type: 'pattern',
      message: 'wrong telephone number format',
    });
    return 'wrong telephone number format';
  };

  const submitForm = async (data: any) => {
    var phone = data.telephone.replace(/[()-+]/g, '');

    if (phone.length < 9 || phone.includes('_')) {
      setError('telephone', {
        type: 'pattern',
        message: 'Wrong format!',
      });
      return;
    }
    if (account.memberType === 2) {
      // PATIENT = 2
      var result = await onValidatePhysicianCode(data.pat_pcode);
      if (result?.valueOf() !== true) return;
    }

    //data.country = Country.getCountryByCode(data.country)?.name ?? '';
    const newCountryValue = Country.getCountryByCode(data.country)?.name ?? '';
    const newMemberData = await updateMember(form.current!!, account.id, newCountryValue);
    setSubmitted(false);
    if (newMemberData && newMemberData.id > 0) {
      setVisibleMsgBox(true);
      dispatch(accountChanged(newMemberData));
    } else {
      alert('Error with your profile please try registering again.');
    }
  };

  return (
    <React.Fragment>
      <div className="content-main-body profile">
        <div className="row">
          <div className="col-12 title">Member Profile</div>
        </div>
        {account && (
          <Fragment>
            <div className="row">
              <div className="col-12 header">
                <div className="header-label">
                  Account No. : <span className="header-value">{account.customerCode}</span>
                </div>
                <div className="header-label">
                  Member Type. :
                  <span className="header-value">
                    {' '}
                    {account.memberType === 1
                      ? 'Healthcare Practitioner'
                      : account.memberType === 2
                      ? 'Patient'
                      : account.memberType === 3
                      ? 'Student [BINM]'
                      : account.memberType === 4
                      ? 'Student [CCNM]'
                      : account.memberType === 5
                      ? 'Student [Others]'
                      : ''}
                  </span>
                </div>
                <div className="header-label">
                  Practitioner Type. :{` `}
                  <span className="header-value">{`${account.practitionerType} ${
                    account.otherPractitionerType ? account.otherPractitionerType : ''
                  }`}</span>
                </div>
                {account.memberType <= 2 && (
                  <div className="header-label">
                    Physician Code :{` `}
                    <span className="header-value">{`${
                      account.memberType == 1 ? account.physicanCode : account.pat_pcode
                    }`}</span>
                  </div>
                )}
                <div className="header-label">
                  Member Status. :{` `}
                  <span className="header-value">
                    {account.memberStatus === 0
                      ? 'INACTIVE'
                      : account.memberStatus === 1
                      ? 'IN REVIEW'
                      : account.memberStatus === 2
                      ? 'REJECTED'
                      : account.memberStatus === 9
                      ? 'ACTIVE'
                      : ''}
                  </span>
                </div>
                <div className="header-label">Account Info. :</div>
              </div>
            </div>
            <form ref={form} className="row profile-row" onSubmit={handleSubmit(submitForm)}>
              <div className="profile-form ">
                <div className="col-12 profile-col-12">
                  <div className="row">
                    <div
                      className="col-12"
                      css={css`
                        padding: 0px;
                      `}
                    >
                      <div className="row">
                        <div className={isMobile ? 'col-12 left-side' : 'col-6 left-side'}>
                          <div className="row">
                            <div className="col-12">
                              <div className="g-input">
                                <select
                                  className={`col-input  ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="prefix"
                                  placeholder=" "
                                  defaultValue={account.prefix}
                                  {...register('prefix', { required: true })}
                                >
                                  <option value="" disabled hidden>
                                    Select prefix*
                                  </option>
                                  <option value="0">Dr.</option>
                                  <option value="1">Mr.</option>
                                  <option value="2">Ms.</option>
                                </select>
                                <label htmlFor="prefix">Prefix*</label>
                                {errors.prefix && (
                                  <div className="error-msg">This field is required</div>
                                )}
                              </div>
                              <div className="col2-div g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="Name"
                                  placeholder=" "
                                  defaultValue={account.name}
                                  {...register('name', {
                                    required: true,
                                  })}
                                />
                                <label htmlFor="Name">Name*</label>
                                {errors.name && (
                                  <div className="error-msg">
                                    {errors.name.type === 'required'
                                      ? 'This field is required'
                                      : errors.name.message}
                                  </div>
                                )}
                              </div>
                            </div>
                          </div>
                          <div className="row">
                            <div className="col-12">
                              <div className="g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="email"
                                  type="email"
                                  placeholder=" "
                                  defaultValue={account.email}
                                  {...register('email', {
                                    required: true,
                                    pattern: {
                                      value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                      message: 'Invalid email address',
                                    },
                                    validate: onValidateEmail,
                                  })}
                                />
                                <label htmlFor="email">Email*</label>
                                {errors.email && (
                                  <div className="error-msg">
                                    {errors.email.type === 'required'
                                      ? 'This field is required'
                                      : errors.email.message}
                                  </div>
                                )}
                              </div>

                              <div className="col2-div g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="ClinicName"
                                  defaultValue={account.clinicName}
                                  placeholder=" "
                                  {...register('clinicName', { required: false })}
                                />
                                <label htmlFor="clinicName">Clinic/Pharmacy Name(Optional)</label>
                              </div>
                            </div>
                          </div>
                          <div className="row">
                            <div className="col-12">
                              <div className="g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  type="tel"
                                  id="telephone"
                                  defaultValue={account.telephone}
                                  placeholder=" "
                                  {...register('telephone', {
                                    required: true,
                                    validate: onValidatePhone,
                                  })}
                                />
                                <label htmlFor="telephone">Phone Number*</label>
                                {errors.telephone && (
                                  <div className="error-msg">
                                    {errors.telephone.type === 'required'
                                      ? 'This field is required'
                                      : errors.telephone.message}
                                  </div>
                                )}
                              </div>
                              <div className="col2-div g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="fax"
                                  placeholder=" "
                                  defaultValue={account.fax}
                                  {...register('fax', { required: false })}
                                />
                                <label htmlFor="fax">Fax</label>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div className={isMobile ? 'col-12 right-side' : 'col-6 right-side'}>
                          <div className="row">
                            <div className="col-12 g-input">
                              <input
                                className={`col-input colspan2 ${
                                  submitted === true ? 'input-disable' : ''
                                }`}
                                id="address1"
                                placeholder=" "
                                defaultValue={account.address}
                                {...register('address', { required: true })}
                              />
                              <label htmlFor="address1">Address*</label>
                              {errors.address && (
                                <div className="error-msg">This field is required</div>
                              )}
                            </div>
                          </div>
                          <div className="row">
                            <div className="col-12">
                              <div className="g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="city"
                                  placeholder=" "
                                  defaultValue={account.city}
                                  {...register('city', { required: true })}
                                />
                                <label htmlFor="city">City*</label>
                                {errors.city && (
                                  <div className="error-msg">This field is required</div>
                                )}
                              </div>
                              <div className="col2-div g-input">
                                <input
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="zipCode"
                                  placeholder=" "
                                  defaultValue={account.zipCode}
                                  {...register('zipCode', { required: true })}
                                />
                                <label htmlFor="zipCode">Zip/Postal Code*</label>
                                {errors.zipCode && (
                                  <div className="error-msg">This field is required</div>
                                )}
                              </div>
                            </div>
                          </div>
                          <div className="row">
                            <div className="col-12">
                              <div className="g-input">
                                {watchCountry && (
                                  <Fragment>
                                    <select
                                      key={`state-${watchCountry}`}
                                      className={`col-input ${
                                        submitted === true ? 'input-disable' : ''
                                      }`}
                                      id="province"
                                      placeholder=" "
                                      defaultValue={account.province}
                                      {...register('province', { required: true })}
                                    >
                                      <option value="" disabled hidden>
                                        Select State/Province*
                                      </option>
                                      {State.getStatesOfCountry(watchCountry).map((x) => {
                                        return (
                                          <option
                                            key={`code-${x.name}`}
                                            value={`${
                                              watchCountry === 'CA' || watchCountry === 'US'
                                                ? x.isoCode
                                                : x.name
                                            }`}
                                          >{`${x.name}`}</option>
                                        );
                                      })}
                                    </select>
                                    <label htmlFor="province">State/Province*</label>
                                    {errors.province && (
                                      <div className="error-msg">This field is required</div>
                                    )}
                                  </Fragment>
                                )}
                              </div>
                              <div className="col2-div g-input">
                                <select
                                  className={`col-input ${
                                    submitted === true ? 'input-disable' : ''
                                  }`}
                                  id="country"
                                  placeholder=" "
                                  defaultValue={webSiteCountry === 'CA' ? 'CA' : 'US'}
                                  {...register('country', { required: true })}
                                >
                                  <option value="" disabled hidden>
                                    Select country*
                                  </option>
                                  {Country.getAllCountries().map((x) => {
                                    return (
                                      <option
                                        key={`${x.isoCode}`}
                                        value={`${x.isoCode}`}
                                      >{`${x.name}`}</option>
                                    );
                                  })}
                                </select>
                                <label htmlFor="country">Country*</label>
                                {errors.country && (
                                  <div className="error-msg">This field is required</div>
                                )}
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-12 button-block">
                      <button type="submit">SUBMIT</button>
                    </div>
                  </div>
                </div>
              </div>
            </form>
          </Fragment>
        )}
      </div>
      <MessageBox
        Title="PROFILE"
        Message="Your request was successfully sent."
        Type="INFO"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
};
