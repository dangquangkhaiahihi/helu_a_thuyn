import React, { useEffect } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { Button, Divider, Grid, IconButton, Tooltip } from '@mui/material';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import RoleService from '@/api/instance/role';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import { Role } from '@/common/DTO/Role/RoleDTO';
import { CreateSecurityMatrixDTO, ScreenDTO } from '@/common/DTO/Role/CreateSecurityMatrixDTO';
import { viewAlert } from '@/common/tools';

interface IUpdateSecurityMatrixRoleModalProps {
  isOpen: boolean;
  selectedItem: Role | null;
  onClose: () => void;
  onSuccess: () => void;
}

const handleGetLookUpScreen = async (keyword: string) => {
  try {
    const param = {
      Keyword: keyword
    };
    const res: ApiResponse<LookUpItem[]> = await RoleService.LookUpScreen(param);
      
    return res.content;
  } catch ( err ) {
    throw err;
  }
}

const handleGetLookUpAction = async (keyword: string) => {
  try {
    const param = {
      Keyword: keyword
    };
    const res: ApiResponse<LookUpItem[]> = await RoleService.LookUpAction(param);
      
    return res.content;
  } catch ( err ) {
    throw err;
  }
}

interface IFormScreen {
  screenId: string;
  actions: string[];
}

const newScreenActions: IFormScreen = {
  screenId: "",
  actions: []
}

interface IFormValues {
  roleId: string;
  screens: IFormScreen[]
}

const FORM_SCHEMA = {
  screenId: {
    required: 'Đây là trường bắt buộc'
  },
  actions: {
    required: 'Đây là trường bắt buộc',
    validate: (value: string[]) => {
      return value.length !== 0 || 'Đây là trường bắt buộc'
    },
  }
};

const UpdateSecurityMatrixRoleModal: React.FC<IUpdateSecurityMatrixRoleModalProps> = ({
  isOpen,
  selectedItem,
  onClose,
  onSuccess,
}) => {

  const dispatch = useDispatch();
  const setLoadingCircularOverlay = (val: boolean) => {
    dispatch(setLoadingCircularOverlayRedux(val));
  };

  const { control, handleSubmit, formState: { errors }, reset, setValue, clearErrors } = useForm<IFormValues>({
    mode: 'onChange',
    defaultValues: {
      roleId: "",
      screens: []
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "screens"
  });

  useEffect(() => {
    if ( !isOpen || !selectedItem) return;

    setValue('roleId', selectedItem.id);
    handleGetSecurityMatrixDetail(selectedItem.id);
  }, [ isOpen, selectedItem ]);

  const handleGetSecurityMatrixDetail = async (roleId: string) => {
    setLoadingCircularOverlay(true);
    try {
      let res: ApiResponse<ScreenDTO[]> = await RoleService.GetSecurityMatrixDetail(roleId);
      setValue("screens", res.content.map(screen => ({
        screenId: screen.screenId,
        actions: screen.actions.map(action => action.actionId.toString()),
      })));
      console.log('handleGetSecurityMatrixDetail res', res);
    } catch (err) {
      console.log('handleGetSecurityMatrixDetail err', err);
    } finally {
      setLoadingCircularOverlay(false);
    }
  };

  const onCloseModal = () => {
    onClose();
    reset();
  };

  const onInvalid = (errors: any) => {
    console.log('Form Submission Failed with Errors:', errors);
  };

  const onSubmit = async (data: IFormValues) => {
    console.log("onSubmit", data);
    try {
      const createSecurityMatrixDTO: CreateSecurityMatrixDTO = {
        roleId: data.roleId,
        screens: data.screens.map(screen => ({
          screenId: screen.screenId,
          actions: screen.actions.map(actionId => ({
            actionId: actionId,
          })),
        }))
      }
      await handleUpdateSecurityMatrix(createSecurityMatrixDTO);
      onSuccess();
      onCloseModal();
    } catch {
      viewAlert('Có lỗi xảy ra, vui lòng thử lại sau.', 'error');
    }
  };

  const handleUpdateSecurityMatrix = async (data: CreateSecurityMatrixDTO) => {
    setLoadingCircularOverlay(true);
    try {
      const res = await RoleService.UpdateSecurityMatrix(data);
      viewAlert('Cập nhật quyền thành công.', 'success');
      console.log('handleUpdateSecurityMatrix res', res);
    } catch (err) {
      console.log('handleUpdateSecurityMatrix err', err);
      throw err;
    } finally {
      setLoadingCircularOverlay(false);
    }
  };

  return (
    <CustomModal
      title={selectedItem ? 'Cập nhật vai trò' : 'Thêm mới vai trò'}
      isOpen={isOpen}
      onSave={() => {handleSubmit(onSubmit, onInvalid)();}}
      onClose={onCloseModal}
      size='lg'
    >
      <form autoComplete="off">
        {
          fields.map((field, index) => {
            return (              
              <div key={field.id}>
                <Grid container spacing={2}>
                  <Grid item xs={12} md={5}>
                    <FormInput
                      type="async-auto-complete"
                      name={`screens.${index}.screenId`}
                      control={control}
                      rules={FORM_SCHEMA.screenId}
                      errors={errors?.screens?.[index]?.screenId}
                      // errors={errors}
                      placeholder="Màn hình"
                      label="Màn hình"
                      fullWidth
                      fetchOptions={handleGetLookUpScreen} 
                    />
                  </Grid>
                  <Grid item xs={12} md={6}>
                    <FormInput
                      type="async-auto-complete-multi-select"
                      name={`screens.${index}.actions`}
                      control={control}
                      rules={FORM_SCHEMA.actions}
                      errors={errors?.screens?.[index]?.actions}
                      // errors={errors}
                      placeholder="Hành động"
                      label="Hành động"
                      fullWidth
                      fetchOptions={handleGetLookUpAction} 
                    />
                  </Grid>
                  <Grid item xs={12} md={1} sx={{ justifyContent: "center", alignItems: "center", display: "flex", padding: 0}}>
                    <Tooltip title="Xóa" arrow>
                      <IconButton
                        aria-label="edit"
                        color="error"
                        size="small"
                        sx={{ fontSize: 2 }}
                        onClick={(e) => {
                          e.stopPropagation();
                          remove(index);
                          clearErrors();
                        }}
                      >
                        <DeleteIcon fontSize="medium" />
                      </IconButton>
                    </Tooltip>
                  </Grid>
                </Grid>

                {index !== fields.length - 1 ? <Divider sx={{mb: 3}}/> : <></>}
              </div>
            )
          })
        }

        <Button
          type="button"
          variant="text"
          disableElevation
          fullWidth
          startIcon={<AddIcon />}
          color="cuaternary"
          onClick={() => {
            append(newScreenActions);
            clearErrors();
          }}
          sx={{ display: 'flex', justifyContent: 'center', paddingY: '10px', alignItems: 'center' }}
        >
          Thêm mới 
        </Button>
      </form>
    </CustomModal>
  );
};

export default UpdateSecurityMatrixRoleModal;