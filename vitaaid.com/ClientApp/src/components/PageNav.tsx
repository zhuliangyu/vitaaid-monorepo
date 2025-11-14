/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';

interface Props {
  isMobile: boolean;
  totalPages: number;
  currentPage: number;
  currentPageChanged: (value: number) => void;
}

export const PageNav = ({ isMobile, totalPages, currentPage, currentPageChanged }: Props) => {
  const navSize = isMobile ? 3 : 8;
  const [startPageOfNav, setStartPageOfNav] = React.useState<number>(1);
  return (
    <div className="row">
      <div className="col-12 page-navigate">
        {totalPages > 1 && (
          <Fragment>
            {isMobile === false && (
              <button
                className="borderless-btn fist-last-idx"
                onClick={() => {
                  setStartPageOfNav(1);
                  currentPageChanged(1);
                }}
              >
                <img
                  src="/img/first-idx.png"
                  srcSet="/img/first-idx@2x.png 2x, /img/first-idx@3x.png 3x"
                  alt=""
                />
              </button>
            )}
            <button
              className="borderless-btn fist-last-idx"
              onClick={() => {
                const newCurrPage = currentPage - 1;
                if (newCurrPage < 1) return;
                if (newCurrPage < startPageOfNav) {
                  setStartPageOfNav(startPageOfNav - 1);
                }
                currentPageChanged(newCurrPage);
              }}
            >
              {isMobile && <div className="prev-idx-m" />}
              {isMobile === false && (
                <img
                  src="/img/prev-idx.png"
                  srcSet="/img/prev-idx@2x.png 2x, /img/prev-idx@3x.png 3x"
                  alt=""
                />
              )}
            </button>
            {startPageOfNav > 1 && <span className="more-page">...</span>}

            {[...Array(navSize)].map((_, idx) => {
              const pageIdx = startPageOfNav + idx;
              if (pageIdx <= 0 || pageIdx > totalPages) return <Fragment />;
              return (
                <button
                  className={`nav-button borderless-btn ${
                    pageIdx === currentPage ? 'selected-idx' : 'unselected-idx'
                  }`}
                  key={pageIdx}
                  onClick={() => {
                    currentPageChanged(pageIdx);
                  }}
                >
                  {pageIdx}
                </button>
              );
            })}
            {startPageOfNav + navSize <= totalPages && <span className="more-page">...</span>}
            <button
              className="borderless-btn fist-last-idx"
              onClick={() => {
                const newCurrPageOfOrders = currentPage + 1;
                if (newCurrPageOfOrders > totalPages) return;
                if (newCurrPageOfOrders > startPageOfNav + navSize - 1) {
                  if (startPageOfNav + navSize + navSize > totalPages)
                    setStartPageOfNav(totalPages - (navSize - 1));
                  else setStartPageOfNav(startPageOfNav + navSize);
                }
                currentPageChanged(newCurrPageOfOrders);
              }}
            >
              {isMobile && <div className="next-idx-m" />}
              {isMobile === false && (
                <img
                  src="/img/next-idx.png"
                  srcSet="/img/next-idx@2x.png 2x, /img/next-idx@3x.png 3x"
                  alt=""
                />
              )}
            </button>
            {isMobile === false && (
              <button
                className="borderless-btn fist-last-idx"
                onClick={() => {
                  setStartPageOfNav(totalPages - (navSize - 1));
                  currentPageChanged(totalPages);
                }}
              >
                <img
                  src="/img/last-idx.png"
                  srcSet="/img/last-idx@2x.png 2x, /img/last-idx@3x.png 3x"
                  alt=""
                />
              </button>
            )}
          </Fragment>
        )}
      </div>
    </div>
  );
};
