/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useSelector } from 'react-redux';
import { isMobileData } from 'redux/features/isMobileSlice';

import { Helmet } from 'react-helmet-async';

export default function MedicalAdvisoryBoardPage() {
  const isMobile = useSelector(isMobileData);

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Medical Consultancy Team</title>
      </Helmet>
      <div className="content-main-body medical-advisory-board">
        <div className="row">
          <div className="col-12">
            <div className="header-block">Medical Advisory Board</div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-john-bender.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-john-bender.png" />
                      <p className="p-name">
                        <span className="name">Dr. John Bender</span>
                        <br /> <span className="title">HBSc, ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. John Bender, HBSc, ND</span>
                    </p>
                  )}
                  <p>
                    John Bender received his BSc (Honours Biology) from the University of Waterloo
                    in 1972. From 1969-1979 he worked as an EMT (Emergency Medical
                    Technician/Paramedic) and Medical Attendant, part-time during the school year
                    and full-time in the summer, at the Kitchener-Waterloo Hospital, Kitchener. He
                    met Dr. Arno Koegler in 1974 and worked and studied with him. In 1979, he
                    graduated from NCNM, Portland, Oregon, and later that year succeeded Dr. Koegler
                    in his practice.
                  </p>
                  <p>
                    He particularly uses low potency and complex remedies and has been influenced
                    primarily by Dr. Koegler, Dr. Bouko-Levy, Dr. Gueniot, Dr. Clement, and Dr.
                    Bianchi. John Bender served on the BDDT-N, the Naturopathic Regulatory Board
                    from 1981 to 1991 and was chair for 2 years. In 2007 he received an "Alumni of
                    Honour" award from the Faculty of Science University of Waterloo. This was one
                    of 50 awards all-time to Science graduates of U of W in recognition "of
                    significant professional achievement and/or contribution to community".
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-peter-bennett-nd.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-peter-bennett-nd.png" />
                      <p className="p-name">
                        <span className="name">Dr. Peter Bennett</span>
                        <br /> <span className="title">ND, RAc, DHANP</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <Fragment>
                      <p>
                        <span className="name">Dr. Peter Bennett ND, RAc, DHANP</span>
                      </p>
                      <ul>
                        <li> Naturopathic Physician</li>
                        <li> Registered Acupuncturist</li>
                        <li> Board Certified Homeopath</li>
                        <li> Author</li>
                        <li> Teacher/Lecturer</li>
                      </ul>
                    </Fragment>
                  )}
                  {isMobile && (
                    <Fragment>
                      <p>
                        <span>
                          - Naturopathic Physician
                          <br />
                        </span>
                        <span>
                          - Registered Acupuncturist
                          <br />
                        </span>
                        <span>
                          - Board Certified Homeopath
                          <br />
                        </span>
                        <span>
                          - Author
                          <br />
                        </span>
                        <span>
                          - Teacher/Lecturer
                          <br />
                        </span>
                      </p>
                    </Fragment>
                  )}
                  <p>
                    Dr. Peter Bennett practices in Langley and Whistler B.C., on the West coast of
                    Canada. Dr. Bennett uses diet, nutrition, herbal medicines, acupuncture,
                    homeopathy, physical medicine and intravenous nutritional medicines to help
                    patients with acute and chronic health problems. He frequently lectures to
                    medical and naturopathic doctors at conferences, teaches at the Boucher
                    Institute of Naturopathic Medicine, and teaches public seminars. Dr. Bennett
                    writes for magazines, and has been a featured expert on the Joe Easingwood Show
                    at CFAX radio, and on the Women's Television Network.
                  </p>
                  <p>
                    Dr. Bennett graduated from the University of British Columbia with a BA in Asian
                    Studies in 1980 and completed the 4 year naturopathic medical school program at
                    Bastyr University in 1987. Dr. Bennett concurrently completed the three-year
                    degree program in Traditional Chinese Medicine (TCM) at the Northwest Clinic of
                    Acupuncture and Oriental Medicine. He received his Doctorate of Naturopathic
                    Medicine (ND) in 1987 and was selected by his peers for a post-graduate
                    residency training program at Bastyr University. After completing his education,
                    Dr. Bennett returned to his home on a small island off the coast of British
                    Columbia where he worked as a sole practitioner for many years.
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-neil-mc-kinney.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-neil-mc-kinney.png" />
                      <p className="p-name">
                        <span className="name">Dr. Neil McKinney</span>
                        <br /> <span className="title">BSc, ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Neil McKinney, BSc, ND</span>
                    </p>
                  )}
                  <p>
                    Dr. Neil McKinney has been practicing since 1985. He has extensive experience in
                    Naturopathic Oncology as well as Naturopathic Medicine for a variety of health
                    concerns and ailments.
                  </p>
                  <p>
                    Dr. McKinney is a member of the Oncology Academy of Naturopathic Physicians, a
                    medical specialist society of the American Association of Naturopathic
                    Physicians.
                  </p>
                  <p>
                    His passion for treating cancer began when he was involved with cancer research
                    in the 1970s. He is a founder and a professor of Naturopathic Oncology at the
                    Boucher Institute of Naturopathic Medicine in New Westminster, British Columbia.
                  </p>
                  <p>
                    He has written three books on Naturopathic Care in Oncology; Naturally There's
                    Hope, Naturally There's Always Hope, and his newest publication released in
                    2011; Naturopathic Oncology: An Encyclopedic Guide For Patients & Physicians.
                  </p>
                  <p>
                    <a href="https://vitalvictoria.ca/publications" target="_blank">
                      Click here
                    </a>
                    &nbsp; for more information on his publications
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-brian-martin.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-brian-martin.png" />
                      <p className="p-name">
                        <span className="name">Dr. Brian Martin</span>
                        <br /> <span className="title">BSc, ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Brian Martin, BSc, ND</span>
                    </p>
                  )}
                  <p>
                    Dr. Martin graduated from the University of Alberta with a Bachelor of Science
                    Degree (BSc) in Biology and Chemistry. He continued his studies for four years
                    at the National College of Naturopathic Medicine in Portland, Oregon obtaining
                    his Naturopathic Doctorate Degree (ND). Dr. Martin spent an extra year of
                    training in the East Moreland Hospital Emergency Ward. In 1993 he opened
                    NaturoMed Health Clinic. In 2002 Dr. Martin opened Canada's first medically
                    supervised fitness clinic, which later changed names to become EnerChange Health
                    Clinic in December 2004.
                  </p>
                  <p>
                    Dr. Martin is a Diplomate in Anti-Aging Medicine from American Academy of
                    Anti-Aging Medicine (A4M) and is board-certified in Chelation Therapy.
                    Additionally, he is a former president of the College of Naturopathic Physicians
                    of British Columbia (CNPBC) with which he has served in various capacities.
                  </p>
                  <p>
                    A visionary and leader in complementary therapies, Dr. Brian Martin specializes
                    in chronic conditions such as cardiovascular disease, and diabetes. He is also
                    expert in treating some difficult common problems such as obesity, infertility,
                    and chronic fatigue. He is also a specialist in menopause, hormonal dysfunction
                    and other anti-aging modalities.
                  </p>
                  <p>
                    Dr. Martin believes that each person has the power to get healthy and stay
                    healthy if given the tools. "Anti-Aging" or "Healthy Aging" is a major theme of
                    his practice and his goal is to educate and empower. Dr. Martin accomplishes
                    this through positive day-to-day living, effective coaching techniques, his
                    "Knowledge and Choices" seminar series and other educational workshops and
                    lectures.
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-martin-kwo.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-martin-kwo.png" />
                      <p className="p-name">
                        <span className="name">Dr. Martin Kwok</span>
                        <br /> <span className="title">ND, MSAOM, Dr. TCM</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Martin Kwok, ND, MSAOM, Dr. TCM</span>
                    </p>
                  )}
                  <p>
                    Dr. Martin Kwok completed his Bachelor of Science degree from the University of
                    British Columbia. He received his Doctor of Naturopathic Medicine and Master of
                    Science in Acupuncture & Oriental Medicine from Bastyr University in Seattle
                    Washington.
                  </p>
                  <p>
                    Dr. Kwok holds a naturopathic physician license and a doctor of traditional
                    Chinese medicine license in BC, and has an active practice in the heart of
                    Richmond, BC. His areas of focus include: cardiovascular conditions, allergies,
                    cancer support, gastrointestinal conditions and soft tissue injuries.
                  </p>
                  <p>
                    Dr. Kwok is actively involved in his profession and the community. He is the
                    current secretary in National Traditional Chinese Medicine Association of Canada
                    (NTCM), and a past board member of the Osteoporosis Society of BC (OSTOP), the
                    Osteoporosis Society of Canada (BC division), and the British Columbia
                    Naturopathic Association (BCNA). He is a physician in good standing with the
                    College of Naturopathic Physicians of BC (CNPBC) and the College of Traditional
                    Chinese Medicine Practitioners and Acupuncturists of British Columbia (CTCMA).
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-bobby-parmar.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-bobby-parmar.png" />
                      <p className="p-name">
                        <span className="name">Dr. Bobby Parmar</span>
                        <br /> <span className="title">BASc., ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Bobby Parmar, BASc., ND </span>
                    </p>
                  )}
                  <p>
                    Dr. Bobby Parmar obtained his Bachelor of Applied Science degree (Kinesiology,
                    Psychology, and the Humanities) from Simon Fraser University. He proceeded to
                    complete naturopathic medical training at The Canadian College of Naturopathic
                    Medicine in Toronto.
                  </p>
                  <p>
                    As a Naturopathic Physician, his scope of practice includes Nutrition, both
                    Western and Eastern Herbology, Acupuncture, Intravenous therapy (Myers
                    Cocktails), and Allergy Scratch Testing. Bobby is also a licensed prescribing
                    physician of pharmaceutical medicines.
                  </p>
                  <p>
                    As a fervent educator, Bobby has tailored his academic career to best manage
                    family medicine. Within a general practice he has a special interest in hormones
                    and the disease states/symptoms caused by their imbalance including:
                    perimenopause, menopause, hypothyroidism (subclinical and Wilson's Temperature
                    Syndrome), adrenal fatigue and female hormone imbalance.
                  </p>
                  <p>
                    Dr. Parmar is driven by a passion to better understand the dynamics of health
                    and above all the every growing field of mind-body medicine and to convey that
                    understanding to anyone in need or with the desire to be educated.
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-carol-y-lin.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-carol-y-lin.png" />
                      <p className="p-name">
                        <span className="name">Dr. Carol Y. Lin</span>
                        <br /> <span className="title">BSc, ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Carol Y. Lin, BSc, ND </span>
                    </p>
                  )}
                  <p>
                    Dr. Carol Lin is a board certified and licensed Naturopathic Physician. She is a
                    University of British Columbia graduate with a degree in biology specializing in
                    cell biology and genetics. She went on to receive a doctorate degree in
                    Naturopathic Medicine from Bastyr University, one of the world's leading centres
                    in natural medicine and research.
                  </p>
                  <p>
                    Her practice offers family health care to create balance and wellness based on
                    the philosophy of naturopathic medicine. She uses a variety of modalities
                    including, lifestyle management, clinical nutrition, botanical medicine,
                    homeopathic medicine, Biotherapeutic Drainage, Psychosomatic energetic (PES),
                    French Mesotherapy and Nambudripad's Allergy Elimination Technique (NAET).
                  </p>
                </div>
              </div>
            </div>

            <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/dr-lorenzo-dian.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/dr-lorenzo-dian.png" />
                      <p className="p-name">
                        <span className="name">Dr. Lorenzo Diana</span>
                        <br /> <span className="title">BA (Hons), ND</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Dr. Lorenzo Diana, BA (Hons), ND</span>
                    </p>
                  )}
                  <p>
                    Lorenzo M. Diana is a graduate of the Canadian College of Naturopathic Medicine
                    and York University. Since graduation, Dr. Diana has established his practice as
                    a Naturopathic Doctor through a number of clinics in Ontario, located in Ajax,
                    Markham, Mississauga and Simcoe. While he has developed an eclectic practice, he
                    has cultivated particular expertise in women's issues, digestive disorders,
                    Naturopathic Oncology, Fibromyalgia, preventive healthcare and more. Dr. Diana
                    uses a wide range of treatment modalities including Homeopathy, Acupuncture and
                    Nutritional and Botanical Medicine for the benefit of his patients.
                  </p>
                  <p>
                    Since 1998, Dr. Diana has acted as Simcoe's first Naturopathic Doctor. His
                    practice has expanded in Simcoe and Dr. Diana intends to continue to serve the
                    community from his new and enlarged premises at the Natural Health Clinic &
                    Educational Centre, 338 Norfolk Street, South, Simcoe Ontario.
                  </p>
                  <p>
                    Dr. Diana regularly contributes to articles on natural health topics in various
                    journals and magazines and has appeared as a guest on CFRB Toronto and community
                    broadcasts elsewhere. In addition, he conducts a series of lectures across the
                    Greater Toronto and Simcoe areas.
                  </p>
                  <p>
                    Aside from running busy practices, Dr. Diana has embarked on a project to
                    develop a line of 100% natural and organic supplements designed to treat a
                    variety of disorders as well as writing a book on Naturopathic Medicine and
                    Natural Health.
                  </p>
                </div>
              </div>
            </div>

            {/* <div className="dr-block">
              {isMobile === false && (
                <div className="dr-img">
                  <img alt="" src="/img/clara-cohen-dtcm-r-ac.png" />
                </div>
              )}
              <div className="dr-desc">
                <div>
                  {isMobile && (
                    <Fragment>
                      <img className="photo-mobile" alt="" src="/img/clara-cohen-dtcm-r-ac.png" />
                      <p className="p-name">
                        <span className="name">Clara Cohen</span>
                        <br /> <span className="title">DTCM, R.Ac</span>
                      </p>
                      <div className="dr-name-separate-line"></div>
                    </Fragment>
                  )}
                  {isMobile === false && (
                    <p>
                      <span className="name">Clara Cohen, DTCM, R.Ac</span>
                    </p>
                  )}
                  <p>
                    Clara Cohen comes from the French Alps where her family used Chiropractic,
                    Acupuncture, Homeopathy, Physiotherapy and Massage as their main healing
                    sources. She graduated from Grenoble University with a Bachelor in Applied
                    Nutrition. When she moved to Canada she became a Personal Trainer and managed 2
                    fitness clubs in Vancouver and West Vancouver for 8 years. She has extensive
                    training in exercise physiology, sports nutrition and weight issues. She has
                    appeared on Breakfast Television twice and has given countless workshops on
                    Natural Health to many corporations, schools and community centres. She
                    maintains her CPR and First Aid certification current.
                  </p>
                  <p>
                    Clara Cohen is a graduate of the International College of Traditional Chinese
                    Medicine of Vancouver where she completed a 5 year program at the Doctor of
                    Traditional Chinese Medicine level. She has been a B.C registered Acupuncturist
                    since 2003 with CTCMA and focuses mainly in Gynecology and mental health issues.
                    She has successfully treated uterus cancer, ovarian cysts & fibroids,
                    dysmenorrhea, fertility issues, depression, anxiety and much more.
                  </p>
                  <p>
                    Clara Cohen is also a professor, currently teaching at the Boucher Institure of
                    Naturopathic Medicine in New Westminster and enjoying every minute of it. She
                    also keeps taking seminars and workshops on natural health and Chinese Medicine
                    to further improve her knowledge and to help her patients with their healing
                    process.
                  </p>
                </div>
              </div>
            </div> */}

          </div>
        </div>
      </div>
    </React.Fragment>
  );
}
