import { useEffect, useState } from 'react';
import ProjectService from '@/api/instance/project';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import { ProjectCreateUpdateDTO } from '@/common/DTO/Project/ProjectCreateUpdateDTO';
import { Project } from '@/common/DTO/Project/ProjectDTO';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Grid } from '@mui/material';
import { enqueueSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import LocationLookUpService from '@/api/instance/location-look-up';

interface IAddUpdateProjectModalProps {
    isOpen: boolean;
    selectedItem: Project | null;
    onClose: () => void;
    onSuccess: () => void;
}

const FORM_SCHEMA = {
    name: {
        required: 'Đây là trường bắt buộc'
    },
    code: {
        required: 'Đây là trường bắt buộc',
    },
    typeProjectId: {
        required: 'Đây là trường bắt buộc',
    },
    description: {},
    communeId: {
        required: 'Đây là trường bắt buộc',
    },
    districtId: {
        required: 'Đây là trường bắt buộc',
    },
    provinceId: {
        required: 'Đây là trường bắt buộc',
    },
};

const AddUpdateProjectModal: React.FC<IAddUpdateProjectModalProps> = ({
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
        selectedItem?.code && setValue("code", selectedItem?.code);
        selectedItem?.description && setValue("description", selectedItem?.description);
        selectedItem?.typeProjectId && setValue("typeProjectId", selectedItem?.typeProjectId);

        selectedItem?.communeId && setValue("communeId", selectedItem?.communeId);
        selectedItem?.districtId && setValue("districtId", selectedItem?.districtId);
        selectedItem?.provinceId && setValue("provinceId", selectedItem?.provinceId);

        selectedItem?.status && setValue("status", selectedItem?.status);
    }, [isOpen])

    const { control, handleSubmit, formState: { errors }, reset, setValue, watch} = useForm({
		mode: 'onChange',
		defaultValues: {
            id: "",
			name: "",
            code: "",
            description: "",
            typeProjectId: "",

            communeId: "",
            districtId: "",
            provinceId: "",

            status: ""
		},
	});

    // START LOGIC SETUP LOOKUP LOCATION
    const provinceId = watch('provinceId');
    const districtId = watch('districtId');
    const [isProvinceInputDisabled, setIsProvinceInputDisabled] = useState<boolean>(false);
    const [isDistrictInputDisabled, setIsDistrictInputDisabled] = useState<boolean>(true);
    const [isCommuneInputDisabled, setIsCommuneInputDisabled] = useState<boolean>(true);

    useEffect(() => {
        // Trigger cái này để clear option của district
        setIsDistrictInputDisabled(true);
        
        // khi provinceId đổi giá trị thì clear luôn giá trị 2 thằng con
        setValue("districtId", "");
        setValue("communeId", "");
        setTimeout(() => {
            setIsDistrictInputDisabled(provinceId ? false : true);
        }, 200)

        if( !provinceId ){
            setIsProvinceInputDisabled(true);
            setTimeout(() => {
                setIsProvinceInputDisabled(false);
            }, 200)
        }
    }, [provinceId])

    useEffect(() => {
        // Trigger cái này để clear option của commune
        setIsCommuneInputDisabled(true);
        
        // khi districtId đổi giá trị thì clear luôn giá trị thằng con
        setValue("communeId", "");
        setTimeout(() => {
            setIsCommuneInputDisabled(districtId ? false : true);
        }, 200)
    }, [districtId])

    const handleGetLookUpProvince = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpProvince(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const handleGetLookUpDistrict = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword,
                provinceId: provinceId
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpDistrict(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const handleGetLookUpCommune = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword,
                districtId: districtId
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpCommune(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }
    // END LOGIC SETUP LOOKUP LOCATION

    const handleGetLookUpTypeProject = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword
            };
            const res: ApiResponse<LookUpItem[]> = await ProjectService.GetLookUpTypeProject(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }
    
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const onSubmit = async (data: any) => {
        const dto: ProjectCreateUpdateDTO = {
            id: data?.id,
            name: data?.name,
            code: data?.code,
            description: data?.description,
            typeProjectId: data?.typeProjectId,

            communeId: data?.communeId,
            districtId: data?.districtId,
            provinceId: data?.provinceId,

            status: data?.status,
        };

        try {
            await handleAddUpdate(dto);
            reset();
            onClose();
            onSuccess();
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        }
    }

    const handleAddUpdate = async (data: ProjectCreateUpdateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<any>;
            if ( !data.id ) {
                res = await ProjectService.Create(data);
                enqueueSnackbar('Thêm mới thành công.', {
                    variant: 'success'
                });
            } else {
                res = await ProjectService.Update(data);
                enqueueSnackbar('Cập nhật thành công.', {
                    variant: 'success'
                });
            }
            console.log("handleAddUpdate res", res);
            
        } catch ( err ) {
            console.log("handleAddUpdate err", err);
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
                            name="name"
                            control={control}
                            rules={FORM_SCHEMA.name}
                            errors={errors}
                            placeholder="Tên dự án"
                            label="Tên dự án"
                            fullWidth
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="code"
                            control={control}
                            rules={FORM_SCHEMA.code}
                            errors={errors}
                            placeholder="Mã dự án"
                            fullWidth
                            label="Mã dự án"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="typeProjectId"
                            control={control}
                            rules={FORM_SCHEMA.typeProjectId}
                            errors={errors}
                            placeholder="Loại dự án"
                            fullWidth
                            label="Loại dự án"
                            fetchOptions={handleGetLookUpTypeProject}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="provinceId"
                            control={control}
                            rules={FORM_SCHEMA.provinceId}
                            errors={errors}
                            placeholder="Tỉnh/Thành phố"
                            fullWidth
                            label="Tỉnh/Thành phố"
                            fetchOptions={handleGetLookUpProvince}
                            disabled={isProvinceInputDisabled}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="districtId"
                            control={control}
                            rules={FORM_SCHEMA.districtId}
                            errors={errors}
                            placeholder="Quận/Huyện"
                            fullWidth
                            label="Quận/Huyện"
                            fetchOptions={handleGetLookUpDistrict}
                            disabled={isDistrictInputDisabled}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="communeId"
                            control={control}
                            rules={FORM_SCHEMA.communeId}
                            errors={errors}
                            placeholder="Phường/Xã"
                            fullWidth
                            label="Phường/Xã"
                            fetchOptions={handleGetLookUpCommune}
                            disabled={isCommuneInputDisabled}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
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

export default AddUpdateProjectModal;
