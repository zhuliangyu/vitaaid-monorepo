/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { accountData } from 'redux/features/account/accountSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { useNavigate } from 'react-router-dom';

import { WebinarData } from 'model/Webinar';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { requireLoginMessageChanged, MessageID } from 'redux/features/requireLoginMessageSlice';

interface Props {
  data: WebinarData;
  showNote: () => void;
  isMobile: boolean;
}
export const WebinarInfoInCategory = ({ data, showNote, isMobile }: Props) => {
  const url = `/webinardetail/${data.id}`;
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const urlAfterLoginData = useSelector(urlAfterLogin);
  let navigate = useNavigate();

  return (
    <tr className="webinar-brief-block">
      <td className="brief-content">
        <div className="brief-topic">
          <button
            css={css`
              text-align: left;
            `}
            className="a-btn"
            dangerouslySetInnerHTML={{ __html: data.topic }}
            onClick={() => {
              if (!account) {
                dispatch(urlAfterLoginChanged(url));
                dispatch(memberTypeForURLAfterLoginChanged(2));
                if (isMobile) dispatch(requireLoginMessageChanged(MessageID.WEBINARS));
                else dispatch(openLoginDlg());
              } else if (account.memberType === 2) {
                if (showNote) showNote();
              } else {
                navigate(url);
              }
            }}
          ></button>
        </div>
        <img className="content-line" alt="" src="/img/blog-brief-content-line.png" />
        <div className="info">
          <span>Speaker : {`${data.author}`}</span>
          <br></br>
          <span>Broadcast Date : {`${data.issue}`}</span>
        </div>
        {/* <div className="issue-vol-no-block">
          <span>{`Issue : ${data.issue} | Vol. ${data.volume} | No. ${data.no}`}</span>
        </div> */}
      </td>
      <td className="td-thumb-img">
        {data.thumbnail && data.thumbnail.length > 0 && (
          <img
            key={data.id}
            className="thumb-img"
            alt={`${data.thumbnail}`}
            src={`${process.env.REACT_APP_WEBINAR_DIR!}${data.thumbnail}`}
          ></img>
        )}
      </td>
    </tr>
  );
};
