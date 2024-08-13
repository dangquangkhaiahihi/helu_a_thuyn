import SomeTableService from '@/api/instance/some-table';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import { SomeTableCreateUpdateDTO } from '@/common/DTO/SomeTable/SomeTableCreateUpdateDTO';
import { SomeTable } from '@/common/DTO/SomeTable/SomeTableDTO';
import { isValidEmail, isValidPhoneNumber } from '@/common/tools';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Grid } from '@mui/material';
import dayjs from 'dayjs';
import { enqueueSnackbar } from 'notistack';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';

interface IAddUpdateSomeTableModalProps {
    isOpen: boolean;
    selectedItem: SomeTable | null;
    onClose: () => void;
    onSuccess: () => void;
    //
    optionsType: Array<{ value: string; label: string }>;
    fetchOptionType_Async: (query: string) => Promise<{ value: string | number; label: string }[]>;
}

const FORM_SCHEMA = {
    normalText: {
        required: 'Đây là trường bắt buộc'
    },
    phoneNumber: {
        required: 'Đây là trường bắt buộc',
        validate: (value: string) => {
            return isValidPhoneNumber(value) || 'Số điện thoại sai định dạng'
        },
    },
    email: {
        required: 'Đây là trường bắt buộc',
        validate: (value: string) => {
            return isValidEmail(value) || 'Email sai định dạng'
        },
    },
    startDate: {
        required: 'Đây là trường bắt buộc',
        validate: (value: any) => {
            return value && dayjs(value).isValid() || 'Ngày được chọn không hợp lệ'
        },
    },
    endDate: {
        required: 'Đây là trường bắt buộc',
        validate: (value: any) => {
            return value && dayjs(value).isValid() || 'Ngày được chọn không hợp lệ'
        },
    },
    status: {
        required: 'Đây là trường bắt buộc'
    },
    type: {
        required: 'Đây là trường bắt buộc'
    },
    // 3 trường dưới đây chỉ để demo, trong api ko định nghĩa, ko filter theo 3 thằng này
    TypeList: {},
    TypeAsync: {},
    TypeListAsync: {},
};

const AddUpdateSomeTableModal: React.FC<IAddUpdateSomeTableModalProps> = ({
    isOpen,
    selectedItem,
    onClose,
    onSuccess,
    //
    optionsType,
    fetchOptionType_Async
}) => {
    const onCloseModal = () => {
        onClose();
        reset();
    }

    useEffect(() => {
        if ( !isOpen ) return;
        selectedItem?.id && setValue("id", selectedItem?.id?.toString());
        selectedItem?.normalText && setValue("normalText", selectedItem?.normalText);
        selectedItem?.phoneNumber && setValue("phoneNumber", selectedItem?.phoneNumber);
        selectedItem?.email && setValue("email", selectedItem?.email);
        selectedItem?.startDate && setValue("startDate", selectedItem?.startDate);
        selectedItem?.endDate && setValue("endDate", selectedItem?.endDate);
        selectedItem?.status && setValue("status", typeof selectedItem?.status === "boolean" && selectedItem?.status ? "true" : "false");
        selectedItem?.type && setValue("type", selectedItem?.type);
    }, [isOpen])

    const { control, handleSubmit, formState: { errors }, reset, setValue} = useForm({
		mode: 'onChange',
		defaultValues: {
            id: "",
			normalText: "",
            phoneNumber: "",
            email: "",
            startDate: "",
            endDate: "",
            status: "",
            type: "",
            TypeList: [],
            TypeAsync: "",
            TypeListAsync: []
		},
	});

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const validateInput = (data: SomeTableCreateUpdateDTO): string => {
        if ( dayjs(data.startDate).isAfter(dayjs(data.endDate)) ) {
            return "Start Date không được lớn hơn End Date.";
        }
    
        return "";
    };

    const onSubmit = async (data: any) => {
        const dto: SomeTableCreateUpdateDTO = {
            id: data?.id,
            email: data?.email,
            normalText: data?.normalText,
            phoneNumber: data?.phoneNumber,
            startDate: dayjs(data?.startDate).format(),
            endDate: dayjs(data?.endDate).format(),
            status: data?.status === 'true',
            type: data?.type,
        };

        const message = validateInput(dto);
        if ( message ) {
            enqueueSnackbar(message, {
                variant: 'warning'
            });
            return;
        }

        if ( dto.startDate ) dto.startDate = dayjs(dto.startDate).format();
        if ( dto.endDate ) dto.endDate = dayjs(dto.endDate).format();

        try {
            await handleAddUpdateSomeTable(dto);
            reset();
            onClose();
            onSuccess();
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }

    const handleAddUpdateSomeTable = async (data: SomeTableCreateUpdateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<any>;
            if ( !data.id ) {
                res = await SomeTableService.Create(data);
                enqueueSnackbar('Thêm mới thành công.', {
                    variant: 'success'
                });
            } else {
                res = await SomeTableService.Update(data);
                enqueueSnackbar('Cập nhật thành công.', {
                    variant: 'success'
                });
            }
            console.log("handleAddUpdateSomeTable res", res);
            
        } catch ( err ) {
            console.log("handleAddUpdateSomeTable err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }

	return (
        <CustomModal
            title={"Thêm mới Some Table"}
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
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="normalText"
                            control={control}
                            rules={FORM_SCHEMA.normalText}
                            errors={errors}
                            placeholder="Normal Text"
                            label="Normal Text"
                            fullWidth
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="phoneNumber"
                            control={control}
                            rules={FORM_SCHEMA.phoneNumber}
                            errors={errors}
                            placeholder="Phone Number"
                            fullWidth
                            label="Phone Number"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="email"
                            control={control}
                            rules={FORM_SCHEMA.email}
                            errors={errors}
                            placeholder="Email"
                            fullWidth
                            label="Email"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="select"
                            name="status"
                            control={control}
                            rules={FORM_SCHEMA.status}
                            errors={errors}
                            placeholder="Status"
                            fullWidth
                            label="Status"
                            options={[
                                {value: "true", label: "True"},
                                {value: "false", label: "False"},
                            ]}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="date"
                            name="startDate"
                            control={control}
                            rules={FORM_SCHEMA.startDate}
                            errors={errors}
                            placeholder="Start Date"
                            fullWidth
                            label="Start Date"
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="date"
                            name="endDate"
                            control={control}
                            rules={FORM_SCHEMA.endDate}
                            errors={errors}
                            placeholder="End Date"
                            fullWidth
                            label="End Date"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="auto-complete"
                            name="type"
                            control={control}
                            rules={FORM_SCHEMA.type}
                            errors={errors}
                            placeholder="Type"
                            fullWidth
                            label="Type"
                            options={optionsType}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="auto-complete-multi-select"
                            name="TypeList"
                            control={control}
                            rules={FORM_SCHEMA.TypeList}
                            errors={errors}
                            placeholder="TypeList"
                            fullWidth
                            label="TypeList"
                            options={optionsType}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="TypeAsync"
                            control={control}
                            rules={FORM_SCHEMA.TypeAsync}
                            errors={errors}
                            placeholder="TypeAsync"
                            fullWidth
                            label="TypeAsync"
                            fetchOptions={fetchOptionType_Async}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete-multi-select"
                            name="TypeListAsync"
                            control={control}
                            rules={FORM_SCHEMA.TypeListAsync}
                            errors={errors}
                            placeholder="TypeListAsync"
                            fullWidth
                            label="TypeListAsync"
                            fetchOptions={fetchOptionType_Async}
                        />
                    </Grid>
                </Grid>
            </form>
        </CustomModal>
	);
}

export default AddUpdateSomeTableModal;
