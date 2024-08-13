// import { useDispatch } from 'react-redux';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Button, Card, Grid, Typography, LinearProgress, CardMedia, Stack, Box, Tooltip, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { UserProject } from '@/common/DTO/UserProject/UserProjectDTO';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import PermissionUserProjectService from '@/api/instance/permission-user-project';
import FormInput from '@/components/formInput';
import { useForm } from 'react-hook-form';
import PermissionProjectService from '@/api/instance/permission-project';
import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import CheckIcon from '@mui/icons-material/Check';
import CloseIcon from '@mui/icons-material/Close';
import DeleteIcon from '@mui/icons-material/Delete';
import CancelScheduleSendIcon from '@mui/icons-material/CancelScheduleSend';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { viewAlert } from '@/common/tools';
import { UpdateUserRoleDTO } from '@/common/DTO/RoleUserProjectDTO/UpdateUserRoleDTO';

const ProjectSettingCollaborator = ({  }: any) => {
    const { projectId } = useParams();
    const [data, setData] = useState<UserProject[]>([]);

    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    };

    const handleFilter = async () => {
        setIsLoadingFilter(true);
        
        try {
            const res: ApiResponse<UserProject[]> = await PermissionUserProjectService.GetUsersInProject(projectId);
            setData(res.content);
        } catch ( err ) {
            console.error("handleFilter error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    useEffect(() => {
        handleFilter();
    }, [])

    //

    const LookupRoleByProject = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword,
                projectId: projectId || ""
            };
            const res: ApiResponse<LookUpItem[]> = await PermissionProjectService.LookupRoleByProject(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const handleUpdateUserRole = async (data: UpdateUserRoleDTO) => {
        setLoadingCircularOverlay(true);
        try {
            let res: ApiResponse<any> = await PermissionUserProjectService.UpdateUserRole(data);
            console.log('handleUpdateUserRole res', res);

            if ( res.content ) {
                handleFilter();
                viewAlert('Chỉnh sửa quyền thành công.', 'success');
            } else {
                viewAlert('Không thể chỉnh sửa thành viên thành chủ sở hữu.', 'error');
            }
            
        } catch (err) {
            const apiError = err as ApiResponse<string>;
            viewAlert(apiError.err.errorMessage, 'error');
            console.log('handleUpdateUserRole err', err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    };

	return (
        <Card variant='outlinedElevation'>
            <Grid container
                direction="row"
                alignItems="start"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                
            >
                <Grid item xs={12} md={6}>
                    <Typography variant="h3" color="primary" mb={3}>
                        Thành viên dự án
                    </Typography>
                </Grid>
                <Grid item xs={12} md={6} display={"flex"} justifyContent={"flex-end"}>
                    <Button
                        type="button"
                        variant="contained"
                        disableElevation
                        startIcon={<AddIcon />}
                        color='primary'
                        onClick={(e) => {
                            e.preventDefault();
                            // onOpenInviteMemberModal(null);
                        }}
                    >
                        Mời thành viên
                    </Button>
                </Grid>
            </Grid>

            <LinearProgress color={"info"} sx={{ opacity: isLoadingFilter ? 1 : 0 }} />

            {
                data.map((item, index) => {
                    return (
                        <UserInProjectCard
                            key={index}
                            item={item}
                            projectId={projectId || ""}
                            LookupRoleByProject={LookupRoleByProject}
                            handleUpdateUserRole={handleUpdateUserRole}
                        />
                    )
                })
            }
        </Card>
	);
}

interface IUserInProjectCard {
    item: UserProject;
    projectId: string;
    LookupRoleByProject: (query: string) => Promise<LookUpItem[]>;
    handleUpdateUserRole: Function;
}

const UserInProjectCard: React.FC<IUserInProjectCard> = ({
    item,
    projectId,
    LookupRoleByProject,
    handleUpdateUserRole,
}) => {
    const [isEdit, setIsEdit] = useState(false);
    const [isProjectOwner] = useState(item.roles[0].code === "PROJECT_OWNER");
    const [isAcceptedInvitation] = useState(typeof item.isPending === "boolean" && item.isPending);

    const { control, handleSubmit, formState: { errors }, setValue, clearErrors } = useForm<UpdateUserRoleDTO>({
        mode: 'onChange',
        defaultValues: {
            userId: item.userId,
            projectId: projectId,
            roleId: item.roles[0].id,
        },
    });

    useEffect(() => {
        item.userId && setValue("userId", item.userId);
        item.roles && setValue("roleId", item.roles[0].id);
        clearErrors();
    }, [isEdit])

    const onSubmit = async (data: UpdateUserRoleDTO) => {
        await handleUpdateUserRole(data);
        setIsEdit(false);
    };

    const onInvalid = () => {
        viewAlert('Chưa chọn chức vụ.', 'error');
    };

    return (
        <Card variant='user-in-project-card'>
            <Stack direction={"row"} alignItems={"center"} justifyContent={"space-between"}>
                <Grid display={"flex"} item xs={12} md={7}>
                    <CardMedia
                        sx={{
                            height: 40,
                            width: 40,
                            backgroundPosition: '50%',
                            backgroundSize: 'contain',
                        }}
                        image={"/assets/images/avatars/default-user.png" }
                        title={item.fullName}
                    />
                    <Box pl={"10px"}>
                        <Typography variant='subtitle1'>{item.fullName}</Typography>
                        <Typography variant='subtitle2'>{item.email}</Typography>
                    </Box>
                </Grid>
                
                <Grid item xs={12} md={5}>
                    <Stack direction={"row"} alignItems={"center"} justifyContent={"flex-end"}>
                        {
                            isProjectOwner ? (
                                <Typography variant='subtitle2'>{`${item.roles[0].name}`}</Typography>
                            ) : 
                            <>
                                {
                                    !isEdit ? (
                                        <>
                                            <Typography variant='subtitle2'>{`${item.roles[0].name} ${isAcceptedInvitation ? "(Chưa chấp nhận lời mời)" : ""}`}</Typography>
                                            <Tooltip title="Chỉnh sửa" arrow>
                                                <IconButton
                                                    aria-label="edit"
                                                    color="warning"
                                                    size="small"
                                                    sx={{ fontSize: 20 }}
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        setIsEdit(true);
                                                    }}
                                                >
                                                    <ModeEditOutlineOutlinedIcon fontSize="medium" />
                                                </IconButton>
                                            </Tooltip>
                                        </>
                                    ) : (
                                        <>
                                            <Grid item xs={12} md={12}>
                                                <FormInput
                                                    type="async-auto-complete"
                                                    name={`roleId`}
                                                    control={control}
                                                    rules={{required: 'Đây là trường bắt buộc.'}}
                                                    errors={errors}
                                                    placeholder="Chức vụ"
                                                    fullWidth
                                                    fetchOptions={LookupRoleByProject}
                                                    hidehelpertext
                                                />
                                            </Grid>
                                            <Tooltip title="Đóng" arrow>
                                                <IconButton
                                                    aria-label="edit"
                                                    color="error"
                                                    size="small"
                                                    sx={{ fontSize: 20 }}
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        setIsEdit(false);
                                                    }}
                                                >
                                                    <CloseIcon fontSize="medium" />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title="Xác nhận" arrow>
                                                <IconButton
                                                    aria-label="edit"
                                                    color="success"
                                                    size="small"
                                                    sx={{ fontSize: 20 }}
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        handleSubmit(onSubmit, onInvalid)();
                                                    }}
                                                >
                                                    <CheckIcon fontSize="medium" />
                                                </IconButton>
                                            </Tooltip>
                                        </>
                                    )
                                }
                                <Tooltip title={!item.isPending ? "Xóa" : "Xóa lời mời"} arrow>
                                    <IconButton
                                        aria-label="edit"
                                        color="error"
                                        size="small"
                                        sx={{ fontSize: 20 }}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                        }}
                                    >
                                        {
                                            !item.isPending ? <DeleteIcon fontSize="medium" /> : <CancelScheduleSendIcon fontSize="medium" />
                                        }
                                    </IconButton>
                                </Tooltip> 
                            </>
                        }
                    </Stack>
                </Grid>
            </Stack>
        </Card>
    )
}

export default ProjectSettingCollaborator;