import { MemberData } from './Member';

export type RegistrationFormData = {
  memberType: number;
  prefix: number;
  practitionerType: string | null;
  otherPractitionerType: string | null;
  firstName: string;
  lastName: string;
  telephone: string;
  email: string;
  password: string;
  confirmPassword: string;
  clinicName: string | null;
  address1: string;
  address2: string | null;
  city: string;
  zipCode: string;
  province: string;
  country: string;
  fax: string | null;
  licenceVerifyMethod: string;
  reCAPTCHAToken: string | null;
  permittedSite: number;
  licencePhoto: string;
  pat_pcode: string | null;
  bReferral: boolean;
  isSubscribe: boolean;
};

export const registerAccount = async (
  data: any,
  memberTypeName: string | null,
): Promise<MemberData> => {
  try {
    const formData = new FormData(data);
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
      body: formData, //JSON.stringify(data),
    };
    let _memberData: Promise<MemberData> = {} as Promise<MemberData>;
    await fetch(`/api/Members?type=${memberTypeName}`, requestOptions)
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

export const validateEmail = async (email: string, except: number): Promise<boolean> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/json',
      },
    };
    let isValid: Promise<boolean> = {} as Promise<boolean>;
    await fetch(`/api/Members/validate/${email}?except=${except}`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return false;
        }
        isValid = response.json();
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return isValid;
  } catch (e) {
    return false;
  }
};
export const validatePhysicianCode = async (physicianCode: string | null): Promise<boolean> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/json',
      },
    };
    let isValid: Promise<boolean> = {} as Promise<boolean>;
    await fetch('/api/Members/validatepcode/' + physicianCode, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return false;
        }
        isValid = response.json();
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return isValid;
  } catch (e) {
    return false;
  }
};

export type JoinUsFormData = {
  profession: string;
  firstName: string;
  lastName: string;
  email: string;
};

export const addToMailingList = async (data: any) => {
  try {
    const formData = new FormData(data);
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
      body: formData, //JSON.stringify(data),
    };

    await fetch(`/api/Members/joinusouremailinglist`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return;
        }
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return;
  } catch (e) {
    return;
  }
};

export type ContactUsFormData = {
  prefix: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  content: string;
};

export const contactUs = async (data: any) => {
  try {
    const formData = new FormData(data);
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
      },
      body: formData, //JSON.stringify(data),
    };

    await fetch(`/api/Members/contactus`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return;
        }
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return;
  } catch (e) {
    return;
  }
};
