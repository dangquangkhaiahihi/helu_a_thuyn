import { useParams } from 'react-router-dom';
import ModelService from '@/api/instance/model';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import { ModelCreateDTO } from '@/common/DTO/Model/ModelCreateDTO';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Grid } from '@mui/material';
import { enqueueSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { Model } from '@/common/DTO/Model/ModelDTO';
import { useEffect } from 'react';

interface IAddModelModalProps {
    isOpen: boolean;
    selectedItem: Model | null;
    onClose: () => void;
    onSuccess: (newNode: Model) => void;
}

const FORM_SCHEMA = {
    name: {
        required: 'Đây là trường bắt buộc'
    },
    description: {},
};

const AddModelModal: React.FC<IAddModelModalProps> = ({
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
        selectedItem?.id && setValue("parentId", selectedItem?.id);
    }, [isOpen])

    const { projectId } = useParams();

    const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm({
		mode: 'onChange',
		defaultValues: {
			name: "",
            description: "",
            projectID: projectId || "",
            parentId: selectedItem?.id || ""
		},
	});
    
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const onSubmit = async (data: ModelCreateDTO) => {
        try {
            const res = await handleAdd(data);
            reset();
            onClose();
            onSuccess( res.content );
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }

    const handleAdd = async (data: ModelCreateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<Model> = await ModelService.Create(data);
            enqueueSnackbar('Thêm mới thành công.', {
                variant: 'success'
            });
            return res;
            
        } catch ( err ) {
            console.log("handleAdd err", err);
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

export default AddModelModal;
