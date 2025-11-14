export interface ShoppingCartToken {
  access_token: string;
  expires_in: number;
}
export let inMemoryShoppingCartToken: ShoppingCartToken = {} as ShoppingCartToken;

export const refreshShoppingCartToken = async (): Promise<ShoppingCartToken> => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    let _authData: Promise<ShoppingCartToken> = {} as Promise<ShoppingCartToken>;
    await fetch(`${process.env.REACT_APP_SHOPPING_CART_URL}/api/oauth/refresh`, requestOptions)
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
    return {} as ShoppingCartToken;
  }
};

export const saveShoppingCartTokenToSession = (token: ShoppingCartToken) => {
  sessionStorage.setItem('shopping_cart_token', token.access_token);
  sessionStorage.setItem('shopping_cart_expires_in', token.expires_in.toString());
};

export const removeShoppingCartTokenFromSession = () => {
  sessionStorage.removeItem('shopping_cart_token');
  sessionStorage.removeItem('shopping_cart_expires_in');
};

export const getShoppingCartTokenFromSession = (): ShoppingCartToken | null => {
  const token = sessionStorage.getItem('shopping_cart_token');
  const expired = sessionStorage.getItem('shopping_cart_expires_in');
  if (token != null && expired != null) {
    return {
      access_token: token,
      expires_in: parseInt(expired!, 10),
    };
  }
  return null;
};
let timeout: NodeJS.Timeout;
export const doRefreshShoppingCartToken = (token: ShoppingCartToken, noRedirect: boolean) => {
  inMemoryShoppingCartToken = token;

  if (timeout != null) clearTimeout(timeout);

  timeout = setTimeout(async () => {
    try {
      const newToken = await refreshShoppingCartToken();
      saveShoppingCartTokenToSession(newToken);
      doRefreshShoppingCartToken(newToken, true);
    } catch (e) {
      if (timeout != null) clearTimeout(timeout);
    }
  }, (inMemoryShoppingCartToken.expires_in - 1) * 60 * 1000);
};

export const shopping_cart_oauth = async (
  customerCode: string,
  site: string,
): Promise<ShoppingCartToken> => {
  try {
    const requestOptions = {
      method: 'POST',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        account: 'vitaaid.com',
        password: '32168421',
        customerCode: `${customerCode}`,
        storeName: `${site}`,
        appName: 'vitaaid.com',
      }),
    };
    let _authData: Promise<ShoppingCartToken> = {} as Promise<ShoppingCartToken>;
    await fetch(`${process.env.REACT_APP_SHOPPING_CART_URL}/api/oauth/token`, requestOptions)
      .then((response) => {
        if (response.status >= 400 && response.status < 600) {
          return {} as ShoppingCartToken;
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
    return {} as ShoppingCartToken;
  }
};
