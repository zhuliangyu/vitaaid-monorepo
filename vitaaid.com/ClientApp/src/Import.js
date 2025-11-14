import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { isMobileData } from 'redux/features/isMobileSlice';

export default function Import({ importCallback, children }) {
  const [Component, setComponent] = useState(null);
  useEffect(() => {
    // Dynamic import call based on the 'isMobile' flag
    // 'desktop' prop might be empty fx.
    if (importCallback) {
      // Executes the `import()` call that returns a promise with
      // component details passed as an argument
      importCallback().then((componentDetails) => {
        // Store imported component data in the local state
        setComponent(componentDetails);
      });
    }
  }, []);

  // The actual component is assigned to the 'default' prop
  return children(Component ? Component.default : () => null);
}

export function ReactiveImport({ mobile, desktop, children }) {
  const [Component, setComponent] = useState(null);
  // non redux version
  const [isMobile, setIsMobile] = useState({
    matches: window.innerWidth <= 767 ? true : false,
  });
  useEffect(() => {
    let mediaQuery = window.matchMedia('(max-width: 767px)');
    mediaQuery.addListener(setIsMobile);
    // this is the cleanup function to remove the listener
    return () => mediaQuery.removeListener(setIsMobile);
  }, []);

  useEffect(() => {
    // Dynamic import call based on the 'isMobile' flag
    // non redux version
    const importCallback = isMobile ? mobile : desktop;

    // 'desktop' prop might be empty fx.
    if (importCallback) {
      // Executes the `import()` call that returns a promise with
      // component details passed as an argument
      importCallback().then((componentDetails) => {
        // Store imported component data in the local state
        setComponent(componentDetails);
      });
    }
  }, [isMobile]);

  // The actual component is assigned to the 'default' prop
  return children(Component ? Component.default : () => null);
}
export function ReactiveImportReduxVersion({ mobile, desktop, children }) {
  const isMobile = useSelector(isMobileData);
  const [Component, setComponent] = useState(null);
  useEffect(() => {
    // Dynamic import call based on the 'isMobile' flag
    const importCallback = isMobile ? mobile : desktop;
    // 'desktop' prop might be empty fx.
    if (importCallback) {
      // Executes the `import()` call that returns a promise with
      // component details passed as an argument
      importCallback().then((componentDetails) => {
        // Store imported component data in the local state
        setComponent(componentDetails);
      });
    }
  }, [isMobile]);

  // The actual component is assigned to the 'default' prop
  return children(Component ? Component.default : () => null);
}
