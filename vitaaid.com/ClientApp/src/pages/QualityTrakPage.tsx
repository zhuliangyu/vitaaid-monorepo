/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import qtSearchImg from 'img/qt-search.png';
import { StabilityForm, getByLot } from '../model/StabilityForm';
import { LoadPanel } from 'devextreme-react/load-panel';
import { StabilityFormHeader } from '../components/StabilityForm/StabilityFormHeader';
import { StabilityFormDetail } from '../components/StabilityForm/StabilityFormDetail';
import { TextBox, Button as TextBoxButton } from 'devextreme-react/text-box';
import { urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { accountData } from '../redux/features/account/accountSlice';
import { loginDlg, openLoginDlg, closeLoginDlg } from 'redux/features/loginDlgSlice';
import { forceUpdateData } from 'redux/features/forceUpdateSlice';
import { memberTypeForURLAfterLoginChanged } from 'redux/features/memberTypeForURLAfterLoginSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { MessageBox } from 'components/MessageBox';
import { requireLoginMessageChanged, MessageID } from 'redux/features/requireLoginMessageSlice';

interface Props {
  LotNo?: string | null | undefined;
}
const useQuery = () => new URLSearchParams(useLocation().search);

export default function QualityTrakPage({ LotNo }: Props) {
  const [SFData, setSFData] = React.useState<StabilityForm | null>();
  const [sLotNo, setLotNo] = React.useState<string | null | undefined>(LotNo);
  const [DataLoading, setDataLoading] = React.useState<boolean>(false);

  const myRef = useRef<null | HTMLDivElement>(null);
  const { country } = useParams();
  let query = useQuery();
  let navigate = useNavigate();
  const lotno = query.get('lotno');
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const requestUpdate = useSelector(forceUpdateData);
  const isMobile = useSelector(isMobileData);

  const searchButton = {
    icon: qtSearchImg,
    type: 'default',
    stylingMode: 'text',
    elementAttr: {
      class: 'btnSearch',
    },
    onClick: () => {
      searchLot(sLotNo);
    },
    focusStateEnabled: true,
  };

  React.useEffect(() => {
    if (country === undefined) navigate('qualitypage/ca');
    if (lotno) {
      setLotNo(lotno);
      searchLot(lotno);
    } else if (sLotNo && sLotNo !== '') {
      setLotNo(sLotNo);
      searchLot(sLotNo);
    }
  }, [country, requestUpdate]);

  const searchLot = async (lotNo: string | null | undefined) => {
    if (account) {
      setDataLoading(true);
      if (lotNo != null && (lotNo.length >= 8 || lotNo === '1111')) {
        const data = await getByLot(lotNo === '1111' ? '134577VC1' : lotNo!, country!);
        setSFData(data);
        //if (data != null && myRef.current != null)
        //  myRef.current?.scrollIntoView();
      } else setSFData({} as StabilityForm);
      setDataLoading(false);
    } else {
      const url = `/qualitytrak/${country}?lotno=${sLotNo}`;
      dispatch(urlAfterLoginChanged(url));
      dispatch(memberTypeForURLAfterLoginChanged(0));
      if (isMobile) {
        dispatch(requireLoginMessageChanged(MessageID.QT));
      } else {
        dispatch(openLoginDlg());
      }
    }
  };

  const onKeyUp = (e: any) => {
    if (e.event.key === 'Enter') {
      searchLot(sLotNo);
      e.event.currentTarget.blur();
    } else setLotNo(e.event.currentTarget.value);
  };

  //const onChange = (e: React.ChangeEvent<HTMLInputElement>) =>
  //  setLotNo(e.currentTarget.value);

  //const onKeyUp = (e: React.KeyboardEvent<HTMLInputElement>) => {
  //  if (e.key === 'Enter') {
  //    searchLot(sLotNo);
  //  }
  //};

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Quality Trak</title>
      </Helmet>
      <div className="row img-fluid QTBanner">
        <div className="banner-block1" />
        <div className="banner-block2" />
        <div className="banner-block3" />
        <div className="banner-title">
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
          <div className="tm">TM</div>
        </div>
        <div className="banner-line" />
        <p className="banner-text">
          As an advocate for high-quality standards,
          {isMobile === false && <br />}
          Vita Aid gives online visibility to quality testing information for every single formula
          we create. <br />
          Find the LOT Number on the bottle.
        </p>
        <div className="searchBlock">
          <table>
            <tbody>
              <tr>
                <td
                  css={css`
                    vertical-align: bottom;
                  `}
                >
                  <div className="labelstr">
                    <span>QualityTrak</span>
                    <span className="tm">TM</span>
                  </div>
                </td>
                <td
                  css={css`
                    vertical-align: bottom;
                  `}
                >
                  <TextBox
                    id="txtLotNo"
                    placeholder="Enter lot #"
                    defaultValue={lotno}
                    onKeyUp={onKeyUp}
                  >
                    <TextBoxButton
                      name="btnSearch"
                      location="after"
                      options={searchButton}
                    ></TextBoxButton>
                  </TextBox>
                </td>
              </tr>
            </tbody>
          </table>
          <div className="tipstr">- Enter your lot # to see quality testing on your product.</div>
        </div>
      </div>

      <div className="qt-content-main-body">
        {SFData != null && (
          <Fragment>
            <StabilityFormHeader SForm={SFData} IsEmpty={SFData.name == null ? true : false} />
            {SFData.name != null && <StabilityFormDetail SForm={SFData} />}
          </Fragment>
        )}
      </div>
      <LoadPanel shadingColor="rgba(0,0,0,0.4)" visible={DataLoading} />
    </React.Fragment>
  );
}
