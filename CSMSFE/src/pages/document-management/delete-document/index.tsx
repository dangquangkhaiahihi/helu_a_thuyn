import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Typography } from '@mui/material';
import { enqueueSnackbar } from 'notistack';
import { useDispatch } from 'react-redux';
import DocumentService from '@/api/instance/document';
import { DocumentDTO } from '@/common/DTO/Document/DocumentDTO';

interface IDeleteDocumentModalProps {
    isOpen: boolean;
    selectedItem: DocumentDTO | null;
    deleteIds: number[];
    onClose: () => void;
    onSuccess: (deletedIds: number[]) => void;
}

const DeleteDocumentModal: React.FC<IDeleteDocumentModalProps> = ({
    isOpen,
    selectedItem,
    deleteIds,
    onClose,
    onSuccess,
}) => {
    const onCloseModal = () => {
        onClose();
    }

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const handleRemoveDocument = async () => {
        setLoadingCircularOverlay(true);
        try {
            await DocumentService.Delete(deleteIds);
            onSuccess(deleteIds);
            enqueueSnackbar('Xóa tài liệu thành công.', {
                variant: 'success'
            });
            onCloseModal();
        } catch ( err ) {
            console.error("handleRemoveDocument error", err);
        } finally {
            setLoadingCircularOverlay(false);
        }
    };

	return (
        <CustomModal
            isOpen={isOpen}
            onClose={onCloseModal}
            size='lg'
            title={"Xác nhận"}
            onSave={handleRemoveDocument}
            titleClose='Hủy bỏ'
            titleSave='Xóa'
        >
            {
                typeof selectedItem?.isFile === "boolean" && selectedItem?.isFile ? (
                    <Typography>Bạn có chắc muốn xóa tài liệu <b>{selectedItem?.name}</b> không?</Typography>
                ) : (
                    <Typography>Bạn có chắc muốn xóa thư mục <b>{selectedItem?.name}</b> không? Tất cả các thư mục con và tài liệu trong đó cũng sẽ bị xóa.</Typography>
                )
            }
        </CustomModal>
	);
}

export default DeleteDocumentModal;
