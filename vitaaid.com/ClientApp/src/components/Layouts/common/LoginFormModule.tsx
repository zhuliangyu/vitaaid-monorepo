/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import { LoadPanel } from 'devextreme-react/load-panel';
import {
  selectedCountry,
  IsEqualSite,
  ChangeCountrySite,
} from 'redux/features/country/countrySlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { accountChanged } from 'redux/features/account/accountSlice';
import { ulogin, doRefreshToken, saveTokenToSession } from 'model/JwtToken';
import { closeLoginDlg } from 'redux/features/loginDlgSlice';
import { urlAfterLogin } from 'redux/features/urlAfterLoginSlice';
import {
  shopping_cart_oauth,
  doRefreshShoppingCartToken,
  saveShoppingCartTokenToSession,
} from 'model/ShoppingCartToken';
import { openForgotPasswordDlg } from 'redux/features/forgotPasswordDlgSlice';

type LoginInfoFormData = {
  email: string;
  password: string;
  site: string;
};
type RegisterTypeData = {
  sMembertype: string;
};

interface LoginFormModuleProps {
  setLoginPopover?: (isVisible: boolean) => void;
  setPopupVisibility?: (popupName: string, isVisible: boolean) => void;
}
export const LoginFormModule = ({ setLoginPopover, setPopupVisibility }: LoginFormModuleProps) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isMobile = useSelector(isMobileData);
  const [loginFail, setLoginFail] = useState<boolean>(false);
  const [submittedState, setSubmittedState] = React.useState(false);
  const country = useSelector(selectedCountry);
  const { register: registerTypeForm, handleSubmit: handleRegisterTypeSubmit } =
    useForm<RegisterTypeData>({ mode: 'onBlur' });

  const {
    register: loginForm,
    handleSubmit: handleLoginSubmit,
    reset,
    formState: { errors: errorsForLogin },
  } = useForm<LoginInfoFormData>({ mode: 'onBlur' });

  const urlAfterLoginData = useSelector(urlAfterLogin);

  const submitLoginForm = async (data: LoginInfoFormData) => {
    if (data.email !== '' && data.password !== '') {
      //???setSubmittedState(true);
      const jwtToken = await ulogin(data.email, data.password, country);
      if (Object.keys(jwtToken).length === 0) {
        setLoginFail(true);
      } else {
        if (IsEqualSite(jwtToken.member!.permittedSite, country) === false) {
          ChangeCountrySite(dispatch, jwtToken.member!.permittedSite);
          if (window.location.pathname.startsWith('/products') === true) {
            if (window.location.search.startsWith('?pcode')) {
              reset();
              //navigate(window.location.pathname + window.location.search);
              window.location.reload();
            } else navigate(window.location.pathname);
          }
        }
        if (isMobile === false) {
          reset();
          setLoginPopover!(false);
        }

        const memberData = jwtToken.member!;

        saveTokenToSession(jwtToken);
        doRefreshToken(jwtToken, false);

        if (jwtToken.access_token != null) {
          //const memberData = await getMember(data.email);
          dispatch(accountChanged(memberData));
          dispatch(closeLoginDlg());
          // get shopping cart token
          if (memberData.customerCode) {
            const shoppingCartToken = await shopping_cart_oauth(memberData.customerCode, country);
            if (Object.keys(shoppingCartToken).length > 0) {
              saveShoppingCartTokenToSession(shoppingCartToken);
              doRefreshShoppingCartToken(shoppingCartToken, false);
            }
          }
        } else {
          reset();
          dispatch(closeLoginDlg());
        }
        if (isMobile && urlAfterLoginData === '') {
          reset();
          navigate(-1);
        }
      }
      //???setSubmittedState(false);
    } else {
      setLoginFail(true);
    }
  };
  const submitRegisterType = (data: RegisterTypeData) => {
    if (setLoginPopover) setLoginPopover!(false);
    dispatch(closeLoginDlg());
    navigate('/register?type=' + data.sMembertype);
  };
  return (
    <div className="row">
      <form className="col-12 col-md-6 login-block" onSubmit={handleLoginSubmit(submitLoginForm)}>
        <div>
          <div className="login-title">Patient or Practitioner Log In</div>
          <input
            className={`login-email-input ${submittedState ? 'input-disable' : ''}`}
            id="email"
            type="email"
            autoComplete="username"
            defaultValue=""
            {...loginForm('email', {
              required: true,
            })}
            placeholder="Email"
          />
          {errorsForLogin.email && (
            <div key="err-email" className="error-msg">
              This field is required
            </div>
          )}
          <input
            className={`login-input ${submittedState ? 'input-disable' : ''}`}
            id="password"
            type="password"
            autoComplete="current-password"
            defaultValue=""
            {...loginForm('password', {
              required: true,
            })}
            placeholder="Password"
          />
          <div className={loginFail ? 'login-fail' : 'login-fail-hide'}>
            Incorrect username or password.
          </div>
          <div>
            <button
              className={`${submittedState ? 'btn-disable login-button' : 'login-button'}`}
              type="submit"
            >
              LOG IN
            </button>
          </div>
          <div>
            <button
              className={`borderless-btn ${
                submittedState ? 'btn-disable forgot-password-button' : 'forgot-password-button'
              }`}
              onClick={() => {
                if (isMobile) {
                  navigate('/forgotpassword');
                } else {
                  dispatch(openForgotPasswordDlg());
                  if (setPopupVisibility) setPopupVisibility!('forgot', true);
                }
              }}
            >
              <span
                css={css`
                  text-decoration: underline;
                `}
              >
                Forgot your password?
              </span>
            </button>
          </div>
        </div>
      </form>
      <form
        className="col-12 col-md-6 register-block"
        onSubmit={handleRegisterTypeSubmit(submitRegisterType)}
      >
        <div className="registration-title">Create an Account</div>
        <div className="registration-text">
          Visitors must be registered with Vita Aid to have the access to the online order system.
        </div>
        <select
          className="register-member-type"
          id="sMembertype"
          placeholder="Select Member Type"
          defaultValue=""
          {...registerTypeForm('sMembertype', {
            required: true,
          })}
        >
          <option value="" disabled hidden>
            Select member type
          </option>
          <option value="Practitioner">Practitioner</option>
          <option value="Patient">Patient</option>
          <option value="MedicalStudent">Medical Student</option>
        </select>
        <div>
          <button className="registration-button" type="submit">
            REGISTRATION
          </button>
        </div>
      </form>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        visible={submittedState}
        message="Please wait ..."
      />
    </div>
  );
};
