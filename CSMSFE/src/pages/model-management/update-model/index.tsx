import { useEffect } from 'react';
import ModelService from '@/api/instance/model';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import { ModelUpdateDTO } from '@/common/DTO/Model/ModelUpdateDTO';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Grid } from '@mui/material';
import { enqueueSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { Model } from '@/common/DTO/Model/ModelDTO';

interface IUpdateModelModalProps {
    isOpen: boolean;
    selectedItem: Model | null;
    onClose: () => void;
    onSuccess: (arg0: string) => void;
}

const FORM_SCHEMA = {
    name: {
        required: 'Đây là trường bắt buộc'
    },
    description: {}
};

const UpdateModelModal: React.FC<IUpdateModelModalProps> = ({
    isOpen,
    selectedItem,
    onClose,
    onSuccess,
}) => {
    const onCloseModal = () => {
        onClose();
        reset();
    }

    useEffect(() => {
        if ( !isOpen ) return;
        selectedItem?.id && setValue("id", selectedItem?.id);
        selectedItem?.name && setValue("name", selectedItem?.name);
        selectedItem?.description && setValue("description", selectedItem?.description);
    }, [isOpen])

    const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm({
		mode: 'onChange',
		defaultValues: {
            id: "",
			name: "",
            description: ""
		},
	});
    
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const onSubmit = async (data: any) => {

        try {
            await handleUpdate(data);
            reset();
            onClose();
            onSuccess( data?.name );
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }

    const handleUpdate = async (data: ModelUpdateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<any> = await ModelService.Update(data);
            enqueueSnackbar('Cập nhật thành công.', {
                variant: 'success'
            });

            console.log("handleUpdate res", res);
            return res
            
        } catch ( err ) {
            console.log("handleUpdate err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }

	return (
        <CustomModal
            title={"Thêm mới Mô hình"}
            isOpen={isOpen}
            onSave={() => {handleSubmit(onSubmit)();}}
            onClose={onCloseModal}
            size='lg'
        >
            <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
                <Grid container
                    direction="row"
                    justifyContent="center"
                    alignItems="center"
                    columnSpacing={2}
                >
                    <Grid item xs={12}>
                        <FormInput
                            type="text"
                            name="name"
                            control={control}
                            rules={FORM_SCHEMA.name}
                            errors={errors}
                            placeholder="Tên mô hình"
                            label="Tên mô hình"
                            fullWidth
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <FormInput
                            type="textarea"
                            name="description"
                            control={control}
                            rules={FORM_SCHEMA.description}
                            errors={errors}
                            placeholder="Mô tả"
                            fullWidth
                            label="Mô tả"
                        />
                    </Grid>
                </Grid>
            </form>
        </CustomModal>
	);
}

export default UpdateModelModal;
