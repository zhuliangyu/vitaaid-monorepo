/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { Country, State } from 'country-state-city';
import {
  MemberData,
  updateMember,
  ResetPasswordFormData,
  resetPassword,
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

export const ResetPasswordPage = () => {
  const navigate = useNavigate();
  const [visibleMsgBox, setVisibleMsgBox] = useState<boolean>(false);
  const [message, setMessage] = useState<string>('Your password has been reset successfully.');
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
  const useQuery = () => new URLSearchParams(useLocation().search);
  let query = useQuery();
  const token = query.get('token');

  React.useEffect(() => {
    if (token && token.length > 0) {
    } else navigate('/');
    async function fetchData() {
      const data = await checkToken(token!);
      if (data == false) {
        setMessage('Your password reset link has expired.');
        setVisibleMsgBox(true);
      }
    }
    fetchData();
  }, []);

  const submitForm = async (data: any) => {
    if (data.newPassword !== data.confirmPassword) {
      setError('confirmPassword', {
        type: 'validate',
        message: 'Passwords do not match',
      });
      return;
    }
    setSubmitted(true);
    var result = await resetPassword(data.email, token!, form.current!!);
    setSubmitted(false);
    if (result.includes('expired')) {
      setMessage('Your password reset link has expired.');
      setVisibleMsgBox(true);
    } else if (result !== 'Ok') {
      setError('email', {
        type: 'validate',
        message: result,
      });
    } else setVisibleMsgBox(true);
  };

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Medical Consultancy Team</title>
      </Helmet>
      <div className="content-main-body reset-password">
        <div className="row">
          <div className="col-12 header-block">Change Password</div>
        </div>

        <form ref={form} className="row" onSubmit={handleSubmit(submitForm)}>
          <div className="col-12 reset-password-form">
            <div className="row">
              <div className="col-12">
                <div className="colspan2 g-input">
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
                    })}
                  />
                  <label htmlFor="email">Account Email*</label>
                  {errors.email && (
                    <div className="error-msg">
                      {errors.email.type === 'required'
                        ? 'This field is required'
                        : errors.email.message}
                    </div>
                  )}
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-12">
                <div className="g-input">
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
                <div className="g-input">
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
