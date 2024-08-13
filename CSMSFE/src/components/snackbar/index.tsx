import { forwardRef, ReactNode, useState } from 'react';
import { SnackbarProvider, closeSnackbar } from 'notistack';

import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
import { IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import LoadingIcon from './loading-icon/loadingIcon';

declare module "notistack" {
	interface VariantOverrides {
		muiSnackbar: true;
    loading: true;
	}
}

  
interface MuiSnackbarProps {
  message: ReactNode;
  severity: 'error' | 'warning' | 'info' | 'success' | 'loading';
  title: string;
  sx?: any;
  alertProps?: any;
}

const MuiSnackbar = (props: any) => {
    const { id, message, severity, title, sx, alertProps } = props;

    const [customStyle] = useState(props.alertProps.severity === 'loading' ? {
      backgroundColor: "#FEC604"
    } : {});

    return (
        <Alert
            severity={severity}
            iconMapping={{
              loading: <LoadingIcon/>,
            }}
            sx={{ boxShadow: 27, alignContent: 'center', ...sx, ...customStyle }}
            {...alertProps}
            action={
                <IconButton
                    aria-label="close"
                    color="inherit"
                    size="small"
                    onClick={() => closeSnackbar(id)}
                >
                    <CloseIcon fontSize="inherit" />
                </IconButton>
            }
        >
            <AlertTitle>{title}</AlertTitle>
            {message}
        </Alert>
    );
};

const MuiSnackbarVariant = forwardRef<HTMLDivElement, MuiSnackbarProps>((props, ref) => (
  <div ref={ref}>
    <MuiSnackbar {...props} />
  </div>
));

export function Provider({ children }: { children: ReactNode }) {
  return (
    <SnackbarProvider
		maxSnack={3}
		anchorOrigin={{ horizontal: 'right', vertical: 'top' }}
		Components={{
			muiSnackbar: MuiSnackbarVariant,
		}}
    >
      {children}
    </SnackbarProvider>
  );
}