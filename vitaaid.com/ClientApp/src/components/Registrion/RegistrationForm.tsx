/** @jsxImportSource @emotion/react */
import { eUNITTYPE, getUnitTypes, UnitTypeData } from 'model/UnitType';
import { css } from '@emotion/react';
import React, { Fragment, useEffect } from 'react';
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
import { useSelector, useDispatch } from 'react-redux';

import { RegistrationFormData, validateEmail, validatePhysicianCode } from 'model/Registration';

import { Country, State } from 'country-state-city';
import { selectedCountry } from '../../redux/features/country/countrySlice';

interface Props {
  register: UseFormRegister<RegistrationFormData>;
  handleSubmit: UseFormHandleSubmit<RegistrationFormData>;
  watch: UseFormWatch<RegistrationFormData>;
  setValue: UseFormSetValue<RegistrationFormData>;
  errors: FieldErrors<RegistrationFormData>;
  submitted: boolean;
  setError: UseFormSetError<RegistrationFormData>;
  control: Control<RegistrationFormData>;
  memberTypeName: string;
  isMobile: boolean;
}

export const RegistrationForm = ({
  register,
  handleSubmit,
  watch,
  setValue,
  errors,
  submitted,
  setError,
  control,
  memberTypeName,
  isMobile,
}: Props) => {
  const webSiteCountry = useSelector(selectedCountry);

  const [practitionerTypes, setPractitionTypes] = React.useState<UnitTypeData[]>([]);

  useEffect(() => {
    async function fetchData() {
      const data = await getUnitTypes(eUNITTYPE.PRACTICE_TYPE);
      setPractitionTypes(data.filter((x) => x.name !== 'Medical Students'));
    }
    fetchData();
  }, []);
  const watchCountry = watch('country', webSiteCountry === 'CA' ? 'CA' : 'US');
  const watchLicenceVerifyMethod = watch('licenceVerifyMethod', '');
  const watchPractitionerType = watch(
    'practitionerType',
    memberTypeName === 'MedicatStudent' ? 'Medical Students' : '',
  );
  const watchMemberType = watch(
    'memberType',
    memberTypeName === 'Practitioner' ? 1 : memberTypeName === 'Patient' ? 2 : 3,
  );

  const onValidateEmail = async (email: string): Promise<ValidateResult> => {
    const isValid = await validateEmail(email, 0);
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

  return (
    <Fragment>
      <div
        key={memberTypeName}
        className="col-12"
        css={css`
          padding: 0px;
        `}
      >
        <div className="row fields-block-m">
          <div className={`${isMobile ? 'col-12' : 'col-6'} left-side`}>
            <div className="row">
              <div className="col-12">
                <div className="g-input input-m">
                  <select
                    className={`col-input  ${submitted === true ? 'input-disable' : ''}`}
                    id="prefix"
                    placeholder=" "
                    defaultValue=""
                    {...register('prefix', { required: true })}
                  >
                    <option value="" disabled hidden></option>
                    <option value="0">Dr.</option>
                    <option value="1">Mr.</option>
                    <option value="2">Ms.</option>
                  </select>
                  <label className="label-of-select" htmlFor="prefix">
                    Prefix*
                  </label>
                  {errors.prefix && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input input-m">
                  <select
                    className={`col-input  
                  ${submitted === true ? 'input-disable' : ''} ${
                      watchPractitionerType === 'Other' ? 'other-pract other-type-m' : ''
                    }`}
                    id="practitionerType"
                    placeholder=" "
                    defaultValue=""
                    {...register('practitionerType', {
                      required: memberTypeName === 'Practitioner' ? true : false,
                    })}
                    hidden={memberTypeName === 'Practitioner' ? false : true}
                  >
                    <option value="" disabled hidden></option>
                    {practitionerTypes &&
                      practitionerTypes.map((x) => {
                        return <option key={`${x.id}`} value={`${x.name}`}>{`${x.name}`}</option>;
                      })}
                  </select>
                  <label
                    className="label-of-select"
                    htmlFor="practitionerType"
                    hidden={memberTypeName === 'Practitioner' ? false : true}
                  >
                    Type*
                  </label>
                  {errors.practitionerType && (
                    <div className="error-msg">This field is required</div>
                  )}
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="pat_pcode"
                    placeholder=" "
                    {...register('pat_pcode', {
                      required: memberTypeName === 'Patient' ? true : false,
                    })}
                    hidden={memberTypeName === 'Patient' ? false : true}
                  />
                  <label htmlFor="pat_pcode" hidden={memberTypeName === 'Patient' ? false : true}>
                    Physician Code*
                  </label>
                  {errors.pat_pcode && (
                    <div className="error-msg">
                      {errors.pat_pcode.type === 'required'
                        ? 'This field is required'
                        : errors.pat_pcode.message}
                    </div>
                  )}
                  <select
                    className={`col-input 
                  ${submitted === true ? 'input-disable' : ''} ${
                      watchMemberType === 5 ? 'other-pract other-type-m' : ''
                    }`}
                    id="memberType"
                    placeholder=" "
                    defaultValue={
                      memberTypeName === 'Practitioner' ? 1 : memberTypeName === 'Patient' ? 2 : 0
                    }
                    {...register('memberType', {
                      required: memberTypeName === 'MedicalStudent' ? true : false,
                    })}
                    hidden={memberTypeName === 'MedicalStudent' ? false : true}
                  >
                    <option value={0} disabled hidden></option>
                    {memberTypeName === 'MedicalStudent' && (
                      <Fragment>
                        <option value={3}>Student [BINM]</option>
                        <option value={4}>Student [CCNM]</option>
                        <option value={5}>Others</option>
                      </Fragment>
                    )}
                  </select>
                  <label
                    className="label-of-select"
                    htmlFor="memberType"
                    hidden={memberTypeName === 'MedicalStudent' ? false : true}
                  >
                    Type*
                  </label>
                  {errors.memberType && <div className="error-msg">This field is required</div>}
                </div>
                {((memberTypeName === 'Practitioner' && watchPractitionerType === 'Other') ||
                  (memberTypeName === 'MedicalStudent' && watchMemberType == 5)) && (
                  <div className="g-input input-m">
                    <input
                      className={`col-input input-other other-type-m ${
                        submitted === true ? 'input-disable' : ''
                      }`}
                      id="otherPractitionerType"
                      placeholder=" "
                      defaultValue=""
                      {...register('otherPractitionerType', {
                        required: true,
                      })}
                    ></input>
                    <label htmlFor="otherPractitionerType">Please specify</label>
                    {errors.otherPractitionerType && (
                      <div className="error-msg">This field is required</div>
                    )}
                  </div>
                )}
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="firstName"
                    placeholder=" "
                    {...register('firstName', { required: true })}
                  />
                  <label htmlFor="firstName">First Name*</label>
                  {errors.firstName && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="lastname"
                    placeholder=" "
                    {...register('lastName', { required: true })}
                  />
                  <label htmlFor="lastname">Last Name*</label>
                  {errors.lastName && <div className="error-msg">This field is required</div>}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input input-m">
                  {/*{webSiteCountry === 'CA' && (*/}
                  {/*  <Controller*/}
                  {/*    name="telephone"*/}
                  {/*    control={control}*/}
                  {/*    render={({ field: { onChange, value } }) => (*/}
                  {/*      <InputMask mask="999-999-9999" value={value} onChange={onChange}>*/}
                  {/*        {(inputProps: any) => (*/}
                  {/*          <Fragment>*/}
                  {/*            <input*/}
                  {/*              {...inputProps}*/}
                  {/*              type="tel"*/}
                  {/*              id="telephone"*/}
                  {/*              name="telephone"*/}
                  {/*              placeholder=" "*/}
                  {/*              className={`col-input ${submitted === true ? 'input-disable' : ''}`}*/}
                  {/*              required*/}
                  {/*            />*/}
                  {/*            <label htmlFor="telephone">Phone Number*</label>*/}
                  {/*          </Fragment>*/}
                  {/*        )}*/}
                  {/*      </InputMask>*/}
                  {/*    )}*/}
                  {/*  />*/}
                  {/*)}*/}
                  {/*{webSiteCountry !== 'CA' && (*/}
                  {/*  <Fragment>*/}
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    type="tel"
                    id="telephone"
                    placeholder=" "
                    {...register('telephone', {
                      required: true,
                      validate: onValidatePhone,
                    })}
                  />
                  <label htmlFor="telephone">Phone Number*</label>
                  {/*  </Fragment>*/}
                  {/*)}*/}
                  {errors.telephone && (
                    <div className="error-msg">
                      {errors.telephone.type === 'required'
                        ? 'This field is required'
                        : errors.telephone.message}
                    </div>
                  )}
                </div>
                <div className="col2-div g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="fax"
                    placeholder=" "
                    {...register('fax', { required: false })}
                  />
                  <label htmlFor="fax">Fax</label>
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12 g-input input-m">
                <input
                  className={`col-input colspan2 ${submitted === true ? 'input-disable' : ''}`}
                  id="email"
                  type="email"
                  placeholder=" "
                  {...register('email', {
                    required: true,
                    pattern: {
                      value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                      message: 'Invalid email address',
                    },
                    validate: onValidateEmail,
                  })}
                />
                <label htmlFor="email">Email Address*</label>
                {errors.email && (
                  <div className="error-msg">
                    {errors.email.type === 'required'
                      ? 'This field is required'
                      : errors.email.message}
                  </div>
                )}
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="password"
                    placeholder=" "
                    type="password"
                    {...register('password', { required: true })}
                  />
                  <label htmlFor="password">Password*</label>
                  {errors.password && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="confirmPassword"
                    placeholder=" "
                    type="password"
                    {...register('confirmPassword', { required: true })}
                  />
                  <label htmlFor="confirmPassword">Confirm Password*</label>
                  {errors.confirmPassword && (
                    <div className="error-msg">This field is required</div>
                  )}
                </div>
              </div>
            </div>
          </div>
          <div className={`${isMobile ? 'col-12' : 'col-6'} right-side`}>
            {memberTypeName === 'Practitioner' && (
              <div className="row">
                <div className="col-12 g-input input-m">
                  <input
                    className={`col-input colspan2 ${submitted === true ? 'input-disable' : ''}`}
                    id="clinicName"
                    placeholder=" "
                    {...register('clinicName', { required: false })}
                  />
                  <label htmlFor="clinicName">Clinic/Pharmacy Name(Optional)</label>
                </div>
              </div>
            )}
            <div className="row">
              <div className="col-12 g-input input-m">
                <input
                  className={`col-input colspan2 ${submitted === true ? 'input-disable' : ''}`}
                  id="address1"
                  placeholder=" "
                  {...register('address1', { required: true })}
                />
                <label htmlFor="address1">Address 1*</label>
                {errors.address1 && <div className="error-msg">This field is required</div>}
              </div>
            </div>
            <div className="row">
              <div className="col-12 g-input input-m">
                <input
                  className={`col-input colspan2 ${submitted === true ? 'input-disable' : ''}`}
                  id="address2"
                  placeholder=" "
                  {...register('address2', { required: false })}
                />
                <label htmlFor="address2">Address 2(Optional)</label>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="city"
                    placeholder=" "
                    {...register('city', { required: true })}
                  />
                  <label htmlFor="city">City*</label>
                  {errors.city && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div  g-input input-m">
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="zipCode"
                    placeholder=" "
                    {...register('zipCode', { required: true })}
                  />
                  <label htmlFor="zipCode">Zip/Postal Code*</label>
                  {errors.zipCode && <div className="error-msg">This field is required</div>}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12 g-input">
                <div className="input-m">
                  {watchCountry && (
                    <Fragment>
                      <select
                        key={`state-${watchCountry}`}
                        className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                        id="province"
                        placeholder=" "
                        defaultValue=""
                        {...register('province', { required: true })}
                      >
                        <option value="" disabled hidden></option>
                        {State.getStatesOfCountry(watchCountry).map((x) => {
                          return (
                            <option
                              key={`code-${x.name}`}
                              value={`${
                                watchCountry === 'CA' || watchCountry === 'US' ? x.isoCode : x.name
                              }`}
                            >{`${x.name}`}</option>
                          );
                        })}
                      </select>
                      <label className="label-of-select" htmlFor="province">
                        State/Province*
                      </label>
                    </Fragment>
                  )}
                  {errors.province && <div className="error-msg">This field is required</div>}
                </div>
                <div className="col2-div g-input input-m">
                  <select
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="country"
                    placeholder=" "
                    defaultValue={webSiteCountry === 'CA' ? 'CA' : 'US'}
                    {...register('country', { required: true })}
                  >
                    <option value="" disabled hidden></option>
                    {Country.getAllCountries().map((x) => {
                      return (
                        <option key={`${x.isoCode}`} value={`${x.isoCode}`}>{`${x.name}`}</option>
                      );
                    })}
                  </select>
                  <label className="label-of-select" htmlFor="country">
                    Country*
                  </label>
                  {errors.country && <div className="error-msg">This field is required</div>}
                </div>
              </div>
            </div>
          </div>
        </div>
        {memberTypeName !== 'Patient' && (
          <div className="row">
            <div className="licence-verify-block">
              {isMobile === false &&
                (watchLicenceVerifyMethod === 'upload licence photo' ||
                  watchLicenceVerifyMethod === null ||
                  watchLicenceVerifyMethod === undefined ||
                  watchLicenceVerifyMethod === '') && (
                  <input
                    type="file"
                    name="licencePhoto"
                    className={
                      watchLicenceVerifyMethod === 'upload licence photo'
                        ? 'file-uploader'
                        : 'file-uploader disable'
                    }
                    accept="application/pdf,image/*"
                    required
                  ></input>
                )}
              <p className="upload-desc">
                In order to have the online account activated, please send your licence/certificate
                to our croporate office. <br />
                Please check one of the following methods below :
              </p>
              <Fragment>
                <div>
                  <input
                    className="radio"
                    id="upload-licence-photo"
                    type="radio"
                    value="upload licence photo"
                    defaultChecked={true}
                    {...register('licenceVerifyMethod', {
                      required: true,
                    })}
                  />
                  <label htmlFor="upload-licence-photo">
                    UPLOAD the licence/certificate to the server.
                  </label>
                </div>
                <div>
                  <input
                    className="radio"
                    id="email-licence-photo"
                    type="radio"
                    value="email licence photo"
                    {...register('licenceVerifyMethod', {
                      required: true,
                    })}
                  />
                  <label htmlFor="email-licence-photo">
                    EMAIL the licence/certificate to info@vitaaid.com.
                  </label>
                </div>
                <div>
                  <input
                    className="radio"
                    id="fax-licence-photo"
                    type="radio"
                    value="fax licence photo"
                    {...register('licenceVerifyMethod', {
                      required: true,
                    })}
                  />
                  <label htmlFor="fax-licence-photo">
                    FAX the licence/certificate to 1-604-465-1299.
                  </label>
                </div>
                {errors.licenceVerifyMethod && (
                  <div className="file-error-msg">This field is required</div>
                )}
                {isMobile &&
                  (watchLicenceVerifyMethod === 'upload licence photo' ||
                    watchLicenceVerifyMethod === null ||
                    watchLicenceVerifyMethod === undefined ||
                    watchLicenceVerifyMethod === '') && (
                    <div
                      css={css`
                        max-width: 300px;
                      `}
                    >
                      <input
                        type="file"
                        name="licencePhoto"
                        className={
                          watchLicenceVerifyMethod === 'upload licence photo'
                            ? 'file-uploader'
                            : 'file-uploader disable'
                        }
                        accept="application/pdf,image/*"
                        required
                      />
                    </div>
                  )}
              </Fragment>
            </div>
          </div>
        )}
      </div>
    </Fragment>
  );
};
