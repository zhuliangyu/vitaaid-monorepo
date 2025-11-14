/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { useForm } from 'react-hook-form';

import { Helmet } from 'react-helmet-async';
import { Header } from '../components/Layouts/Header';
import { Footer } from '../components/Layouts/Footer';
import { JoinUsFormData, addToMailingList } from 'model/Registration';
import { MessageBox } from 'components/MessageBox';
import { LoadPanel } from 'devextreme-react/load-panel';

export default function JoinOurMailingListPage() {
  const [submittedState, setSubmittedState] = React.useState(false);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const {
    register: joinUsForm,
    handleSubmit: handleJoinUsSubmit,
    watch,
    formState: { errors: errorsForJoinUs },
  } = useForm<JoinUsFormData>({ mode: 'onBlur' });
  const form = useRef(null);

  const submitJoinUsForm = async (data: JoinUsFormData) => {
    if (
      data.profession !== '' &&
      data.firstName !== '' &&
      data.lastName !== '' &&
      data.email !== ''
    ) {
      setSubmittedState(true);
      await addToMailingList(form.current!!);
      setSubmittedState(false);
      setVisibleMsgBox(true);
    }
  };
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Join Our Mailing List</title>
      </Helmet>
      <div className="content-main-body">
        <div className="join-us">
          <div className="row">
            <div className="col-12">
              <div className="header-block">Join Our Mailing List</div>
              <div className="desc">
                Receive monthly clinical pearls, featured products, and more.
              </div>
            </div>
          </div>
          <div className="row">
            <form
              ref={form}
              className="col-12 join-us-form"
              onSubmit={handleJoinUsSubmit(submitJoinUsForm)}
            >
              <select
                className="profession-type"
                id="sProfessiontype"
                placeholder="Practitioner or Patient"
                defaultValue=""
                {...joinUsForm('profession', {
                  required: true,
                })}
              >
                <option value="" disabled hidden>
                  Practitioner or Patient
                </option>
                <option value="Practitioner">Practitioner</option>
                <option value="Patient">Patient</option>
              </select>
              <div className="line-2">
                <div className="first-col">
                  <input
                    className={`join-us-input ${submittedState ? 'input-disable' : ''}`}
                    id="firstname"
                    type="text"
                    {...joinUsForm('firstName', {
                      required: true,
                    })}
                    placeholder="First Name"
                  />
                  {errorsForJoinUs.firstName && (
                    <div key="err-firstName" className="error-msg">
                      This field is required
                    </div>
                  )}
                </div>
                <div className="second-col ">
                  <input
                    className={`join-us-input ${submittedState ? 'input-disable' : ''}`}
                    id="lastname"
                    type="text"
                    {...joinUsForm('lastName', {
                      required: true,
                    })}
                    placeholder="Last Name"
                  />
                  {errorsForJoinUs.lastName && (
                    <div key="err-lastName" className="error-msg">
                      This field is required
                    </div>
                  )}
                </div>
              </div>
              <div>
                <input
                  className={`join-us-input full-col ${submittedState ? 'input-disable' : ''}`}
                  id="email"
                  type="text"
                  {...joinUsForm('email', {
                    required: true,
                  })}
                  placeholder="Email Address"
                />
                {errorsForJoinUs.email && (
                  <div key="err-email" className="error-msg">
                    This field is required
                  </div>
                )}
              </div>
              <div className="line-3">
                <button className={`${submittedState ? 'btn-disable submit-btn' : 'submit-btn'}`}>
                  SUBMIT
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
      <LoadPanel
        shadingColor="rgba(0,0,0,0.4)"
        visible={submittedState}
        message="Please wait ..."
      />
      <MessageBox
        Title="THANK YOU"
        Message="The form was submitted successfully."
        Type="BACK_TO_HOME"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
}
