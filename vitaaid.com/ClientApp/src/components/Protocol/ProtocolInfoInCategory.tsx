/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { accountData } from 'redux/features/account/accountSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { useNavigate } from 'react-router-dom';

import { ProtocolData } from 'model/Protocol';
import { protocolIDChanged } from '../../redux/features/protocolIDSlice';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { requireLoginMessageChanged, MessageID } from 'redux/features/requireLoginMessageSlice';

interface Props {
  data: ProtocolData;
  showNote: () => void;
  isMobile: boolean;
}
export const ProtocolInfoInCategory = ({ data, showNote, isMobile }: Props) => {
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  let navigate = useNavigate();

  function DoClick(id: number) {
    if (!account) {
      dispatch(urlAfterLoginChanged(`/protocols?id=${id}`));
      dispatch(memberTypeForURLAfterLoginChanged(2));
      if (isMobile) dispatch(requireLoginMessageChanged(MessageID.TP));
      else dispatch(openLoginDlg());
    } else if (account.memberType === 2) {
      if (showNote) showNote();
    } else {
      dispatch(protocolIDChanged(id));
    }
  }

  const BriefBlock = ({ data, showNote, isMobile }: Props) => {
    return (
      <Fragment>
        <div className="brief-topic">
          <button
            css={css`
              text-align: left;
            `}
            className="a-btn"
            dangerouslySetInnerHTML={{ __html: data.topic }}
            onClick={() => DoClick(data.id)}
          ></button>
        </div>
        <div className="seperate-block">
          <img className="content-line" alt="" src="/img/protocol-brief-content-line.png" />
        </div>
        <div className="info-header">
          <span>Author(s) : {`${data.author}`}</span>
          <br></br>
          <span>Date : {`${data.issue}`}</span>
        </div>
        <div className="info">
          <div dangerouslySetInnerHTML={{ __html: data.blogContent }} />
          <button
            css={css`
              text-align: left;
            `}
            className="a-btn"
            onClick={() => DoClick(data.id)}
          >
            <span className="read-more">...READ MORE</span>
          </button>
        </div>
      </Fragment>
    );
  };
  return (
    <Fragment>
      {isMobile && (
        <div className="protocol-brief-block">
          {data.banner && data.banner.length > 0 && (
            <div className="banner-img-div">
              <div className="overlay-m" />
              <img
                key={data.id}
                className="banner-img"
                alt={`${data.banner}`}
                src={`${process.env.REACT_APP_PROTOCOL_DIR!}${data.banner}`}
              ></img>
            </div>
          )}
          <div className="brief-content">
            <BriefBlock data={data} showNote={showNote} isMobile={isMobile} />
          </div>
        </div>
      )}
      {isMobile === false && (
        <tr className="protocol-brief-block">
          <td className="td-banner-img">
            {data.banner && data.banner.length > 0 && (
              <div className="banner-img-div">
                <img
                  key={data.id}
                  className="banner-img"
                  alt={`${data.banner}`}
                  src={`${process.env.REACT_APP_PROTOCOL_DIR!}${data.banner}`}
                ></img>
              </div>
            )}
          </td>
          <td className="brief-content">
            <BriefBlock data={data} showNote={showNote} isMobile={isMobile} />
          </td>
        </tr>
      )}
    </Fragment>
  );
};
