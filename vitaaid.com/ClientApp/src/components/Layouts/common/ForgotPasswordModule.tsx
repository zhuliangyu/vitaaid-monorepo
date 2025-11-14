/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { requestResetPassword } from 'model/Member';
import { isMobileData } from 'redux/features/isMobileSlice';
import { forgotPasswordDlg, closeForgotPasswordDlg } from 'redux/features/forgotPasswordDlgSlice';
import { LoadPanel } from 'devextreme-react/load-panel';
import { MessageBox } from 'components/MessageBox';

interface ForgotPasswordModuleleProps {
  setPopupVisibility?: (popupName: string, isVisible: boolean) => void;
}

export const ForgotPasswordModule = ({ setPopupVisibility }: ForgotPasswordModuleleProps) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isMobile = useSelector(isMobileData);
  const [forgotSubmitFinish, setForgotSubmitFinish] = React.useState<boolean>(false);
  const [afterSubmitText, setAfterSubmitText] = React.useState<string>('');
  const [emailForgotPassword, setEmailForgotPassword] = React.useState<string>('');
  const [submittedState, setSubmittedState] = React.useState(false);
  const bShowForgotPasswordDlg = useSelector(forgotPasswordDlg);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const RequestPasswordLink = async () => {
    if (emailForgotPassword && emailForgotPassword.length > 0) {
      setSubmittedState(true);
      var result = await requestResetPassword(emailForgotPassword);
      if (result === 'Ok') {
        if (isMobile) {
          setVisibleMsgBox(true);
        } else {
          setAfterSubmitText('Your request has been sent.');
        }
        setForgotSubmitFinish(true);
      } else setAfterSubmitText(result);

      setSubmittedState(false);
    }
  };
  useEffect(() => {
    setAfterSubmitText('');
    setEmailForgotPassword('');
    setForgotSubmitFinish(false);
  }, [bShowForgotPasswordDlg]);

  return (
    <React.Fragment>
      <div className="row">
        <div className="forgot-password-title">Forgot your password ?</div>
        <div className="forgot-pwd-text">
          Please enter the email address you’d like your password reset information send to
        </div>
        <input
          value={emailForgotPassword}
          className={`email-input ${submittedState ? 'input-disable' : ''}`}
          id="emailForgotPassword"
          type="email"
          placeholder="Email Address"
          onChange={(e: any) => {
            e.preventDefault(); // prevent the default action
            setEmailForgotPassword(e.target.value);
          }}
        />
        <div className="action-div">
          <div
            className={
              forgotSubmitFinish
                ? 'normal-color after-submit-text'
                : 'warning-color after-submit-text'
            }
          >
            {afterSubmitText}
          </div>
          {(forgotSubmitFinish === false || isMobile === false) && (
            <button
              className={`request-line-button ${submittedState ? 'btn-disable' : ''}`}
              onClick={() => {
                if (forgotSubmitFinish === true) {
                  if (isMobile) navigate('/');
                  else setPopupVisibility!('forgot', false);
                  dispatch(closeForgotPasswordDlg());
                } else RequestPasswordLink();
              }}
            >
              {forgotSubmitFinish === true && 'CLOSE'}
              {forgotSubmitFinish === false && 'Request reset link'}
            </button>
          )}
        </div>
      </div>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        visible={submittedState}
        message="Please wait ..."
      />
      <MessageBox
        Title="Reset Password"
        Message="Your request has been sent."
        Type="BACK_TO_HOME"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
};
