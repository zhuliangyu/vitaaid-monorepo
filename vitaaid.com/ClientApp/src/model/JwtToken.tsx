import { MemberData, getMemberFromSession } from './Member';

export interface JwtToken {
  access_token: string;
  expires_in: number;
  member: MemberData | null;
}

export let inMemoryToken: JwtToken = {} as JwtToken;
/*
export const auth = async (
  username: string,
  password: string,
  appname: string,
): Promise<JwtToken> => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        name: `${username}`,
        password: `${password}`,
        appname: `${appname}`,
      }),
    };
    const reponse = await fetch('/api/auth', requestOptions);
    const _authData = reponse.json();
    return _authData;
  } catch (e) {
    return {} as JwtToken;
  }
};
*/

export const refresh = async (): Promise<JwtToken> => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let _authData: Promise<JwtToken> = {} as Promise<JwtToken>;
    await fetch('/api/ulogin/refresh', requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          throw new Error('Bad response from server');
        }
        _authData = response.json();
      })
      .catch((error) => {
        throw error;
      });

    return _authData;
  } catch (e) {
    return {} as JwtToken;
  }
};

export const saveTokenToSession = (token: JwtToken) => {
  sessionStorage.setItem('access_token', token.access_token);
  sessionStorage.setItem('expires_in', token.expires_in.toString());
};

export const removeTokenFromSession = () => {
  sessionStorage.removeItem('access_token');
  sessionStorage.removeItem('expires_in');
};

export const getTokenFromSession = (): JwtToken | null => {
  const token = sessionStorage.getItem('access_token');
  const expired = sessionStorage.getItem('expires_in');
  const member = getMemberFromSession();
  if (token != null && expired != null) {
    return {
      access_token: token,
      expires_in: parseInt(expired!, 10),
      member: member,
    };
  }
  return null;
};
let timeout: NodeJS.Timeout;
export const doRefreshToken = (token: JwtToken, noRedirect: boolean) => {
  inMemoryToken = token;

  if (timeout != null) clearTimeout(timeout);

  timeout = setTimeout(async () => {
    try {
      const newToken = await refresh();
      saveTokenToSession(newToken);
      doRefreshToken(newToken, true);
    } catch (e) {
      if (timeout != null) clearTimeout(timeout);
    }
  }, (inMemoryToken.expires_in - 1) * 60 * 1000);
};

export const signout = async (reason: string) => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        reason: reason,
      }),
    };
    await fetch('/api/ulogin/signout', requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return;
        }
      })
      .catch((error) => {
        throw error;
      });

    return;
  } catch (e) {
    return;
  }
};

export const ulogin = async (email: string, password: string, site: string): Promise<JwtToken> => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        email: `${email}`,
        password: `${password}`,
        onSite: `${site}`,
      }),
    };
    let _authData: Promise<JwtToken> = {} as Promise<JwtToken>;
    await fetch('/api/ulogin', requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return {} as JwtToken;
        }
        _authData = response.json();
      })
      .catch((error) => {
        throw error;
      });

    // const reponse = await fetch('/api/ulogin', requestOptions);
    // const _authData = reponse.json();
    return _authData;
  } catch (e) {
    return {} as JwtToken;
  }
};
