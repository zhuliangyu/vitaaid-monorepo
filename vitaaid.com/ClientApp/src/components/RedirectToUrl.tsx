/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';

interface Props {
  loc: string;
}
export const RedirectToUrl = ({ loc }: Props) => {
  React.useEffect(() => {
    window.location.href = loc;
  }, []);

  return (
    <section>
      <img
        className="redirect-img"
        alt="vitaaid"
        css={css`
          max-width: 50%;
        `}
      />
    </section>
  );
};
