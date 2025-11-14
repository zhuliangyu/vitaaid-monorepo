/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import { Link, useNavigate, useLocation } from 'react-router-dom';
import React, { Fragment, useState, useRef, MutableRefObject } from 'react';
import { useSelector, useDispatch } from 'react-redux';

import { selectedCountry } from 'redux/features/country/countrySlice';
import { Country, InitialCountryByIP } from 'redux/features/country/Country';
import { Popover } from 'devextreme-react/popover';
import { UnitTypeData, getProductCategories } from 'model/UnitType';
import { getAlphabetList } from 'model/Product';
import { blogCategoryChanged } from 'redux/features/BlogCategorySlice';
import { productCategoryChanged } from 'redux/features/product/productCategorySlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { closeForgotPasswordDlg } from 'redux/features/forgotPasswordDlgSlice';
import { byCategory, byAlphabet } from 'redux/features/product/productFilterMethodSlice';
import { accountChanged, accountData } from 'redux/features/account/accountSlice';
import { signout, removeTokenFromSession } from 'model/JwtToken';
import { OrderData } from 'model/ShoppingCart';
import { MessageBox } from 'components/MessageBox';

import { productCodeChanged } from 'redux/features/product/productCodeSlice';
import { ca } from 'redux/features/country/countrySlice';

import {
  cartChanged,
  ShoppingCartItem,
  shoppingCart,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import {
  bCartBrief,
  viewCartBrief,
  hideCartBrief,
} from 'redux/features/shoppingcart/showCartBriefSlice';

import { orderChanged } from 'redux/features/shoppingcart/orderSlice';
import { shopping_cart_page } from 'redux/features/shoppingcart/cartPageSlice';
import {
  productAlphabets,
  productAlphabetsChanged,
} from 'redux/features/product/productAlphabetsSlice';

import {
  profile_page,
  order_history_page,
  patient_order_history_page,
  change_password_page,
} from 'redux/features/account/accountPageSlice';
import { SearchResultData, doSearch } from 'model/Search';
import ScrollView from 'devextreme-react/scroll-view';
import { forceUpdate } from 'redux/features/forceUpdateSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import {
  memberTypeForURLAfterLogin,
  memberTypeForURLAfterLoginChanged,
} from 'redux/features/memberTypeForURLAfterLoginSlice';
import {
  visiblePractitionerOnlyMsgBox,
  visiblePractitionerOnlyMsgBoxChanged,
} from 'redux/features/visiblePractitionerOnlyMsgBoxSlice';

import { LoginFormModule } from '../common/LoginFormModule';
import { ForgotPasswordModule } from '../common/ForgotPasswordModule';
interface SearchModuleProps {
  refSearch: MutableRefObject<HTMLInputElement | null>;
}

