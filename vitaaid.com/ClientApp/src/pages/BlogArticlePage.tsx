/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { BreadCrumbs } from 'components/Layouts/BreadCrumbs';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { blogCategoryChanged, blogCategory } from 'redux/features/BlogCategorySlice';
import { eUNITTYPE, UnitTypeData, getUnitTypes } from 'model/UnitType';
import { BlogData, getBlog, getBlogArticle, getBlogByCategory } from 'model/Blog';
import { accountData } from 'redux/features/account/accountSlice';
import { forceUpdateData } from 'redux/features/forceUpdateSlice';
import { BlogCategoryList } from 'components/Blog/BlogCategoryList';
import { BlogRelatedProductsList } from 'components/Blog/BlogRelatedProductsList';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function BlogArticlePage() {
  const [article, setArticle] = React.useState<BlogData | undefined>();
  const country = useSelector(selectedCountry);
  const category = useSelector(blogCategory);
  const account = useSelector(accountData);
  const isMobile = useSelector(isMobileData);
  let { id } = useParams();
  const dispatch = useDispatch();
  const requestUpdate = useSelector(forceUpdateData);

  let navigate = useNavigate();

  const [categories, setCategories] = React.useState<UnitTypeData[]>([]);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getBlogArticle(parseInt(id!, 10), country);
      setArticle(data);
      const cateData = await getUnitTypes(eUNITTYPE.THERAPEUTIC_FOCUS);
      setCategories(cateData);
    }
    if (id === undefined) navigate('/blog');
    else if (account) fetchData();
    else navigate('/');
    // Specify how to clean up after this effect:
    return () => {
      setArticle(undefined);
      setCategories([]);
    };
  }, [, requestUpdate]);

  React.useEffect(() => {
    dispatch(blogCategoryChanged(article?.sCategory));
  }, [article]);
  return (
    <Fragment>
      <Helmet>
        <title>Vita Aid - Blog</title>
      </Helmet>
      <div className="content-main-body">
        {article && (
          <BreadCrumbs
            subNodes={['Blog', `${article.sCategory}`]}
            hrefs={['/blog', '/blog']}
            actions={[
              () => {
                dispatch(blogCategoryChanged(''));
              },
              () => {},
            ]}
          />
        )}
        <div className="blog-article">
          <div className="row">
            <div className={`col-${isMobile ? '12' : '10'} article-content-col`}>
              {article && (
                <div>
                  {article.banner && article.banner.length > 0 && (
                    <div className="banner-div">
                      {isMobile && (
                        <img
                          className="banner"
                          alt={`${article.thumb}`}
                          src={`${process.env.REACT_APP_BLOG_DIR!}${article.thumb}`}
                        />
                      )}
                      {isMobile === false && (
                        <img
                          alt={article.banner}
                          className="banner"
                          src={`${process.env.REACT_APP_BLOG_DIR!}${article.banner}`}
                        />
                      )}
                    </div>
                  )}
                  <div className="title" dangerouslySetInnerHTML={{ __html: article.topic }} />
                  <div className="issue-vol-no-block">
                    <span>{`Issue : ${article.issue} | Vol. ${article.volume} | No. ${article.no}`}</span>
                    {article.author && (
                      <span key={`blogArticle-${article.id}`}>
                        {isMobile && <br />}
                        {isMobile === false && <Fragment>&nbsp;&nbsp;&nbsp;&nbsp;</Fragment>}
                        {`Author : ${article.author}`}
                      </span>
                    )}
                  </div>
                  <div
                    className="article-content"
                    dangerouslySetInnerHTML={{ __html: article.blogContent }}
                  ></div>
                  <div className="article-reference">
                    <div
                      css={css`
                        margin-top: 4px;
                      `}
                    >
                      Reference:
                    </div>
                    <div dangerouslySetInnerHTML={{ __html: article.reference }}></div>
                  </div>
                  <div className="notice">
                    <div>† This presentation is for educational purpose only.</div>
                    <p>
                      The entire contents are not intended to be a substitute for professional
                      medical advice, diagnosis, or treatment. Always seek the advice of your
                      physician or other qualified health provider with any questions you may have
                      regarding a medical condition.
                    </p>
                    <p>
                      Never disregard professional medical advice or delay in seeking it because of
                      something you have read in this presentation.
                    </p>
                    <p>
                      All statements in this article have not been evaluated by the Food and Drug
                      Administration and are not intended to be used to diagnose, treat, or prevent
                      any diseases.
                    </p>
                  </div>
                </div>
              )}
            </div>
            {isMobile === false && (
              <div className="col-2 blog-related-resources">
                {categories && (
                  <BlogCategoryList
                    categories={categories}
                    onSelectedCategory={(x) => {
                      dispatch(blogCategoryChanged(x.name));
                      navigate(`/blog`);
                    }}
                  />
                )}
                {article?.relatedProducts && article.relatedProducts.length > 0 && (
                  <BlogRelatedProductsList
                    key="blogRelatedProductList"
                    products={article.relatedProducts}
                  />
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </Fragment>
  );
}
