/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { StabilityForm, StabilityTestData } from '../../model/StabilityForm';
import { PerformanceTestGroup } from './PerformanceTestGroup';
import { ChemicalContaminantGroup } from './ChemicalContaminantGroup';
import { MicrobialContaminantGroup } from './MicrobialContaminantGroup';
import { MiscTestGroup } from './MiscTestGroup';
import { CriticalIngredientGroup } from './CriticalIngredientGroup';

interface Props {
  SForm: StabilityForm;
}

export const StabilityFormDetail = ({ SForm }: Props) => {
  const [PerformanceTests, setPerformanceTests] = React.useState<StabilityTestData[]>([]);
  const [ChemicalContaminants, setChemicalContaminants] = React.useState<StabilityTestData[]>([]);
  const [MicrobialContaminants, setMicrobialContaminants] = React.useState<StabilityTestData[]>([]);
  const [MiscTests, setMiscTests] = React.useState<StabilityTestData[]>([]);
  const [CriticalIngredient, setCriticalIngredient] = React.useState<StabilityTestData[]>([]);

  React.useEffect(() => {
    setPerformanceTests(SForm.oTestData.filter((x) => x.groupName === 'Performance Tests'));
    setChemicalContaminants(SForm.oTestData.filter((x) => x.groupName === 'Chemical Contaminants'));
    setMicrobialContaminants(SForm.oTestData.filter((x) => x.groupName === 'Microbiological'));
    setMiscTests(SForm.oTestData.filter((x) => x.groupName === 'Misc. Tests'));
    setCriticalIngredient(
      SForm.oTestData
        .filter((x) => x.groupName.includes('Ingredient'))
        .map((x) => {
          x.groupName =
            'Ingredient(s)/' +
            (SForm.servingUnit === null || SForm.servingUnit === ''
              ? 'Capsule'
              : SForm.servingUnit); //x.groupName + '/' + SForm.dosingText;
          return x;
        }),
    );
  }, [SForm.oTestData]);

  return (
    <div className="row sf-detail">
      <div className="col-12">
        {PerformanceTests.length > 0 && <PerformanceTestGroup TestDataAry={PerformanceTests} />}
        {ChemicalContaminants.length > 0 && (
          <ChemicalContaminantGroup TestDataAry={ChemicalContaminants} />
        )}
        {MicrobialContaminants.length > 0 && (
          <MicrobialContaminantGroup TestDataAry={MicrobialContaminants} />
        )}
        {MiscTests.length > 0 && <MiscTestGroup TestDataAry={MiscTests} />}
        {CriticalIngredient.length > 0 && (
          <CriticalIngredientGroup TestDataAry={CriticalIngredient} />
        )}
      </div>
    </div>
  );
};
