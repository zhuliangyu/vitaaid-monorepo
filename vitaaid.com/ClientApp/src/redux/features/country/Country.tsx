/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { ca, usa, caAsync, usaAsync, selectedCountry } from './countrySlice';
import { Popover } from 'devextreme-react/popover';

interface Props {
  onClicked?: (newValue: string) => void;
}

export function InitialCountryByIP(dispatch: any) {
  try {
    const ipcountry = sessionStorage.getItem('ipcountry');
    if (ipcountry) {
      if (ipcountry === 'CA') dispatch(ca());
      else dispatch(usa());
    } else {
      fetch(
        'https://api.ipdata.co?api-key=9a37c0bb85e4688d112496acef38e84ba558d2af6aae800ad6426248',
      ).then((response) => {
        try {
          if (response.status < 400 || response.status >= 600)
            response.json().then((parsed) => {
              sessionStorage.setItem('ipcountry', parsed.country_code);
              if (parsed.country_code === 'CA') dispatch(ca());
              else dispatch(usa());
            });
        } catch (e) {}
      });
    }
  } catch (e) {}
}

export const Country = ({ onClicked }: Props) => {
  const country = useSelector(selectedCountry);
  const dispatch = useDispatch();
  const [showPopover, setShowPopover] = useState<boolean>(false);

  return (
    <div className="CountrySelector">
      <div>
        {country === 'CA' && (
          <React.Fragment>
            <img
              className="flag"
              alt="canada"
              src="/img/canada.png"
              srcSet="/img/canada@2x.png 2x, /img/canada@3x.png 3x"
            />
            <span className="countryname v-center-helper">CANADA</span>
            <button
              id="popTarget"
              className="borderless-btn open-country-menu"
              onClick={() => {
                setShowPopover(!showPopover);
              }}
            >
              <img src="/img/openbutton-w.png" alt="" />
            </button>
          </React.Fragment>
        )}
        {country === 'US' && (
          <React.Fragment>
            <img
              className="flag"
              alt="usa"
              src="/img/usa.png"
              srcSet="/img/usa@2x.png 2x, /img/usa@3x.png 3x"
            />
            <span className="countryname v-center-helper">USA</span>
            <button
              id="popTarget"
              className="borderless-btn open-country-menu"
              onClick={() => {
                setShowPopover(!showPopover);
              }}
            >
              <img src="/img/openbutton-w.png" alt="" />
            </button>
          </React.Fragment>
        )}
      </div>
      <Popover
        position={{ my: 'right top', at: 'right bottom', of: '#popTarget' }}
        visible={showPopover}
      >
        <div
          className="CountrySelector"
          css={css`
            cursor: pointer;
          `}
          onClick={(e: any) => {
            setShowPopover(false);
            if (country === 'CA') {
              dispatch(usa());
              if (onClicked) onClicked('US');
            }
            //usaAsync(dispatch);
            else {
              dispatch(ca()); //caAsync();
              if (onClicked) onClicked('CA');
            }
          }}
        >
          {country === 'CA' && (
            <React.Fragment>
              <img
                className="flag"
                alt="usa"
                src="/img/usa.png"
                srcSet="/img/usa@2x.png 2x, /img/usa@3x.png 3x"
              />
              <span className="countryname v-center-helper">USA</span>
            </React.Fragment>
          )}
          {country === 'US' && (
            <React.Fragment>
              <img
                className="flag"
                alt="canada"
                src="/img/canada.png"
                srcSet="/img/canada@2x.png 2x, /img/canada@3x.png 3x"
              />
              <span className="countryname v-center-helper">CANADA</span>
            </React.Fragment>
          )}
        </div>
      </Popover>
    </div>
  );
};
