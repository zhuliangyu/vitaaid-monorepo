/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { useSelector } from 'react-redux';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function BuildingTrustThroughQualityPage() {
  const country = useSelector(selectedCountry);
  const isMobile = useSelector(isMobileData);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid Professional Therapeutics </title>
      </Helmet>
      <div className="building-trust-page-header">
        <div className="qnt-block1" />
        <div className="qnt-block2" />
        <div className="qnt-block3" />
        <div className="qnt-block4" />
        <div className="qnt-header">
          <div className="text-1">
            Building Trust
            <span
              css={css`
                color: var(--marine-blue);
              `}
            >
              {isMobile && (
                <Fragment>
                  <br />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </Fragment>
              )}
              &nbsp;Through Quality
            </span>
          </div>
          <div className="qnt-line1" />
          <div className="text-2">
            <p>
              Through careful sourcing and advanced testing of raw materials and finished products,
              we strive to earn your trust by providing reliable and consistent therapeutic
              solutions.
            </p>
          </div>
        </div>
      </div>
      <div className="content-main-body">
        <div className="qnt-main-body">
          <div>
            <p>
              With over 20 years of experience in the natural health industry, we work with
              dedicated and forward-thinking Health Care professionals to provide them with
              effective therapeutic tools that positively impact patients’ lives.
            </p>
            <p>
              By meticulously monitoring the quality of raw materials, we ensure our products yield
              consistent, top-level results. Each product is approved by Health Canada and produced
              in-house at our GMP- licensed production facility, providing the peace of mind that
              comes with the superior efficacy and quality.
            </p>
          </div>
          <div className="contect-section">
            <div className="rm-text-block">
              <div className="header-line">Raw Materials</div>
              Our work starts with a thorough screening process on every lot of raw material (i.e.
              identity, potency, chemical and microbial contaminants, and impurities), through
              third-party laboratories and following pharmacopeial standards, to eliminate the risk
              of adulteration and contamination, especially in herbal extracts. In addition, all
              ingredients are carefully selected and ensured to be gluten-free and non-GMO.
            </div>
            <div className="qnt-rawmaterial-img" />
          </div>
          <div className="contect-section formulation-section">
            <div className="formulation-text-block">
              <div className="header-line">Formulations</div>
              <p>
                Vita Aid specializes in creating synergistic, evidence-based formulations; every
                medicinal ingredient is included for a specific purpose. The process involves
                extensive research into clinical applications and human clinical trials of the
                ingredients.
              </p>
              <p>
                At Vita Aid, we place special emphasis on enhancing the bioavailability of our
                products. This is done by utilizing capsules instead of tablets, minimizing the
                amount of non-medicinal ingredients, as well as avoiding stearic acid lubricants
                such as magnesium stearate, to altogether maximize the effectiveness of active
                ingredients.
              </p>
              <p
                css={css`
                  font-style: italic;
                  font-weight: 600;
                `}
              >
                Why avoid magnesium stearate? How exactly does it affect bioavailability of
                medicinal ingredients?
              </p>
              <p>
                Magnesium stearate is a mineral-bound, long-chain, saturated fatty acid. It
                effectively coats every particle of ingredients and acts as a lubricant, allowing
                production to proceed faster and decreasing wear and tear on the machinery. However,
                due to its poor solubility in aqueous environments, a coating of magnesium stearate
                (Figure 1) can delay and decrease the absorption of active ingredients in the
                gastrointestinal tract (Figure 2).
              </p>
              <div className="figure-section">
                <div className="figure-block">
                  <div className="text-img-group">
                    <div className="coating-text">
                      Coating of Magnesium Stearate {isMobile === false && <br />}( must be broken
                      down for the digestive system to reach the ingredeint )
                    </div>
                    <div className="ingredient-text">Ingredeint</div>
                    <div className="qnt-formulation-1" />
                  </div>
                  <div className="figure1-text figure-text">
                    Figure 1. Coating of magnesium stearate on each particle.
                  </div>
                </div>
                <div className="figure-block">
                  <div className="qnt-formulation-2" />
                  <div className="figure2-text figure-text">
                    Figure 2. Effect of lubricant on release of active ingredient.
                  </div>
                </div>
              </div>
              <div className="by-not">
                By not using magnesium stearate in our products, we ensure that patients are
                receiving the maximum benefits from the active ingredients in our products.
              </div>
            </div>
          </div>

          <div className="contect-section inspection-section">
            <div className="inspection-text-block">
              <div className="header-line">Finished Product Inspection & Testing</div>
              <p>
                Every lot of finished product undergoes a series of screening inspections and tests
                that include package integrity, capsule disintegration tests, microbiological tests,
                assaying of medicinal ingredients, and specific allergen tests, before they can be
                released.
              </p>
              <p>
                As an advocate for high-quality standards, we provide transparency to quality
                testing information for every lot of our finished products, via our state-of-art
                online platform –{' '}
                <a href={`/qualitytrak/${country}`}>
                  <span className="quality-text">Quality</span>
                  <span className="trak-text">Trak</span>
                  <sup className="tm-text">TM</sup>
                </a>
                .
              </p>
            </div>
            <div className="qnt-inspection-img" />
          </div>

          <div className="contect-section building-section">
            <div className="building-text-block">
              <div className="header-line">Building Trust Through Quality</div>
              <p>
                Every year, a significant portion of our budget is re-invested into Research and
                Development initiatives and sourcing the best available raw materials, rather than
                marketing efforts.
              </p>
              <p>
                Through collaborations with our medical advisory team, consisting of ND's, MD's and
                TCM doctors, along with our strict quality system, our aim has always been to
                provide clinically relevant and highly effective formulas. Our commitment to the
                extraordinary has been rewarded by positive feedback from doctors and their patients
                alike.
              </p>
              <p className="last-line">
                At the end of the day, practitioners want to provide patients with products they can
                trust.
              </p>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
