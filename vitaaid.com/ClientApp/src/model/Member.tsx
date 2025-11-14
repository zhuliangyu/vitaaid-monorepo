import { inMemoryToken } from './JwtToken';
import { OrderData } from './ShoppingCart';

export interface MemberData {
  id: number;
  name: string;
  practitionerType: string | null;
  otherPractitionerType: string | null;
  clinicName: string;
  address: string;
  province: string;
  country: string;
  city: string;
  zipCode: string;
  telephone: string;
  fax: string;
  email: string;
  joinTime: Date;
  password: string;
  licence: string;
  bReferral: boolean;
  memberType: number; // 1:Practitioner, 2:Patient, 3...: Student
  physicanCode: string;
  pat_pcode: string;
  licencePhoto: string;
  salesRep: string;
  customerCode: string;
  firstName: string;
  lastName: string;
  memberStatus: number;
  isSubscribe: boolean;
  permittedSite: number;
  prefix: number;
  newPassword: string;
  confirmPassword: string;
  hasPatients: boolean;
}

export interface OrderHistoryData {
  index: number;
  orderID: number;
  orderNo: string;
  orderDate: string;
  name: string;
  status: string;
  paymentMethod: string;
  shippingMethod: string;
  amount: number;
}

export interface ResetPasswordFormData {
  email: string;
  newPassword: string;
  confirmPassword: string;
  token: string;
}

export const getMember = async (email: string): Promise<MemberData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/Members/${email}`, requestOptions);
    return data.json();
  } catch (e) {
    return {} as MemberData;
  }
};

export const saveMemberToSession = (data: MemberData | null) => {
  if (data == null) sessionStorage.removeItem('member');
  else sessionStorage.setItem('member', JSON.stringify(data));
};

export const getMemberFromSession = (): MemberData | null => {
  const data = sessionStorage.getItem('member');
  if (data != null) {
    return JSON.parse(data);
  }
  return null;
};

export const updateMember = async (
  data: any,
  id: number,
  newCountryValue: string,
): Promise<MemberData> => {
  try {
    const formData = new FormData(data);
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
      body: formData, //JSON.stringify(data),
    };
    let _memberData: Promise<MemberData> = {} as Promise<MemberData>;
    await fetch(`/api/Members/${id}?newCountryValue=${newCountryValue}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return {} as MemberData;
        }
        _memberData = response.json();
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return _memberData;
  } catch (e) {
    return {} as MemberData;
  }
};

export const getOrderHistory = async (customerCode: string): Promise<OrderHistoryData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/Members/${customerCode}/orderhistory`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as OrderHistoryData[];
  }
};
export const getPatientOrderHistory = async (customerCode: string): Promise<OrderHistoryData[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(`/api/Members/${customerCode}/patientorderhistory`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as OrderHistoryData[];
  }
};

// GET: api/Members/{CustomerCode}/orderdetail?orderNo={orderNo}
export const getOrderDetail = async (customerCode: string, orderNo: string): Promise<OrderData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(
      `/api/Members/${customerCode}/orderdetail?orderNo=${orderNo}`,
      requestOptions,
    );
    return data.json();
  } catch (e) {
    return {} as OrderData;
  }
};

// GET: api/Members/{CustomerCode}/patientorderdetail?orderNo={orderNo}
export const getPatientOrderDetail = async (
  customerCode: string,
  orderNo: string,
): Promise<OrderData> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    var data = await fetch(
      `/api/Members/${customerCode}/patientorderdetail?orderNo=${orderNo}`,
      requestOptions,
    );
    return data.json();
  } catch (e) {
    return {} as OrderData;
  }
};

// GET: api/Members/requestresetpassword
export const requestResetPassword = async (email: string): Promise<string> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: `Bearer ${inMemoryToken.access_token}`,
      },
    };
    let data: Promise<string> = {} as Promise<string>;
    await fetch(`/api/Members/requestresetpassword/${email}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return '';
        }
        data = response.text();
      })
      .catch((error) => {
        throw error;
      });

    return data;
  } catch (e) {
    return '';
  }
};

// GET: api/Members/checktoken?token=
export const checkToken = async (token: string): Promise<boolean> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let data: Promise<boolean> = {} as Promise<boolean>;
    await fetch(`/api/Members/checktoken?token=${token}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return '';
        }
        data = response.json();
      })
      .catch((error) => {
        throw error;
      });

    return data;
  } catch (e) {
    return false;
  }
};

// PUT: api/Members/resetpassword
export const resetPassword = async (email: string, token: string, data: any): Promise<string> => {
  try {
    const formData = new FormData(data);
    formData.append('token', token);
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
      body: formData,
    };
    let result: Promise<string> = {} as Promise<string>;
    await fetch(`/api/Members/resetpassword/${email}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return '';
        }
        result = response.text();
      })
      .catch((error) => {
        throw error;
      });

    return result;
  } catch (e) {
    return '';
  }
};

// PUT: api/Members/resetpassword
export const changePassword = async (email: string, data: any): Promise<string> => {
  try {
    const formData = new FormData(data);
    formData.append('email', email);
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
      body: formData,
    };
    let result: Promise<string> = {} as Promise<string>;
    await fetch(`/api/Members/changepassword/${email}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return '';
        }
        result = response.text();
      })
      .catch((error) => {
        throw error;
      });

    return result;
  } catch (e) {
    return '';
  }
};
