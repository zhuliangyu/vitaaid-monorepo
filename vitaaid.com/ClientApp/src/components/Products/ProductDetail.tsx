/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import ReactImageMagnify from 'lib/react-image-magnify/ReactImageMagnify';
import { UnitTypeData, getProductCategories, getAllergyCategories } from 'model/UnitType';
import ScrollView from 'devextreme-react/scroll-view';

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
import { BreadCrumbsBlock } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { accountData } from 'redux/features/account/accountSlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import {
  bCartBrief,
  viewCartBrief,
  hideCartBrief,
} from 'redux/features/shoppingcart/showCartBriefSlice';
import { useSelector, useDispatch } from 'react-redux';
import {
  ProductData,
  getProduct,
  ProductImageData,
  getRelatedProducts,
  snp,
  getStockNPrice,
} from 'model/Product';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { TextBox, Button as TextBoxButton } from 'devextreme-react/text-box';
import productSearchImg from 'img/product-search.png';
import { inMemoryShoppingCartToken } from 'model/ShoppingCartToken';
import { WishData, putWishProduct } from 'model/Product';
import {
  cartChanged,
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  addCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import {
  shopping_cart_page,
  shopping_summary_page,
} from '../../redux/features/shoppingcart/cartPageSlice';
import { Popover, ToolbarItem } from 'devextreme-react/popover';
import { Popup } from 'devextreme-react/popup';
import { MessageBox } from 'components/MessageBox';
import { visiblePractitionerOnlyMsgBoxChanged } from 'redux/features/visiblePractitionerOnlyMsgBoxSlice';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { requireLoginMessageChanged, MessageID } from 'redux/features/requireLoginMessageSlice';
import { Center } from 'devextreme-react/map';

interface ToggleProps {
  IsVisible: boolean;
  onVisibleChange: () => void;
}
const ToggleButton = ({ IsVisible, onVisibleChange }: ToggleProps) => {
  return (
    <button
      className={IsVisible ? 'minus_button' : 'plus_button'}
      onClick={() => {
        onVisibleChange();
      }}
    ></button>
  );
};

interface RelatedProdProps {
  product: ProductData;
}
const RelatedProduct = ({ product }: RelatedProdProps) => {
  const dispatch = useDispatch();
  let navigate = useNavigate();
  const location = useLocation();
  const useQuery = () => new URLSearchParams(location.search);
  let query = useQuery();
  const pcode = query.get('pcode');

  const GoToRelatedProductDetail = (relatedProductCode: string) => {
    dispatch(byKeyword());
    dispatch(productCodeChanged(relatedProductCode));
    dispatch(productCategoryChanged(''));
    if (pcode && pcode !== relatedProductCode) {
      navigate(`/products?pcode=${relatedProductCode}`);
    }
  };

  return (
    <Fragment>
      <div>
        <img
          className="related-product-img"
          alt={`${product.productName}`}
          src={`${process.env.REACT_APP_PRODUCT_DIR!}${product.representativeImage}`}
          onClick={() => {
            GoToRelatedProductDetail(product.productCode);
            return true;
          }}
        ></img>
      </div>
      <div className="related-product-name">
        <button
          className="a-btn"
          dangerouslySetInnerHTML={{ __html: product.productName }}
          onClick={() => {
            GoToRelatedProductDetail(product.productCode);
            return true;
          }}
        />
      </div>
    </Fragment>
  );
};

interface Props {
  code: string;
  allergyCategoriesData: UnitTypeData[];
}

export const ProductDetail = ({ code, allergyCategoriesData }: Props) => {
  const useStateWithLabel = (initialValue: any, label: string) => {
    const [value, setValue] = React.useState(initialValue);
    React.useDebugValue(`${label}: ${value}`);
    return [value, setValue];
  };

  const [product, setProduct] = React.useState<ProductData>();
  const [stockCount, setStockCount] = React.useState<number>(0);
  const [unitPrice, setUnitPrice] = React.useState<number>(0); //useStateWithLabel(0, 'unitPrice');
  const [dropShipPrice, setDropShipPrice] = React.useState<number>(0); //useStateWithLabel(0, 'unitPrice');

  const [relatedProducts, setRelatedProducts] = React.useState<ProductData[]>([]);
  const [currentPageOfRelatedProducts, setCurrentPageOfRelatedProducts] = React.useState<number>(0);
  const [totalPagesOfRelatedProducts, setTotalPagesOfRelatedProducts] = React.useState<number>(0);

  const [focusedImage, setFocusedImage] = React.useState<ProductImageData>();
  const [qtLotNo, setQtLotNo] = React.useState<string>('');
  const [infoVisible, setInfoVisible] = React.useState<boolean>(true);
  const [ingredientsVisible, setIngredientsVisible] = React.useState<boolean>(true);
  const [suggestedUseVisible, setSuggestedUseVisible] = React.useState<boolean>(true);
  const [cautionVisible, setCautionVisible] = React.useState<boolean>(true);
  const [allergyCategory, setAllergyCategory] =
    React.useState<UnitTypeData[]>(allergyCategoriesData);
  const [showAlert, setShowAlert] = React.useState<boolean>(false);
  const [showZoomingImage, setZoomingImage] = React.useState<boolean>(false);
  const [wishProductSubmitFinish, setWishProductSubmitSuccess] = React.useState(false);
  const country = useSelector(selectedCountry);
  const account = useSelector(accountData);
  const category = useSelector(productCategory);
  const filterMethod = useSelector(productFilterMethod);
  const isMobile = useSelector(isMobileData);
  let navigate = useNavigate();
  const dispatch = useDispatch();
  const pathname = useLocation().pathname;
  let qtyRef = useRef<HTMLInputElement | null>(null);
  let reqQtyRef = useRef<HTMLInputElement | null>(null);

  const [touchStart, setTouchStart] = React.useState(0);
  const [touchEnd, setTouchEnd] = React.useState(0);
  const handleTouchStart = (e: any) => {
    if (e.targetTouches.length === 1) {
      setTouchStart(e.targetTouches[0].clientX);
    } else setTouchStart(0);
    e.preventDefault();
  };
  const handleTouchMove = (e: any) => {
    if (e.targetTouches.length === 1) {
      setTouchEnd(e.targetTouches[0].clientX);
    } else setTouchEnd(0);
    e.preventDefault();
  };
  function handleTouchEnd() {
    const diff = touchStart - touchEnd;
    if (touchStart === 0) return;
    setTouchStart(0);
    if (touchEnd === 0) {
      setZoomingImage(true);
      return;
    }
    setTouchEnd(0);

    var idx = 0;
    for (idx = 0; idx < (product?.oProductImages?.length ?? 0); idx++) {
      if (product!.oProductImages[idx].id === focusedImage!.id) break;
    }
    if (diff > 25 && idx < product!.oProductImages!.length - 1) {
      // do your stuff here for left swipe
      setFocusedImage(product!.oProductImages[idx + 1]);
    }
    if (diff < -25 && idx > 0) {
      // do your stuff here for right swipe
      setFocusedImage(product!.oProductImages[idx - 1]);
    }
  }

  React.useEffect(() => {
    if (!code) navigate('/products');
    async function fetchAllergyCategoriesData() {
      const data = await getAllergyCategories();
      setAllergyCategory(data);
    }

    async function fetchData() {
      // get product detail information
      const data1 = await getProduct(code, country);
      if (Object.keys(data1).length == 0) {
        navigate('/products');
        return;
      }
      setProduct(data1);
      // get stock and price
      if (
        account != null &&
        account.id > 0 &&
        account.customerCode != null &&
        account.customerCode.length > 0
      ) {
        const data = await getStockNPrice(code, account.customerCode, country);
        setStockCount(data.stock);
        setUnitPrice(data.price);
        setDropShipPrice(data.dropShipPrice);
      }
      // get product images
      if (data1.oProductImages.length > 0) setFocusedImage(data1.oProductImages[0]);
      // get related products
      const data2 = await getRelatedProducts(code, country);
      if (data2.length > 0) setTotalPagesOfRelatedProducts(Math.ceil(data2.length / 4));
      else setTotalPagesOfRelatedProducts(0);
      setCurrentPageOfRelatedProducts(0);
      setRelatedProducts(data2);
    }
    if (allergyCategoriesData.length === 0) fetchAllergyCategoriesData();
    fetchData();
  }, []);

  React.useEffect(() => {
    if (
      account == null ||
      account.id == 0 ||
      account.customerCode == null ||
      account.customerCode.length == 0
    )
      return;

    async function fetchData() {
      const data = await getStockNPrice(code, account.customerCode, country);
      setStockCount(data.stock);
      setUnitPrice(data.price);
      setDropShipPrice(data.dropShipPrice);
    }
    if (product != null && unitPrice <= 0) fetchData();
  }, [account]);

  const goToQtPage = () => {
    const url = `/qualitytrak/${country}?lotno=${qtLotNo}`;
    if (account) {
      navigate(url);
    } else {
      dispatch(urlAfterLoginChanged(url));
      dispatch(memberTypeForURLAfterLoginChanged(0));
      if (isMobile) dispatch(requireLoginMessageChanged(MessageID.QT));
      else dispatch(openLoginDlg());
    }
  };

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

  const addItemToCart = (
    code: string,
    name: string,
    price: number,
    dropShipPrice: number,
    size: string,
    imageName: string,
  ) => {
    if (!(qtyRef.current && qtyRef.current.value)) return;

    const qty = parseInt(qtyRef.current!.value, 10);
    if (!qty) return;

    const item: ShoppingCartItem = {
      code: code,
      name: name,
      qty: qty,
      price: price,
      dropShipPrice,
      size: size,
      image: imageName,
    };
    dispatch(addCartItem(item));
    //  setShowAlert(true);
  };

  const RequestSumbit = async (productCode: string) => {
    if (!(reqQtyRef.current && reqQtyRef.current.value)) return;

    const qty = parseInt(reqQtyRef.current!.value, 10);
    if (!qty) return;
    const wishData: WishData = { customerCode: account.customerCode, qty: qty };
    await putWishProduct(productCode, wishData);
    setWishProductSubmitSuccess(true);
  };

  return (
    <Fragment>
      <div className="content-main-body">
        {product && (
          <BreadCrumbsBlock
            subNodes={['Products', product.productName]}
            actions={[
              () => {
                dispatch(productCodeChanged(''));
                dispatch(byCategory());
                dispatch(productCategoryChanged(''));
                navigate('/products');
              },
              null,
            ]}
          />
        )}
        <div className="va-product-page">
          <div className="col-12">
            <div className="row product-header-container">
              {product && (
                <div className="col-12">
                  <div className="row">
                    <div className={isMobile ? 'col-12' : 'col-6'}>
                      <div
                        className="row"
                        css={css`
                          height: 100%;
                        `}
                      >
                        {isMobile === false && (
                          <div className="col-2">
                            {product.oProductImages.map((i) => {
                              return (
                                <img
                                  key={`${i.id}`}
                                  className="thumb-image"
                                  alt=""
                                  src={`${process.env.REACT_APP_PRODUCT_DIR!}${i.imageName}`}
                                  onClick={() => {
                                    setFocusedImage(i);
                                  }}
                                />
                              );
                            })}
                          </div>
                        )}
                        <div className={isMobile ? 'col-12' : 'col-10'}>
                          <div className="row align-items-top product-image-block">
                            {isMobile && (
                              <Fragment>
                                <img
                                  className="col-12 large-image"
                                  alt=""
                                  src={`${process.env.REACT_APP_PRODUCT_DIR!}${
                                    focusedImage ? focusedImage.imageName : 'EmptyProduct.png'
                                  }`}
                                  onTouchStart={handleTouchStart}
                                  onTouchMove={handleTouchMove}
                                  onTouchEnd={handleTouchEnd}
                                ></img>
                                <div className="dot-img-div">
                                  {product.oProductImages.map((i) => {
                                    return (
                                      <div
                                        key={`${i.id}`}
                                        className={
                                          i.id === (focusedImage?.id ?? 0)
                                            ? 'focused-dot-image'
                                            : 'dot-image'
                                        }
                                        onClick={() => {
                                          setFocusedImage(i);
                                        }}
                                      />
                                    );
                                  })}
                                </div>
                              </Fragment>
                            )}
                            {isMobile === false && (
                              <ReactImageMagnify
                                {...{
                                  className: 'col-12 large-image',
                                  imageClassName: 'large-image',
                                  enlargedImageContainerClassName: 'zoom-image-conrainer',
                                  enlargedImageClassName: 'enlarged-image',
                                  smallImage: {
                                    alt: '',
                                    isFluidWidth: true,
                                    src: `${process.env.REACT_APP_PRODUCT_DIR!}${
                                      focusedImage ? focusedImage.imageName : 'EmptyProduct.png'
                                    }`,
                                    // width: 450,
                                    // height: 450,
                                  },
                                  largeImage: {
                                    src: `${process.env.REACT_APP_PRODUCT_DIR!}${
                                      focusedImage
                                        ? focusedImage.largeImageName
                                        : 'EmptyProduct.png'
                                    }`,
                                    width: focusedImage?.largeWidth ?? 558,
                                    height: focusedImage?.largeHeight ?? 969,
                                  },
                                  enlargedImagePortalId: 'portal',
                                  enlargedImageContainerDimensions: {
                                    width: '160%',
                                    height: '160%',
                                  },
                                }}
                              />
                            )}
                          </div>
                          <div className="row row align-items-end va-product-allergy">
                            <div className="col-12">
                              {allergyCategory &&
                                product.allergyCategory.map((x) => {
                                  const category = allergyCategory.filter(
                                    (a) => parseInt(a.abbrName) === x,
                                  )?.[0];
                                  if (category)
                                    return (
                                      <img
                                        key={`allergy-${x}`}
                                        className="allergy-image"
                                        alt=""
                                        src={`/img/${category.comment}`}
                                      />
                                    );
                                })}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div className={`col-${isMobile ? '12' : '6'} va-product-brif-div`}>
                      <div id="portal" className="portal" />
                      <div className="row align-items-top va-product-info">
                        <div className="col-12">
                          <div
                            className="va-product-name"
                            dangerouslySetInnerHTML={{
                              __html: product.productName,
                            }}
                          ></div>
                          <div
                            className="va-product-function"
                            dangerouslySetInnerHTML={{
                              __html: product.function,
                            }}
                          />
                          <div className="row va-product-code">
                            <div className="col-12">
                              <div className="code-part">Code | {product.productCode}</div>
                              {country === 'CA' && (
                                <div className="npn-part">
                                  {product.npn && product.npn.length > 1 && (
                                    <Fragment>NPN | {product.npn}</Fragment>
                                  )}
                                </div>
                              )}
                              <div className="size-part">Size | {product.size}</div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div className="row align-items-end">
                        <div className="col-12">
                          {account && account.memberStatus === 9 && unitPrice > 0 && (
                            <table key="stocknprice">
                              <tbody>
                                <tr>
                                  <td>
                                    <div className="va-product-price">
                                      ${unitPrice.toFixed(2)}
                                      <span className="va-product-currency">{`${
                                        country === 'CA' ? 'CAD' : 'USD'
                                      }`}</span>
                                    </div>
                                  </td>
                                  <td>
                                    {stockCount <= 0 && (
                                      <Fragment>
                                        <div className="out-of-stock">Out of Stock</div>
                                        {isMobile && (
                                          <button
                                            className="va-back-order"
                                            onClick={() => {
                                              setShowAlert(true);
                                            }}
                                          >
                                            Add to Back Order
                                          </button>
                                        )}
                                      </Fragment>
                                    )}
                                  </td>
                                </tr>
                                {(stockCount > 0 || isMobile === false) && (
                                  <tr>
                                    <td>
                                      <div className="va-product-cart">
                                        <span className="align-middle va-product-qty-label ">
                                          QTY :
                                        </span>
                                        {stockCount <= 0 && (
                                          <input
                                            ref={qtyRef}
                                            type="text"
                                            defaultValue={0}
                                            readOnly={true}
                                            className="align-middle va-product-qty"
                                          ></input>
                                        )}
                                        {stockCount > 0 && (
                                          <input
                                            ref={qtyRef}
                                            type="number"
                                            defaultValue={1}
                                            className="align-middle va-product-qty"
                                          ></input>
                                        )}
                                      </div>
                                    </td>
                                    <td>
                                      {stockCount > 0 && (
                                        <button
                                          className="align-middle va-add-to-cart"
                                          onClick={() =>
                                            addItemToCart(
                                              product.productCode,
                                              product.productName,
                                              unitPrice,
                                              dropShipPrice,
                                              product.size,
                                              product.representativeImage,
                                            )
                                          }
                                        >
                                          Add to Cart
                                        </button>
                                      )}
                                      {stockCount <= 0 && (
                                        <button
                                          className="align-middle va-back-order"
                                          onClick={() => {
                                            setShowAlert(true);
                                          }}
                                        >
                                          Add to Back Order
                                        </button>
                                      )}
                                    </td>
                                  </tr>
                                )}
                              </tbody>
                            </table>
                          )}
                          {!account && (
                            <button
                              className="va-purchase"
                              onClick={() => {
                                if (isMobile)
                                  dispatch(requireLoginMessageChanged(MessageID.SHOPPING));
                                else dispatch(openLoginDlg());
                              }}
                            >
                              Purchase
                            </button>
                          )}
                          <div
                            className="va-product-download"
                            css={css`
                              padding-top: ${product.productSheet ? '15' : '0'}px;
                            `}
                          >
                            {product.productSheet && (
                              <Fragment>
                                <button
                                  className="a-btn borderless-btn"
                                  onClick={() => {
                                    const url = `${process.env.REACT_APP_PRODUCT_DIR!}datasheet/${
                                      product.productSheet
                                    }`;
                                    if (!account) {
                                      dispatch(urlAfterLoginChanged(url));
                                      dispatch(memberTypeForURLAfterLoginChanged(2));
                                      if (isMobile)
                                        dispatch(requireLoginMessageChanged(MessageID.CATALOGUE));
                                      else dispatch(openLoginDlg());
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
                                </button>
                                <span className="download-title">Product Monograph Download</span>
                              </Fragment>
                            )}
                          </div>
                          <div className="va-qt-block">
                            <div className="col-12">
                              <div className="qt-labelstr">
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
                                <div className="tipstr">
                                  - Enter your lot # to see quality testing on your product.
                                </div>
                              </div>
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
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              )}
            </div>
          </div>
          <div className="row product-description-container">
            {product && (
              <Fragment>
                <div className="row product-desc-block">
                  <div className="col-11 product-desc-title">Product Info</div>
                  <div className="col-1">
                    <ToggleButton
                      IsVisible={infoVisible}
                      onVisibleChange={() => setInfoVisible(!infoVisible)}
                    />
                  </div>
                </div>
                {infoVisible && (
                  <div className="row product-content-block">
                    <div
                      className="col-12 product-content"
                      dangerouslySetInnerHTML={{
                        __html: product?.description,
                      }}
                    ></div>
                  </div>
                )}

                <div
                  className={
                    country === 'CA' || ingredientsVisible === false
                      ? 'row product-desc-block'
                      : 'row product-desc-block add-line-us'
                  }
                >
                  <div className="col-11 product-desc-title">
                    {country === 'CA' ? 'Product Facts' : 'Supplement Facts'}
                    {country === 'US' && ingredientsVisible && (
                      <Fragment>
                        <div className="supplement-subtitle">
                          Serving Size: {product.sServingSize}
                        </div>
                        <div className="supplement-subtitle">
                          Servings Per Container: {product.servingsPerContainer}
                        </div>
                      </Fragment>
                    )}
                  </div>
                  <div className="col-1">
                    <ToggleButton
                      IsVisible={ingredientsVisible}
                      onVisibleChange={() => setIngredientsVisible(!ingredientsVisible)}
                    />
                  </div>
                </div>

                {ingredientsVisible && (
                  <div className="row product-content-block">
                    <div className="col-12 product-content">
                      <div className="row ingredients-header">
                        <div className={isMobile ? 'col-12' : 'col-4'}>
                          Medicinal Ingredients {country === 'CA' ? `/ ${product.servingUnit}` : ''}
                        </div>
                        {isMobile === false && (
                          <Fragment>
                            <div className="col-6">Specification</div>
                            <div
                              className="col-2"
                              css={css`
                                text-align: right;
                              `}
                            >
                              Amount
                            </div>
                          </Fragment>
                        )}
                      </div>
                      {product.oIngredients
                        .filter((x) => x.groupNo !== 5)
                        .map((x) => {
                          return (
                            <Fragment>
                              <div key={`ig-${x.id}`} className="row ingredients-content">
                                <div className={isMobile ? 'col-9' : 'col-4'}>{x.name}</div>
                                {isMobile === false && (
                                  <div className="col-6">{x.additionalInfo}</div>
                                )}
                                <div
                                  className={isMobile ? 'col-3' : 'col-2'}
                                  css={css`
                                    text-align: right;
                                  `}
                                >
                                  {country === 'CA' ? x.labelClaim : x.labelClaimUS}
                                </div>
                                {isMobile && (
                                  <div className="col-12 spec-desc-m">{x.additionalInfo}</div>
                                )}
                              </div>
                            </Fragment>
                          );
                        })}
                      {product.additionalInfo && (
                        <div
                          className="additional-info"
                          dangerouslySetInnerHTML={{ __html: product.additionalInfo }}
                        ></div>
                      )}
                      {product.oIngredients
                        .filter((x) => x.groupNo === 5)
                        .map((x) => {
                          return (
                            <Fragment>
                              <div key={`ig-${x.id}`} className="row non-medicinal-ingredients">
                                <div className="col-12">
                                  <div
                                    css={css`
                                      font-weight: 600;
                                    `}
                                  >
                                    Non-medicinal Ingredients:
                                  </div>
                                  <div>{x.name}</div>
                                </div>
                              </div>
                            </Fragment>
                          );
                        })}
                    </div>
                  </div>
                )}
                <div className="row product-desc-block">
                  <div className="col-8 product-desc-title">Suggested Use</div>
                  <div className="col-4">
                    <ToggleButton
                      IsVisible={suggestedUseVisible}
                      onVisibleChange={() => setSuggestedUseVisible(!suggestedUseVisible)}
                    />
                  </div>
                </div>
                {suggestedUseVisible && (
                  <div className="row product-content-block">
                    <div
                      className="col-12 product-content"
                      dangerouslySetInnerHTML={{
                        __html: product!.suggest,
                      }}
                    />
                  </div>
                )}
                {product.caution && (
                  <Fragment>
                    <div className="row product-desc-block">
                      <div className="col-8 product-desc-title">Caution</div>
                      <div className="col-4">
                        <ToggleButton
                          IsVisible={cautionVisible}
                          onVisibleChange={() => setCautionVisible(!cautionVisible)}
                        />
                      </div>
                    </div>
                    {cautionVisible && (
                      <div className="row product-content-block">
                        <div
                          className="col-12 product-content"
                          dangerouslySetInnerHTML={{
                            __html: product!.caution,
                          }}
                        />
                      </div>
                    )}
                  </Fragment>
                )}
              </Fragment>
            )}
            <div className="row product-desc-block">
              <div className={`col-${isMobile ? '12' : '11'} product-desc-notics`}>
                <table>
                  <tbody>
                    <tr>
                      <td
                        css={css`
                          vertical-align: top;
                        `}
                      >
                        †
                      </td>
                      <td>
                        These statements have not been evaluated by the Food and Drug
                        Administration. This product is not in tended to diagnose, treat, cure, or
                        prevent any disease.
                      </td>
                    </tr>
                    <tr>
                      <td></td>
                      <td>
                        Vita Aid Professional Therapeutics products are{' '}
                        <span
                          css={css`
                            font-weight: 400;
                          `}
                        >
                          exclusively available for licensed healthcare practitioners{' '}
                        </span>
                        to ensure the proper use of high-quality nutrition supplements.
                        <br />
                        Some formulas may be slightly different outside Canada and the US.
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
          {relatedProducts && relatedProducts.length > 0 && (
            <div className="row related-products">
              <div className="col-12">
                <div className="related-products-label">Related Products</div>
                <div
                  className="row related-products-list"
                  css={css`
                    width: 100%;
                  `}
                >
                  {isMobile && (
                    <Fragment>
                      {relatedProducts.map((x) => (
                        <div className="col-6">
                          <RelatedProduct key={x.id} product={x} />
                        </div>
                      ))}
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <Fragment>
                      <div className="col-auto align-self-center">
                        {currentPageOfRelatedProducts > 0 && (
                          <button
                            className="borderless-btn"
                            onClick={() => {
                              if (currentPageOfRelatedProducts > 0)
                                setCurrentPageOfRelatedProducts(currentPageOfRelatedProducts - 1);
                            }}
                          >
                            <img className="navigate-btn" src="/img/prev-page.png" alt="" />
                          </button>
                        )}
                      </div>
                      <div className="col">
                        <div className="row products-list-col">
                          <div className="col-3">
                            {currentPageOfRelatedProducts < totalPagesOfRelatedProducts &&
                              currentPageOfRelatedProducts * 4 < relatedProducts.length && (
                                <RelatedProduct
                                  key={relatedProducts[currentPageOfRelatedProducts * 4].id}
                                  product={relatedProducts[currentPageOfRelatedProducts * 4]}
                                />
                              )}
                          </div>
                          <div className="col-3">
                            {currentPageOfRelatedProducts < totalPagesOfRelatedProducts &&
                              currentPageOfRelatedProducts * 4 + 1 < relatedProducts.length && (
                                <RelatedProduct
                                  key={relatedProducts[currentPageOfRelatedProducts * 4 + 1].id}
                                  product={relatedProducts[currentPageOfRelatedProducts * 4 + 1]}
                                />
                              )}
                          </div>
                          <div className="col-3">
                            {currentPageOfRelatedProducts < totalPagesOfRelatedProducts &&
                              currentPageOfRelatedProducts * 4 + 2 < relatedProducts.length && (
                                <RelatedProduct
                                  key={relatedProducts[currentPageOfRelatedProducts * 4 + 2].id}
                                  product={relatedProducts[currentPageOfRelatedProducts * 4 + 2]}
                                />
                              )}
                          </div>
                          <div className="col-3">
                            {currentPageOfRelatedProducts < totalPagesOfRelatedProducts &&
                              currentPageOfRelatedProducts * 4 + 3 < relatedProducts.length && (
                                <RelatedProduct
                                  key={relatedProducts[currentPageOfRelatedProducts * 4 + 3].id}
                                  product={relatedProducts[currentPageOfRelatedProducts * 4 + 3]}
                                />
                              )}
                          </div>
                        </div>
                      </div>
                      <div className="col-auto align-self-center ">
                        {currentPageOfRelatedProducts < totalPagesOfRelatedProducts - 1 && (
                          <button
                            className="borderless-btn"
                            css={css`
                              float: right;
                            `}
                            onClick={() => {
                              if (currentPageOfRelatedProducts < totalPagesOfRelatedProducts - 1)
                                setCurrentPageOfRelatedProducts(currentPageOfRelatedProducts + 1);
                            }}
                          >
                            <img className="navigate-btn" src="/img/next-page.png" alt="" />
                          </button>
                        )}
                      </div>
                    </Fragment>
                  )}
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
      <Popover
        position={
          isMobile
            ? {
                my: 'center',
                at: 'center',
                of: window,
              }
            : {
                my: { x: 'left', y: 'bottom' },
                at: { x: 'left', y: 'top' },
                of: '.va-product-qty-label',
              }
        }
        title="Add to Back Order"
        shading={true}
        shadingColor="rgba(0, 0, 0, 0.5)"
        showTitle={false}
        visible={showAlert}
        showCloseButton={true}
        closeOnOutsideClick={true}
        onHiding={() => {
          setShowAlert(false);
        }}
        className="notify-me-popover"
      >
        <div className="notify-me-popover-body">
          <div className="notify-me-title">
            <div>Add to Back Order</div>
            <button
              className="img-btn close-img-btn"
              onClick={() => {
                setShowAlert(false);
              }}
            >
              <img
                className="close-img"
                alt="close"
                src="/img/x-object.png"
                srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
              />
            </button>
          </div>
          <div>
            Request Quantity* :
            <input
              ref={reqQtyRef}
              type="number"
              className="align-middle request-qty"
              defaultValue={1}
            ></input>
          </div>
          <div className="notice-text">
            * This is not a reservation, but for our reference only.
            {isMobile === false && (
              <Fragment>
                <br />
                &nbsp;
              </Fragment>
            )}
            &nbsp;You will be notified via email when the product is back in stock.
          </div>
          <div className="submit-line">
            {wishProductSubmitFinish === true && (
              <span className="after-submit-text">Your request has been sent.</span>
            )}
            <button
              className="align-middle summit-button"
              onClick={() => {
                if (wishProductSubmitFinish === true) setShowAlert(false);
                else RequestSumbit(product!.productCode);
              }}
            >
              {wishProductSubmitFinish === true && 'CLOSE'}
              {wishProductSubmitFinish === false && 'SUBMIT'}
            </button>
          </div>
        </div>
      </Popover>
      <Popup
        position={{
          my: { x: 'left', y: 'top' },
          at: { x: 'left', y: 'top' },
          of: window,
        }}
        animation={{
          show: {
            type: 'fadeIn',
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
            type: 'fadeOut',
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
        className="zooming-popup"
        visible={showZoomingImage}
        showCloseButton={false}
        showTitle={false}
        closeOnOutsideClick={true}
        onHiding={() => {
          setZoomingImage(false);
        }}
      >
        <div className="container">
          <div className="zooming-popup-body">
            <div className="close-img-block">
              <img
                className="close-img"
                alt="close"
                src="/img/x-m-object.png"
                srcSet="/img/x-m-object@2x.png 2x, /img/x-m-object@3x.png 3x"
                onClick={() => setZoomingImage(false)}
              ></img>
            </div>
            <div className="row">
              <img
                className="col-12 zooming-image"
                alt=""
                src={`${process.env.REACT_APP_PRODUCT_DIR!}${
                  focusedImage ? focusedImage.largeImageName : 'EmptyProduct.png'
                }`}
              ></img>
              <div className="row thumb-img-div">
                <ScrollView
                  className="col-12"
                  width="100%"
                  height={`${window.innerHeight - 507}px`}
                  direction="horizontal"
                >
                  <div className="scrollview-body">
                    {product &&
                      product!.oProductImages.map((i, idx) => {
                        return (
                          <img
                            key={`z-${i.id}`}
                            className={`thumb-image-for-zooming ${
                              idx < product!.oProductImages.length - 1 ? 'inner-space' : ''
                            }`}
                            alt=""
                            src={`${process.env.REACT_APP_PRODUCT_DIR!}${i.imageName}`}
                            onClick={() => {
                              setFocusedImage(i);
                            }}
                          />
                        );
                      })}
                  </div>
                </ScrollView>
              </div>
            </div>
          </div>
        </div>
      </Popup>
    </Fragment>
  );
};
