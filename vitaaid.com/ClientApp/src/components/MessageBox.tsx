/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Popover } from 'devextreme-react/popover';
import { useNavigate } from 'react-router-dom';

interface Props {
  Title: string;
  Message: string;
  Type: string;
  IsVisible: boolean;
  onVisibleChange: () => void;
  BeforeAction?: () => void;
  children?: React.ReactNode;
}

export const MessageBox = ({
  Title,
  Message,
  Type,
  IsVisible,
  onVisibleChange,
  BeforeAction,
  children,
}: Props) => {
  let navigate = useNavigate();
  return (
    <Popover
      position={{
        my: 'center',
        at: 'center',
        of: window,
      }}
      shading={true}
      shadingColor="rgba(0, 0, 0, 0.5)"
      visible={IsVisible}
      showCloseButton={Type === 'BACK_TO_HOME' ? false : true}
      closeOnOutsideClick={Type === 'BACK_TO_HOME' ? false : true}
      onHiding={() => {
        onVisibleChange();
      }}
      className="messagebox"
    >
      {(Type === 'INFO' || Type === 'LOGIN') && (
        <button
          className="img-btn close-img-btn"
          onClick={() => {
            onVisibleChange();
          }}
        >
          <img
            className="close-img"
            alt="close"
            src="/img/x-object.png"
            srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
          />
        </button>
      )}
      <div className="title">
        {Type === 'BACK_TO_HOME' && <img className="icon" alt="" src="/img/messagebox-icon.png" />}
        {Title}
      </div>
      <div className="message">{Message}</div>
      <div className="message">{children}</div>
      <div className="action">
        {Type === 'BACK_TO_HOME' && (
          <button
            className="a-btn borderless-btn back-to-home"
            onClick={() => {
              if (BeforeAction) BeforeAction();
              onVisibleChange();
              navigate('/');
            }}
          >
            BACK TO HOME
          </button>
        )}
        {Type === 'LOGIN' && (
          <button
            className="a-btn borderless-btn back-to-home"
            onClick={() => {
              if (BeforeAction) BeforeAction();
              onVisibleChange();
              navigate('/login');
            }}
          >
            Log in
          </button>
        )}
      </div>
    </Popover>
  );
};
