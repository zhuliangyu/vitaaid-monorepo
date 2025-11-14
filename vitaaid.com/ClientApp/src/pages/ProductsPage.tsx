/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { accountData } from 'redux/features/account/accountSlice';
import { productCode, productCodeChanged } from 'redux/features/product/productCodeSlice';
import { useSelector, useDispatch } from 'react-redux';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { ProductDetail } from 'components/Products/ProductDetail';
import { ProductsMaster } from 'components/Products/ProductsMaster';
import { useLocation, useNavigate } from 'react-router-dom';
import { UnitTypeData, getProductCategories, getAllergyCategories } from 'model/UnitType';
import {
  productFilterMethod,
  byCategory,
  byAlphabet,
  byKeyword,
  eFILTERMETHOD,
} from 'redux/features/product/productFilterMethodSlice';
import {
  productCategory,
  productCategoryChanged,
} from 'redux/features/product/productCategorySlice';
import {
  shopping_cart_page,
  shopping_summary_page,
} from 'redux/features/shoppingcart/cartPageSlice';
import {
  bCartBrief,
  viewCartBrief,
  hideCartBrief,
} from 'redux/features/shoppingcart/showCartBriefSlice';
import {
  cartChanged,
  ShoppingCartItem,
  shoppingCart,
  shoppingCartSlice,
  addCartItem,
} from 'redux/features/shoppingcart/shoppingCartSlice';
import { Popover, ToolbarItem } from 'devextreme-react/popover';
import ScrollView from 'devextreme-react/scroll-view';

export const ProductsPage = () => {
  const account = useSelector(accountData);
  const dispatch = useDispatch();
  let navigate = useNavigate();
  const code = useSelector(productCode);
  const [allergyCategory, setAllergyCategory] = React.useState<UnitTypeData[]>([]);
  const useQuery = () => new URLSearchParams(useLocation().search);
  let query = useQuery();
  const pcode = query.get('pcode');
  const bySearch = query.get('bySearch');
  let cart = useSelector(shoppingCart);
  const country = useSelector(selectedCountry);
  const showCartBrief = useSelector(bCartBrief);
  const filterMethod = useSelector(productFilterMethod);
  const isMobile = useSelector(isMobileData);

  const proceedToCheckoutOption = {
    text: 'Proceed to Checkout',
    onClick: () => {
      dispatch(shopping_cart_page());
      navigate('/cart');
    },
  };

  React.useEffect(() => {
    async function fetchData() {
      const data = await getAllergyCategories();
      setAllergyCategory(data);
    }
    if (pcode) {
      //dispatch(byKeyword());
      dispatch(productCodeChanged(pcode));
      //dispatch(productCategoryChanged(''));
      //  navigate('/products');
    } else {
      if (bySearch == undefined || bySearch == null || bySearch !== '1') {
        if (filterMethod == eFILTERMETHOD.KEYWORD) dispatch(byCategory());
        dispatch(productCodeChanged(''));
      } else navigate('/products');

      fetchData();
    }
  }, [, pcode]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Products</title>
      </Helmet>
      {code && <ProductDetail key={code} code={code} allergyCategoriesData={allergyCategory} />}
      {!code && <ProductsMaster isMobile={isMobile} />}
      {isMobile === false && cart && cart.length > 0 && (
        <Fragment>
          <div className="cart-popover-pos"></div>
          <Popover
            position={{
              my: { x: 'right', y: 'top' },
              at: { x: 'right', y: 'top' },
              of: '.cart-popover-pos',
            }}
            title="Shopping Cart"
            showTitle={true}
            visible={showCartBrief === true}
            showCloseButton={false}
            closeOnOutsideClick={false}
            className="shopping-cart-popover"
          >
            <ScrollView width="100%" height="100%">
              <div
                className="container-fluid shopping-cart-popover-body"
                css={css`
                  padding-left: 0;
                  padding-right: 0;
                `}
              >
                {cart.map((x) => {
                  return (
                    <Fragment>
                      <div key={`${x.code}`} className="row">
                        <div className="col-12 cart-item-block">
                          <div
                            className="prod-name"
                            dangerouslySetInnerHTML={{ __html: x.name }}
                          ></div>
                          <div className="prod-code">
                            {x.code} | {x.size}
                          </div>
                          <div
                            css={css`
                              text-align: center;
                            `}
                          >
                            <img
                              className="thumb-image"
                              alt=""
                              src={`${process.env.REACT_APP_PRODUCT_DIR!}${x.image}`}
                              onClick={() => {
                                dispatch(productCodeChanged(x.code));
                              }}
                            />
                            <div className="prod-price">
                              {`$${x.price.toFixed(2)}${country === 'CA' ? ' CAD' : ' USD'}`}
                            </div>
                            <div className="prod-qty">Qty: {x.qty}</div>
                          </div>
                        </div>
                      </div>
                    </Fragment>
                  );
                })}
              </div>
            </ScrollView>
            <ToolbarItem
              widget="dxButton"
              toolbar="bottom"
              location="after"
              options={proceedToCheckoutOption}
            ></ToolbarItem>
          </Popover>
        </Fragment>
      )}
    </React.Fragment>
  );
};
