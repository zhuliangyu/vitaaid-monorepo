export const addCampaignUser = async (
  firstname: string,
  lastname: string,
  email: string,
  programname: string,
): Promise<void> => {
  try {
    const requestOptions = {
      method: 'PUT',
      headers: {
        ApiKey: `${process.env.REACT_APP_API_KEY!}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    };
    //api/Bingo/compaignuser?firstname={0}&lastname={1}&email={2}&programname={3}
    await fetch(
      `api/Bingo/compaignuser?firstname=${firstname}&lastname=${lastname}&email=${email}&programname=${programname}`,
      requestOptions,
    );
    return;
  } catch (e) {
    return;
  }
};
