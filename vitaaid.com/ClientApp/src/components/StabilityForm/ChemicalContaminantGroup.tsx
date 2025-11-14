/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useState } from 'react';
import { useSelector } from 'react-redux';
import { StabilityTestData } from '../../model/StabilityForm';
import { ChemicalContaminantResultDetail } from './ChemicalContaminantResultDetail';

import {
  GroupHeader,
  PassResult,
  ToggleButton,
  LineInTestData,
  ResultDetailPanel,
} from './StabilityFormComponents';
import { isMobileData } from 'redux/features/isMobileSlice';

interface TestResultProps {
  TestData: StabilityTestData;
}

const TestResultView = ({ TestData }: TestResultProps) => {
  const [IsVisible, setIsVisible] = useState(false);
  const isMobile = useSelector(isMobileData);
  return (
    <Fragment>
      <div className="row">
        <div className={`${isMobile === false ? 'col-3' : 'col-7'} sf-CellData`}>
          {TestData.testName}
        </div>
        {isMobile === false && (
          <Fragment>
            <div className="col-5 sf-CellDataCenter">{TestData.testSpec}</div>
            <div className="col-2 sf-CellDataCenter">{TestData.testMethod}</div>
          </Fragment>
        )}
        <div className={`${isMobile === false ? 'col-2' : 'col-5'} sf-CellDataCenter`}>
          <PassResult />
          <ToggleButton IsVisible={IsVisible} onVisibleChange={() => setIsVisible(!IsVisible)} />
        </div>
      </div>
      {IsVisible && (
        <ResultDetailPanel TestData={TestData} isMobile={isMobile}>
          <ChemicalContaminantResultDetail TestData={TestData} />
        </ResultDetailPanel>
      )}
      <LineInTestData />
    </Fragment>
  );
};

interface Props {
  TestDataAry: StabilityTestData[];
}
export const ChemicalContaminantGroup = ({ TestDataAry }: Props) => {
  return (
    <div className="ChemicalContaminantGroup" key="ChemicalContaminantGroup">
      <GroupHeader GroupName={TestDataAry[0].groupName} />
      {TestDataAry.map((x) => (
        <TestResultView key={x.testName} TestData={x} />
      ))}
    </div>
  );
};
