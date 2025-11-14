/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { Helmet } from 'react-helmet-async';
import { BingoUser, DrawPrize } from '../model/BingoUser';
import { useLocation } from 'react-router-dom';

const useQuery = () => new URLSearchParams(useLocation().search);
export default function DrawPage() {
  let query = useQuery();
  const winners = query.get('winners');
  const [BingoUsers, setBingoUsers] = React.useState<BingoUser[]>([]);

  React.useEffect(() => {
    async function fetchData() {
      const data = await DrawPrize(winners);
      setBingoUsers(data);
    }
    fetchData();
  }, []);
  return (
    <React.Fragment>
      <Helmet>
        <title>BINGO - Draw Prizes</title>
      </Helmet>
      <Fragment>
        <table>
          <tr>
            <th>Prize Item</th>
            <th>First name</th>
            <th>Last name</th>
            <th>Email</th>
            <th>Date</th>
          </tr>
          {BingoUsers.filter((x) => x.prizeItem !== '')
            .sort((a, b) => (a.prizeItem > b.prizeItem ? 1 : -1))
            .map((x) => {
              return (
                <tr>
                  <td>{x.prizeItem}</td>
                  <td>{x.firstName}</td>
                  <td>{x.lastName}</td>
                  <td>{x.email}</td>
                  <td>{x.createdDate.toString()}</td>
                </tr>
              );
            })}
        </table>
        <br />
        <div>All participants:</div>
        <table>
          <tr>
            <th>ID</th>
            <th>Prize Item</th>
            <th>First name</th>
            <th>Last name</th>
            <th>Email</th>
            <th>Date</th>
          </tr>
          {BingoUsers.map((x, idx) => {
            return (
              <tr>
                <td>{`${idx + 1}`}</td>
                <td>{x.prizeItem}</td>
                <td>{x.firstName}</td>
                <td>{x.lastName}</td>
                <td>{x.email}</td>
                <td>{x.createdDate.toString()}</td>
              </tr>
            );
          })}
        </table>{' '}
      </Fragment>
    </React.Fragment>
  );
}
