/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { Helmet } from 'react-helmet-async';
import { useNavigate } from 'react-router-dom';
import { ProductData, getFeaturedProducts } from 'model/Product';
import { useSelector, useDispatch } from 'react-redux';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { productCategoryChanged } from 'redux/features/product/productCategorySlice';
import { productCodeChanged } from 'redux/features/product/productCodeSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { isChrome, isSafari, isEdge } from 'react-device-detect';
import { Popover } from 'devextreme-react/popover';
import PopupAd from 'components/PopupAd';

interface FeaturedProductProp {
  product: ProductData;
  idx: number;
  isMobile: boolean;
}
const FeaturedProduct = ({ product, idx, isMobile }: FeaturedProductProp) => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  return (
    <React.Fragment>
      <div className={`home-featured-product block-${idx}-adjust`}>
        <div className={`info-block`}>
          <div
            className="product-name"
            dangerouslySetInnerHTML={{ __html: product.productName }}
          ></div>
          <div className="delimite-line">
            <img
              className="delimite-line-img"
              alt=""
              src="/img/home-feature-line-1.png"
              srcSet="/img/home-feature-line-1@2x.png 2x, /img/home-feature-line-1@3x.png 3x"
            ></img>
          </div>
          <div className="detail-block">
            <table>
              <tbody>
                <tr>
                  <td>
                    <div
                      className="product-function"
                      css={css`
                        padding-top: 0px;
                      `}
                      dangerouslySetInnerHTML={{ __html: product.function }}
                    ></div>
                  </td>
                  <td rowSpan={2}>
                    <img
                      className="product-img"
                      alt={`${product.productName}`}
                      src={`${process.env.REACT_APP_PRODUCT_DIR!}${product.representativeImage}`}
                      onClick={() => {
                        navigate(`/products?pcode=${product.productCode}`);
                        dispatch(productCodeChanged(''));
                        dispatch(productCategoryChanged(''));
                        return true;
                      }}
                    ></img>
                  </td>
                </tr>
                <tr>
                  <td
                    css={css`
                      vertical-align: bottom !important;
                    `}
                  >
                    <button
                      className={`know-more`}
                      onClick={() => {
                        navigate(`/products?pcode=${product.productCode}`);
                        dispatch(productCodeChanged(''));
                        dispatch(productCategoryChanged(''));
                      }}
                    >
                      <span>LEARN MORE</span>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export const HomePage = () => {
  let navigate = useNavigate();
  // const [bMuted] = React.useState<boolean>(true);
  const country = useSelector(selectedCountry);
  const isMobile = useSelector(isMobileData);
  const [browserRestrictionMsgBox, setBrowserRestrictionMsgBox] = React.useState(false);
  const [featuredProducts, setFeaturedProducts] = React.useState<ProductData[]>([]);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getFeaturedProducts(country);
      setFeaturedProducts(data);
    }
    if (isChrome || isSafari || isEdge) setBrowserRestrictionMsgBox(false);
    else setBrowserRestrictionMsgBox(true);

    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setFeaturedProducts([]);
    };
  }, []);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid Professional Therapeutics </title>
      </Helmet>
      {/*<div className="marquee-div">*/}
      {/*  <div className="marquee-text">*/}
      {/*    *Holiday Office Hour*&nbsp;*/}
      {/*    <span className="bold-text">Last Shipping Day of 2021 :</span> Friday, December 24, 2021*/}
      {/*    &nbsp;<span className="bold-text">Closed :</span> Saturday, December 25, 2021 to Monday,*/}
      {/*    January 3, 2022*/}
      {/*  </div>*/}
      {/*</div>*/}
      <div className="home-page">
        {isMobile === false && (
          <div className="home-video">
            <div className="oath-sentance"></div>
            <div className="overlay">
              <span>Study Nature . Think Innovative . Re-Define Therapeutics</span>
              {/*  <img*/}
              {/*    className={bMuted ? 'sound-on' : 'sound-off'}*/}
              {/*    onClick={() => setMuted(!bMuted)}*/}
              {/*  />*/}
            </div>
            <video
              className="bg-video"
              id="bg-video"
              autoPlay={true}
              loop={true}
              preload={'metadata'}
              muted={true}
              poster="/ProductImages/video-poster.png"
              src="/ProductImages/Vita%20Aid%201min_v8.mp4"
            />
          </div>
        )}
        {isMobile && (
          <div>
            <img
              className="home-banner-img"
              alt="study-nature-think-innovative-re-define-therapeutics"
              src="img/study-nature-think-innovative-re-define-therapeutics.png"
              srcSet="img/study-nature-think-innovative-re-define-therapeutics@2x.png 2x, img/study-nature-think-innovative-re-define-therapeutics@3x.png 3x"
            ></img>
          </div>
        )}
        {/*<div className="design-by-hcp-block">*/}
        {/*  <div className="home-block1" />*/}
        {/*  <div className="home-block2" />*/}
        {/*  <div className="home-block3" />*/}
        {/*  <div className="title">*/}
        {/*    <span className="text-1">Designed by</span>*/}
        {/*    <span className="text-2"> HCPs</span>*/}
        {/*    <span className="text-1"> for</span>*/}
        {/*    <span className="text-2"> HCPs.</span>*/}
        {/*  </div>*/}
        {/*  <div className="home-line1" />*/}
        {/*  <div className="desc">*/}
        {/*    <p>*/}
        {/*      Practitioner-Exclusive Product Line Clinically Relevant, Evidence-Based Formulas*/}
        {/*      Trusted Source of Nutraceuticals for Health Care Professionals (HCPs) since 1999.*/}
        {/*    </p>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {/* <PopupAd /> */}        
        <div className="quality-transparency">
          <div className="home-new-block1" />
          <div className="home-new-block2" />
          <div className="text-1">
            QUALITY & TRANSPARENCY
            <div className="home-section2-p1" />
          </div>
          <div className="detail">
            <div className="left-block">
              <div className="text-2">
                Building Trust
                <br /> Through Quality
              </div>
              <div className="home-quality-line" />
              <div className="text-3">
                <p>
                  Through careful sourcing and advanced testing of raw materials and finished
                  products,
                  <br /> we strive to earn your trust by providing reliable and consistent
                  therapeutic solutions.
                </p>
              </div>
              <button
                className="learn-more-btn"
                onClick={() => navigate('/buildingtrustthroughquality')}
              >
                <span>LEARN MORE</span>
              </button>
            </div>
            <div className="right-block">
              <div className="text-2">
                Transparency,
                <br /> What We Believe In
              </div>
              <div className="home-quality-line" />
              <div className="text-3">
                <p>
                  QualityTrak™, a state-of-art platform,
                  <br />
                  allows you to quickly track
                  <br />
                  the quality of every lot of
                  <br />
                  our products in the market.
                </p>
              </div>
              <button
                className="learn-more-btn"
                onClick={() => navigate(`/qualitytrak/${country}`)}
              >
                <span>START</span>
              </button>
            </div>
          </div>
          <div className="home-section-line" />
        </div>
        <div className="products-section">
          <div className="home-new-block3" />
          <div className="text-1">
            <div className="products-section-p1" />
            <div className="products-section-p2" />
            PRODUCTS
          </div>
          <div className="text-2">Re-Define Therapeutics</div>
          <div className="home-product-line" />
          <div className="text-3">
            <p>We put science first in formulating clinical-relevant, evidence-based products.</p>
          </div>
          {featuredProducts && (
            <div className="feature-products">
              {featuredProducts.slice(0, 3).map((x, idx) => {
                return <FeaturedProduct key={x.id} product={x} idx={idx} isMobile={isMobile} />;
              })}
            </div>
          )}
          <div className="home-section-line" />
        </div>
        <div className="join-now-section">
          <div className="home-new-block4" />
          <div className="home-new-block5" />
          <div className="mail" />
          <div className="text-1">JOIN OUR MAILING LIST</div>
          <div className="text-2">Stay Connected &amp; Up-To-Date</div>
          <div className="home-join-line" />
          <div className="text-3">
            Receive monthly clinical pearls, featured products, and more.
          </div>
          <button
            className="join-now-btn"
            onClick={() => {
              navigate('/joinourmailinglist');
            }}
          >
            <span>JOIN NOW</span>
          </button>
        </div>
        <Popover
          position={{
            my: 'center',
            at: 'center',
            of: window,
          }}
          shading={true}
          shadingColor="rgba(0, 0, 0, 0.5)"
          visible={browserRestrictionMsgBox}
          showCloseButton={true}
          closeOnOutsideClick={true}
          onHiding={() => setBrowserRestrictionMsgBox(false)}
          className="browser-restriction-msg-box"
        >
          <button
            className="img-btn close-img-btn"
            onClick={() => setBrowserRestrictionMsgBox(false)}
          >
            <img
              className="close-img"
              alt="close"
              src="/img/x-object.png"
              srcSet="/img/x-object@2x.png 2x, /img/x-object@3x.png 3x"
            ></img>
          </button>

          <div className="title">
            <img className="icon" alt="" src="/img/error-warning-fill.png" />
            Your browser is currently not supported.
          </div>
          <div className="message">
            <p>
              We recommend using one of the following browsers for an optimal browsing experience:
            </p>
            <p>
              <img className="browser-icon" alt="" src="/img/chrome.png" />
              <img className="browser-icon" alt="" src="/img/edge.png" />
              <img className="browser-icon" alt="" src="/img/safari.png" />
            </p>
            <p>We apologize for any inconvenience. Thank you.</p>
          </div>
        </Popover>
      </div>
    </React.Fragment>
  );
};
