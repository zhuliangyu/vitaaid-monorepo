/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { StabilityTestData } from '../../model/StabilityForm';

import {
  CircularGauge,
  Size,
  Scale,
  RangeContainer,
  Range,
  Geometry,
  ValueIndicator,
} from 'devextreme-react/circular-gauge';

interface Props {
  TestData: StabilityTestData;
}

export const ChemicalContaminantResultDetail = ({ TestData }: Props) => {
  const startVal = 0;
  const endVal = TestData.highestLimit * 2;
  const tickInterval = TestData.highestLimit <= 1 ? 0.1 : endVal < 20 ? 1 : endVal < 30 ? 5 : 10;
  return (
    <CircularGauge className="sf-result_gauge" id="gauge" value={TestData.numericResult}>
      <Size height={100} width={180} />
      <ValueIndicator type="triangleNeedle" color="#5FC468"></ValueIndicator>
      <Scale startValue={startVal} endValue={endVal} tickInterval={tickInterval}></Scale>
      <RangeContainer>
        <Range startValue={startVal} endValue={TestData.highestLimit} color="#5FC468" />
        <Range startValue={TestData.highestLimit} endValue={endVal} color="#EA4949" />
      </RangeContainer>
      <Geometry startAngle={180} endAngle={0}></Geometry>
    </CircularGauge>
  );
};
