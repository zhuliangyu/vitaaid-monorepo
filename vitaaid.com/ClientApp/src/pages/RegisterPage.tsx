/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { useForm, ValidateResult } from 'react-hook-form';
import { selectedCountry } from '../redux/features/country/countrySlice';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { RegistrationForm } from 'components/Registrion/RegistrationForm';
import ReCAPTCHA from 'react-google-recaptcha';
import { RegistrationFormData, registerAccount, validatePhysicianCode } from 'model/Registration';
import { Country, State } from 'country-state-city';
import { MessageBox } from 'components/MessageBox';
import { isMobileData } from 'redux/features/isMobileSlice';
import { LoadPanel } from 'devextreme-react/load-panel';

function onChange(value: string | null) {}

export default function RegisterPage() {
  const webSiteCountry = useSelector(selectedCountry);
  const isMobile = useSelector(isMobileData);
  const query = new URLSearchParams(useLocation().search);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const memberTypeName = query.get('type');
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
    setError,
    control,
  } = useForm<RegistrationFormData>({ mode: 'onBlur' });
  const [submittedState, setSubmittedState] = React.useState(false);
  const form = useRef(null);
  React.useEffect(() => {
    if (memberTypeName === null || memberTypeName === undefined) navigate('/');
    setValue('practitionerType', '');
    setValue('otherPractitionerType', '');
  }, []);

  const handleToken = (token: string | null) => {
    setValue('reCAPTCHAToken', token);
  };
  const handleExpire = () => {
    setValue('reCAPTCHAToken', null);
  };

  const onValidatePhysicianCode = async (physicianCode: string | null): Promise<ValidateResult> => {
    if (memberTypeName !== 'Patient') return true;
    const isValid = await validatePhysicianCode(physicianCode);
    if (isValid === true) return true;
    setError('pat_pcode', {
      type: 'validate',
      message: "Couldn't find your physician code.",
    });
    return "Couldn't find your physician code.";
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
    if (data.password !== data.confirmPassword) {
      setError('confirmPassword', {
        type: 'validate',
        message: 'Passwords do not match',
      });
      return;
    }

    if (memberTypeName === 'Patient') {
      var result = await onValidatePhysicianCode(data.pat_pcode);
      if (result?.valueOf() !== true) return;
    }
    setSubmittedState(true);
    if (memberTypeName === 'Practitioner') data.memberType = 1;
    else if (memberTypeName === 'Patient') data.memberType = 2;
    /*
    else {
      if (data.sMemberTypeName === '3') data.memberType = 3;
      else if (data.sMemberTypeName === '4') data.memberType = 4;
      else if (data.sMemberTypeName === '5') data.memberType = 5;
    }*/
    data.country = Country.getCountryByCode(data.country)?.name ?? '';
    setValue('permittedSite', webSiteCountry === 'CA' ? 1 : 2);
    const member = await registerAccount(form.current!!, memberTypeName);
    setSubmittedState(false);
    if (member && member.id > 0) {
      setVisibleMsgBox(true);
    } else {
      alert('Error with your registration please try registering again.');
    }
  };

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Registration</title>
      </Helmet>
      <div className="content-main-body">
        <div className="registration">
          <div className="row registration-row">
            <div className="col-12 title">Registration</div>
            <div className="col-12 complete-account-information">
              <div className="subtitle">Complete Account Information</div>
              <div className="info-content">
                <p>
                  For our registered healthcare practitioners, we offer online access to our
                  exclusive product information and shopping cart service to help you stay updated
                  with the latest Vita Aid product developments & make your purchasing experience
                  much easier.
                </p>
                <p>
                  If you are a healthcare practitioner and have not had the chance to become one of
                  our members, we invite you to join us by filling out the information below. The
                  registration will be processed within the next 48 hours, and upon acceptance, your
                  account information will be emailed to you.
                </p>
              </div>
            </div>
          </div>
          <form
            ref={form}
            className="row registration-form registration-row"
            onSubmit={handleSubmit(submitForm)}
          >
            <div className="col-12 registration-col-12">
              <div className="row">
                <RegistrationForm
                  key="RegistrationForm"
                  register={register}
                  handleSubmit={handleSubmit}
                  watch={watch}
                  setValue={setValue}
                  errors={errors}
                  submitted={submittedState}
                  setError={setError}
                  control={control}
                  memberTypeName={memberTypeName!!}
                  isMobile={isMobile}
                />
              </div>
              <div className="row">
                <div className="col-12  verification-block">
                  {/* <input
                    css={css`
                      display: none;
                    `}
                    id="reCAPTCHAToken"
                    {...register('reCAPTCHAToken', { required: true })}
                  /> */}
                  {/* <ReCAPTCHA
                    sitekey="6LeMx-kSAAAAAO48uHQxedudHjsDVuGGk5gW1Tn4"
                    onChange={handleToken}
                    onExpired={handleExpire}
                  />
                  {errors.reCAPTCHAToken && (
                    <div className="error-msg">
                      Before you procees to the registration, please complete the captcha above.
                    </div>
                  )} */}
                  <input
                    css={css`
                      display: none;
                    `}
                    id="permittedSite"
                    {...register('permittedSite')}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-12">
                  <input
                    className="checkbox"
                    type="checkbox"
                    id="referral"
                    name="referral"
                    defaultChecked={false}
                  />
                  <label htmlFor="referral">
                    Do you wish to be on our referral list where your clinic information will be
                    shared with patients in your region upon request ?
                  </label>
                </div>
              </div>
              <div className="row">
                <div className="col-12">
                  <input
                    className="checkbox"
                    type="checkbox"
                    id="newsletters"
                    name="newsletters"
                    defaultChecked={false}
                  />
                  <label htmlFor="newsletters">I wish to receive newsletters from VitaAid.</label>
                </div>
              </div>
              <div className="row">
                <div className="col-12">
                  <input className="checkbox" type="checkbox" id="terms" name="terms" required />
                  <label htmlFor="terms">
                    I have read the Terms & Conditions (please check the box before you can Submit).
                  </label>
                </div>
              </div>
              <div className="row ">
                <div className="col-12 button-block">
                  <button type="submit">SUBMIT</button>
                  <button
                    type="reset"
                    css={css`
                      margin-left: 8px;
                    `}
                  >
                    RESET
                  </button>
                </div>
              </div>
            </div>
          </form>
        </div>
      </div>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        visible={submittedState}
        message="Please wait ..."
      />
      <MessageBox
        Title="THANK YOU"
        Message="Your request to have merchant account was successfully sent.We will confirm your registration and inform you of the status of your account within the next 48 hours."
        Type="BACK_TO_HOME"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
}
