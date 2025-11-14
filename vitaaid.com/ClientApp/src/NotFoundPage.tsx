/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';

interface Props {
  redirect?: boolean;
}
export const NotFoundPage = ({ redirect = false }: Props) => {
  const navigate = useNavigate();

  let timeout: NodeJS.Timeout;

  React.useEffect(() => {
    if (timeout != null) clearTimeout(timeout);
    if (redirect && redirect === true) navigate('/');
    else {
      timeout = setTimeout(async () => {
        try {
          navigate('/');
        } catch (e) {
          if (timeout != null) clearTimeout(timeout);
        }
      }, 10000);
    }
  }, []);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Not Found Page</title>
      </Helmet>
      <div className="content-main-body">
        <span
          css={css`
            font-size: 20px;
            font-weight: 600;
          `}
        >
          Page not found
        </span>
      </div>
    </React.Fragment>
  );
};
