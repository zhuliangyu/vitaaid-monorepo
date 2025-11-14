/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useState } from 'react';
import { useSelector } from 'react-redux';
import { StabilityTestData } from '../../model/StabilityForm';
import line_sf_td from 'img/line_sf_td.png';
import { isMobileData } from 'redux/features/isMobileSlice';
interface ItemProps {
  GroupName: string;
}
export const GroupHeader = ({ GroupName }: ItemProps) => {
  const isMobile = useSelector(isMobileData);

  return (
    <Fragment>
      <div className="row">
        <div className={`${isMobile === false ? 'col-3' : 'col-7'} sf-TableHdr`}>{GroupName}</div>
        {isMobile === false && (
          <Fragment>
            <div className="col-5 sf-TableHdrCenter">Specifications</div>
            <div className="col-2 sf-TableHdrCenter">Test Methods</div>
          </Fragment>
        )}
        <div className={`${isMobile === false ? 'col-2' : 'col-5'} sf-TableHdrCenter`}>Results</div>
      </div>
      <div className="row">
        <img className="col-12 img-fluid line_td_sf" alt="" src={line_sf_td}></img>
      </div>
    </Fragment>
  );
};
export const LineInTestData = () => {
  return (
    <div className="row">
      <img className="col-12 img-fluid line_td_sf" alt="" src={line_sf_td}></img>
    </div>
  );
};
export const PassResult = () => <div className="sf-ResultData">PASS</div>;
export const MoreDetail = (result: string) => {
  const upperStr = result.toUpperCase();
  return !(
    upperStr.includes('ABSENT') ||
    upperStr.includes('CONFORMS') ||
    upperStr.includes('CONFORM') ||
    upperStr.includes('N/A') ||
    upperStr.includes('PASS')
  );
};

interface TestResultProps {
  TestData: StabilityTestData;
  isMobile: boolean;
}
export const PassResultInDetailPanel = ({ TestData, isMobile }: TestResultProps) => {
  return (
    <div className={`${isMobile ? 'col-6' : 'col-3'} sf-ResultColInDetailPanel`}>
      <span className="ResultHeader">Batch Test Results</span>
      {MoreDetail(TestData.result0) ? (
        <div className="sf-ResultDataDetail">{TestData.result0}</div>
      ) : (
        <div className="sf-ResultDataDetail">PASS</div>
      )}
    </div>
  );
};

export const TestLimitInDetailPanel = ({ TestData, isMobile }: TestResultProps) => {
  return (
    <Fragment>
      {TestData.testDesc && TestData.testDesc.length > 1 && (
        <div className="row">
          <div className="col-8">
            <div className="sf-TestDescHeader">Test Limit Information:</div>
            <div className="sf-ResultDataDetail">{TestData.testDesc}</div>
          </div>
          <div className="col-4" />
        </div>
      )}
    </Fragment>
  );
};

interface ResultDetailProps {
  TestData: StabilityTestData;
  isMobile: boolean;
  children: React.ReactNode;
}
export const ResultDetailPanel = ({ TestData, isMobile, children }: ResultDetailProps) => {
  return (
    <Fragment>
      {isMobile && (
        <Fragment>
          <div className="row spec-method-m">
            <div className="col-7 sf-TableHdr">Specifications</div>
            <div className="col-5 sf-TableHdr">Test Methods</div>
          </div>
          <div className="row">
            <div className="col-7 spec-method-CellData-m">{TestData.testSpec}</div>
            <div className="col-5 spec-method-CellData-m">{TestData.testMethod}</div>
          </div>
        </Fragment>
      )}
      <div className="row align-items-end sf-RowInResultDetailPanel">
        <PassResultInDetailPanel TestData={TestData} isMobile={isMobile} />
        {isMobile && (
          <div className="col-6">
            {TestData.numericResult > 0 && <Fragment>{children}</Fragment>}
          </div>
        )}
        {isMobile === false && (
          <Fragment>
            <div className="col-5">
              {TestData.numericResult > 0 && <Fragment>{children}</Fragment>}
            </div>
            <div className="col-4 sf-DummyColInDetailPanel" />
          </Fragment>
        )}
      </div>
      <TestLimitInDetailPanel TestData={TestData} isMobile={isMobile} />
    </Fragment>
  );
};
interface ToggleProps {
  IsVisible: boolean;
  onVisibleChange: () => void;
}
export const ToggleButton = ({ IsVisible, onVisibleChange }: ToggleProps) => {
  return (
    <button
      className={IsVisible ? 'sf-minus_button' : 'sf-plus_button'}
      onClick={() => {
        onVisibleChange();
      }}
    ></button>
  );
};
