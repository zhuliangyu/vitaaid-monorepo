/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector } from 'react-redux';
import { StabilityForm } from '../../model/StabilityForm';
import { isMobileData } from 'redux/features/isMobileSlice';

const EmptyProductDesc = () => {
  return (
    // <div className="sf-empty-product-info">
    //   <div className="line1">Lot Number Not Found</div>
    //   <div className="empty-product-line" />
    //   <p className="line2">
    //     Sorry the Lot Number you have entered might be expired or no longer in
    //     our system.
    //   </p>
    // </div>
    <div className="sf-product-info">
      <table className="sf-product-info-table">
        <tr>
          <td className="sf-product-info-title">Lot Number Not Found</td>
        </tr>
        <tr>
          <td>
            <p className="sf-empty-product-desc">
              Sorry the Lot Number you have entered might be expired or no longer in our system.
            </p>
          </td>
        </tr>
      </table>
    </div>
  );
};

const ProductDesc = (SForm: StabilityForm) => {
  return (
    <div className="sf-product-info">
      <table className="sf-product-info-table">
        <tr>
          <td className="sf-product-info-title">Product&nbsp;:</td>
          <td className="sf-product-info-data">{SForm.name}</td>
        </tr>
        {/* <tr>
          <td className="sf-product-info-title">Dosage Form&nbsp;:</td>
          <td className="sf-product-info-data">{SForm.packageForm}</td>
        </tr> */}
        <tr>
          <td className="sf-product-info-title">Expiry Date&nbsp;:</td>
          <td className="sf-product-info-data">{SForm.sExpiryDate}</td>
        </tr>
        <tr>
          <td className="sf-product-info-title">Lot Number&nbsp;:</td>
          <td className="sf-product-info-data">{SForm.lotNumber}</td>
        </tr>
      </table>
    </div>
  );
};
interface Props {
  SForm: StabilityForm;
  IsEmpty: boolean;
}

export const StabilityFormHeader = ({ SForm, IsEmpty }: Props) => {
  const isMobile = useSelector(isMobileData);
  return (
    <div className="row sf-row sf-header img-fluid">
      <table className="col-12">
        <tbody>
          {isMobile && (
            <Fragment>
              <tr>
                <td className="sf-product-img-col">
                  <img
                    className="sf-product-img"
                    alt=""
                    src={`${process.env.REACT_APP_PRODUCT_DIR!}${
                      IsEmpty ? 'EmptyProduct.png' : SForm.lProductImg
                    }`}
                  ></img>
                </td>
              </tr>
              <tr>
                <td>{IsEmpty ? EmptyProductDesc() : ProductDesc(SForm)}</td>
              </tr>
            </Fragment>
          )}
          {isMobile === false && (
            <tr>
              <td className="sf-product-img-col">
                <img
                  className="sf-product-img"
                  alt=""
                  src={`${process.env.REACT_APP_PRODUCT_DIR!}${
                    IsEmpty ? 'EmptyProduct.png' : SForm.lProductImg
                  }`}
                ></img>
              </td>
              <td>{IsEmpty ? EmptyProductDesc() : ProductDesc(SForm)}</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};
