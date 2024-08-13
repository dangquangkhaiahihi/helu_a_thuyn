import { Grid } from "@mui/material";
import React, { useEffect } from "react";
import FormInput from "@/components/formInput";
import CustomModal from "@/components/modalCustom";
import { setLoadingCircularOverlayRedux } from "@/store/redux/app";
import { enqueueSnackbar } from "notistack";
import { useForm } from "react-hook-form";
import { useDispatch } from "react-redux";
import { DocumentDTO } from "@/common/DTO/Document/DocumentDTO";
import DocumentService from "@/api/instance/document";
import DocumentUpdateDTO from "@/common/DTO/Document/DocumentUpdateDTO";
import { getFileNameWithoutExtension } from "../component/helper";

interface IUpdateDocumentModalProps {
    isOpen: boolean;
    selectedItem: DocumentDTO | null;
    onClose: () => void;
    onSuccess: (newName: string, destinationDocument: DocumentDTO) => void;
}

const FORM_SCHEMA = {
    name: {
        required: 'Đây là trường bắt buộc'
    },
};

const UpdateDocumentModal: React.FC<IUpdateDocumentModalProps> = ({
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
            id: 0,
        }
	});

    useEffect(() => {
        if ( !isOpen || !selectedItem ) return;
        selectedItem?.name && setValue("name", getFileNameWithoutExtension(selectedItem?.name) || selectedItem?.name);
        selectedItem?.id && setValue("id", selectedItem?.id);
    }, [isOpen])

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const onSubmit = async (data: any) => {
        if ( !selectedItem ) return;
        const documentFolderCreateDTO: DocumentUpdateDTO = {
            id: data?.id,
            name: data?.name,
        };
    
        try {
            await handleUpdate(documentFolderCreateDTO);
            reset();
            console.log("data?.name + selectedItem.fileExtension || ", data?.name + selectedItem.fileExtension || "");
            
            onSuccess(selectedItem.fileExtension ? data?.name + selectedItem.fileExtension : data?.name, selectedItem);
            onClose();
        } catch (err) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }
    
    const handleUpdate = async (data: DocumentUpdateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            await DocumentService.Update(data);
        } catch (err) {
            console.log("handleUpdate err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }
    
	return (
        <CustomModal
            title={ "Đổi tên" }
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
                            placeholder={ selectedItem ? (selectedItem.isFile ? "Tên thư mục" : "Tên tài liệu") : "" }
                            label={ selectedItem ? (selectedItem.isFile ? "Tên thư mục" : "Tên tài liệu") : "" }
                            fullWidth
                        />
                    </Grid>
                </Grid>
            </form>
        </CustomModal>
	);
}

export default UpdateDocumentModal;