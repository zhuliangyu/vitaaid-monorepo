/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import { Link, useNavigate } from 'react-router-dom';
import React, { Fragment, useState, useRef, MutableRefObject } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';

import {
  ca,
  usa,
  selectedCountry,
  IsEqualSite,
  ChangeCountrySite,
} from 'redux/features/country/countrySlice';
import { InitialCountryByIP } from 'redux/features/country/Country';
import { Country } from './Country';
import { Popover } from 'devextreme-react/popover';
import { Popup } from 'devextreme-react/popup';
import { UnitTypeData, getProductCategories } from 'model/UnitType';
import { getAlphabetList } from 'model/Product';
import { blogCategoryChanged } from 'redux/features/BlogCategorySlice';
import {
  productCategory,
  productCategoryChanged,
} from 'redux/features/product/productCategorySlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import {
  eFILTERMETHOD,
  byCategory,
  byAlphabet,
  byKeyword,
} from 'redux/features/product/productFilterMethodSlice';
import { accountChanged, accountData } from 'redux/features/account/accountSlice';
import {
  ulogin,
  signout,
  doRefreshToken,
  saveTokenToSession,
  removeTokenFromSession,
} from 'model/JwtToken';
import {
  shopping_cart_oauth,
  ShoppingCartToken,
  doRefreshShoppingCartToken,
  saveShoppingCartTokenToSession,
} from 'model/ShoppingCartToken';
import { MemberData, getMember, saveMemberToSession } from 'model/Member';
import { OrderData, OrderItemData } from 'model/ShoppingCart';
import { MessageBox } from 'components/MessageBox';

import { productCodeChanged } from 'redux/features/product/productCodeSlice';
import {
  cartChanged,
  ShoppingCartItem,
  shoppingCart,
} from 'redux/features/shoppingcart/shoppingCartSlice';

import { orderChanged } from 'redux/features/shoppingcart/orderSlice';
import {
  shopping_cart_page,
  shopping_summary_page,
} from 'redux/features/shoppingcart/cartPageSlice';
import {
  productAlphabets,
  productAlphabetsChanged,
} from 'redux/features/product/productAlphabetsSlice';

import {
  AccountPageType,
  accountPageIdx,
  profile_page,
  address_book_page,
  order_history_page,
  order_history_detail_page,
  patient_order_history_page,
  patient_order_history_detail_page,
  change_password_page,
} from 'redux/features/account/accountPageSlice';
import { SearchResultData, searchResults, doSearch } from 'model/Search';
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
import {
  requireLoginMessageData,
  requireLoginMessageChanged,
  MessageID,
} from 'redux/features/requireLoginMessageSlice';
import { isMobile } from 'react-device-detect';
import styles from 'scss/abstracts/_variables.scss';

interface SearchModuleProps {
  refSearch: MutableRefObject<HTMLInputElement | null>;
}

