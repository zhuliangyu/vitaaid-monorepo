import { inMemoryToken } from './JwtToken';

export enum eUNITTYPE {
  PRODUCT_CATEGORY = 100,
  ALLERGY_CATEGORY = 101,
  THERAPEUTIC_FOCUS = 102,
  PRACTICE_TYPE = 103,
  SALES_REP = 104,
}

export interface UnitTypeData {
  id: number;
  name: string;
  abbrName: string;
  uType: eUNITTYPE;
  comment: string;
}

export let allProductCategories: UnitTypeData[] = [] as UnitTypeData[];
export let allAllergyCategories: UnitTypeData[] = [] as UnitTypeData[];

let sortFn = (n1: UnitTypeData, n2: UnitTypeData): number => {
  return n1.name > n2.name ? 1 : n1.name < n2.name ? -1 : 0;
};
export const getProductCategories = async (): Promise<UnitTypeData[]> => {
  if (allProductCategories.length == 0) {
    const data = await getUnitTypes(eUNITTYPE.PRODUCT_CATEGORY);
    allProductCategories = data.sort(sortFn);
  }
  return allProductCategories;
};
export const getAllergyCategories = async (): Promise<UnitTypeData[]> => {
  if (allAllergyCategories.length == 0) {
    const data = await getUnitTypes(eUNITTYPE.ALLERGY_CATEGORY);
    allAllergyCategories = data.sort(sortFn);
  }
  return allAllergyCategories;
};

export const getUnitTypes = async (type: eUNITTYPE): Promise<UnitTypeData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/UnitTypes/${type}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as UnitTypeData[];
  }
};
