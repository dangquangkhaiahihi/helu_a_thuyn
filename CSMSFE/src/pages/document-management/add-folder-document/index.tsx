import { Grid } from "@mui/material";
import React, { useEffect } from "react";
import FormInput from "@/components/formInput";
import CustomModal from "@/components/modalCustom";
import { setLoadingCircularOverlayRedux } from "@/store/redux/app";
import { enqueueSnackbar } from "notistack";
import { useForm } from "react-hook-form";
import { useDispatch } from "react-redux";
import { DocumentDTO } from "@/common/DTO/Document/DocumentDTO";
import { DocumentFolderCreateDTO } from "@/common/DTO/Document/DocumentFolderCreateDTO";
import DocumentService from "@/api/instance/document";

interface IAddFolderDocumentModalProps {
    isOpen: boolean;
    selectedItem: DocumentDTO | null;
    onClose: () => void;
    onSuccess: (documentRes: DocumentDTO, destinationDocument: DocumentDTO) => void;
}

const FORM_SCHEMA = {
    name: {
        required: 'Đây là trường bắt buộc'
    },
};

const AddFolderDocumentModal: React.FC<IAddFolderDocumentModalProps> = ({
    isOpen,
    selectedItem,
    onClose,
    onSuccess,
}) => {
    const onCloseModal = () => {
        onClose();
        reset();
    }

    const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm({
		mode: 'onChange',
        defaultValues: {
            name: "",
            parentId: 0,
            projectId: "",
        }
	});

    useEffect(() => {
        if ( !isOpen || !selectedItem ) return;
        selectedItem?.id && setValue("parentId", selectedItem?.id);
        selectedItem?.projectId && setValue("projectId", selectedItem?.projectId);
    }, [isOpen])

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const onSubmit = async (data: any) => {
        if ( !selectedItem ) return;
        const documentFolderCreateDTO: DocumentFolderCreateDTO = {
            projectId: data?.projectId,
            name: data?.name,
            parentId: data?.parentId,
            isFile: false,
        };
    
        try {
            const documentRes: DocumentDTO = await handleAdd(documentFolderCreateDTO);
            reset();
            onSuccess(documentRes, selectedItem);
            onClose();
        } catch (err) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }
    
    const handleAdd = async (data: DocumentFolderCreateDTO): Promise<DocumentDTO> => {
        setLoadingCircularOverlay(true);
        try {
            const res = await DocumentService.CreateFolder(data);
            return res.content;
        } catch (err) {
            console.log("handleAdd err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }
    
	return (
        <CustomModal
            title={ "Thêm thư mục" }
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
                            placeholder={ "Tên thư mục" }
                            label={ "Tên thư mục" }
                            fullWidth
                        />
                    </Grid>
                </Grid>
            </form>
        </CustomModal>
	);
}

export default AddFolderDocumentModal;