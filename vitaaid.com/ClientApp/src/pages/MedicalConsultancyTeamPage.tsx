/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector } from 'react-redux';
import { Helmet } from 'react-helmet-async';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function MedicalConsultancyTeamPage() {
  const isMobile = useSelector(isMobileData);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Medical Consultancy Team</title>
      </Helmet>
      <div className="content-main-body medical-consultancy-team">
        <div className="row">
          <div className="col-12">
            <div className="header-block">Medical Consultancy Team</div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/adam-293-x-300.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/adam-293-x-300.png" />
                      <p className="p-name">
                        <span className="name">Dr. Adam Amodeo</span>
                        <br /> <span className="title">ND, HBSc</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Adam Amodeo, ND, HBSc | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}

                  <p>
                    Adam is a board-certified Naturopathic Doctor, registered with the College of
                    Naturopaths of Ontario. He obtained his undergraduate degree from The University
                    of Western Ontario with an Honours Specialization in Biology. He graduated from
                    the Canadian College of Naturopathic Medicine in Toronto, Ontario in 2015.
                  </p>
                  <p>
                    In 2018 he co-founded Collingwood Naturopathic where he focuses on men's health
                    and longevity. He is also the Education and Technical Support Manager of Eastern
                    Canada for Vita Aid Professional Therapeutics, keeping other health care
                    providers across the country up to date on clinical research and new tools for
                    practice.
                  </p>
                </div>
              </div>
            </div>
            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-brent-barlow-nd-advsior.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img
                        className="photo-mobile"
                        alt=""
                        src="/img/dr-brent-barlow-nd-advsior.png"
                      />
                      <p className="p-name">
                        <span className="name">Dr. Brent Barlow</span>
                        <br /> <span className="title">ND</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Brent Barlow, ND | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Brent Barlow is a Naturopathic Physician practicing in Kelowna, British
                    Columbia. He graduated from the Boucher Institute of Naturopathic Medicine in
                    New Westminster, BC.
                  </p>
                  <p>
                    Dr. Barlow's practice focuses on food allergies, various digestive issues, pain
                    management, MSK, detoxification, hormone, etc.
                  </p>
                  <p>
                    He utilizes diet therapy, botanical medicine, nutritional supplementation,
                    acupuncture, homeopathy and the specialized treatments of Prolotherapy, Neural
                    Therapy, Intravenous Nutrient Infusions, and Chelation Therapy.
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-erika-kneeland-b-sc-hons-nd-advsior.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img
                        className="photo-mobile"
                        alt=""
                        src="/img/dr-erika-kneeland-b-sc-hons-nd-advsior.png"
                      />
                      <p className="p-name">
                        <span className="name">Dr. Erika Kneeland</span>
                        <br /> <span className="title">BSc (Hons), ND</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Erika Kneeland, BSc (Hons), ND | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Erika Kneeland is a Naturopathic physician practicing in Courtenay, B.C. She
                    is thrilled to be practicing naturopathic medicine in such a beautiful and
                    health-oriented community. She currently sees patients who come from Courtenay,
                    Comox, Cumberland, Campbell River, Port Hardy, Powell River, Denman, Hornby,
                    Quadra and Cortez Islands. Dr. Kneeland studied with and took over the practice
                    of Dr. Heather Marinaccio, ND, the first Naturopathic doctor to begin practicing
                    in the Comox Valley.
                  </p>
                  <p>
                    Dr. Kneeland is a graduate from the Boucher Institute of Naturopathic Medicine,
                    Western Canada's only accredited Naturopathic Medical School. Prior to her four
                    years of Naturopathic Medical training, she obtained a Bachelor of Science
                    degree, with honours, from the University of Calgary, and wrote an undergraduate
                    thesis on the level of empirical evidence standing behind natural medicines.
                  </p>
                  <p>
                    In addition to the medical training standard for all primary care physicians,
                    Dr. Kneeland has further training in clinical nutrition (diet and supplements),
                    Western botanical medicine, Traditional Chinese Medicine and acupuncture,
                    classical and complex homeopathy, Bowen therapy and lifestyle counselling. She
                    is certified in First Aid, CPR, Advanced Cardiac Life Support and Intravenous
                    therapies. Dr. Kneeland has her prescriptive authority, which means that she is
                    licensed to prescribe pharmaceuticals. This allows Dr. Kneeland to work with her
                    patients on safely lowering medication doses where appropriate as well helps her
                    educate her patients on the side effects of medications and the interactions
                    between supplements and medications. She loves naturopathic medicine because it
                    has provided her with many tools to choose from when designing individual
                    treatment plans for her patients.{' '}
                  </p>
                </div>
              </div>
            </div>
            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-hamid-tajbakhsh-advsior.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img
                        className="photo-mobile"
                        alt=""
                        src="/img/dr-hamid-tajbakhsh-advsior.png"
                      />
                      <p className="p-name">
                        <span className="name">Dr. Hamid Tajbakhsh</span>
                        <br /> <span className="title">BSc, ND</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Hamid Tajbakhsh, BSc, ND | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Hamid graduated from the Boucher Institute of Naturopathic Medicine and was
                    awarded the Academic Excellence Award. Previously, he had completed his
                    post-secondary education at UBC majoring in Biology with a minor in Psychology
                    after being awarded an entrance scholarship into his program of studies.
                  </p>
                  <p>
                    Dr. Hamid has always been interested in furthering his education and has taken
                    on extra-curricular programs and focus areas such as delving deeper into the
                    studies of Chinese Medicine & Applied Kinesiology. Dr. Hamid applies a gentle
                    approach to acupuncture ensuring utilization of a minimal number of needles with
                    no excessive needle manipulation.
                  </p>
                  <p>
                    Dr. Hamid is also well versed in fascial techniques such as Bowen Therapy and
                    CranioSacral Therapy which are gentle non-invasive procedures designed to
                    normalize body functions and help break the body out of repetitive pain
                    patterns. These techniques coupled with Eastern & Western herbal formulas, and
                    nutritional optimization allow Dr. Hamid the ability to facilitate healing and
                    provide exceptional care to his patients.
                  </p>
                </div>
              </div>
            </div>
            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/drjosephphoto.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/drjosephphoto.png" />
                      <p className="p-name">
                        <span className="name">Dr. Joseph Cheng</span>
                        <br /> <span className="title">ND, BSc (Biochem)</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Joseph Cheng, ND, BSc (Biochem) | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Joseph Cheng is a board-certified Naturopathic Doctor with the College of
                    Naturopathic Physicians of BC and the Product Director of Vita Aid Professional
                    Therapeutics. Dr. Cheng invests deeply in learning about evidence-based,
                    clinically relevant ingredients and treatments, and applies them to not only his
                    practice but also the development of Vita Aid formulas and protocols.
                  </p>
                  <p>
                    He currently practices in Vancouver, British Columbia, with a clinical focus on
                    digestive and metabolic health. He obtained his undergraduate degree in
                    Biochemistry from the University of British Columbia and the Naturopathic
                    Medical degree from the Canadian College of Naturopathic Medicine (formerly
                    Boucher Institute of Naturopathic Medicine).
                  </p>
                </div>
              </div>
            </div>
            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/04-ll-headshot.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/04-ll-headshot.png" />
                      <p className="p-name">
                        <span className="name">Dr. Liam LaTouche</span>
                        <br /> <span className="title">ND, HBSc (Kin)</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Liam LaTouche, ND, HBSc (Kin) | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Liam LaTouche graduated from Western University with a Bachelor of Science
                    Honors Specialization in Kinesiology degree and the Canadian College of
                    Naturopathic Medicine with a Doctor of Naturopathy degree, allowing him to
                    combine his passions for fitness, modern scientific knowledge, and traditional
                    systems of medicine.
                  </p>
                  <p>
                    Dr. LaTouche practices in Barrie, Ontario with a clinical focus in complex
                    chronic disease (chronic infections, biotoxin illness, autoimmune disease,
                    chronic pain, chronic fatigue). Using an integrative mind-body approach, Dr.
                    LaTouche helps patients who experience conditions that have been difficult to
                    diagnosis and treat and who have had limited success working with other
                    practitioners. He is also a part-time faculty member at the Canadian School of
                    Natural Nutrition in Toronto.
                  </p>
                </div>
              </div>
            </div>
            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-lisa-ghent-advsior.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-lisa-ghent-advsior.png" />
                      <p className="p-name">
                        <span className="name">Dr. Lisa Ghent</span>
                        <br /> <span className="title">BSc, ND</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Lisa Ghent, BSc, ND | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Lisa Ghent is a Naturopathic Physician practicing in South Delta (Ladner,
                    Tsawwassen), BC. She graduated from the Boucher Institute of Naturopathic
                    Medicine in New Westminster, BC and from Athabasca University with a B.Sc. in
                    Life Sciences.
                  </p>
                  <p>
                    Dr. Ghent's has two family practices that provide acute, chronic and
                    preventative care to people of all ages. Dr. Ghent has a special interest and
                    additional education in women's health (fertility, OB/GYN, pre-/post-natal care)
                    as well as paediatrics (including newborns). Mental health is also a passion of
                    Dr. Ghent's, particularly natural treatment of anxiety, depression and insomnia.
                  </p>
                  <p>
                    As a busy mom of 3, Dr. Ghent understands the importance of balance, and
                    provides practical, affordable naturopathic care. She primarily utilizes diet
                    and lifestyle amendments, botanical medicine, nutritional supplementation, IV
                    therapies as well as acupuncture. Dr. Ghent also hold prescriptive authority in
                    the province of BC.
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-tomah-phillips-advsior.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img
                        className="photo-mobile"
                        alt=""
                        src="/img/dr-tomah-phillips-advsior.png"
                      />
                      <p className="p-name">
                        <span className="name">Dr. Tomah Phillips</span>
                        <br /> <span className="title">ND</span>
                        <br /> <span className="title-2">| Professional Biography</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Tomah Phillips, ND | </span>
                      <span className="title-2">Professional Biography</span>
                    </p>
                  )}
                  <p>
                    Dr. Tomah received his Naturopathic Doctor (ND) degree from the Boucher
                    Institute of Naturopathic Medicine, including over 1100 patient hours. He also
                    holds a Bachelor of Science (B.Sc.) in Physiology from McGill University in
                    Montreal.
                  </p>
                  <p>
                    Currently accepting new patients at Kinetic Patterns in Vancouver (#603-805 W
                    Broadway), Dr. Tomah focuses of Men's Health and Urology, addressing complaints
                    "below the belt", including urinary issues, prostate dysfunction (BPH,
                    prostatitis), erectile dysfunction, low libido, and male infertility.
                  </p>
                  <p>
                    Dr. Tomah also has a passion for digestion, and believes that most disease
                    begins in the gut. So whether you suffer from a gastrointestinal disease such as
                    Irritable Bowel Syndrome or Inflammatory Bowel Disease, or a systemic issue such
                    as eczema, psoriasis, fatigue, or immune issues, Dr. Tomah can help.
                  </p>
                  <p>
                    Dr. Tomah is also an adjunct faculty member of the Boucher Institute of
                    Naturopathic Medicine, where he teaches Biomedical Sciences to first and second
                    year students. He is also an instructor at the Canadian School of Natural
                    Nutrition, where he teaches Anatomy & Physiology and Pathology courses.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
