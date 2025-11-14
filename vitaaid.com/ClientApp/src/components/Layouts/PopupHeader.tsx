/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import { Fragment } from 'react';
interface Props {
  pageName?: string;
  country?: string;
  account?: string;
}

export const PopupHeader = ({ country, account }: Props) => {
  return (
    <Fragment>
      <div className="row popup-top1Header">
        <div className="col-12 text-right "></div>
      </div>
      <div className="row">
        <div className="col-3 clear-col-padding">
          <img className="popup-logo" alt="logo"></img>
        </div>
        <div className="col-1"></div>
        <div className="col-8 text-right"></div>
      </div>
    </Fragment>
  );
};
