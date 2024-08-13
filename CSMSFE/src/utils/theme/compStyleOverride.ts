import { ThemeOptions } from '@mui/material/styles';
import { alpha } from '@mui/material/styles';
import getPalette from './palette';

// Extend MUI types to include custom variants
declare module '@mui/material/Paper' {
  interface PaperPropsVariantOverrides {
    outlinedElevation: true;
    cardHover: true;
    'model-folder': true;
    'model-model': true;
    'action-popper': true;
    'user-in-project-card': true;
  }
}

declare module '@mui/material/Button' {
  interface ButtonPropsVariantOverrides {
    'action-popper': true;
  }
}

const getComponentStyleOverride = (mode: 'dark' | 'light'): ThemeOptions['components'] => ({
  MuiButton: {
    styleOverrides: {
      root: {
        '&.MuiButton-containedPrimary:not(:disabled)': {
          // ?.primary[400]
          backgroundColor: getPalette(mode).primary,
          '&:hover': {
            backgroundColor: getPalette(mode)?.primary?.main,
          },
        },
      },
    },

    variants: [
      {
        props: {
          variant: 'action-popper',
        },
        style: () => ({
          border: `1px solid #b3b3b3`,
          justifyContent: 'start'
        }),
      },
    ],
  },
  MuiPaper: {
    styleOverrides: {
      elevation: {
        backgroundImage: 'none',
      },
    },
    defaultProps: {
      elevation: 24,
      variant: 'outlinedElevation' as 'outlinedElevation',
    },
    variants: [
      {
        props: {
          variant: 'outlinedElevation',
        },
        style: () => ({
          boxShadow: "0px 15px 10px -15px #0003",
          border: `1px solid ${getPalette(mode)?.border}`,
        }),
      },
    ],
  },
  MuiCard: {
    variants: [
      {
        props: {
        },
        style: {
          padding: 0,
        },
      },
      {
        props: {
        },
        style: {
          padding: 25,
          '@media (max-width: 600px)': {
            padding: '5%',
          },
        },
      },
      {
        props: {
        },
        style: {
          paddingTop: 40,
          paddingBottom: 40,
          paddingLeft: 30,
          paddingRight: 30,
          '@media (max-width: 600px)': {
            padding: '5%',
          },
        },
      },
      {
        props: {
        },
        style: {
          '&:hover': {
            boxShadow: '0px 10px 20px -10px #0005',
          },
        },
      },
      {
        props: {
        },
        style: {
          '&:hover': {
            boxShadow: `0px 10px 20px -15px ${getPalette(mode)?.primary?.main}`,
          },
        },
      },
      {
        props: {
          variant: "cardHover"
        },
        style: {
          padding: 10,
          boxShadow: "0px 0px 10px 0px #00000030",
          '@media (max-width: 600px)': {
            padding: '5%',
          },
          '&:hover': {
            boxShadow: `0px 10px 20px -15px ${getPalette(mode)?.primary?.main}`,
            // borderColor: getPalette(mode)?.primary?.main,
            border: `solid 1px ${getPalette(mode)?.primary?.main}`,
            transform: 'scale(1.02)',
            transition: 'transform 0.2s ease-in-out', 
          },
        },
      },
      {
        props: {
          variant: "model-folder"
        },
        style: {
          padding: 10,
          paddingBottom: 0,
          paddingRight: 0,
          borderLeft: `solid 3px #6d6d6d`,
          borderRadius: '8px !important',
          boxShadow: "0px 0px 10px 0px #00000030",
          '@media (max-width: 600px)': {
            padding: '5%',
          },
          '&:hover': {
            borderLeft: `solid 3px ${getPalette(mode)?.primary?.main}`,
          },
        },
      },
      {
        props: {
          variant: "model-model"
        },
        style: {
          padding: 10,
          paddingBottom: 0,
          borderRadius: '8px !important',
          // borderLeft: `solid 3px #6d6d6d`,
          // boxShadow: "0px 0px 10px 0px #00000030",
          // '@media (max-width: 600px)': {
          //   padding: '5%',
          // },
          '&:hover': {
            borderLeft: `solid 3px ${getPalette(mode)?.primary?.main}`,
          },
        },
      },
      {
        props: {
          variant: "action-popper"
        },
        style: {
          padding: 10,
          border: `solid 1px #b3b3b3`,
          borderRadius: '8px'
          // boxShadow: "0px 0px 10px 0px #00000030",
          // '@media (max-width: 600px)': {
          //   padding: '5%',
          // },
        },
      },
      {
        props: {
          variant: "user-in-project-card"
        },
        style: {
          padding: 10,
          marginBottom: 15,
          borderRadius: '8px !important',
          boxShadow: "0px 0px 10px 0px #00000030",
          '@media (max-width: 600px)': {
            padding: '5%',
          },
          '&:hover': {
            boxShadow: "0px 0px 10px 0px #00000030",          },
        },
      },
    ],
    defaultProps: {
      variant: 'outlinedElevation',
    },
  },
  MuiPopover: {
    defaultProps: {
      elevation: 24,
    },
  },
  MuiMenuItem: {
    styleOverrides: {
      root: {
        color: getPalette(mode)?.text?.secondary,
        borderRadius: '3px',
        '&.Mui-selected': {
          color: getPalette(mode)?.primary?.contrastText,
          backgroundColor: getPalette(mode)?.primary?.[mode === 'light' ? 300 : 400],
          '&>.MuiListItemIcon-root': {
            color: getPalette(mode)?.primary?.contrastText,
          },
          '&:hover': {
            backgroundColor: getPalette(mode)?.primary?.[mode === 'light' ? 400 : 300],
            color: getPalette(mode)?.primary?.contrastText,
            '&>.MuiListItemIcon-root': {
              color: getPalette(mode)?.primary?.contrastText,
            },
          },
        },
        '&:hover': {
          backgroundColor: alpha(getPalette(mode)?.primary?.light || "", 0.2),
          ...(mode === 'light' && {
            color: getPalette(mode)?.primary?.[400],
            '&>.MuiListItemIcon-root': {
              color: getPalette(mode)?.primary?.main,
            },
          }),
          ...(mode === 'dark' && {
            color: getPalette(mode)?.primary?.contrastText,
          }),
        },
      },
    },
  },
  MuiOutlinedInput: {
    styleOverrides: {
      root: {
        ...(mode === 'light' && {
          '&:hover': {
            backgroundColor: '#eee8',
          },
        }),
        ...(mode === 'dark' && {
          '&:hover': {
            backgroundColor: '#eee1',
          },
        }),
        '&:not(.Mui-error).Mui-focused .MuiOutlinedInput-notchedOutline': {
          borderColor: getPalette(mode)?.primary?.[400] || '#000',
        },
        '&:not(.Mui-error):hover .MuiOutlinedInput-notchedOutline': {
          borderColor: getPalette(mode)?.primary?.[400] || '#000',
        },
      },
    },
  },
  MuiInputBase: {
    styleOverrides: {
      root: {
        '&.Mui-disabled, &.Mui-readOnly': {
          pointerEvents: 'none',
        },
      },
    },
  },
  MuiTableHead: {
    styleOverrides: {
      root: {
        backgroundColor: getPalette(mode)?.background?.default,
        '& .MuiTableCell-head': {...{
				fontSize: '0.85rem',
				color: getPalette(mode)?.text?.primary,
				fontWeight: 500,
			},
		  
			textTransform: 'uppercase',
			borderTop: `1px solid ${getPalette(mode)?.border}`,
			borderBottom: `1px solid ${getPalette(mode)?.border}`,
        },
      },
    },
  },
  MuiTableRow: {
    styleOverrides: {
      root: {
        '&.MuiTableRow-hover:hover': {
          backgroundColor: alpha(getPalette(mode)?.background?.default || "", 0.4),
        },
      },
    },
  },
  MuiRadio: {
    styleOverrides: {
      root: {
        '& .MuiSvgIcon-root': {
          fontSize: 35,
        },
      },
    },
  },
  MuiLink: {
    styleOverrides: {
      root: {
        color: getPalette(mode)?.primary?.[300],
      },
    },
  },
  MuiAlert: {
    styleOverrides: {
      outlined: {
        backgroundColor: getPalette(mode)?.background?.paper,
      },
      filled: {
        border: 0,
      },
      standard: {
        border: 0,
      },
      filledSuccess: {
        color: getPalette(mode)?.success?.contrastText,
      },
    },
  },
  MuiMobileStepper: {
    styleOverrides: {
      root: {
        background: getPalette(mode)?.background?.paper,
      },
    },
  },
});

export default getComponentStyleOverride;