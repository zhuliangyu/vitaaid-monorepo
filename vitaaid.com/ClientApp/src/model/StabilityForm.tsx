import { inMemoryToken } from './JwtToken';
import { handleErrors } from 'HandleErrors';

export interface StabilityTestData {
  groupName: string;
  testName: string;
  testDesc: string;
  testSpec: string;
  testMethod: string;
  result0: string;
  testUser0: string;
  testDate0: Date;
  testLimitInformation: string;
  lowestLimit: number;
  highestLimit: number;
  numericResult: number;
  specUnit: string;
}

export interface StabilityForm {
  code: string;
  name: string;
  lotNumber: string;
  produceDate: Date;
  sExpiryDate: string;
  servings: string;
  packageForm: string;
  packageClosure: string;
  packageCap: string;
  packageBottle: string;
  packageDesiccant: string;
  packageCotton: string;
  comment: string;
  reviewedBy: string;
  reviewedDate: Date;
  reviewedResult: string;
  shelfLife: number;
  storageCondition: number;
  lProductImg: string;
  servingSize: number;
  sServingSize: string;
  servingUnit: string;
  servingsPerContainer: number;
  oTestData: StabilityTestData[];
}

export const getByLot = async (
  LotNo: string | null | undefined,
  country: string,
): Promise<StabilityForm | null> => {
  try {
    if (LotNo != null && LotNo.length >= 8) {
      const SForm = await fetch('/api/StabilityForms/' + LotNo + '?country=' + country, {
        method: 'GET',
        headers: {
          ApiKey: `${process.env.REACT_APP_API_KEY}`,
        },
      })
        .then(handleErrors)
        .then((response) => response.json())
        .catch(() => {
          throw 'Network error';
        });
      /*
      const response = await fetch(
        '/api/StabilityForms/' + LotNo + '?country=' + country,
      );
      const SForm = await response.json();*/
      return SForm;
    }
    return {} as StabilityForm;
  } catch (e) {
    return {} as StabilityForm;
  }
};
