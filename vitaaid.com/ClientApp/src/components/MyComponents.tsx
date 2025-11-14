/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useForm, UseFormRegisterReturn, FieldError } from 'react-hook-form';

interface RequiredFieldProps {
  LabelName: string;
  FieldName: string;
  FormRegister: UseFormRegisterReturn;
  Errors: FieldError | undefined;
  IsEnable: boolean | undefined;
  FieldType?: string | undefined;
  LabelWidth?: number | undefined;
  InputHeight?: number | undefined;
}

export const RequiredField = ({
  LabelName,
  FieldName,
  FormRegister,
  Errors,
  IsEnable,
  FieldType = 'text',
  LabelWidth = 120,
  InputHeight = 22,
}: RequiredFieldProps) => {
  return (
    <table>
      <tbody>
        <tr>
          <td
            css={css`
              width: ${LabelWidth}px;
            `}
          >
            <label htmlFor={FieldName}>{LabelName}</label>
          </td>
          <td>
            <input
              className={IsEnable === false ? 'input-disable' : ''}
              css={css`
                height: ${InputHeight}px;
              `}
              id={FieldName}
              type={FieldType}
              {...FormRegister}
            />
          </td>
        </tr>
        <tr
          css={css`
            line-height: 8px;
            font-size: 14px;
            color: #f24747;
          `}
        >
          <td>&nbsp;</td>
          <td>
            {Errors && Errors.type === 'required' && <div>This field is required</div>}
            {Errors && Errors.type === 'pattern' && <div>{Errors.message}</div>}
          </td>
        </tr>
      </tbody>
    </table>
  );
};
