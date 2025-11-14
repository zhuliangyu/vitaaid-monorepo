/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment, useRef } from 'react';
import { range } from 'underscore';
import { Helmet } from 'react-helmet-async';
import { useSelector, useDispatch } from 'react-redux';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { selectedCountry } from 'redux/features/country/countrySlice';
import { Country, State } from 'country-state-city';
import { isMobileData } from 'redux/features/isMobileSlice';
import {
  useForm,
  UseFormRegister,
  UseFormHandleSubmit,
  UseFormWatch,
  UseFormSetValue,
  FieldErrors,
  ValidateResult,
  UseFormSetError,
  Controller,
  Control,
} from 'react-hook-form';
import { ElementalNutritionData, getNutrition } from 'model/Product';

interface DosingData {
  product: string;
  gender: string;
  age: string;
  weight: string;
  feet: string;
  inches: string;
  activity: string;
  duraction: string;
  intake: string;
}

export const DosingCaculatorPage = () => {
  const [helpIdx, setHelpIdx] = React.useState(0);
  const [nutritionData, setNutritionData] = React.useState({} as ElementalNutritionData[]);
  const [caloriesNeeded, setCaloriesNeeded] = React.useState('--');
  const [unitsNeeded, setUnitsNeeded] = React.useState('--');
  const [costPerDay, setCostPerDay] = React.useState('--');
  const [showProductDesc, setShowProductDesc] = React.useState(true);
  const webSiteCountry = useSelector(selectedCountry);
  const navigate = useNavigate();
  const isMobile = useSelector(isMobileData);

  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
    setError,
    control,
  } = useForm<DosingData>({ mode: 'onBlur' });

  const form = useRef(null);

  React.useEffect(() => {
    async function fetchData() {
      const data = await getNutrition(webSiteCountry);
      setNutritionData(data);
    }
    fetchData();
  }, []);

  const submitForm = async (data: any) => {
    // Basic Options
    var gender = data.gender;
    var age = parseInt(data.age, 10);
    var weight = parseInt(data.weight, 10); //parseInt(jQuery('#weight').val());
    var feet = parseInt(data.feet, 10); //jQuery('#feet').val();
    var inches = parseInt(data.inches, 10); //jQuery('#inches').val();
    var level = parseFloat(data.activity); //jQuery('#level').val();
    var height = 0;

    // Weight needs to be converted into Kilograms, because the calculations are built on it
    //    o 1 pound = .453592 kilograms
    weight = weight * 0.453592;

    // Height needs to be converted into centimeters
    //    o 1 inch = 2.54 centimeters
    height = (feet + inches) * 2.54;

    var BMR = 0;
    var maintain = 0;
    var lose = 0;

    // Run the appropriate calculation
    //Men: 66.5 + (13.75 X weight in kg) + (5.003 X height in cm) - (6.775 X age in years)
    //Women: 655.1 + (9.563 X weight in kg) + (1.85 X height in cm) - (4.676 X age in years)
    if (gender == 'male') {
      BMR = 66.5 + 13.75 * weight + 5.003 * height - 6.775 * age;
    } else if (gender == 'female') {
      BMR = 655.1 + 9.563 * weight + 1.85 * height - 4.676 * age;
    }

    // BMR is multiplied by the activity factor to determine one’s daily caloric needs.
    var dailyCalories = BMR * level;

    // Display the results
    setCaloriesNeeded(dailyCalories.toFixed(0).toString());

    // Product use options
    var duration = parseInt(data.duraction, 10);
    var percentOfIntake = parseFloat(data.intake);
    var chosenProduct = data.product;

    // make sure we at least default a part code so we don't break
    if (!chosenProduct) {
      chosenProduct = 'VA-149';
    }

    // now do the calculations for how many bags and cost per day
    // The needed calories calculation is:  % of daily caloric intake x (caloric needs per day x the number of days being used).
    var productCalories = percentOfIntake * (dailyCalories * duration);

    // Determine how many large bags that corresponds to
    // Round up to the nearest large bag
    var selectedProd = nutritionData.filter((x) => x.productCode === chosenProduct)[0];
    var largeBags = productCalories / selectedProd.calories;
    largeBags = Math.ceil(largeBags);

    // Display the results

    var patientCost = (selectedProd.price * (productCalories / selectedProd.calories)) / duration;
    setUnitsNeeded(largeBags.toFixed(0).toString());
    setCostPerDay('$' + patientCost.toFixed(0).toString());
  };

  return (
    <React.Fragment>
      <Helmet>
        <title>Vita Aid - Dosing Caculator</title>
      </Helmet>
      <div className="content-main-body dosing-caculator">
        {isMobile && (
          <div className="result-m">
            <div className="title">Your Results </div>
            <div className="row result-detail">
              <div className="col-4">
                <div className="result-value">{caloriesNeeded}</div>
                <div>Calories Needed/Day</div>
              </div>
              <div className="col-4">
                <div className="result-value">{unitsNeeded}</div>
                <div>Units Needed</div>
              </div>
              <div className="col-4">
                <div>
                  {webSiteCountry === 'CA' ? 'CAD ' : 'USD '}
                  <span className="result-value">{costPerDay}</span>
                </div>
                <div>
                  Patient Cost
                  <br /> Per Day
                </div>
              </div>
            </div>
          </div>
        )}
        <div className="row">
          <div className="col-12">
            <div className="header-block">Dosing Caculator</div>
            <div className="seperate-line"></div>
            <div className="detail-block">
              <div className="left-block">
                <div
                  className="row"
                  css={css`
                    margin-left: 0px !important;
                    margin-right: 0px !important;
                  `}
                >
                  <form
                    ref={form}
                    className="col-12 dosing-caculator-form"
                    onSubmit={handleSubmit(submitForm)}
                  >
                    <div className="preface full-col">
                      Which product will you be using{' '}
                      <button
                        className="a-btn borderless-btn help-button"
                        onClick={() => {
                          setHelpIdx(0);
                          if (isMobile) setShowProductDesc(true);
                        }}
                      >
                        ?
                      </button>
                    </div>
                    <select
                      className="product-type full-col"
                      id="product"
                      {...register('product', {
                        required: true,
                      })}
                    >
                      <option selected value="VA-149">
                        Elemental Nutrition
                      </option>
                      <option value="VA-156">Keto-Elemental Nutrition</option>
                    </select>
                    {isMobile && showProductDesc && (
                      <div>
                        <div className="help">
                          <div className="close-img-block">
                            <img
                              className="close-img"
                              alt="close"
                              src="/img/x-m-object.png"
                              srcSet="/img/x-m-object@2x.png 2x, /img/x-m-object@3x.png 3x"
                              onClick={() => setShowProductDesc(false)}
                            ></img>
                          </div>{' '}
                          <div>
                            <p>
                              <span
                                css={css`
                                  font-weight: 600;
                                `}
                              >
                                Elemental Nutrition:
                              </span>
                              <br />
                              Consists of pre-digested/elemental nutrients (ie. amino acids, simple
                              carbohydrates, medium chain triglycerides), and essential vitamins and
                              minerals. Calorie Distributions: 50% carbohydrates (from dextrose &
                              maltodextrin), 20% protein (complete profile of free amino acids in
                              RDA amounts), and 30% fats (MCT from coconut).
                            </p>
                            <p>
                              <span
                                css={css`
                                  font-weight: 600;
                                `}
                              >
                                Keto-Elemental Nutrition:
                              </span>
                              <br />
                              Incorporating ketogenesis in Elemental Nutrition can further limit the
                              food sources for the bacterial overgrowth, as well as providing a
                              better sense of satiety. It is also the better option for patients
                              with diabetes/hyperglycemia compared to the conventional elemental
                              diet. Calorie Distributions: 15% carbohydrates (from maltodextrin),
                              15% protein (complete profile of free amino acids in RDA amounts), and
                              70% fats (MCT from coconut).
                              <br />
                              <span
                                css={css`
                                  color: red;
                                `}
                              >
                                *This product may be used in conjunction with a ketogenic diet.
                                Starting on a ketogenic diet may initially cause nausea / vomiting
                                due to the stimulation of bile secretions; in which case, you may
                                reduce the amount of each serving and/or drink slowly (e.g. 45
                                minutes/serving).
                              </span>
                            </p>
                          </div>
                        </div>
                      </div>
                    )}
                    <div className="two-col-line">
                      <div className="first-col">
                        <select
                          className="dosing-caculator-input"
                          id="gender"
                          placeholder="Gender"
                          {...register('gender', {
                            required: true,
                          })}
                        >
                          <option value="" disabled hidden>
                            Select gender
                          </option>
                          <option value="male">Male</option>
                          <option value="female">Female</option>{' '}
                        </select>
                        {errors.gender && (
                          <div key="err-gender" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div>
                      <div className="second-col ">
                        <select
                          className="dosing-caculator-input"
                          id="age"
                          placeholder="Age"
                          {...register('age', {
                            required: true,
                          })}
                        >
                          <option value="" disabled hidden>
                            Select age
                          </option>
                          {range(18, 100).map((x) => {
                            return <option value={`${x}`}>{x}</option>;
                          })}
                        </select>
                        {errors.age && (
                          <div key="err-age" className="error-msg">
                            This field is required
                          </div>
                        )}
                      </div>
                    </div>
                    <div className="two-col-line">
                      <div className="first-col">
                        <input
                          className="form-column form-input weight-content"
                          id="weight"
                          type="text"
                          {...register('weight', {
                            required: true,
                          })}
                          placeholder="Weight (in pounds)"
                        />
                      </div>
                      <div className="second-col ">
                        <div className="clearfix row form-column">
                          <div className="col-6 text-left">
                            <div className="input-group">
                              <select
                                className="form-control form-input"
                                data-role="none"
                                id="feet"
                                css={css`
                                  padding: 6px 6px;
                                  text-align-last: center;
                                `}
                                {...register('feet', {
                                  required: true,
                                })}
                              >
                                <option value="" disabled hidden>
                                  height
                                </option>
                                {range(1, 9).map((x) => {
                                  return <option value={`${x * 12}`}>{x}</option>;
                                })}
                              </select>
                              <div
                                className="input-group-addon"
                                css={css`
                                  padding: 6px 10px;
                                `}
                              >
                                Ft
                              </div>
                            </div>
                            {errors.feet && (
                              <div key="err-feet" className="error-msg">
                                This field is required
                              </div>
                            )}
                          </div>
                          <div className="col-6 text-left">
                            <div className="input-group">
                              <select
                                className="form-control form-input"
                                data-role="none"
                                id="inches"
                                css={css`
                                  padding: 6px 6px;
                                  text-align-last: center;
                                `}
                                {...register('inches', {
                                  required: true,
                                })}
                              >
                                <option value="" disabled hidden>
                                  height
                                </option>
                                {range(12).map((x) => {
                                  return <option value={`${x}`}>{x}</option>;
                                })}
                              </select>
                              <div
                                className="input-group-addon"
                                css={css`
                                  padding: 6px 10px;
                                `}
                              >
                                In
                              </div>
                            </div>
                            {errors.inches && (
                              <div key="err-inches" className="error-msg">
                                This field is required
                              </div>
                            )}
                          </div>
                        </div>
                      </div>
                    </div>
                    <div className="full-col">
                      <select
                        className="dosing-caculator-input"
                        id="activity"
                        {...register('activity', {
                          required: true,
                        })}
                      >
                        <option value="" disabled hidden>
                          Select activity level
                        </option>
                        <option value="1.2">Little to no exercise</option>
                        <option value="1.375">Light exercise (1-3 days per week)</option>
                        <option value="1.55">Moderate exercise (3-5 days per week)</option>
                        <option value="1.725">Heavy exercise (6-7 days per week)</option>
                        <option value="1.9">Intense exercise (twice a day)</option>
                      </select>
                      {errors.activity && (
                        <div key="err-activity" className="error-msg">
                          This field is required
                        </div>
                      )}
                    </div>
                    <div className="full-col exercise-desc">
                      Exercise = 15-30 minutes with an elevated heart rate
                      <br />
                      Intense Exercise = 45+ minutes with an elevated heart rate
                    </div>
                    <div className="two-col-line">
                      <div className="first-col form-column">
                        <div className="preface">
                          Duration of Product Use{' '}
                          {isMobile === false && (
                            <button
                              className="a-btn borderless-btn help-button"
                              onClick={() => setHelpIdx(1)}
                            >
                              ?
                            </button>
                          )}
                        </div>
                        <div>
                          <select
                            className="form-input input-content"
                            id="duraction"
                            placeholder="duraction"
                            {...register('duraction', {
                              required: true,
                            })}
                          >
                            <option value="" disabled hidden>
                              Select duraction
                            </option>
                            {range(3, 31).map((x) => (
                              <option value={`${x}`}>{x} Days</option>
                            ))}
                          </select>
                          {errors.duraction && (
                            <div key="err-duraction" className="error-msg">
                              This field is required
                            </div>
                          )}
                        </div>
                      </div>
                      {isMobile && (
                        <div className="help">
                          The number of days for which the product will be utilized.
                        </div>
                      )}
                      <div className="second-col form-column">
                        <div className="preface">
                          % of Daily Caloric Intake{' '}
                          {isMobile === false && (
                            <button
                              className="a-btn borderless-btn help-button"
                              onClick={() => setHelpIdx(2)}
                            >
                              ?
                            </button>
                          )}
                        </div>
                        <div>
                          <select
                            className="form-input input-content"
                            id="intake"
                            placeholder="intake"
                            {...register('intake', {
                              required: true,
                            })}
                          >
                            <option value="" disabled hidden>
                              Select intake
                            </option>
                            {range(25, 125, 25).map((x) => (
                              <option value={`${(x / 100).toFixed(2)}`}>{x}%</option>
                            ))}
                          </select>
                          {errors.intake && (
                            <div key="err-intake" className="error-msg">
                              This field is required
                            </div>
                          )}
                        </div>
                        {isMobile && (
                          <div className="help">
                            For moderate to severe GI dysfunction, a 14-21 day duration of 100% of
                            the daily caloric intake coming from an elemental diet is advised. Other
                            applications may include the use of an elemental diet for less than 100%
                            of the caloric needs.
                          </div>
                        )}{' '}
                      </div>
                    </div>

                    <div className="full-col submit-line">
                      <button className="submit-btn">CALCULATE</button>
                    </div>
                  </form>
                </div>
              </div>
              {isMobile === false && (
                <div className="right-block">
                  <div className="row">
                    <div className="col-8 help-col">
                      <div className="help">
                        {helpIdx === 0 && (
                          <div>
                            <p>
                              <span
                                css={css`
                                  font-weight: 600;
                                `}
                              >
                                Elemental Nutrition:
                              </span>
                              <br />
                              Consists of pre-digested/elemental nutrients (ie. amino acids, simple
                              carbohydrates, medium chain triglycerides), and essential vitamins and
                              minerals. Calorie Distributions: 50% carbohydrates (from dextrose &
                              maltodextrin), 20% protein (complete profile of free amino acids in
                              RDA amounts), and 30% fats (MCT from coconut).
                            </p>
                            <p>
                              <span
                                css={css`
                                  font-weight: 600;
                                `}
                              >
                                Keto-Elemental Nutrition:
                              </span>
                              <br />
                              Incorporating ketogenesis in Elemental Nutrition can further limit the
                              food sources for the bacterial overgrowth, as well as providing a
                              better sense of satiety. It is also the better option for patients
                              with diabetes/hyperglycemia compared to the conventional elemental
                              diet. Calorie Distributions: 15% carbohydrates (from maltodextrin),
                              15% protein (complete profile of free amino acids in RDA amounts), and
                              70% fats (MCT from coconut).
                              <br />
                              <span
                                css={css`
                                  color: red;
                                `}
                              >
                                *This product may be used in conjunction with a ketogenic diet.
                                Starting on a ketogenic diet may initially cause nausea / vomiting
                                due to the stimulation of bile secretions; in which case, you may
                                reduce the amount of each serving and/or drink slowly (e.g. 45
                                minutes/serving).
                              </span>
                            </p>
                          </div>
                        )}
                        {helpIdx === 1 && (
                          <div>The number of days for which the product will be utilized.</div>
                        )}
                        {helpIdx === 2 && (
                          <div>
                            For moderate to severe GI dysfunction, a 14-21 day duration of 100% of
                            the daily caloric intake coming from an elemental diet is advised. Other
                            applications may include the use of an elemental diet for less than 100%
                            of the caloric needs.
                          </div>
                        )}
                      </div>
                    </div>
                    <div className="col-4 result-col">
                      <div className="result">
                        <div className="result-value">{caloriesNeeded}</div>
                        <div>Calories Needed/Day</div>
                        <br />
                        <div className="result-value">{unitsNeeded}</div>
                        <div>Units Needed</div>
                        <br />
                        <div>
                          {webSiteCountry === 'CA' ? 'CAD ' : 'USD '}
                          <span className="result-value">{costPerDay}</span>
                        </div>
                        <div>Patient Cost Per Day</div>
                      </div>
                    </div>
                  </div>
                </div>
              )}
            </div>
            {/*  <div className="bottom-seperate-line"></div>*/}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
