import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchProjectInfo, projectInfoRedux } from '@/store/redux/project';
import { Button, Card, Divider, Grid, Stack, Typography } from '@mui/material';
import { useForm } from 'react-hook-form';
import FormInput from '@/components/formInput';
import { ProjectCreateUpdateDTO } from '@/common/DTO/Project/ProjectCreateUpdateDTO';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import ProjectService from '@/api/instance/project';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import LocationLookUpService from '@/api/instance/location-look-up';
import CustomModal from '@/components/modalCustom';
import { viewAlert } from '@/common/tools';
import { UrlRouteCollection } from '@/common/url-route-collection';

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

const ProjectSettingInfo = ({  }: any) => {
    const { projectId } = useParams();
    const dispatch = useDispatch();
    const projectInfo = useSelector(projectInfoRedux);
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    useEffect(() => {
        // if ( projectInfo ) return;
        dispatch(fetchProjectInfo(projectId || ""));
    }, [])

    useEffect(() => {
        if ( !projectInfo ) return;
        projectInfo.id && setValue("id", projectInfo.id);
        projectInfo.name && setValue("name", projectInfo.name);
        projectInfo.code && setValue("code", projectInfo.code);
        projectInfo.description && setValue("description", projectInfo.description);
        projectInfo.typeProjectId && setValue("typeProjectId", projectInfo.typeProjectId);

        projectInfo.communeId && setValue("communeId", projectInfo.communeId);
        projectInfo.districtId && setValue("districtId", projectInfo.districtId);
        projectInfo.provinceId && setValue("provinceId", projectInfo.provinceId);

        projectInfo.status && setValue("status", projectInfo.status);
        setIsFirstLoadAfterProjectInfo(false);
    }, [projectInfo])

    const { control, handleSubmit, formState: { errors }, setValue, watch} = useForm({
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

            status: "",
		},
	});
    
    // START LOGIC SETUP LOOKUP LOCATION
    const provinceId = watch('provinceId');
    const districtId = watch('districtId');
    const [isProvinceInputDisabled, setIsProvinceInputDisabled] = useState<boolean>(false);
    const [isDistrictInputDisabled, setIsDistrictInputDisabled] = useState<boolean>(false);
    const [isCommuneInputDisabled, setIsCommuneInputDisabled] = useState<boolean>(false);

    const [isFirstLoadAfterProjectInfo, setIsFirstLoadAfterProjectInfo] = useState<boolean>(true);

    useEffect(() => {
        if ( isFirstLoadAfterProjectInfo ) return;
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
        if ( isFirstLoadAfterProjectInfo ) return;
        // Trigger cái này để clear option của commune
        setIsCommuneInputDisabled(true);
        
        // khi districtId đổi giá trị thì clear luôn giá trị thằng con
        // setValue("communeId", "");
        setTimeout(() => {
            setIsCommuneInputDisabled(districtId ? false : true);
        }, 200)

        if( !districtId ){
            setIsCommuneInputDisabled(true);
            setTimeout(() => {
                setIsCommuneInputDisabled(false);
            }, 200)
        }
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
    // END LOGIC SETUP LOOKUP LOCATION

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
        } catch ( err ) {
            viewAlert("Có lỗi xảy ra, vui lòng thử lại sau.", "error");
        }
    }

    const handleAddUpdate = async (data: ProjectCreateUpdateDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<any> = await ProjectService.Update(data);
            viewAlert('Cập nhật thành công.', 'success');
            console.log("handleAddUpdate res", res);
            
        } catch ( err ) {
            console.log("handleAddUpdate err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }

	return (
        <>
            <Card variant='outlinedElevation'>
                <Typography variant="h3" color="primary" mb={3}>
                    Thông tin dự án
                </Typography>
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
                        <Grid item xs={12} md={12}>
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
                    <Stack direction="row" spacing={3} justifyContent="flex-end">
                        <Button size="small" type='submit' variant='contained' color='primary'>
                            Lưu
                        </Button>
                    </Stack>
                </form>
            </Card>

            <Divider sx={{m:3}}/>

            <DeleteProjectSection projectInfo={projectInfo}/>
        </>
	);
}

export default ProjectSettingInfo;

const FORM_DELETE_SCHEMA = {
    name_project: {
        required: 'Đây là trường bắt buộc',
    },
};

const DeleteProjectSection = ({projectInfo} : any) => {
    const { control, handleSubmit, formState: { errors }, reset, setError } = useForm({
		mode: 'onChange',
		defaultValues: {
            name_project: ""
		},
	});
    const navigate = useNavigate();

    const [openDelete, setOpenDelete] = useState(false);

    const onOpenDelete = () => {
        setOpenDelete(true);
    }

    const onCloseDelete = () => {
        setOpenDelete(false);
        reset();
    }

    const onSubmitDelete = async (data: any) => {
        if ( data.name_project !== projectInfo?.name ) {
            setError("name_project", { type: "required", message: "Tên dự án chưa đúng." });
            return;
        }
        
        try {
            await handleDelete(projectInfo.id);
            navigate(UrlRouteCollection.ProjectManagement);
        } catch ( err ) {
            viewAlert("Có lỗi xảy ra, vui lòng thử lại sau.", "error");
        }
    }
    
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const handleDelete = async(projectId: string) => {
        setLoadingCircularOverlay(true);
        try {
            await ProjectService.Delete(projectId);
            viewAlert("Xóa dự án thành công.", "success");
        } catch ( err ) {
            viewAlert("Có lỗi xảy ra, vui lòng thử lại sau.", "error");
            console.log("handleAddUpdate err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }

    return <>
        <Card variant='outlinedElevation'>
            <Typography variant="h3" color="primary" mb={3}>
                Xóa dự án
            </Typography>
            <Typography variant='subtitle1'>
                Xóa vĩnh viễn dự án và tất cả nội dung bên trong. Hành động này không thể hoàn tác.
            </Typography>
            <Stack direction="row" spacing={3} justifyContent="flex-end">
                <Button size="small" onClick={onOpenDelete} variant='contained' color='error'>
                    Xóa
                </Button>
            </Stack>
        </Card>

        <CustomModal
            isOpen={openDelete}
            onClose={onCloseDelete}
            size='lg'
            title={"Xác nhận"}
            onSave={() => {handleSubmit(onSubmitDelete)()}}
            titleClose='Hủy bỏ'
            titleSave='Xác nhận'
        >
            <Typography>Bạn có chắc chắn muốn xóa vĩnh viễn dự án <b>{projectInfo?.name}</b> và tất cả nội dung bên trong. Hành động này không thể hoàn tác.</Typography>
            <form autoComplete="off">
                <FormInput
                    type="text"
                    name="name_project"
                    control={control}
                    rules={FORM_DELETE_SCHEMA.name_project}
                    errors={errors}
                    placeholder="Tên dự án"
                    label="Nhập tên dự án để xác nhận"
                    fullWidth
                />
            </form>
        </CustomModal>
    </>
}