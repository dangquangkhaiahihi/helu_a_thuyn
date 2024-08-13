import React from 'react';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
// MUI
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Dialog from '@mui/material/Dialog';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';
// Icons
import CloseIcon from '@mui/icons-material/Close';

export type MAX_WIDTH_KEYS = 'xs' | 'sm' | 'md' | 'lg' | 'xl' | 'fullScreen';
export type MODAL_TYPE_KEYS = 'contained' | 'underline' | 'clean';
export type TRANSITION_KEYS = 'slide' | 'collapse' | 'fade' | 'zoom';

interface ModalProps {
    openModal: boolean;
    fnCloseModal: () => void;
    title?: string;
    maxWidth?: MAX_WIDTH_KEYS;
    type?: 'contained' | 'underline' | 'clean';
    padding?: boolean;
    children: React.ReactNode;
	TransitionComponent?: React.ComponentType<any>;
	disableBackdropClick?: boolean;
}

function Modal(props: ModalProps) {
    const theme = useTheme();
    const fullScreen = useMediaQuery(theme.breakpoints.down('sm'));
    const {
        openModal,
        fnCloseModal,
        title,
        maxWidth = 'sm',
        type = 'contained',
        padding = false,
        children,
		disableBackdropClick,
        ...rest
    } = props;

    return (
        <Dialog
            fullScreen={maxWidth === 'fullScreen' ? true : fullScreen}
            maxWidth={maxWidth === 'fullScreen' ? false : maxWidth}
            open={openModal}
            scroll="paper"
            onClose={(_event, reason) => {
				if ( disableBackdropClick && reason === 'backdropClick' ) return;
				fnCloseModal();
			}}
            PaperProps={{
                variant: 'elevation',
                sx: {
                    backgroundImage: 'none',
                    // bgcolor: theme.palette.mode === 'dark' ? '#000' : 'background.paper',
                    width: '100%',
                },
            }}
            {...rest}
        >
            <Box position="relative">
                {type === 'contained' && <ContainedBox title={title} fnCloseModal={fnCloseModal} />}
                {type === 'underline' && <UnderlineBox title={title} fnCloseModal={fnCloseModal} />}
                <Box p={padding ? '5%' : ''}>{children}</Box>
            </Box>
        </Dialog>
    );
}

interface UnderlineBoxProps {
    title?: string;
    fnCloseModal: () => void;
}

function UnderlineBox({ title, fnCloseModal }: UnderlineBoxProps) {
    return (
        <Box top={0} zIndex={9999} position="sticky">
            <Stack direction="row" alignItems="center" height={50} bgcolor="background.paper" px={1}>
                <Box width="33%" height="80%">
                    <Box component="img" height="100%" py={1} ml={1} src={'/assets/images/logo/png/Color_logotext2_nobg.png'} alt="logo" />
                </Box>
                <Box width="34%">
                    <ModalText text={title} />
                </Box>
                <Box width="33%">
                    <CloseButton fnCloseModal={fnCloseModal} />
                </Box>
            </Stack>
            <Divider variant="middle" sx={{ border: 1, borderColor: 'primary.main' }} />
        </Box>
    );
}

interface ContainedBoxProps {
    title?: string;
    fnCloseModal: () => void;
}

function ContainedBox({ title, fnCloseModal }: ContainedBoxProps) {
    return (
        <Box top="0" position="sticky" zIndex={9999}>
            <Stack direction="row" alignItems="center" height={50} bgcolor="primary.main">
                <Box width="33%" height="100%">
                    <Box component="img" height="100%" py={1} ml={1} src={'/assets/images/logo/png/White_logotext_nobg.png'} alt="logo" />
                </Box>
                <Box width="34%">
                    <ModalText text={title} color="primary.contrastText" />
                </Box>
                <Box width="33%">
                    <CloseButton fnCloseModal={fnCloseModal} color={(theme) => theme?.palette?.primary?.contrastText} />
                </Box>
            </Stack>
        </Box>
    );
}

interface CloseButtonProps {
    fnCloseModal: () => void;
    color?: string | ((theme: any) => string);
}

function CloseButton({ fnCloseModal, color }: CloseButtonProps) {
    return (
        <IconButton aria-label="close" onClick={fnCloseModal} size="large" sx={{ float: 'right' }}>
            <CloseIcon sx={{ color }} />
        </IconButton>
    );
}

interface ModalTextProps {
    text?: string;
    color?: string;
}

function ModalText({ text, color }: ModalTextProps) {
    return (
        <Typography variant="h4" fontWeight="400" color={color} align="center">
            {text}
        </Typography>
    );
}

export default Modal;
