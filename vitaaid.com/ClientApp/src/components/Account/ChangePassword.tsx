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
import {
  MemberData,
  updateMember,
  ResetPasswordFormData,
  resetPassword,
  changePassword,
  checkToken,
} from 'model/Member';
import { accountChanged, accountData } from 'redux/features/account/accountSlice';
import { validateEmail, validatePhysicianCode } from 'model/Registration';
import { eUNITTYPE, getUnitTypes, UnitTypeData } from 'model/UnitType';
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
import { useState } from 'react';
import { LoadPanel } from 'devextreme-react/load-panel';
export const ChangePassword = () => {
  const navigate = useNavigate();
  const [visibleMsgBox, setVisibleMsgBox] = useState<boolean>(false);
  const [message, setMessage] = useState<string>('Your password has been changed successfully.');
  const account = useSelector(accountData);
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
    setError,
    control,
  } = useForm<ResetPasswordFormData>({ mode: 'onBlur' });
  const [submitted, setSubmitted] = React.useState(false);
  const form = useRef(null);

  const submitForm = async (data: any) => {
    if (data.newPassword !== data.confirmPassword) {
      setError('confirmPassword', {
        type: 'validate',
        message: 'Passwords do not match',
      });
      return;
    }
    setSubmitted(true);
    var result = await changePassword(account.email, form.current!!);
    setSubmitted(false);
    if (result !== 'Ok') {
      setError('token', {
        type: 'validate',
        message: result,
      });
    } else setVisibleMsgBox(true);
  };

  return (
    <React.Fragment>
      <div className="content-main-body reset-password">
        <div className="row">
          <div className="col-12 header-block">Change Password</div>
        </div>

        <form ref={form} className="row" onSubmit={handleSubmit(submitForm)}>
          <div className="col-12 reset-password-form">
            <div className="row">
              <div className="col-12">
                <div
                  className="g-input"
                  css={css`
                    width: 100%;
                  `}
                >
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="token"
                    type="password"
                    placeholder=" "
                    {...register('token', {
                      required: true,
                    })}
                  />
                  <label htmlFor="token">Old Password*</label>
                  {errors.token && (
                    <div className="error-msg">
                      {errors.token.type === 'required'
                        ? 'This field is required'
                        : errors.token.message}
                    </div>
                  )}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div
                  className="g-input"
                  css={css`
                    width: 100%;
                  `}
                >
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="newpassword"
                    type="password"
                    placeholder=" "
                    {...register('newPassword', {
                      required: true,
                    })}
                  />
                  <label htmlFor="newpassword">Create New Password*</label>
                  {errors.newPassword && (
                    <div className="error-msg">
                      {errors.newPassword.type === 'required'
                        ? 'This field is required'
                        : errors.newPassword.message}
                    </div>
                  )}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div
                  className="g-input"
                  css={css`
                    width: 100%;
                  `}
                >
                  <input
                    className={`col-input ${submitted === true ? 'input-disable' : ''}`}
                    id="confirmpassword"
                    type="password"
                    placeholder=" "
                    {...register('confirmPassword', {
                      required: true,
                    })}
                  />
                  <label htmlFor="confirmpassword">Confirm New Password*</label>
                  {errors.confirmPassword && (
                    <div className="error-msg">
                      {errors.confirmPassword.type === 'required'
                        ? 'This field is required'
                        : errors.confirmPassword.message}
                    </div>
                  )}
                </div>
              </div>
            </div>
            <div className="row">
              <div className={`col-12 button-block ${submitted === true ? 'btn-disable' : ''}`}>
                <button type="submit">SUBMIT</button>
              </div>
            </div>
          </div>
        </form>
      </div>
      <LoadPanel shadingColor="rgba(0,0,0,0.4)" visible={submitted} message="Please wait ..." />
      <MessageBox
        Title="RESET PASSWORD"
        Message={message}
        Type="BACK_TO_HOME"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
};
