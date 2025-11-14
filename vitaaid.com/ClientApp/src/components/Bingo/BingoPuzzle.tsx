/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { useRef, useEffect } from 'react';

import { BingoQuestion } from '../../model/Bingo';

interface Props {
  puzzle: BingoQuestion;
  backState: number;
  refObj: any;
}

export const BingoPuzzle = (attrs: Props) => {
  const [answers, setAnswers] = React.useState<string[]>([]);

  React.useEffect(() => {
    attrs.puzzle.choiceAry.map((value) => {
      console.log(value);
    });
  }, []);

  return (
    <div
      className={
        attrs.backState === 0
          ? 'BingoPuzzle BingoInitBack'
          : attrs.backState === 1
          ? 'BingoPuzzle BingoOKBack'
          : 'BingoPuzzle BingoNGBack'
      }
    >
      <span className="question">{attrs.puzzle.question}</span>

      <div className="otherBlock">
        <select className="answerChoice" ref={attrs.refObj}>
          <option value=""></option>
          {attrs.puzzle.choiceAry.map((value) => {
            return <option value={`${value}`}>{value}</option>;
          })}
        </select>
        <div className="sponsorBlock">
          <span className="label">{attrs.puzzle.sponsorType}:&nbsp;</span>
          <span className="name">{attrs.puzzle.sponsor}</span>
        </div>
      </div>
    </div>
  );
};
