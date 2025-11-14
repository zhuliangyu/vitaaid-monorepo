/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { createRef, useRef, RefObject } from 'react';
import { Helmet } from 'react-helmet-async';
import { BingoPuzzle } from '../components/Bingo/BingoPuzzle';
import { BingoQuestion, getBingoQuestions } from '../model/Bingo';
import { useForm } from 'react-hook-form';
import { addCampaignUser } from '../model/CampaignUser';
import { RequiredField } from '../components/MyComponents';

type UserInfoFormData = {
  firstname: string;
  lastname: string;
  email: string;
};

export default function BingoPage() {
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<UserInfoFormData>({
    mode: 'onBlur',
  });

  const submitForm = async (data: UserInfoFormData) => {
    if (data.firstname !== '' && data.lastname !== '' && data.email !== '') {
      await addCampaignUser(
        data.firstname,
        data.lastname,
        data.email,
        'Clinical Success Summit 2022',
      );
      setSubmittedState(1);
    }
  };

  const [Questions, setQuestions] = React.useState<BingoQuestion[]>([]);
  const puzzleSize = 4;
  const [refObjs, setRefObjs] = React.useState([]);
  const [backStates, setBackStates] = React.useState([] as number[]);
  const [winState, setWinState] = React.useState(0);
  const [submittedState, setSubmittedState] = React.useState(0);
  // const [correctLine, setLine] = React.useState(0);
  let idx = 0;
  const range = (min: number, max: number) => {
    const arr = Array(max - min + 1)
      .fill(0)
      .map((_, i) => i + min);
    return arr;
  };
  const handleCheckAnswers = () => {
    const newBackStates = Array(puzzleSize * puzzleSize)
      .fill(0)
      .map((_, idx) => {
        const refObj = refObjs[idx] as RefObject<HTMLSelectElement>;
        const userAnswer = refObj.current?.value;
        if (userAnswer === '') return 0;
        else if (userAnswer === Questions[idx].correctValue) return 1;
        else return 2;
      });
    setBackStates(newBackStates);
    let cLine = 0;
    // for row line
    range(0, puzzleSize - 1).forEach((idx, _) => {
      if (
        range(idx * puzzleSize, (idx + 1) * puzzleSize - 1).filter(
          (x, idx) => newBackStates[x] === 1,
        ).length === puzzleSize
      )
        cLine++;
    });
    // for column line
    range(0, puzzleSize - 1).forEach((startIdx, _) => {
      if (
        Array(puzzleSize)
          .fill(0)
          .map((_, idx) => startIdx + idx * puzzleSize)
          .filter((x, idx) => newBackStates[x] === 1).length === puzzleSize
      )
        cLine++;
    });
    // diagonals
    if (
      Array(puzzleSize)
        .fill(0)
        .map((_, idx) => idx + idx * puzzleSize)
        .filter((x, idx) => newBackStates[x] === 1).length === puzzleSize
    )
      cLine++;
    if (
      Array(puzzleSize)
        .fill(0)
        .map((_, idx) => (idx + 1) * (puzzleSize - 1))
        .filter((x, idx) => newBackStates[x] === 1).length === puzzleSize
    )
      cLine++;

    if (cLine > 1) setWinState(1);
    else setWinState(0);
    // setLine(cLine);
  };

  React.useEffect(() => {
    async function fetchData() {
      const data = await getBingoQuestions();
      data.forEach((x) => {
        x.correctValue = x.choiceAry[Number(x.answerAry[0]) - 1];
        const sub = x.choiceAry[0].substr(0, 2).trim().toUpperCase();
        // randomize answers feature is disable temporally 
        // if (sub !== '1:' && sub !== 'A:') x.choiceAry = x.choiceAry.sort(() => 0.5 - Math.random());
      });
      setQuestions(data.sort(() => 0.5 - Math.random()));
    }
    fetchData();
    //initialize;
    setRefObjs((prevObjs) =>
      Array(puzzleSize * puzzleSize)
        .fill(null)
        .map((_, i) => prevObjs[i] || createRef()),
    );
    setBackStates(Array(puzzleSize * puzzleSize).fill(0));
  }, []);
  return (
    <React.Fragment>
      <Helmet>
        <title>BINGO - Master</title>
      </Helmet>
      <div className="bingo-page">
        <div className="row rowpuzzle bingo-welcome">
          <div className="col-12 ">
            <div className="line1">Welcome to BINGO Master</div>
            <div className="line2">&#x40;Clinical Success Summit</div>
            <span className="line3">
              Please complete{' '}
              <span
                css={css`
                  text-decoration: underline;
                `}
              >
                TWO LINES
              </span>{' '}
              of correct answers in the following BINGO card to enter the Raffle Price Draw.
            </span>
          </div>
        </div>
        {Questions.length > 0 &&
          range(1, puzzleSize).map((rowIdx) => {
            return (
              <div className="row rowpuzzle justify-content-center">
                {range(1, puzzleSize).map((colIdx) => {
                  idx = (rowIdx - 1) * 4 + colIdx - 1;
                  return (
                    <div className="col-3">
                      <BingoPuzzle
                        key={`${rowIdx}${colIdx}`}
                        puzzle={Questions[idx]}
                        backState={backStates[idx]}
                        refObj={refObjs[idx]}
                      ></BingoPuzzle>
                    </div>
                  );
                })}
              </div>
            );
          })}
        <div className="row bingo-check-answer">
          {winState === 1 && (
            <div className="col-12 d-flex justify-content-left align-items-center">
              <div className="BingoWin animate__animated animate__bounceInLeft">
                <span className="bingo-flag">BINGO!</span>
                <br />
                Please enter your contact information below to Enter the Raffle Draw!
              </div>
            </div>
          )}
          {winState !== 1 && (
            <div className="col-12">
              <button className="checkAnswerButton" onClick={handleCheckAnswers}>
                Check Answers
              </button>
            </div>
          )}
        </div>
        {winState === 1 && (
          <React.Fragment>
            <div className="row bingo-user-info">
              <form className="col-12" onSubmit={handleSubmit(submitForm)}>
                <div>
                  <RequiredField
                    LabelName="First Name&nbsp;:"
                    FieldName="firstname"
                    FormRegister={register('firstname', { required: true })}
                    Errors={errors.firstname}
                    IsEnable={submittedState === 1 ? false : true}
                    LabelWidth={143}
                    InputHeight={35}
                  />
                </div>
                <div>
                  <RequiredField
                    LabelName="Last Name&nbsp;:"
                    FieldName="lastname"
                    FormRegister={register('lastname', { required: true })}
                    Errors={errors.lastname}
                    IsEnable={submittedState === 1 ? false : true}
                    LabelWidth={143}
                    InputHeight={35}
                  />
                </div>
                <div>
                  <RequiredField
                    LabelName="Email&nbsp;:"
                    FieldName="email"
                    FormRegister={register('email', {
                      required: true,
                      pattern: {
                        value: /\S+@\S+.\S+/,
                        message: 'Entered value does not match email format',
                      },
                    })}
                    Errors={errors.email}
                    IsEnable={submittedState === 1 ? false : true}
                    LabelWidth={143}
                    InputHeight={35}
                  />
                </div>
                <button
                  className={submittedState === 1 ? 'btn-disable summitButton' : 'summitButton'}
                  type="submit"
                >
                  Submit
                </button>
              </form>
            </div>
            {submittedState === 1 && (
              <div className="row bingo-user-info">
                <span className="col-12 success-msg">
                  You have successfully submitted your BINGO Card.
                  <br /> Please stay for the closing of the conference when we will announce the
                  winners.
                </span>
              </div>
            )}
          </React.Fragment>
        )}
        <div className="row bingo-footer">
          <span className="col-12 line1">Platinum Sponsors</span>
          {/* <div className="bingo-logos"></div> */}
        </div>
        <div className="row rowpuzzle img-fluid d-flex justify-content-center align-items-center">
          {/* <div className="col-6">
            <a href="https://canadarna.com/" target="_blank">
              <img className="crna-logo" alt="CRNA"></img>
            </a>
          </div> */}
          <div className="col-6">
            <a href="https://vitaaid.com/" target="_blank">
              <img className="vitaaid-logo" alt="vitaaid"></img>
            </a>
          </div>
          <div className="col-6">
            <a href="https://sibodiagnostics.com/" target="_blank">
              <img className="sibo_diagnostics_logo" alt="sibo_diagnostics"></img>
            </a>
          </div>
        </div>
        <div className="row rowpuzzle img-fluid d-flex justify-content-center align-items-center">
          {/* <div className="col-6">
            <a href="https://sibodiagnostics.com/" target="_blank">
              <img className="sibo-logo" alt="sibo"></img>
            </a>
          </div> */}
          {/* <div className="col-6">
            <a href="https://vitaaid.com/" target="_blank">
              <img className="vitaaid-logo" alt="vitaaid"></img>
            </a>
          </div> */}
        </div>


        <div className="row bingo-footer">
          <span className="col-12 line2">Gold Sponsors</span>
          {/* <div className="bingo-logos"></div> */}
        </div>
        <div
          className="row rowpuzzle img-fluid d-flex justify-content-center align-items-center"
          css={css`
            margin-bottom: 40px;
          `}
        >
          <div className="col-6">
            <a href="https://canadarna.com/" target="_blank">
              <img className="crna-logo" alt="CRNA_Banner.jpg"></img>
            </a>
          </div>
          <div className="col-6">
            <a href="https://fullscript.com/" target="_blank">
              <img className="fullscript_logo" alt="fullscript"></img>
            </a>
          </div>
          {/* <div className="col-4">
            <a href="https://quantumallergycanada.com/" target="_blank">
              <img className="qac-logo" alt="excellanalytical_logo.png"></img>
            </a>
          </div>
          <div className="col-4">
            <a href="https://sibo.ca/" target="_blank">
              <img className="sibo-white-2023-logo" alt="sibo_white_logo.jpg"></img>
            </a>
          </div> */}
        </div>

        {/* <div
          className="row rowpuzzle img-fluid d-flex justify-content-center align-items-center"
          css={css`
            margin-bottom: 40px;
          `}
        >
          <div className="col-4" />
          <div className="col-4">
            <a href="http://quantumallergycanada.com/" target="_blank">
              <img className="qac-logo" alt="qac"></img>
            </a>
          </div>
          <div className="col-4" />
        </div> */}
      </div>
    </React.Fragment>
  );
}
