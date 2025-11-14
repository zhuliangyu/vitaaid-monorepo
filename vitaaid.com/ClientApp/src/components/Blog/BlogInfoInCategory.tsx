/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useNavigate } from 'react-router-dom';
import { BlogData, getBlog, getBlogByCategory } from 'model/Blog';
import { useSelector, useDispatch } from 'react-redux';
import { accountData } from 'redux/features/account/accountSlice';
import { urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { openLoginDlg } from 'redux/features/loginDlgSlice';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { requireLoginMessageChanged, MessageID } from 'redux/features/requireLoginMessageSlice';

interface Props {
  data: BlogData;
  showNote: () => void;
  isMobile: boolean;
}
export const BlogInfoInCategory = ({ data, showNote, isMobile }: Props) => {
  const url = `/blogarticle/${data.id}`;
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  let navigate = useNavigate();

  const BriefBlock = ({ data, showNote, isMobile }: Props) => {
    return (
      <Fragment>
        <div className="brief-topic">
          <button
            className="a-btn"
            dangerouslySetInnerHTML={{ __html: data.topic }}
            onClick={() => {
              if (!account) {
                dispatch(urlAfterLoginChanged(url));
                dispatch(memberTypeForURLAfterLoginChanged(2));
                if (isMobile) dispatch(requireLoginMessageChanged(MessageID.BLOG));
                else dispatch(openLoginDlg());
              } else if (account.memberType === 2) {
                if (showNote) showNote();
              } else {
                navigate(url);
              }
            }}
          ></button>
        </div>
        {isMobile === false && (
          <img className="content-line" alt="" src="/img/blog-brief-content-line.png" />
        )}
        <div className="issue-vol-no-block">
          <span>{`Issue : ${data.issue} | Vol. ${data.volume} | No. ${data.no}`}</span>
          {isMobile === false && data.author && (
            <span key={`blogCategory-${data.id}`}>
              &nbsp;&nbsp;&nbsp;&nbsp;{`Author : ${data.author}`}
            </span>
          )}
        </div>
      </Fragment>
    );
  };

  return (
    <Fragment>
      {isMobile && (
        <div className="blog-brief-block">
          {data.thumb && data.thumb.length > 0 && (
            <div className="thumb-img-div">
              <div className="overlay-m" />
              <img
                key={data.id}
                className="thumb-img"
                alt={`${data.thumb}`}
                src={`${process.env.REACT_APP_BLOG_DIR!}${data.thumb}`}
              ></img>
            </div>
          )}
          <div className="brief-content">
            <BriefBlock data={data} showNote={showNote} isMobile={isMobile} />
          </div>
        </div>
      )}
      {isMobile === false && (
        <tr className="blog-brief-block">
          <td className="td-thumb-img">
            {data.thumb && data.thumb.length > 0 && (
              <div className="thumb-img-div">
                <img
                  key={data.id}
                  className="thumb-img"
                  alt={`${data.thumb}`}
                  src={`${process.env.REACT_APP_BLOG_DIR!}${data.thumb}`}
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