export default function HeaderMobile() {
  const [showMenuPopover, setMenuPopover] = useState<boolean>(false);

  const [categories, setCategories] = React.useState<UnitTypeData[]>([]);
  const [showProductCategoryPopover, setProductCategoryPopover] = useState<boolean>(false);
  const [showEducationPopover, setEducationPopover] = useState<boolean>(false);
  const [showSearchPopover, setSearchPopover] = useState<boolean>(false);
  const [showAboutPopover, setAboutPopover] = useState<boolean>(false);
  const [showQualityPopover, setQualityPopover] = useState<boolean>(false);
  const [loginFail, setLoginFail] = useState<boolean>(false);
  const [showAccountPopover, setAccountPopover] = useState<boolean>(false);
  const alphabetList = useSelector(productAlphabets);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  //let emailForgotPassword = useRef<HTMLInputElement | null>(null);
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  const cart = useSelector(shoppingCart);
  const memberTypeDataForURLAfterLogin = useSelector(memberTypeForURLAfterLogin);
  const isVisiblePractitionerOnlyMsgBox = useSelector(visiblePractitionerOnlyMsgBox);
  const requireLoginMessage = useSelector(requireLoginMessageData);
  const refResultDiv = useRef<HTMLDivElement>(null);
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
        window.history.back();
      } else {
        if (urlAfterLoginData.endsWith('.pdf')) {
          //window.history.back();
          window.open(urlAfterLoginData, '_blank');
          window.history.back();
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
    setLoginFail(false);
    navigate('/', { replace: true });
  };
  const GoToCart = () => {
    dispatch(shopping_cart_page());
    navigate('/cart');
  };
  React.useEffect(() => {
    async function fetchData() {
      const data = await getProductCategories();
      setCategories(data);
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
  }, [country]);

  // React.useEffect(() => {
  //   if (refSearchInput.current) refSearchInput.current.focus();
  // }, [refSearchInput]);
  const SetPopupVisibility = (popupName: string, bShow: boolean) => {
    setMenuPopover(popupName === 'menu' ? bShow : false);
    /*
    setLoginPopover(popupName === 'login' ? bShow : false);
    setEducationPopover(popupName === 'eduication' ? bShow : false);
    setProductCategoryPopover(popupName === 'product' ? bShow : false);
    setAboutPopover(popupName === 'about' ? bShow : false);
    setQualityPopover(popupName === 'quality' ? bShow : false);
    setForgotPwdPopover(popupName === 'forgot' ? bShow : false);
    setSearchPopover(popupName === 'search' ? bShow : false);
    */
  };

  const SearchModule = ({ refSearch }: SearchModuleProps) => {
    const [scrollHeight, setScrollHeight] = React.useState('0px');
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
      if (keyword.length === 0 || keyword === '') {
        setScrollHeight('0px');
      }
      const results = await doSearch(keyword, country, account ? account.memberType : 0);

      setSearchResultProducts(results.products);
      if (results.blogs)
        results.blogs.map((x) => {
          const regex = /(<([^>]+)>)/gi;
          x.content = x.content.replace(regex, '');
        });
      setSearchResultBlogs(results.blogs);
      setSearchResultWebinars(results.webinars);
      setSearchResultProtocols(results.protocols);
      if (
        results.products.length > 0 ||
        results.blogs.length > 0 ||
        results.webinars.length > 0 ||
        results.protocols.length > 0
      ) {
        var h = window.innerHeight - 236;
        if (h > 0) setScrollHeight(`${h}px`);
        else {
          setScrollHeight(`${window.screen.height - 336}px`);
        }
      } else setScrollHeight('0px');
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
        <ScrollView
          className="serch-scroll-view"
          css={css`
            height: ${scrollHeight};
          `}
        >
          <div className="result-div" ref={refResultDiv}>
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
                        //SetPopupVisibility('search', false);
                        setSearchPopover(false);
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
                        //SetPopupVisibility('search', false);
                        setSearchPopover(false);
                        const url = `/blogarticle/${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(requireLoginMessageChanged(MessageID.BLOG));
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
                        //SetPopupVisibility('search', false);
                        setSearchPopover(false);
                        const url = `/webinardetail/${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(requireLoginMessageChanged(MessageID.WEBINARS));
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
                        //SetPopupVisibility('search', false);
                        setSearchPopover(false);
                        const url = `/protocols?id=${x.identity}`;
                        if (!account) {
                          dispatch(urlAfterLoginChanged(url));
                          dispatch(memberTypeForURLAfterLoginChanged(2));
                          dispatch(requireLoginMessageChanged(MessageID.TP));
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

  return (
    <div className="Vitaaid-Header-m">
      <div className="top2Header">
        <table className="top2-table">
          <tbody>
            <tr>
              <td className="menu-col">
                <img
                  className="menu-search-icon img-btn"
                  alt="menu"
                  src="/img/menu-m.png"
                  srcSet="/img/menu-m@2x.png 2x, /img/menu-m@3x.png 3x"
                  onClick={() => SetPopupVisibility('menu', true)}
                />
              </td>
              <td className={account ? 'logo-col-with-cart' : 'logo-col'}>
                <a href="/">
                  <div className="logo-icon" />
                </a>
              </td>
              <td className="search-col">
                <button
                  className="img-btn"
                  onClick={() => {
                    setSearchPopover(true);
                  }}
                >
                  <img
                    className="menu-search-icon img-btn"
                    alt="search"
                    src="/img/search-m.png"
                    srcSet="/img/search-m@2x.png 2x, /img/search-m@3x.png 3x"
                  />
                </button>
              </td>
              {account && (
                <td className="cart-col">
                  <div className="cart-block">
                    <div>
                      {account && cart && cart.length > 0 && (
                        <div className="cart-qty">
                          {cart.map((x) => x.qty).reduce((s, c) => s + c, 0)}
                        </div>
                      )}
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
                    </div>
                  </div>
                </td>
              )}
            </tr>
          </tbody>
        </table>
        <Popup
          position={{
            my: { x: 'left', y: 'top' },
            at: { x: 'left', y: 'top' },
            of: window,
          }}
          className="menu-popup"
          animation={{
            show: {
              type: 'slide',
              duration: 400,
              from: {
                position: {
                  my: { x: 'right', y: 'top' },
                  at: { x: 'left', y: 'top' },
                  of: window,
                },
              },
              to: {
                position: {
                  my: { x: 'right', y: 'top' },
                  at: { x: 'right', y: 'top' },
                  of: window,
                },
              },
            },
            hide: {
              type: 'slide',
              duration: 400,
              from: {
                position: {
                  my: { x: 'right', y: 'top' },
                  at: { x: 'right', y: 'top' },
                  of: window,
                },
              },
              to: {
                position: {
                  my: { x: 'right', y: 'top' },
                  at: { x: 'left', y: 'top' },
                  of: window,
                },
              },
            },
          }}
          visible={showMenuPopover}
          showCloseButton={false}
          showTitle={false}
          closeOnOutsideClick={true}
          onHiding={() => {
            setMenuPopover(false);
          }}
        >
          <div className="menu-popup-body">
            <div className="close-img-block">
              <img
                className="close-img"
                alt="close"
                src="/img/x-m-object.png"
                srcSet="/img/x-m-object@2x.png 2x, /img/x-m-object@3x.png 3x"
                onClick={() => setMenuPopover(false)}
              ></img>
            </div>
            <div className="country-block">
              <Country
                onClicked={(e: string) => {
                  if (account) {
                    DoSignOut('Country changed!');
                  }
                }}
              />
            </div>

            <ScrollView width="100%" height={`${window.innerHeight - 76}px`}>
              <div className="container menu-content">
                <div className="row">
                  <div className="col-12 menu-item">
                    {account == null && (
                      <div
                        className="menu-item-name"
                        onClick={() => {
                          setMenuPopover(false);
                          navigate('/login');
                        }}
                      >
                        Log In / Create Account
                      </div>
                    )}
                    {account && (
                      <Fragment>
                        <div
                          className="menu-item-name"
                          onClick={() => {
                            setAccountPopover(!showAccountPopover);
                          }}
                        >
                          M<span className="smaller-letter">Y ACCOUNT</span>
                        </div>
                        <div
                          className={
                            showAccountPopover
                              ? 'CloseSubmenuImg open-submenu'
                              : 'OpenSubmenuImg open-submenu'
                          }
                          onClick={() => {
                            setAccountPopover(!showAccountPopover);
                          }}
                        />
                      </Fragment>
                    )}

                    {account && showAccountPopover && (
                      <Fragment key="k-open-about">
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
                            css={css`
                              display: inline-block;
                              white-space: nowrap;
                            `}
                          >
                            <button
                              className="a-btn"
                              onClick={() => {
                                setMenuPopover(false);
                                navigate('/account');
                                dispatch(profile_page());
                              }}
                            >
                              Member Profile
                            </button>
                          </div>
                        </div>
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
                            css={css`
                              display: inline-block;
                              white-space: nowrap;
                            `}
                          >
                            <button
                              className="a-btn"
                              onClick={() => {
                                setMenuPopover(false);
                                navigate('/account');
                                dispatch(order_history_page());
                              }}
                            >
                              Order History
                            </button>
                          </div>
                        </div>
                        {account && account.memberType === 1 && account.hasPatients === true && (
                          <div className="row">
                            <div
                              className="col-12 submenu-item-m"
                              css={css`
                                display: inline-block;
                                white-space: nowrap;
                              `}
                            >
                              <button
                                className="a-btn"
                                onClick={() => {
                                  setMenuPopover(false);
                                  navigate('/account');
                                  dispatch(patient_order_history_page());
                                }}
                              >
                                Patient Order History
                              </button>
                            </div>
                          </div>
                        )}
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
                            css={css`
                              display: inline-block;
                              white-space: nowrap;
                            `}
                          >
                            <button
                              className="a-btn"
                              onClick={() => {
                                setMenuPopover(false);
                                navigate('/account');
                                dispatch(change_password_page());
                              }}
                            >
                              Change Password
                            </button>
                          </div>
                        </div>
                      </Fragment>
                    )}
                  </div>
                </div>
                <div className="row">
                  <div className="col-12 menu-item">
                    <div
                      className="menu-item-name"
                      onClick={() => setAboutPopover(!showAboutPopover)}
                    >
                      A<span className="smaller-letter">BOUT</span>
                    </div>
                    <div
                      className={
                        showAboutPopover
                          ? 'CloseSubmenuImg open-submenu'
                          : 'OpenSubmenuImg open-submenu'
                      }
                      onClick={() => setAboutPopover(!showAboutPopover)}
                    />
                    {showAboutPopover && (
                      <Fragment key="k-open-about">
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                      </Fragment>
                    )}
                  </div>
                </div>
                <div className="row">
                  <div className="col-12 menu-item">
                    <div
                      className="menu-item-name"
                      onClick={() => setQualityPopover(!showQualityPopover)}
                    >
                      Q<span className="smaller-letter">UALITY</span>
                    </div>
                    <div
                      className={
                        showQualityPopover
                          ? 'CloseSubmenuImg open-submenu'
                          : 'OpenSubmenuImg open-submenu'
                      }
                      onClick={() => setQualityPopover(!showQualityPopover)}
                    />
                    {showQualityPopover && (
                      <Fragment key="k-quality-about">
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                      </Fragment>
                    )}
                  </div>
                </div>
                <div className="row">
                  <div className="col-12 menu-item">
                    <div
                      className="menu-item-name"
                      onClick={() => setProductCategoryPopover(!showProductCategoryPopover)}
                    >
                      P<span className="smaller-letter">RODUCTS</span>
                    </div>
                    <div
                      className={
                        showProductCategoryPopover
                          ? 'CloseSubmenuImg open-submenu'
                          : 'OpenSubmenuImg open-submenu'
                      }
                      onClick={() => setProductCategoryPopover(!showProductCategoryPopover)}
                    />
                    {showProductCategoryPopover && (
                      <Fragment key="k-product-about">
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
                            css={css`
                              display: inline-block;
                              white-space: nowrap;
                            `}
                          >
                            <a
                              css={css`
                                padding: 0px;
                              `}
                              href="/products"
                              onClick={() => {
                                dispatch(productCodeChanged(''));
                                dispatch(byCategory());
                                dispatch(productCategoryChanged(''));
                                return false;
                              }}
                            >
                              Product Categories
                            </a>
                          </div>
                        </div>
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
                            css={css`
                              display: inline-block;
                              white-space: nowrap;
                            `}
                          >
                            <a
                              css={css`
                                padding: 0px;
                              `}
                              href="/products"
                              onClick={() => {
                                dispatch(productCodeChanged(''));
                                dispatch(productCategoryChanged(''));
                                dispatch(byAlphabet());
                                return false;
                              }}
                            >
                              Product Alphabet
                            </a>
                          </div>
                        </div>
                        {(!account || account.memberType !== 2) && (
                          <div className="row product-catalogue-download-m">
                            <div className="col-12">
                              <div
                                css={css`
                                  width: fit-content;
                                  margin-left: auto;
                                  margin-right: 7px;
                                `}
                                onClick={() => {
                                  const url = `${process.env
                                    .REACT_APP_PRODUCT_DIR!}datasheet/Product_Catalogue_2025.pdf`;
                                  if (!account) {
                                    // dispatch(urlAfterLoginChanged(url));
                                    // dispatch(memberTypeForURLAfterLoginChanged(2));
                                    //navigate('/login');
                                    dispatch(visiblePractitionerOnlyMsgBoxChanged(true));
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
                                <div className="btn-download" />
                                <span className="download-title">Product Catalogue</span>
                              </div>
                            </div>
                          </div>
                        )}
                      </Fragment>
                    )}
                  </div>
                </div>
                <div className="row">
                  <div className="col-12 menu-item">
                    <div
                      className="menu-item-name"
                      onClick={() => setEducationPopover(!showEducationPopover)}
                    >
                      P<span className="smaller-letter">RACTITIONERS</span>
                    </div>
                    <div
                      className={
                        showEducationPopover
                          ? 'CloseSubmenuImg open-submenu'
                          : 'OpenSubmenuImg open-submenu'
                      }
                      onClick={() => setEducationPopover(!showEducationPopover)}
                    />
                    {showEducationPopover && (
                      <Fragment key="k-education-about">
                        <div className="row">
                          <div
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                            className="col-12 submenu-item-m"
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
                      </Fragment>
                    )}
                  </div>
                </div>
                <div className="row">
                  <div className="col-12 menu-item">
                    <div
                      className="menu-item-name"
                      onClick={() => {
                        setMenuPopover(false);
                        navigate('/infoforpatients');
                      }}
                    >
                      P<span className="smaller-letter">ATIENTS</span>
                    </div>
                  </div>
                </div>
                {account && (
                  <div className="row">
                    <div className="col-12 menu-item">
                      <div
                        className="menu-item-name"
                        css={css`
                          color: var(--peacock-blue) !important;
                        `}
                        onClick={() => {
                          setMenuPopover(false);
                          DoSignOut('');
                        }}
                      >
                        S<span className="smaller-letter">IGN OUT</span>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            </ScrollView>
          </div>
        </Popup>

        <Popover
          position={{
            my: { x: 'left', y: 'top' },
            at: { x: 'left', y: 'top' },
            offset: { x: 0, y: styles.topOffset - 9 },
            of: window,
          }}
          className="search-popover-m"
          visible={showSearchPopover}
          closeOnOutsideClick={true}
          shading={true}
          shadingColor="rgba(0, 0, 0, 0.4)"
          onShown={(e: any) => {
            ////e.component.focus();
            ////const input: HTMLInputElement = document.getElementsByClassName(
            ////  'search-input',
            ////)[0] as HTMLInputElement;
            ////input.focus();
            ////input.scrollIntoView();
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
          <div className="container search-popover-body">
            <div className="search-close-block">
              <button
                className="img-btn search-close-img-btn"
                onClick={() => {
                  setSearchPopover(false);
                  if (timerID) {
                    clearTimeout(timerID!);
                    timerID = null;
                  }
                }}
              >
                <img
                  className="search-close-img"
                  alt="close"
                  src="/img/x-m-object.png"
                  srcSet="/img/x-m-object@2x.png 2x, /img/x-m-object@3x.png 3x"
                ></img>
              </button>
            </div>
            <SearchModule refSearch={refSearchInput} />
          </div>
        </Popover>
        <MessageBox
          Title="PLEASE NOTE:"
          Message=""
          Type="INFO"
          IsVisible={isVisiblePractitionerOnlyMsgBox}
          onVisibleChange={() => {
            dispatch(visiblePractitionerOnlyMsgBoxChanged(false));
          }}
          // BeforeAction={() => {
          //   dispatch(visiblePractitionerOnlyMsgBoxChanged(false));
          //   setMenuPopover(false);
          // }}
        >
          The following pages
          {account ? '' : 'require log-in and '} are only available to licensed health care
          practitioners.
        </MessageBox>
        <MessageBox
          Title="PLEASE NOTE"
          Message=""
          Type="LOGIN"
          IsVisible={requireLoginMessage !== MessageID.NONE}
          onVisibleChange={() => dispatch(requireLoginMessageChanged(MessageID.NONE))}
        >
          {requireLoginMessage === MessageID.QT && (
            <Fragment>
              The information provided in{' '}
              <span
                css={css`
                  color: #003655;
                `}
              >
                [QualityTRAK]
              </span>{' '}
              requires log-in.
            </Fragment>
          )}
          {requireLoginMessage === MessageID.SHOPPING && (
            <Fragment>Please log-in to continue shopping.</Fragment>
          )}
          {requireLoginMessage === MessageID.CATALOGUE && (
            <Fragment>
              The information provided in{' '}
              <span
                css={css`
                  color: #003655;
                `}
              >
                [Product Catalogue]
              </span>{' '}
              requires log-in and is only available to licensed health care practitioners.
            </Fragment>
          )}
          {requireLoginMessage === MessageID.BLOG && (
            <Fragment>
              The information provided in{' '}
              <span
                css={css`
                  color: #003655;
                `}
              >
                [Blog]
              </span>{' '}
              requires log-in and is only available to licensed health care practitioners.
            </Fragment>
          )}
          {requireLoginMessage === MessageID.WEBINARS && (
            <Fragment>
              The information provided in{' '}
              <span
                css={css`
                  color: #003655;
                `}
              >
                [Webinars]
              </span>{' '}
              requires log-in and is only available to licensed health care practitioners.
            </Fragment>
          )}
          {requireLoginMessage === MessageID.TP && (
            <Fragment>
              The information provided in{' '}
              <span
                css={css`
                  color: #003655;
                `}
              >
                [Therapeutic Protocol]
              </span>{' '}
              requires log-in and is only available to licensed health care practitioners.
            </Fragment>
          )}
        </MessageBox>
      </div>
    </div>
  );
}
