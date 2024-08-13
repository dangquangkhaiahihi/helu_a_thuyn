import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Typography } from '@mui/material';
import { enqueueSnackbar } from 'notistack';
import { useDispatch } from 'react-redux';
import { Model } from '@/common/DTO/Model/ModelDTO';
import ModelService from '@/api/instance/model';

interface IDeleteModelModalProps {
    isOpen: boolean;
    selectedItem: Model | null;
    onClose: () => void;
    onSuccess: () => void;
}

const DeleteModelModal: React.FC<IDeleteModelModalProps> = ({
    isOpen,
    selectedItem,
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

    const handleRemoveModel = async () => {
        setLoadingCircularOverlay(true);
        try {
            await ModelService.Delete(selectedItem?.id || "");
            onSuccess();
            enqueueSnackbar('Xóa mô hình thành công.', {
                variant: 'success'
            });
            onCloseModal();
        } catch ( err ) {
            console.error("handleRemoveModel error", err);
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
            onSave={handleRemoveModel}
            titleClose='Hủy bỏ'
            titleSave='Xóa'
        >
            {
                selectedItem?.type === "MODEL" ? (
                    <Typography>Bạn có chắc muốn xóa mô hình <b>{selectedItem?.name}</b> không?</Typography>
                ) : (
                    <Typography>Bạn có chắc muốn xóa thư mục <b>{selectedItem?.name}</b> không? Tất cả các thư mục con và mô hình trong đó cũng sẽ bị xóa.</Typography>
                )
            }
        </CustomModal>
	);
}

export default DeleteModelModal;
