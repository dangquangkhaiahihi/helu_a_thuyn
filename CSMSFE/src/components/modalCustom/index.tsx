import { forwardRef, ReactElement, ReactNode } from 'react';
// MUI
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';

import Fade from '@mui/material/Fade';

import Modal, { MAX_WIDTH_KEYS } from '@components/modal';

interface TransitionProps {
    children: ReactElement;
    [key: string]: any;
}

const FadeTransition = forwardRef((props: TransitionProps, ref) => <Fade ref={ref} {...props}>{props.children}</Fade>);

interface ICustomTableProps {
    title: string;
    isOpen: boolean;
    onClose: () => void;
    onSave?: Function;
    children: ReactNode;
    titleClose?: string;
    titleSave?: string;
    size?: MAX_WIDTH_KEYS;
    onlyCloseButton?: boolean;
}

const CustomModal: React.FC<ICustomTableProps> = ({
    title,
    isOpen,
    onClose,
    onSave,
    children,
    titleClose,
    titleSave,
    size = "lg",
    onlyCloseButton
}) => {
    
	return (
        <Modal
            type={'contained'}
            openModal={isOpen}
            maxWidth={size}
            fnCloseModal={onClose}
            title={title}
            TransitionComponent={FadeTransition}
            disableBackdropClick
        >
            <Stack p={3} spacing={3}>
                {children}
                <Divider />
                <Stack direction="row" spacing={3} justifyContent="flex-end">
                    <Button size="small" onClick={onClose} variant='contained' color='error'>
                        {titleClose || 'Đóng'}
                    </Button>
                    {
                        !onlyCloseButton ? (
                            <Button size="small" onClick={() => {onSave && onSave()}} variant='contained' color='primary'>
                                {titleSave || 'Lưu'}
                            </Button>
                        ) : <></>
                    }
                </Stack>
            </Stack>
        </Modal>
	);
}

export default CustomModal;
