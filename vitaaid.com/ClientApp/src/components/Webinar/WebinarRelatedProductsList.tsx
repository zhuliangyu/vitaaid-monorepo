/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useNavigate } from 'react-router-dom';
import { ProductData } from 'model/Product';
import { productCategoryChanged } from 'redux/features/product/productCategorySlice';
import { productCode, productCodeChanged } from 'redux/features/product/productCodeSlice';
import { useDispatch } from 'react-redux';

interface RelatedProdProps {
  products: ProductData[];
}

export const WebinarRelatedProductsList = ({ products }: RelatedProdProps) => {
  const dispatch = useDispatch();
  return (
    <Fragment>
      <div className="webinar-product-block">
        <div className="related-title">
          <img
            className="list-img"
            alt=""
            src="/img/category-list.png"
            srcSet="/img/category-list@2x.png 2x, /img/category-list@3x.png 3x"
          />
          <span>RELATED PRODUCTS</span>
        </div>
        <div className="related-product-list">
          {products.map((x) => (
            <Fragment>
              <div>
                <a
                  href={`/products?pcode=${x.productCode}`}
                  onClick={() => {
                    dispatch(productCodeChanged(''));
                    dispatch(productCategoryChanged(''));
                    return true;
                  }}
                >
                  <img
                    className="related-product-img"
                    alt={`${x.productName}`}
                    src={`${process.env.REACT_APP_PRODUCT_DIR!}${x.representativeImage}`}
                  />
                </a>
              </div>
              <div className="related-product-name">
                <a
                  dangerouslySetInnerHTML={{ __html: x.productName }}
                  href={`/products?pcode=${x.productCode}`}
                  onClick={() => {
                    dispatch(productCodeChanged(''));
                    dispatch(productCategoryChanged(''));
                    return true;
                  }}
                />
              </div>
              <div className="related-product-info">
                <span>
                  {x.productCode} | {x.size}
                </span>
              </div>
            </Fragment>
          ))}
        </div>
      </div>
    </Fragment>
  );
};
