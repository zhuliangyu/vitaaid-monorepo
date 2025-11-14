/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

import { ProductData } from 'model/Product';
import { Fragment } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { productCodeChanged } from 'redux/features/product/productCodeSlice';

interface Props {
  idx: number;
  data: ProductData;
  isMobile: boolean;
}
export const ProductInfoInCategory = ({ idx, data, isMobile }: Props) => {
  let navigate = useNavigate();
  const dispatch = useDispatch();
  return (
    <Fragment>
      <div className={`${isMobile ? 'col-6' : 'col-4'}`}>
        <div className={`ProductInfoInCategory blockouter-${idx % (isMobile ? 2 : 3)}`}>
          <div className={`info-block info-block-${idx % (isMobile ? 2 : 3)}-adjust`}>
            <img
              className="product-img"
              alt={`${data.productName}`}
              src={`${process.env.REACT_APP_PRODUCT_DIR!}${data.representativeImage}`}
              onClick={() => {
                navigate(`/products?pcode=${data.productCode}`);
                //dispatch(productCodeChanged(data.productCode));
              }}
            ></img>
            <div
              className="product-name"
              dangerouslySetInnerHTML={{ __html: data.productName }}
            ></div>
            {isMobile === false && (
              <Fragment>
                <div className="product-code">
                  {`${data.productCode} | ${data.size}`}
                  {data.npn && ` | NPN ${data.npn}`}
                </div>
                <div className="delimite-line">
                  <img
                    alt=""
                    src="/img/line-2.png"
                    srcSet="/img/line-2@2x.png 2x, /img/line-2@3x.png 3x"
                  ></img>
                </div>
                <div
                  className="product-function"
                  css={css`
                    padding-top: 0px;
                  `}
                  dangerouslySetInnerHTML={{ __html: data.function }}
                ></div>
              </Fragment>
            )}
          </div>
          {isMobile && (
            <div className="view-more-btn-m-block">
              <button
                className="view-more-btn-m"
                onClick={() => dispatch(productCodeChanged(data.productCode))}
              >
                <span>VIEW MORE</span>
              </button>
            </div>
          )}
          {isMobile === false && (
            <button
              className={`img-btn view-more-btn-${idx % (isMobile ? 2 : 3)}`}
              onClick={() => {
                dispatch(productCodeChanged(data.productCode));
              }}
            >
              <img
                className="view-more"
                alt="view more"
                src="/img/view-more.png"
                srcSet="/img/view-more@2x.png 2x, /img/view-more@3x.png 3x"
              ></img>
            </button>
          )}
        </div>
      </div>
    </Fragment>
  );
};
