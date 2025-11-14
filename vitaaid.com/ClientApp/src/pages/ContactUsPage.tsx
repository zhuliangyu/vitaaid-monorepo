/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { useForm } from 'react-hook-form';

import { Helmet } from 'react-helmet-async';
import { ContactUsFormData, contactUs } from 'model/Registration';
import { MessageBox } from 'components/MessageBox';
import { LoadPanel } from 'devextreme-react/load-panel';

export default function ContactUsPage() {
  const [submittedState, setSubmittedState] = React.useState(false);
  const [visibleMsgBox, setVisibleMsgBox] = React.useState(false);
  const {
    register: contactUsForm,
    handleSubmit: handleContactUsSubmit,
    watch,
    formState: { errors: errorsContactUs },
  } = useForm<ContactUsFormData>({ mode: 'onBlur' });
  const form = useRef(null);

  const submitContactUsForm = async (data: ContactUsFormData) => {
    if (data.firstName !== '' && data.lastName !== '' && data.email !== '') {
      setSubmittedState(true);
      await contactUs(form.current!!);
      setSubmittedState(false);
      setVisibleMsgBox(true);
    }
  };
  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Contact Us</title>
      </Helmet>
      <div className="content-main-body">
        <div className="contact-us">
          <div className="row">
            <div className="col-12">
              <div className="header-block">Contact Us</div>
              <div className="detail-block">
                <div className="left-block">
                  <div className="preface">
                    Healthcare professionals are more than welcome to contact us. We appreciate your
                    insightful comments and suggestions for they will help us grow and provide
                    products with the best therapeutic values. We can be reached through email, fax
                    or by filling out our on-line contact form.
                  </div>
                  <div
                    className="row"
                    css={css`
                      margin-left: 0px !important;
                      margin-right: 0px !important;
                    `}
                  >
                    <form
                      ref={form}
                      className="col-12 contact-us-form"
                      onSubmit={handleContactUsSubmit(submitContactUsForm)}
                    >
                      <select
                        className="prefix-type"
                        id="sPrefix"
                        placeholder="Prefix"
                        defaultValue=""
                        {...contactUsForm('prefix', {
                          required: true,
                        })}
                      >
                        <option value="" disabled hidden>
                          Select prefix
                        </option>
                        <option value="Dr.">Dr.</option>
                        <option value="Mr.">Mr.</option>
                        <option value="Ms.">Ms.</option>
                      </select>
                      <div className="line-2">
                        <div className="first-col">
                          <input
                            className={`contact-us-input ${submittedState ? 'input-disable' : ''}`}
                            id="firstname"
                            type="text"
                            {...contactUsForm('firstName', {
                              required: true,
                            })}
                            placeholder="First Name*"
                          />
                          {errorsContactUs.firstName && (
                            <div key="err-firstName" className="error-msg">
                              This field is required
                            </div>
                          )}
                        </div>
                        <div className="second-col ">
                          <input
                            className={`contact-us-input ${submittedState ? 'input-disable' : ''}`}
                            id="lastname"
                            type="text"
                            {...contactUsForm('lastName', {
                              required: true,
                            })}
                            placeholder="Last Name*"
                          />
                          {errorsContactUs.lastName && (
                            <div key="err-lastName" className="error-msg">
                              This field is required
                            </div>
                          )}
                        </div>
                      </div>
                      <div>
                        <input
                          className={`contact-us-input full-col ${
                            submittedState ? 'input-disable' : ''
                          }`}
                          id="phone"
                          type="text"
                          {...contactUsForm('phone', {
                            required: false,
                          })}
                          placeholder="Phone Number"
                        />
                      </div>
                      <div>
                        <input
                          className={`contact-us-input full-col ${
                            submittedState ? 'input-disable' : ''
                          }`}
                          id="email"
                          type="email"
                          {...contactUsForm('email', {
                            required: true,
                          })}
                          placeholder="Email Address*"
                        />
                        {errorsContactUs.email && (
                          <div key="err-email" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div>
                      <div>
                        <textarea
                          className={`contact-us-input full-col question ${
                            submittedState ? 'input-disable' : ''
                          }`}
                          id="content"
                          {...contactUsForm('content', {
                            required: true,
                          })}
                          placeholder="Content"
                        />
                      </div>
                      <div className="line-3">
                        <button
                          className={`${submittedState ? 'btn-disable submit-btn' : 'submit-btn'}`}
                        >
                          SEND
                        </button>
                      </div>
                    </form>
                  </div>
                </div>
                <div className="right-block">
                  <div className="title">Contact Info</div>
                  <div>Address: #303-20285 Stewart Crescent, Maple Ridge, BC, Canada V2X 8G1</div>
                  <div>Tel (Canada &amp; US) : 1-800-490-1738</div>
                  <div>Tel (Other) : 1-604-260-0696</div>
                  <div>Fax : 1-604-465-1299</div>
                  <div>Email : info&#64;vitaaid.com</div>
                  <br />
                  <div className="title">Office Hours </div>
                  <div>Mondays to Fridays 9:00am to 5:00pm (PST).</div>
                </div>
              </div>
            </div>
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
        Message="Your inguiry has been sent."
        Type="BACK_TO_HOME"
        IsVisible={visibleMsgBox}
        onVisibleChange={() => setVisibleMsgBox(false)}
      />
    </React.Fragment>
  );
}