export default function HeaderDesktop() {
  const [categories, setCategories] = React.useState<UnitTypeData[]>([]);
  const [showProductCategoryPopover, setProductCategoryPopover] = useState<boolean>(false);
  const [showEducationPopover, setEducationPopover] = useState<boolean>(false);
  const [showSearchPopover, setSearchPopover] = useState<boolean>(false);
  const [showAboutPopover, setAboutPopover] = useState<boolean>(false);
  const [showQualityPopover, setQualityPopover] = useState<boolean>(false);
  const [showLoginPopover, setLoginPopover] = useState<boolean>(false);
  const [showAccountPopover, setAccountPopover] = useState<boolean>(false);
  const [showForgotPwdPopover, setForgotPwdPopover] = useState<boolean>(false);
  const alphabetList = useSelector(productAlphabets);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  //let emailForgotPassword = useRef<HTMLInputElement | null>(null);
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  const bShowLoginDlg = useSelector(loginDlg);
  const cart = useSelector(shoppingCart);
  const showCartBrief = useSelector(bCartBrief);
  const memberTypeDataForURLAfterLogin = useSelector(memberTypeForURLAfterLogin);
  const isVisiblePractitionerOnlyMsgBox = useSelector(visiblePractitionerOnlyMsgBox);
  const countryURL = useLocation();
  let timerID: NodeJS.Timeout | undefined | null = null;
  let searchTerm: string = '';
  let refSearchInput: MutableRefObject<HTMLInputElement | null> = useRef<HTMLInputElement | null>(
    null,
  );
  const urlAfterLoginData = useSelector(urlAfterLogin);
  React.useEffect(() => {
    if (urlAfterLoginData && account && urlAfterLoginData !== '') {
      if (memberTypeDataForURLAfterLogin === 2 && account.memberType === 2) {
        dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
      } else {
        if (urlAfterLoginData.endsWith('.pdf')) {
          window.open(urlAfterLoginData, '_blank');
        } else {
          navigate(urlAfterLoginData);
          dispatch(forceUpdate());
        }
      }
      dispatch(urlAfterLoginChanged(''));
      dispatch(memberTypeForURLAfterLoginChanged(0));
    }
  }, [urlAfterLoginData, account]);
  const DoSignOut = async (reason: string) => {
    await signout(reason);
    removeTokenFromSession();
    dispatch(accountChanged(null));
    dispatch(cartChanged([] as ShoppingCartItem[]));
    dispatch(orderChanged({} as OrderData));
    navigate('/', { replace: true });
  };
  const GoToCart = () => {
    setLoginPopover(false);
    dispatch(shopping_cart_page());
    navigate('/cart');
  };
  React.useEffect(() => {
    async function fetchData() {
      const data = await getProductCategories();
      setCategories(data);
    }
    // if american user can use the ca website when using the ca url
    if (countryURL.pathname.toLowerCase() === '/ca') {
      sessionStorage.setItem('country', 'CA');
      sessionStorage.setItem('ipcountry', 'CA');
      dispatch(ca());
    }
    if (sessionStorage.getItem('country') == null) InitialCountryByIP(dispatch);
    fetchData();
  }, []);

  React.useEffect(() => {
    async function fetchData() {
      const alphabets = await getAlphabetList(country);
      dispatch(productAlphabetsChanged(alphabets));
    }
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [country]);

  const SetPopupVisibility = (popupName: string, bShow: boolean) => {
    setLoginPopover(popupName === 'login' ? bShow : false);
    setEducationPopover(popupName === 'eduication' ? bShow : false);
    setProductCategoryPopover(popupName === 'product' ? bShow : false);
    setAboutPopover(popupName === 'about' ? bShow : false);
    setQualityPopover(popupName === 'quality' ? bShow : false);
    setForgotPwdPopover(popupName === 'forgot' ? bShow : false);
    setSearchPopover(popupName === 'search' ? bShow : false);
  };

  const SearchModule = ({ refSearch }: SearchModuleProps) => {
    const account = useSelector(accountData);
    const [searchResultProducts, setSearchResultProducts] = React.useState<SearchResultData[]>(
      [] as SearchResultData[],
    );
    const [searchResultBlogs, setSearchResultBlogs] = React.useState<SearchResultData[]>(
      [] as SearchResultData[],
    );
    const [searchResultWebinars, setSearchResultWebinars] = React.useState<SearchResultData[]>(
      [] as SearchResultData[],
    );
    const [searchResultProtocols, setSearchResultProtocols] = React.useState<SearchResultData[]>(
      [] as SearchResultData[],
    );

    const DoSearch = async (keyword: string) => {
      const results = await doSearch(keyword, country, account ? account.memberType : 0);
      setSearchResultProducts(results.products);
      if (results.blogs)
        results.blogs.forEach((x) => {
          const regex = /(<([^>]+)>)/gi;
          x.content = x.content.replace(regex, '');
        });
      setSearchResultBlogs(results.blogs);
      setSearchResultWebinars(results.webinars);
      setSearchResultProtocols(results.protocols);
    };
    return (
      <div>
        <input
          autoFocus
          ref={refSearch}
          className={`search-input`}
          id="search"
          type="text"
          placeholder="Search"
          onChange={(e) => {
            searchTerm = e.target.value;
            if (!timerID) {
              timerID = setTimeout(() => {
                DoSearch(searchTerm);
                console.log(searchTerm);
                clearTimeout(timerID!);
                timerID = null;
              }, 500);
            }
          }}
          key="search-input"
        />
        <ScrollView width="100%" height="100%">
          <div className="result-div">
            {searchResultProducts && searchResultProducts.length > 0 && (
              <Fragment>
                <div className="search-result-category-title">Product</div>
                <div>
                  {searchResultProducts.map((x) => (
                    <button
                      key={x.identity}
                      className="a-btn search-content-btn"
                      dangerouslySetInnerHTML={{ __html: x.content }}
                      onClick={() => {
                        SetPopupVisibility('search', false);
                        //dispatch(byKeyword());
                        //dispatch(productCodeChanged(x.identity));
                        //dispatch(productCategoryChanged(''));
                        navigate(`/products?pcode=${x.identity}`);
                      }}
                    ></button>
                  ))}
                </div>
              </Fragment>
            )}
            {searchResultBlogs && searchResultBlogs.length > 0 && (
              <Fragment>
                <div className="search-result-category-title">Blog</div>
                <div>
                  {searchResultBlogs.map((x) => (
                    <button
                      key={x.identity}
                      className="a-btn search-content-btn"
                      onClick={() => {
                        SetPopupVisibility('search', false);
                        const url = `/blogarticle/${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(openLoginDlg());
                        } else if (account.memberType === 2) {
                          dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
                        } else {
                          navigate(url);
                          dispatch(forceUpdate());
                        }
                      }}
                    >
                      {x.content}
                    </button>
                  ))}
                </div>
              </Fragment>
            )}
            {searchResultWebinars && searchResultWebinars.length > 0 && (
              <Fragment>
                <div className="search-result-category-title">Webinar</div>
                <div>
                  {searchResultWebinars.map((x) => (
                    <button
                      key={x.identity}
                      className="a-btn search-content-btn"
                      dangerouslySetInnerHTML={{ __html: x.content }}
                      onClick={() => {
                        SetPopupVisibility('search', false);
                        const url = `/webinardetail/${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(openLoginDlg());
                        } else if (account.memberType === 2) {
                          dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
                        } else {
                          navigate(url);
                          dispatch(forceUpdate());
                        }
                      }}
                    ></button>
                  ))}
                </div>
              </Fragment>
            )}
            {searchResultProtocols && searchResultProtocols.length > 0 && (
              <Fragment>
                <div className="search-result-category-title">Therapeutic Protocol</div>
                <div>
                  {searchResultProtocols.map((x) => (
                    <button
                      key={x.identity}
                      className="a-btn search-content-btn"
                      dangerouslySetInnerHTML={{ __html: x.content }}
                      onClick={() => {
                        SetPopupVisibility('search', false);
                        const url = `/protocols?id=${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(openLoginDlg());
                        } else if (account.memberType === 2) {
                          dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
                        } else {
                          navigate(url);
                          dispatch(forceUpdate());
                        }
                      }}
                    ></button>
                  ))}
                </div>
              </Fragment>
            )}
          </div>
        </ScrollView>
      </div>
    );
  };

  const PrintCategoryItem = (x: UnitTypeData) => {
    return (
      <div
        key={`type-${x.id}`}
        className="col-6 popup-submenu-item"
        css={css`
          display: inline-block;
          white-space: nowrap;
        `}
      >
        <button
          className="a-btn"
          key={`${x.id}`}
          css={css`
            padding: 0px;
          `}
          dangerouslySetInnerHTML={{
            __html: x.name,
          }}
          onClick={() => {
            dispatch(productCodeChanged(''));
            dispatch(byCategory());
            dispatch(productCategoryChanged(x.name));
            setProductCategoryPopover(false);
            navigate('/products');
            if (
              window.location.pathname !== '/products' &&
              window.location.pathname !== '/products#'
            ) {
              window.location.pathname = '/products';
              return true;
            } else return false; //window.location.pathname !== '/products';
          }}
        ></button>
        {/* <button
                        css={css`
                          padding: 0px;
                          color: var(--marine-blue);
                        `}
                        className="borderless-btn"
                        onClick={() => {}}
                      >
                        {x.sub_Category.toUpperCase()}
                      </button> */}
      </div>
    );
  };

  return (
    <div className="Vitaaid-Header">
      <div className="top1Header">
        <div className="top1-header-content">
          <div className="left-block text-left">
            <Country
              onClicked={(e: string) => {
                if (account) {
                  DoSignOut('Country changed!');
                }
              }}
            />
          </div>
          <div className="right-block text-right">
            {account == null && (
              <button
                key="loginpop"
                className="borderless-btn login-item"
                onClick={() => {
                  SetPopupVisibility('login', true);
                }}
              >
                LOG IN / CREATE ACCOUNT
              </button>
            )}
            {account && (
              <button
                className="img-btn account-item"
                onClick={() => {
                  setAccountPopover(true);
                }}
              >
                <img
                  className="search"
                  alt="member"
                  src="/img/member.png"
                  srcSet="/img/member@2x.png 2x, /img/member@3x.png 3x"
                ></img>
                <span className="greeting">Hi, {account.name}</span>
              </button>
            )}
            <div id="login-dlg-pos" />
          </div>
        </div>
      </div>
      <div className="top2Header">
        <table className="top2-table">
          <tbody>
            <tr>
              <td className="logo">
                <a href="/">
                  <img alt="logo"></img>
                </a>
              </td>
              <td
                css={css`
                  padding-bottom: 0px;
                `}
              >
                <nav className="nav justify-content-begin">
                  <div className="linkPage">
                    <button
                      className="nav-item about-link a-btn"
                      onClick={() => SetPopupVisibility('about', !showAboutPopover)}
                    >
                      ABOUT
                    </button>
                    <button
                      id="popAbout"
                      className="borderless-btn"
                      onClick={() => {
                        SetPopupVisibility('about', !showAboutPopover);
                      }}
                    >
                      <img className="open-submenu" src="/img/openbutton.png" alt="" />
                    </button>
                  </div>
                  <div className="linkPage">
                    <button
                      className="nav-item quality-link a-btn"
                      onClick={() => SetPopupVisibility('quality', !showQualityPopover)}
                    >
                      QUALITY
                    </button>
                    <button
                      id="popQuality"
                      className="borderless-btn"
                      onClick={() => {
                        SetPopupVisibility('quality', !showQualityPopover);
                      }}
                    >
                      <img className="open-submenu" src="/img/openbutton.png" alt="" />
                    </button>
                  </div>
                  <div className="linkPage">
                    <button
                      className="nav-item a-btn"
                      onClick={() => {
                        dispatch(productCodeChanged(''));
                        dispatch(byCategory());
                        dispatch(productCategoryChanged(''));
                        SetPopupVisibility('product', !showProductCategoryPopover);
                      }}
                    >
                      PRODUCTS
                    </button>
                    <button
                      id="popCategory"
                      className="borderless-btn"
                      onClick={() => {
                        SetPopupVisibility('product', !showProductCategoryPopover);
                      }}
                    >
                      <img className="open-submenu" src="/img/openbutton.png" alt="" />
                    </button>
                  </div>
                  <div className="linkPage">
                    <button
                      className="nav-item education-link a-btn"
                      onClick={() => SetPopupVisibility('eduication', !showEducationPopover)}
                    >
                      PRACTITIONERS
                    </button>
                    <button
                      id="popEducation"
                      className="borderless-btn"
                      onClick={() => {
                        SetPopupVisibility('eduication', !showEducationPopover);
                      }}
                    >
                      <img className="open-submenu" src="/img/openbutton.png" alt="" />
                    </button>
                  </div>
                  <div className="linkPage">
                    <Link className="nav-item education-link" to="/infoforpatients">
                      PATIENTS
                    </Link>
                  </div>

                  {/*  <div className="linkPage">*/}
                  {/*    <Link className="nav-item" to="/services">*/}
                  {/*      SERVICE*/}
                  {/*    </Link>*/}
                  {/*    <button className="borderless-btn">*/}
                  {/*      <img className="open-submenu" src="/img/openbutton.png" alt="" />*/}
                  {/*    </button>*/}
                  {/*  </div>*/}
                </nav>
              </td>
              <td
                css={css`
                  vertical-align: bottom;
                `}
              >
                <div className="text-right">
                  <div className="search-block">
                    <button
                      className="img-btn"
                      onClick={() => {
                        SetPopupVisibility('search', !showSearchPopover);
                      }}
                    >
                      <img
                        className="search"
                        alt="search"
                        src="/img/search.png"
                        srcSet="/img/search@2x.png 2x, /img/search@3x.png 3x"
                      ></img>
                    </button>
                  </div>
                  <div className="cart-block">
                    {account && (
                      <button className="img-btn" onClick={GoToCart}>
                        <img
                          className="shoppingCart"
                          alt="Shopping Cart"
                          src={
                            cart && cart.length > 0
                              ? '/img/shopping-cart.png'
                              : '/img/empty-shopping-cart.png'
                          }
                          srcSet={
                            cart && cart.length > 0
                              ? '/img/shopping-cart@2x.png 2x, /img/shopping-cart@3x.png 3x'
                              : '/img/empty-shopping-cart@2x.png 2x, /img/empty-shopping-cart@3x.png 3x'
                          }
                        ></img>
                      </button>
                    )}
                    {account && cart && cart.length > 0 && (
                      <button
                        className="img-btn"
                        onClick={() => {
                          if (showCartBrief) dispatch(hideCartBrief());
                          else dispatch(viewCartBrief());
                        }}
                      >
                        <span
                          css={css`
                            margin-left: 6px;
                          `}
                        >
                          ({cart.map((x) => x.qty).reduce((s, c) => s + c, 0)})
                        </span>
                        {window.location.pathname.startsWith('/products') && (
                          <img
                            className="toggle-cart-brief-img"
                            alt={showCartBrief === true ? 'close' : 'open'}
                            src={
                              showCartBrief === true
                                ? '/img/close-cart-brief.png'
                                : '/img/open-cart-brief.png'
                            }
                            srcSet={
                              showCartBrief === true
                                ? '/img/close-cart-brief@2x.png 2x, /img/close-cart-brief@3x.png 3x'
                                : '/img/open-cart-brief@2x.png 2x, /img/open-cart-brief@3x.png 3x'
                            }
                          ></img>
                        )}
                      </button>
                    )}
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>

        <Popover
          position={{
            my: { x: 'right', y: 'top' },
            at: { x: 'right', y: 'bottom' },
            of: '.about-link',
          }}
          visible={showAboutPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            setAboutPopover(false);
          }}
        >
          <div className="container about-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setAboutPopover(false);
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <div className="row">
              <div className="col-12 popup-title">ABOUT</div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/ourvision"
                >
                  Our Vision
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/medicaladvisoryboard"
                >
                  Medical Advisory Board
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/medicalconsultancyteam"
                >
                  Medical Consultancy Team
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/contactus"
                >
                  Contact Us
                </a>
              </div>
            </div>
          </div>
        </Popover>
        <Popover
          position={{
            my: { x: 'right', y: 'top' },
            at: { x: 'right', y: 'bottom' },
            of: '.quality-link',
          }}
          visible={showQualityPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            setQualityPopover(false);
          }}
        >
          <div className="container quality-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setQualityPopover(false);
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <div className="row">
              <div className="col-12 popup-title">QUALITY</div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/buildingtrustthroughquality"
                >
                  Building Trust Through Quality
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href={`/qualitytrak/${country}`}
                >
                  QualityTRAK<sup>TM</sup>
                </a>
              </div>
            </div>
          </div>
        </Popover>

        <Popover
          position={{
            my: { x: 'left', y: 'top' },
            at: { x: 'left', y: 'bottom' },
            of: '.logo',
          }}
          visible={showProductCategoryPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            setProductCategoryPopover(false);
          }}
        >
          <div className="container product-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setProductCategoryPopover(false);
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <div className="row">
              <div className="col-9 clear-col-padding">
                <div className="row">
                  <div className="col-12 popup-title">PRODUCT CATEGORIES</div>
                </div>
                <div className="row">
                  {categories &&
                    categories.map((x, idx) => {
                      if (idx % 2 === 0) return PrintCategoryItem(categories[idx / 2]);
                      else
                        return PrintCategoryItem(
                          categories[Math.ceil(categories.length / 2) - 1 + (idx + 1) / 2],
                        );
                    })}
                </div>
              </div>
              <div className="col-3 clear-col-padding">
                <div className="row">
                  <div className="col-12 popup-title">PRODUCT ALPHABET</div>
                </div>
                <div className="row">
                  <div
                    className="col-12"
                    css={css`
                      padding-left: 0px;
                      padding-right: 0px;
                    `}
                  >
                    {alphabetList &&
                      (alphabetList as string[]).map((x, idx) => (
                        <Fragment key={`alphabet-hdr-${idx}`}>
                          <div className="alphabet-block">
                            <button
                              className="a-btn"
                              onClick={() => {
                                dispatch(productCodeChanged(''));
                                dispatch(byAlphabet());
                                dispatch(productCategoryChanged(x));
                                setProductCategoryPopover(false);
                                navigate('/products');
                                if (
                                  window.location.pathname !== '/products' &&
                                  window.location.pathname !== '/products#'
                                ) {
                                  window.location.pathname = '/products';
                                  return true;
                                } else return false; //window.location.pathname !== '/products';
                              }}
                            >
                              {x}
                            </button>
                          </div>
                          {(idx + 1) % 6 === 0 && <br key={`br${idx}`}></br>}
                        </Fragment>
                      ))}
                  </div>
                </div>
                <div className="row  product-catalogue-download">
                  <div className="col-12">
                    <button
                      className="a-btn borderless-btn"
                      onClick={() => {
                        const url = `${process.env
                          .REACT_APP_PRODUCT_DIR!}datasheet/Product_Catalogue_2025.pdf`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(openLoginDlg());
                        } else if (account.memberType === 2) {
                          //2:Patient
                          dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
                        } else {
                          //window.history.pushState({}, '', location.href);
                          window.open(url, '_blank');
                          //window.location.href = url;
                        }
                      }}
                    >
                      <img className="btn-download" src="/img/download.png" alt="" />
                      <span className="download-title">Product Catalogue</span>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </Popover>
        <Popover
          position={{
            my: { x: 'right', y: 'top' },
            at: { x: 'right', y: 'bottom' },
            of: '.education-link',
          }}
          visible={showEducationPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            setEducationPopover(false);
          }}
        >
          <div className="container education-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setEducationPopover(false);
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <div className="row">
              <div className="col-12 popup-title">PRACTITIONERS</div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/blog"
                  onClick={() => {
                    dispatch(blogCategoryChanged(''));
                    return true;
                  }}
                >
                  Blog
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/webinars"
                >
                  Webinar
                </a>
              </div>
            </div>
            <div className="row">
              <div
                className="col-12 popup-submenu-item"
                css={css`
                  display: inline-block;
                  white-space: nowrap;
                `}
              >
                <a
                  css={css`
                    padding: 0px;
                  `}
                  href="/protocols"
                >
                  Therapeutic Protocol
                </a>
              </div>
            </div>
          </div>
        </Popover>
        <Popover
          position={{
            my: { x: 'right', y: 'top' },
            at: { x: 'right', y: 'bottom' },
            of: '.search-block',
          }}
          visible={showSearchPopover}
          closeOnOutsideClick={true}
          onShown={(e: any) => {
            //e.component.focus();
            refSearchInput?.current?.focus();
          }}
          onHiding={() => {
            setSearchPopover(false);
            if (timerID) {
              clearTimeout(timerID!);
              timerID = null;
            }
          }}
        >
          <div className="container search-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setSearchPopover(false);
                if (timerID) {
                  clearTimeout(timerID!);
                  timerID = null;
                }
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <SearchModule refSearch={refSearchInput} />
          </div>
        </Popover>
        <Popover
          position={{
            my: 'center',
            at: 'center',
            of: window,
          }}
          shading={true}
          shadingColor="rgba(0, 0, 0, 0.5)"
          visible={showForgotPwdPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            SetPopupVisibility('forgot', false);
            dispatch(closeForgotPasswordDlg());
          }}
        >
          <div className="forgot-pwd-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                SetPopupVisibility('forgot', false);
                dispatch(closeForgotPasswordDlg());
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <ForgotPasswordModule
              setPopupVisibility={(popupName, isVisible) =>
                SetPopupVisibility(popupName, isVisible)
              }
            />
          </div>
        </Popover>
        <Popover
          position={
            bShowLoginDlg
              ? {
                  my: 'center',
                  at: 'center',
                  of: window,
                }
              : showLoginPopover
              ? {
                  my: { x: 'right', y: 'top' },
                  at: { x: 'right', y: 'bottom' },
                  of: '#login-dlg-pos',
                }
              : {
                  my: 'top',
                  at: 'bottom',
                  of: window,
                }
          }
          shading={bShowLoginDlg ? true : false}
          shadingColor="rgba(0, 0, 0, 0.5)"
          visible={bShowLoginDlg || showLoginPopover}
          closeOnOutsideClick={true}
          onHiding={() => {
            dispatch(closeLoginDlg());
            setLoginPopover(false);
            if (!account) {
              dispatch(urlAfterLoginChanged(''));
              dispatch(memberTypeForURLAfterLoginChanged(0));
            }
          }}
        >
          <div className="container login-popover">
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                dispatch(closeLoginDlg());
                setLoginPopover(false);
                if (!account) {
                  dispatch(urlAfterLoginChanged(''));
                  dispatch(memberTypeForURLAfterLoginChanged(0));
                }
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              ></img>
            </button>
            <LoginFormModule
              setLoginPopover={(isVisible) => setLoginPopover(isVisible)}
              setPopupVisibility={(popupName, isVisible) =>
                SetPopupVisibility(popupName, isVisible)
              }
            />
          </div>
        </Popover>

        <Popover
          position={{
            my: { x: 'left', y: 'top' },
            at: { x: 'left', y: 'bottom' },
            of: '.account-item',
          }}
          title="MY ACCOUNT"
          showTitle={true}
          visible={showAccountPopover}
          showCloseButton={true}
          closeOnOutsideClick={true}
          onHiding={() => {
            setAccountPopover(false);
          }}
          className="account-popover"
        >
          <div className="account-popover-body">
            <div>
              <button
                className="borderless-btn a-btn account-action-btn"
                onClick={() => {
                  navigate('/account');
                  dispatch(profile_page());
                }}
              >
                Member Profile
              </button>
            </div>
            {/*<div>*/}
            {/*  <button*/}
            {/*    className="borderless-btn a-btn account-action-btn"*/}
            {/*    onClick={() => {*/}
            {/*      navigate('/account');*/}
            {/*      dispatch(address_book_page());*/}
            {/*    }}*/}
            {/*  >*/}
            {/*    Address Book*/}
            {/*  </button>*/}
            {/*</div>*/}
            <div>
              <button
                className="borderless-btn a-btn account-action-btn"
                onClick={() => {
                  navigate('/account');
                  dispatch(order_history_page());
                }}
              >
                Order History
              </button>
            </div>
            {account && account.memberType === 1 && account.hasPatients === true && (
              <div>
                <button
                  className="borderless-btn a-btn account-action-btn"
                  onClick={() => {
                    navigate('/account');
                    dispatch(patient_order_history_page());
                  }}
                >
                  Patient Order History
                </button>
              </div>
            )}
            <div>
              <button
                className="borderless-btn a-btn account-action-btn"
                onClick={() => {
                  navigate('/account');
                  dispatch(change_password_page());
                }}
              >
                Change Password
              </button>
            </div>
            <div>
              <button
                className="borderless-btn a-btn sign-out-btn"
                onClick={() => {
                  setAccountPopover(false);
                  DoSignOut('');
                }}
              >
                Sign Out
              </button>
            </div>
          </div>
        </Popover>
        <MessageBox
          Title="PLEASE NOTE:"
          Message="The following pages are only for Licensed Health Care Practitioner."
          Type="INFO"
          IsVisible={isVisiblePractitionerOnlyMsgBox}
          onVisibleChange={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(false))}
        />
      </div>
    </div>
  );
}
