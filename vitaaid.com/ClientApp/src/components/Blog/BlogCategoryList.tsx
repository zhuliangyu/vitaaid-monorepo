/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React, { Fragment } from 'react';
import { useNavigate } from 'react-router-dom';

import { eUNITTYPE, UnitTypeData, getUnitTypes } from 'model/UnitType';
import { BlogData, getBlog, getBlogByCategory } from 'model/Blog';

interface Props {
  categories: UnitTypeData[];
  onSelectedCategory: (x: UnitTypeData) => void;
}

export const BlogCategoryList = ({ categories, onSelectedCategory }: Props) => {
  return (
    <div className="blog-category-block">
      <div className="related-title">
        <img
          className="list-img"
          alt=""
          src="/img/category-list.png"
          srcSet="/img/category-list@2x.png 2x, /img/category-list@3x.png 3x"
        />
        <span>CATEGORIES</span>
      </div>
      <div>
        {categories.map((x) => (
          <button
            className="borderless-btn category-link"
            key={`${x.id}`}
            onClick={() => onSelectedCategory(x)}
          >
            {x.name}
          </button>
        ))}
      </div>
    </div>
  );
};
