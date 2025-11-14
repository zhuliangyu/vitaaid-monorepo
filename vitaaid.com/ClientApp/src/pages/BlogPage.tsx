/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { blogCategoryChanged, blogCategory } from 'redux/features/BlogCategorySlice';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { eUNITTYPE, UnitTypeData, getUnitTypes } from 'model/UnitType';
import { BlogData, getBlog, getBlogByCategory } from 'model/Blog';
import { BlogInfoInCategory } from 'components/Blog/BlogInfoInCategory';
import { BlogCategoryList } from 'components/Blog/BlogCategoryList';
import { accountData } from '../redux/features/account/accountSlice';
import { urlAfterLogin, urlAfterLoginChanged } from 'redux/features/urlAfterLoginSlice';
import { visiblePractitionerOnlyMsgBoxChanged } from 'redux/features/visiblePractitionerOnlyMsgBoxSlice';
import { isMobileData } from 'redux/features/isMobileSlice';
import { PageNav } from 'components/PageNav';
import { Popup } from 'devextreme-react/popup';
import styles from 'scss/abstracts/_variables.scss';

export default function BlogPage() {
  let navigate = useNavigate();
  const category = useSelector(blogCategory);
  const dispatch = useDispatch();
  const account = useSelector(accountData);
  const isMobile = useSelector(isMobileData);
  const listSizePerPage = 8;
  const [categories, setCategories] = React.useState<UnitTypeData[]>([]);
  const [articles, setArticles] = React.useState<BlogData[]>([]);
  const [currentPageOfArticles, setCurrentPageOfArticles] = React.useState<number>(0);
  const [totalPagesOfArticles, setTotalPagesOfArticles] = React.useState<number>(0);
  const [showCategoryPopup, setShowCategoryPopup] = React.useState<boolean>(false);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getUnitTypes(eUNITTYPE.THERAPEUTIC_FOCUS);
      setCategories(data);
    }
    fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setCategories([]);
    };
  }, []);
  /*
  React.useEffect(() => {
    if (categories.length > 0) {
        const qualified = categories.filter((x) => x.name === category);
        if (qualified.length > 0) setSelectedCategory(qualified[0]);
        else setSelectedCategory(categories[0]);
    }
    // Specify how to clean up after this effect:
    return () => {
      setSelectedCategory(undefined);
    };
  }, [categories]);
*/
  React.useEffect(() => {
    async function fetchData() {
      let data: any;
      if (category) {
        const selected = categories.filter((x) => x.name === category)[0];
        data = await getBlogByCategory(parseInt(selected.abbrName, 10));
      } else data = await getBlog();
      setArticles(data);
    }
    if (categories.length > 0) fetchData();
    // Specify how to clean up after this effect:
    return () => {
      setArticles([]);
    };
  }, [categories, category]);

  React.useEffect(() => {
    if (articles) {
      setTotalPagesOfArticles(Math.ceil(articles.length / listSizePerPage));
      setCurrentPageOfArticles(1);
    } else {
      setTotalPagesOfArticles(1);
      setCurrentPageOfArticles(1);
    }
  }, [articles, isMobile]);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Blog</title>
      </Helmet>
      <div className="content-main-body blogs">
        <div className="row">
          <div className={`col-${isMobile ? '12' : '10'} blog-list`}>
            <div className="title-block">
              <div className="title">
                Blog
                {category && (
                  <span className="category-title">&nbsp;&nbsp; | &nbsp;&nbsp;{category}</span>
                )}
              </div>
              {isMobile && (
                <div className="open-category-m" onClick={() => setShowCategoryPopup(true)}></div>
              )}
            </div>
            {isMobile && (
              <Fragment>
                {articles &&
                  [...Array(listSizePerPage)].map((_, idx) => {
                    const artIdx = (currentPageOfArticles - 1) * listSizePerPage + idx;
                    if (artIdx < 0 || artIdx >= articles.length)
                      return <div key={`articles-${idx}`} />;
                    const art = articles[artIdx];
                    return (
                      <BlogInfoInCategory
                        key={art.id}
                        data={art}
                        showNote={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(true))}
                        isMobile={isMobile}
                      />
                    );
                  })}
              </Fragment>
            )}
            {isMobile === false && (
              <table className="blog-info-table">
                <tbody>
                  {articles &&
                    [...Array(listSizePerPage)].map((_, idx) => {
                      const artIdx = (currentPageOfArticles - 1) * listSizePerPage + idx;
                      if (artIdx < 0 || artIdx >= articles.length)
                        return (
                          <tr key={`articles-${idx}`}>
                            <td />
                          </tr>
                        );
                      const art = articles[artIdx];
                      return (
                        <BlogInfoInCategory
                          key={art.id}
                          data={art}
                          showNote={() => dispatch(visiblePractitionerOnlyMsgBoxChanged(true))}
                          isMobile={isMobile}
                        />
                      );
                    })}
                </tbody>
              </table>
            )}
          </div>
          {isMobile === false && (
            <div className="col-2">
              <div className="blog-related-resources">
                {categories && (
                  <BlogCategoryList
                    categories={categories}
                    onSelectedCategory={(x) => {
                      dispatch(blogCategoryChanged(x.name));
                    }}
                  />
                )}
              </div>
            </div>
          )}
        </div>
        {totalPagesOfArticles > 1 && (
          <PageNav
            isMobile={isMobile}
            totalPages={totalPagesOfArticles}
            currentPage={currentPageOfArticles}
            currentPageChanged={(v) => setCurrentPageOfArticles(v)}
          />
        )}
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
        className="blog-category-filter-popup"
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
                    Blog
                  </span>
                  <span
                    css={css`
                      font-size: 18px;
                      color: var(--marine-blue);
                    `}
                  >
                    {' '}
                    | {category}
                  </span>
                </div>
                <div className="close-category-m" onClick={() => setShowCategoryPopup(false)} />
              </div>
            </Fragment>
          );
        }}
        onHidden={() => setShowCategoryPopup(false)}
      >
        <div className="blog-filter-popup-body">
          {categories.map((x) => (
            <div key={x.id}>
              <button
                className="borderless-btn blog-filter-popup-item"
                onClick={() => {
                  dispatch(blogCategoryChanged(x.name));
                  setShowCategoryPopup(false);
                }}
              >
                {x.name}
              </button>
            </div>
          ))}
        </div>
      </Popup>
    </React.Fragment>
  );
}
