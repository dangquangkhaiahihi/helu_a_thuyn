import { selectThemeConfig } from '@/store/theme/selectors';
import { useSelector } from '@/store';
import getPalette from './palette';
import { TypographyOptions } from '@mui/material/styles/createTypography';

// Define the expected config interface
interface ThemeConfig {
  fontFamily: string;
  mode: 'light' | 'dark'; // Adjust based on your actual mode type
}

// Define the typography return type
const getTypography = (): TypographyOptions => {
  const { fontFamily, mode } = useSelector(selectThemeConfig) as ThemeConfig;

  return {
    fontFamily: `${fontFamily}, "Helvetica", "-apple-system", "Arial", sans-serif`,
    fontSize: 14,
    button: {
      textTransform: 'none',
    },
    h6: {
      fontWeight: 500,
      color: getPalette(mode)?.text?.primary,
      fontSize: '0.75rem',
    },
    h5: {
      fontSize: '0.85rem',
      color: getPalette(mode)?.text?.primary,
      fontWeight: 500,
    },
    h4: {
      fontSize: '1rem',
      color: getPalette(mode)?.text?.primary,
      fontWeight: 600,
    },
    h3: {
      fontSize: '1.25rem',
      color: getPalette(mode)?.text?.primary,
      fontWeight: 600,
    },
    h2: {
      fontSize: '1.5rem',
      color: getPalette(mode)?.text?.primary,
      fontWeight: 700,
    },
    h1: {
      fontSize: '2.125rem',
      color: getPalette(mode)?.text?.primary,
      fontWeight: 700,
    },
    subtitle1: {
      fontSize: '0.875rem',
      fontWeight: 500,
      color: getPalette(mode)?.text?.primary,
    },
    subtitle2: {
      fontSize: '0.75rem',
      fontWeight: 400,
      color: getPalette(mode)?.text?.secondary,
    },
    caption: {
      fontSize: '0.7rem',
      color: getPalette(mode)?.text?.secondary,
      fontWeight: 400,
    },
    body1: {
      fontSize: '0.775rem',
      fontWeight: 400,
      lineHeight: '1.334em',
    },
    body2: {
      fontSize: '0.875rem',
      letterSpacing: '0em',
      fontWeight: 400,
      lineHeight: '1.5em',
      color: getPalette(mode)?.text?.primary,
    },
  };
};

export default getTypography;
