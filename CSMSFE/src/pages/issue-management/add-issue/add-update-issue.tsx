
import IssueService from '@/api/instance/issue';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import { IssueCreateUpdateDTO } from '@/common/DTO/Issue/IssueCreateUpdateDTO';
import { Issue } from '@/common/DTO/Issue/IssueDTO';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Grid } from '@mui/material';
import dayjs from 'dayjs';
import { enqueueSnackbar } from 'notistack';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

interface IAddIssueModalProps {
    isOpen: boolean;
    selectedItem: Issue | null;
    onClose: () => void;
    onSuccess: () => void;
    optionsType: Array<{ value: string; label: string }>;
    // fetchOptionType_Async: (query: string) => Promise<{ value: string | number; label: string }[]>;
}

const FORM_SCHEMA = {
    title: {
        required: 'Đây là trường bắt buộc'
    },
    type: {
        required: 'Đây là trường bắt buộc',
    },
    status: {
        required: 'Đây là trường bắt buộc',

    },
    // decription: {
    //     required: 'Đây là trường bắt buộc',

    // },
  
};

const AddIssueModal: React.FC<IAddIssueModalProps> = ({
    
    isOpen,
    selectedItem,
    onClose,
    onSuccess,
    optionsType,
    //   fetchOptionType_Async
}) => {
    const { modelId } = useParams();
    const onCloseModal = () => {
        onClose();
        reset();
    }

    useEffect(() => {
        if (!isOpen) return;
        selectedItem?.id && setValue("id", selectedItem?.id?.toString());
        selectedItem?.name && setValue("name", selectedItem?.name);
        selectedItem?.type && setValue("type", selectedItem?.type);
        selectedItem?.status && setValue("status", selectedItem?.status);
        selectedItem?.description && setValue("description", selectedItem?.description);
        // selectedItem?.createBy && setValue("reporter", selectedItem?.createBy);
        // selectedItem?.createDate && setValue("endDate", selectedItem?.createDate);
        // selectedItem?.modifiedBy && setValue("assignee", selectedItem?.modifiedBy);
        // selectedItem?.modifiedDate && setValue("image", selectedItem?.modifiedDate);
    }, [isOpen])

    const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm({
        mode: 'onChange',
        defaultValues: {
            id: "",
            name: "",
            type: "",
            status: "",
            description: "",
            modelId:modelId??'',
           
        },
    });

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    // const validateInput = (data: IssueCreateUpdateDTO): string => {
    //     const currentDate = new Date();
    //     if ( dayjs(data.createDate).isBefore(currentDate.setHours(0,0,0,0)) ) {
    //         return "End Date không được nhỏ hơn ngày hôm nay.";
    //     }

    //     return "";
    // };

    const onSubmit = async (data: any) => {
        const dto: IssueCreateUpdateDTO = {
            id:data?.id,
            name: data?.name,
            type: data?.type,
            status: data?.status,
            description: data?.description,
            modelId :data?.modelId,
            // createBy: data?.reporter,
            // createDate: dayjs(data?.endDate).format(),
            // modifiedBy: data?.assignee,
            // modifiedDate: data?.modifiedDate,
        };
       
        // const message = validateInput(dto);
        // if ( message ) {
        //     enqueueSnackbar(message, {
        //         variant: 'warning'
        //     });
        //     return;
        // }

        // if ( dto.createDate ) dto.createDate = dayjs(dto.createDate).format();

        try {
            await handleAddUpdateIssue(dto);
            reset();
            onClose();
            onSuccess();
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }
        

        const handleAddUpdateIssue = async (data: IssueCreateUpdateDTO) => {
            setLoadingCircularOverlay(true);
            try {
                
                let res: ApiResponse<any>;
                if ( !data.id ) {
                    res = await IssueService.Create(data);
                    enqueueSnackbar('Thêm mới thành công.', {
                        variant: 'success'
                    });
                } else {
                    res = await IssueService.Update(data);
                    enqueueSnackbar('Cập nhật thành công.', {
                        variant: 'success'
                    });
                }
                console.log("handleAddUpdateIssue res", res);

            } catch ( err ) {
                console.log("handleAddUpdateIssue err", err);
                throw err;
            } finally {
                setLoadingCircularOverlay(false);
            }
        }

        return (
            <CustomModal
                title={"Thêm mới vấn đề"}
                isOpen={isOpen}
                onSave={() => { handleSubmit(onSubmit)(); }}
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
                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="text"
                                name="name"
                                control={control}
                                rules={FORM_SCHEMA.title}
                                errors={errors}
                                placeholder="Tiêu đề"
                                label="Tiêu đề"
                                fullWidth
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="text"
                                name="type"
                                control={control}
                                rules={FORM_SCHEMA.type}
                                errors={errors}
                                placeholder="Kiểu"
                                fullWidth
                                label="Kiểu"
                            />
                        </Grid>
                        <Grid item xs={12} md={6}  >
                            <FormInput
                                type="select"
                                name="status"
                                control={control}
                                rules={FORM_SCHEMA.status}
                                errors={errors}
                                fullWidth
                                label="Trạng thái"
                                options={[
                                    { value: "Đang xử lý", label: "Đang xử lý" },
                                    { value: "Chờ xử lý", label: "Chờ xử lý" },
                                    { value: "Đã xử lý", label: "Đã xử lý" },
                                ]}
                            />
                        </Grid>
                    
                        <Grid item xs={12} md={6} style={{paddingTop:'20.5px'}}>
                            <FormInput
                                type="text"
                                name="description"
                                control={control}
                                // rules={FORM_SCHEMA.decription}
                                errors={errors}
                                placeholder="Mô tả"
                                fullWidth
                                label="Mô tả"
                            />
                        </Grid>


{/* 
                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="text"
                                name="reporter"
                                control={control}
                                // rules={FORM_SCHEMA.reporter}
                                errors={errors}
                                placeholder="Người báo cáo"
                                fullWidth
                                label="Người báo cáo"
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="date"
                                name="endDate"
                                control={control}
                                // rules={FORM_SCHEMA.endDate}
                                errors={errors}
                                placeholder="Hạn"
                                fullWidth
                                label="Hạn"
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="text"
                                name="assignee"
                                control={control}
                                // rules={FORM_SCHEMA.asignee}
                                errors={errors}
                                placeholder="Giao cho"
                                fullWidth
                                label="Giao cho"
                            />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <FormInput
                                type="text"
                                name="image"
                                control={control}
                                // rules={FORM_SCHEMA.image}
                                errors={errors}
                                placeholder="Ảnh"
                                fullWidth
                                label="Ảnh"
                            />
                        </Grid> */}
                    </Grid>
                </form>
            </CustomModal>
        );
    };

    export default AddIssueModal;
