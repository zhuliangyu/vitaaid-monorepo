/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';

interface Props {
  subNodes: string[];
  hrefs: string[];
  actions?: ((() => void) | null)[];
}

export const BreadCrumbs = ({ subNodes, hrefs, actions }: Props) => {
  return (
    <div className="row img-fluid BreadCrumbs">
      <ol
        css={css`
          padding-left: 0px;
          margin-bottom: 0px;
        `}
      >
        {/* <li>
          <a key={99999} href="/">
            HOME
          </a>
        </li> */}
        {subNodes.length >= 1 &&
          subNodes
            .filter((x) => x.length > 0)
            .map((x, idx) => {
              return hrefs[idx].length > 0 ? (
                <li key={idx}>
                  {idx > 0 && <span className="delimiter">&gt;</span>}
                  <a
                    href={`${hrefs[idx]}`}
                    onClick={() => {
                      if (
                        actions != null &&
                        actions !== undefined &&
                        actions!.length - 1 >= idx &&
                        actions[idx] != null
                      )
                        actions[idx]!();
                      return true;
                    }}
                    dangerouslySetInnerHTML={{ __html: x }}
                  ></a>
                </li>
              ) : (
                <li key={idx}>
                  <span className="delimiter">&gt;</span>
                  <span
                    className="last-item"
                    dangerouslySetInnerHTML={{
                      __html: subNodes[subNodes.length - 1],
                    }}
                  ></span>
                </li>
              );
            })}
      </ol>
    </div>
  );
};

interface BCBProps {
  subNodes: string[];
  actions: ((() => void) | null)[];
}

export const BreadCrumbButtons = ({ subNodes, actions }: BCBProps) => {
  return (
    <div className="row img-fluid BreadCrumbs">
      <ol
        css={css`
          padding-left: 0px;
          margin-bottom: 0px;
        `}
      >
        {/* <li>
          <a key={99999} href="/">
            HOME
          </a>
        </li> */}
        {subNodes.length >= 1 &&
          subNodes
            .filter((x) => x.length > 0)
            .map((x, idx) => {
              return actions[idx] != null ? (
                <li key={idx}>
                  {idx > 0 && <span className="delimiter">&gt;</span>}
                  <button
                    className="a-btn"
                    onClick={() => {
                      actions[idx]!();
                      return true;
                    }}
                    dangerouslySetInnerHTML={{ __html: x }}
                  ></button>
                </li>
              ) : (
                <li key={idx}>
                  <span className="delimiter">&gt;</span>
                  <span
                    className="last-item"
                    dangerouslySetInnerHTML={{
                      __html: subNodes[subNodes.length - 1],
                    }}
                  ></span>
                </li>
              );
            })}
      </ol>
    </div>
  );
};

interface BreadCrumbBlockProps {
  subNodes: string[];
  hrefs?: string[];
  actions?: ((() => void) | null)[];
}

export const BreadCrumbsBlock = ({ subNodes, hrefs, actions }: BreadCrumbBlockProps) => {
  return hrefs ? (
    <BreadCrumbs subNodes={subNodes} hrefs={hrefs!} actions={actions} />
  ) : (
    <BreadCrumbButtons subNodes={subNodes} actions={actions!} />
  );
};
