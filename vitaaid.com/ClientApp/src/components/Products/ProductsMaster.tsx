/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { UnitTypeData, getProductCategories, getAllergyCategories } from 'model/UnitType';
import { Popup } from 'devextreme-react/popup';
import {
  productCategory,
  productCategoryChanged,
} from 'redux/features/product/productCategorySlice';
import {
  productFilterMethod,
  byCategory,
  byAlphabet,
  byKeyword,
  eFILTERMETHOD,
} from 'redux/features/product/productFilterMethodSlice';
import { productCode, productCodeChanged } from 'redux/features/product/productCodeSlice';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { useSelector, useDispatch } from 'react-redux';
import {
  getProductsByCategory,
  ProductData,
  getProductsByKeyword,
  getProductsByAlphabet,
} from 'model/Product';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { TextBox, Button as TextBoxButton } from 'devextreme-react/text-box';
import productSearchImg from 'img/product-search.png';
import { ProductInfoInCategory } from 'components/Products/ProductInfoInCategory';
import { productAlphabets } from 'redux/features/product/productAlphabetsSlice';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { ScrollView } from 'devextreme-react';
import styles from 'scss/abstracts/_variables.scss';
interface Props {
  isMobile: boolean;
}
export const ProductsMaster = ({ isMobile }: Props) => {
  const [categories, setCategories] = React.useState<UnitTypeData[]>([]);
  const [products, setProducts] = React.useState<ProductData[]>([]);
  const [qtLotNo, setQtLotNo] = React.useState<string>('');
  const [productKeyword, setProductKeyword] = React.useState<string>('');
  const [showCategoryPopup, setShowCategoryPopup] = React.useState<boolean>(false);
  const [showAlphabetPopup, setShowAlphabetPopup] = React.useState<boolean>(false);
  const country = useSelector(selectedCountry);
  const category = useSelector(productCategory);
  const filterMethod = useSelector(productFilterMethod);
  const code = useSelector(productCode);
  const account = useSelector(accountData);
  const alphabetList = useSelector(productAlphabets);
  const dispatch = useDispatch();
  let navigate = useNavigate();

  React.useEffect(() => {
    async function fetchCategoryData() {
      const data = await getProductCategories();
      setCategories(data);
    }
    async function fetchProductData() {
      const data = await getProductsByAlphabet(category, country);
      setProducts(data);
    }

    if (filterMethod === eFILTERMETHOD.CATEGORY) {
      if (categories.length === 0) fetchCategoryData();
      else {
        if (category.length === 0) dispatch(productCategoryChanged(categories[0].name));
      }
    } else if (filterMethod === eFILTERMETHOD.ALPHABET) {
      if (category.length === 0) dispatch(productCategoryChanged('A'));
      else {
        fetchProductData();
      }
    }
    return () => {
      setCategories([]);
    };
  }, [filterMethod]);

  React.useEffect(() => {
    if (categories.length === 0 || filterMethod !== eFILTERMETHOD.CATEGORY) return;

    async function fetchData() {
      const selected = categories.filter((x) => x.name === category)[0];
      const data = await getProductsByCategory(parseInt(selected.abbrName, 10), country);
      setProducts(data);
    }

    if (filterMethod === eFILTERMETHOD.CATEGORY && category.length === 0)
      dispatch(productCategoryChanged(categories[0].name));
    else {
      fetchData();
    }
  }, [categories]);

  React.useEffect(() => {
    async function fetchCategoryData() {
      const data = await getProductCategories();
      setCategories(data);
    }
    if (category.length === 0) {
      if (filterMethod === eFILTERMETHOD.CATEGORY && categories.length > 0)
        dispatch(productCategoryChanged(categories[0].name));
      return;
    }
    if (filterMethod === eFILTERMETHOD.CATEGORY && categories.length === 0) {
      fetchCategoryData();
      return;
    }
    async function fetchData() {
      let data: ProductData[] = [];
      if (filterMethod === eFILTERMETHOD.CATEGORY) {
        const selected = categories.filter((x) => x.name === category)[0];
        data = await getProductsByCategory(parseInt(selected.abbrName, 10), country);
      } else if (filterMethod === eFILTERMETHOD.ALPHABET)
        data = await getProductsByAlphabet(category, country);
      setProducts(data);
    }
    fetchData();
    return () => {
      setProducts([]);
    };
  }, [category, country]);

  const qtSearchButton = {
    icon: productSearchImg,
    type: 'default',
    stylingMode: 'text',
    elementAttr: {
      class: 'product-search-btn',
    },
    onClick: () => {
      goToQtPage();
    },
    focusStateEnabled: true,
  };

  const goToQtPage = () => {
    const url = `/qualitytrak/${country}?lotno=${qtLotNo}`;
    if (account) {
      navigate(url);
    } else {
      dispatch(urlAfterLoginChanged(url));
      dispatch(memberTypeForURLAfterLoginChanged(0));
      dispatch(openLoginDlg());
    }
  };

  return (
    <Fragment>
      <div className="content-main-body products">
        {isMobile === false && filterMethod === eFILTERMETHOD.ALPHABET && (
          <Fragment>
            <div className="row img-fluid categories-filter">
              <div className="col-6 align-self-end title">By Alphabet</div>
            </div>
            <div key={category} className="row">
              <table className="categories-list">
                <tbody>
                  <tr>
                    <td>
                      {alphabetList &&
                        (alphabetList as string[]).map((x, idx) => (
                          <Fragment>
                            <div
                              css={css`
                                display: inline-block;
                                white-space: nowrap;
                              `}
                              key={`alphabet-${idx}`}
                            >
                              <button
                                css={css`
                                  padding: 0px;
                                  color: var(--marine-blue);
                                `}
                                className="borderless-btn"
                                onClick={() => dispatch(productCategoryChanged(x))}
                              >
                                <span className="alphabet-char">{x}</span>
                              </button>
                              {idx < (alphabetList as string[]).length - 1 && (
                                <span className="delimiter">|</span>
                              )}
                              {idx >= (alphabetList as string[]).length - 1 && (
                                <span className="delimiter"></span>
                              )}
                            </div>
                          </Fragment>
                        ))}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </Fragment>
        )}
        {isMobile === false && filterMethod === eFILTERMETHOD.CATEGORY && (
          <Fragment>
            <div className="row img-fluid categories-filter">
              <div className="col-6 align-self-end title">All Categories</div>
            </div>
            <div key={category} className="row">
              <table className="categories-list">
                <tbody>
                  <tr>
                    <td>
                      {categories.map((x) => (
                        <div
                          css={css`
                            display: inline-block;
                            white-space: nowrap;
                          `}
                          key={x.id}
                        >
                          <button
                            css={css`
                              padding: 0px;
                              color: var(--marine-blue);
                            `}
                            className="borderless-btn"
                            onClick={() => dispatch(productCategoryChanged(x.name))}
                          >
                            {x.name}
                          </button>
                          <span className="delimiter">|</span>
                        </div>
                      ))}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </Fragment>
        )}

        {isMobile && filterMethod === eFILTERMETHOD.CATEGORY && (
          <Fragment>
            <div className="method-type-block">
              <div>
                <span
                  css={css`
                    font-size: 25px;
                    color: var(--marine-blue);
                  `}
                >
                  Products
                </span>
                <span
                  css={css`
                    font-size: 18px;
                    color: var(--marine-blue);
                  `}
                >
                  {' '}
                  | By Categories
                </span>
              </div>
              <div className="open-category-m" onClick={() => setShowCategoryPopup(true)} />
            </div>
          </Fragment>
        )}
        {isMobile && filterMethod === eFILTERMETHOD.ALPHABET && (
          <Fragment>
            <div className="method-type-block">
              <div>
                <span
                  css={css`
                    font-size: 25px;
                    color: var(--marine-blue);
                  `}
                >
                  Products
                </span>
                <span
                  css={css`
                    font-size: 18px;
                    color: var(--marine-blue);
                  `}
                >
                  {' '}
                  | By Alphabet
                </span>
              </div>
              <div className="open-alphabet-m" onClick={() => setShowAlphabetPopup(true)} />
            </div>
          </Fragment>
        )}
        <div className="row product-list-title">
          <div className="col-12 title">
            {filterMethod === eFILTERMETHOD.CATEGORY && category.length > 0 && (
              <Fragment>{category}</Fragment>
            )}
            {filterMethod === eFILTERMETHOD.ALPHABET && category.length > 0 && (
              <Fragment>{category.toUpperCase()}</Fragment>
            )}
            {filterMethod === eFILTERMETHOD.KEYWORD && productKeyword && (
              <Fragment>{`Search Results For "${productKeyword}"`}</Fragment>
            )}
          </div>
        </div>
        {products && (
          <div className="row product-list">
            {products.map((x, idx) => {
              return (
                <ProductInfoInCategory key={`${x.id}`} idx={idx} data={x} isMobile={isMobile} />
              );
            })}
          </div>
        )}
      </div>
      <div className="row qt-block">
        <div
          className="col-12"
          css={css`
            margin: auto;
          `}
        >
          <div className="qt-searchBlock">
            <table>
              <tbody>
                <tr>
                  <td>
                    <div className="labelstr">
                      <span
                        css={css`
                          color: var(--peacock-blue);
                        `}
                      >
                        Quality
                      </span>
                      <span
                        css={css`
                          color: var(--marine-blue);
                        `}
                      >
                        Trak
                      </span>
                      <span className="tm">TM</span>
                    </div>
                  </td>
                  <td>
                    <TextBox
                      id="qtLotNo"
                      placeholder="Enter lot #"
                      onKeyUp={(e: any) => {
                        if (e.event.key === 'Enter') goToQtPage();
                        else setQtLotNo(e.event.currentTarget.value);
                      }}
                    >
                      <TextBoxButton
                        name="btnSearch"
                        location="after"
                        options={qtSearchButton}
                      ></TextBoxButton>
                    </TextBox>
                  </td>
                </tr>
              </tbody>
            </table>
            <ul className="tipstr">
              <li>Enter your lot # to see quality testing on your product.</li>
            </ul>
          </div>
        </div>
      </div>
      <Popup
        position={{
          my: { x: 'left', y: 'top' },
          at: { x: 'left', y: 'top' },
          offset: { x: 0, y: styles.topOffset },
          of: window,
        }}
        animation={{
          show: {
            type: 'slide',
            duration: 400,
            from: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'right', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
            to: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'left', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
          },
          hide: {
            type: 'slide',
            duration: 400,
            from: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'left', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
            to: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'right', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
          },
        }}
        className="product-category-filter-popup"
        visible={showCategoryPopup}
        closeOnOutsideClick={true}
        shading={true}
        shadingColor="rgba(0, 0, 0, 0.4)"
        showTitle={true}
        showCloseButton={false}
        titleRender={() => {
          return (
            <Fragment>
              <div className="method-type-block">
                <div>
                  <span
                    css={css`
                      font-size: 25px;
                      color: var(--marine-blue);
                    `}
                  >
                    Products
                  </span>
                  <span
                    css={css`
                      font-size: 18px;
                      color: var(--marine-blue);
                    `}
                  >
                    {' '}
                    | By Categories
                  </span>
                </div>
                <div className="close-category-m" onClick={() => setShowCategoryPopup(false)} />
              </div>
            </Fragment>
          );
        }}
        onHidden={() => setShowCategoryPopup(false)}
      >
        <ScrollView width="100%" height={`${window.innerHeight - styles.topOffset - 50}px`}>
          <div className="product-filter-popup-body">
            {categories.map((x) => (
              <div key={x.id}>
                <button
                  className="borderless-btn product-filter-popup-item"
                  onClick={() => {
                    dispatch(productCategoryChanged(x.name));
                    setShowCategoryPopup(false);
                  }}
                >
                  {x.name}
                </button>
              </div>
            ))}
          </div>
        </ScrollView>
      </Popup>
      <Popup
        position={{
          my: { x: 'left', y: 'top' },
          at: { x: 'left', y: 'top' },
          offset: { x: 0, y: styles.topOffset },
          of: window,
        }}
        animation={{
          show: {
            type: 'slide',
            duration: 400,
            from: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'right', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
            to: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'left', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
          },
          hide: {
            type: 'slide',
            duration: 400,
            from: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'left', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
            to: {
              position: {
                my: { x: 'left', y: 'top' },
                at: { x: 'right', y: 'top' },
                offset: { y: styles.topOffset },
                of: window,
              },
            },
          },
        }}
        className="product-alphabet-filter-popup"
        visible={showAlphabetPopup}
        closeOnOutsideClick={true}
        shading={true}
        shadingColor="rgba(0, 0, 0, 0.4)"
        showTitle={true}
        showCloseButton={false}
        titleRender={() => {
          return (
            <Fragment>
              <div className="method-type-block">
                <div>
                  <span
                    css={css`
                      font-size: 25px;
                      color: var(--marine-blue);
                    `}
                  >
                    Products
                  </span>
                  <span
                    css={css`
                      font-size: 18px;
                      color: var(--marine-blue);
                    `}
                  >
                    {' '}
                    | By Alphabet
                  </span>
                </div>
                <div className="close-alphabet-m" onClick={() => setShowAlphabetPopup(false)} />
              </div>
            </Fragment>
          );
        }}
        onHidden={() => setShowAlphabetPopup(false)}
      >
        <ScrollView width="100%" height={190}>
          <div className="row product-filter-popup-body">
            {alphabetList &&
              (alphabetList as string[]).map((x, idx) => (
                <Fragment>
                  <button
                    key={`alphabet-${idx}`}
                    className="col-2 borderless-btn product-filter-popup-item"
                    onClick={() => {
                      dispatch(productCategoryChanged(x));
                      setShowAlphabetPopup(false);
                    }}
                  >
                    <span className="alphabet-char">{x}</span>
                  </button>
                </Fragment>
              ))}
          </div>
        </ScrollView>
      </Popup>
    </Fragment>
  );
};
