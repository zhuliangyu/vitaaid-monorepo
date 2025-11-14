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
  SubvalueIndicator,
  Geometry,
  ValueIndicator,
} from 'devextreme-react/circular-gauge';

interface Props {
  TestData: StabilityTestData;
}

export const CriticalIngredientResultDetail = ({ TestData }: Props) => {
  const safeRange = TestData.highestLimit - TestData.lowestLimit;
  var startVal =
    safeRange <= 1
      ? Math.floor(TestData.lowestLimit - safeRange * 0.5)
      : safeRange <= 10
      ? Math.floor(TestData.lowestLimit - safeRange)
      : Math.floor((TestData.lowestLimit - safeRange * 0.5) * 0.1) * 10;
  const endVal =
    safeRange <= 1
      ? Math.ceil(TestData.highestLimit + safeRange * 0.5)
      : safeRange <= 10
      ? Math.ceil(TestData.highestLimit + safeRange)
      : Math.ceil((TestData.highestLimit + safeRange * 0.5) * 0.1) * 10;
  if (startVal < 0) startVal = 0;
  const tickInterval =
    safeRange <= 1 ? 0.1 : safeRange <= 10 ? 1 : endVal < 20 ? 1 : endVal < 30 ? 5 : 10;
  return (
    <CircularGauge
      className="sf-result_gauge"
      id="gauge"
      value={TestData.numericResult}
      subvalues={[TestData.lowestLimit, TestData.highestLimit]}
    >
      <Size height={100} width={180} />
      <ValueIndicator type="triangleNeedle" color="#5FC468"></ValueIndicator>
      <SubvalueIndicator type="triangleMarker" color="#C2C2C2"></SubvalueIndicator>
      <Scale startValue={startVal} endValue={endVal} tickInterval={tickInterval}></Scale>
      <RangeContainer>
        <Range startValue={startVal} endValue={TestData.lowestLimit} color="#EA4949" />
        <Range startValue={TestData.lowestLimit} endValue={TestData.highestLimit} color="#5FC468" />
        <Range startValue={TestData.highestLimit} endValue={endVal} color="#EA4949" />
      </RangeContainer>
      <Geometry startAngle={180} endAngle={0}></Geometry>
    </CircularGauge>
  );
};
