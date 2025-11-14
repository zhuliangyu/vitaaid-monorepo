import { inMemoryToken } from './JwtToken';
export interface BingoQuestion {
  id: number;
  sponsor: string;
  question: string;
  sponsorType: string;
  choiceAry: string[];
  answerAry: string[];
  correctValue: string;
}

export const getBingoQuestions = async (): Promise<BingoQuestion[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    var data = await fetch('api/Bingo', requestOptions);
    return data.json();
  } catch (e) {
    return [] as BingoQuestion[];
  }
};
