export interface BingoUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  prizeItem: string;
  createdDate: Date;
}

export const DrawPrize = async (winners: string | null): Promise<BingoUser[]> => {
  try {
    const requestOptions = {
      method: 'GET',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    if (winners == null) return [] as BingoUser[];
    var data = await fetch(`api/Bingo/draw?winners=${winners}`, requestOptions);
    return data.json();
  } catch (e) {
    return [] as BingoUser[];
  }
};
