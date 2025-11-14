/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { ca, usa, selectedCountry } from 'redux/features/country/countrySlice';

interface Props {
  onClicked?: (newValue: string) => void;
}
export const Country = ({ onClicked }: Props) => {
  const country = useSelector(selectedCountry);
  const dispatch = useDispatch();

  return (
    <div className="CountrySelector-m">
      <div
        onClick={(e: any) => {
          if (country === 'CA') {
            dispatch(usa());
          } else {
            dispatch(ca()); //caAsync();
          }
        }}
      >
        {country === 'CA' && (
          <React.Fragment>
            <img
              className="flag"
              alt="canada"
              src="/img/canada-m.png"
              srcSet="/img/canada-m@2x.png 2x, /img/canada-m@3x.png 3x"
            />
            <span className="countryname v-center-helper">CANADA</span>
            <img
              className="open-country-menu"
              alt="open"
              src="/img/next-idx.png"
              srcSet="/img/next-idx@2x.png 2x, /img/next-idx@3x.png 3x"
            />
          </React.Fragment>
        )}
        {country === 'US' && (
          <React.Fragment>
            <img
              className="flag"
              alt="usa"
              src="/img/usa-m.png"
              srcSet="/img/usa-m@2x.png 2x, /img/usa-m@3x.png 3x"
            />
            <span className="countryname v-center-helper">USA</span>
            <img
              className="open-country-menu"
              alt="open"
              src="/img/next-idx.png"
              srcSet="/img/next-idx@2x.png 2x, /img/next-idx@3x.png 3x"
            />
          </React.Fragment>
        )}
      </div>
    </div>
  );
};
