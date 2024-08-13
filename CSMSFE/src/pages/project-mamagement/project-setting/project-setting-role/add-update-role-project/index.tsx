import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { Grid } from '@mui/material';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import { viewAlert } from '@/common/tools';
import { RoleProject } from '@/common/DTO/RoleProjectDTO/RoleProjectDTO';
import PermissionProjectService from '@/api/instance/permission-project';
import { CreateUpdateRoleProjectDTO } from '@/common/DTO/RoleProjectDTO/CreateUpdateRoleProjectDTO';
import { useParams } from 'react-router-dom';

interface IAddUpdateRoleProjectModalProps {
  isOpen: boolean;
  selectedItem: RoleProject | null;
  onClose: () => void;
  onSuccess: () => void;
}

const FORM_SCHEMA = {
  code: { required: 'Đây là trường bắt buộc' },
  name: { required: 'Đây là trường bắt buộc' },
  actionIds: { required: 'Đây là trường bắt buộc' },
};

const AddUpdateRoleProjectModal: React.FC<IAddUpdateRoleProjectModalProps> = ({
  isOpen,
  selectedItem,
  onClose,
  onSuccess,
}) => {
  const { projectId } = useParams();

  const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm<CreateUpdateRoleProjectDTO>({
    mode: 'onChange',
    defaultValues: {
      id: '',
      code: '',
      name: '',
      actionIds: []
    },
  });

  useEffect(() => {
    if ( !isOpen ) return;

    selectedItem?.id && setValue('id', selectedItem.id);
    selectedItem?.code && setValue('code', selectedItem.code);
    selectedItem?.name && setValue('name', selectedItem.name);
    selectedItem?.actionIds && setValue('actionIds', selectedItem.actionIds);
  }, [isOpen]);

  const dispatch = useDispatch();
  const setLoadingCircularOverlay = (val: boolean) => {
    dispatch(setLoadingCircularOverlayRedux(val));
  };

  const onSubmit = async (data: any) => {
    let dto: CreateUpdateRoleProjectDTO = {
      id: selectedItem?.id || "",
      code: data.code,
      name: data.name,
      projectId: projectId || "",
      actionIds: data.actionIds
    };

    try {
      await handleAddUpdateRole(dto);
      reset();
      onClose();
      onSuccess();
    } catch (err) {
      viewAlert('Có lỗi xảy ra, vui lòng thử lại sau.', 'error');
    }
  };

  const handleAddUpdateRole = async (data: CreateUpdateRoleProjectDTO) => {
    setLoadingCircularOverlay(true);
    try {
      let res: ApiResponse<any> = await PermissionProjectService.CreateOrUpdateRole(data);
      if (!selectedItem?.id) {
        viewAlert('Thêm mới thành công.', 'success');
      } else {
        viewAlert('Cập nhật thành công.', 'success');
      }
      console.log('handleAddUpdateRole res', res);
    } catch (err) {
      console.log('handleAddUpdateRole err', err);
      throw err;
    } finally {
      setLoadingCircularOverlay(false);
    }
  };

  const onCloseModal = () => {
    onClose();
    reset();
  };

  const handleGetLookUpAction = async (keyword: string) => {
    try {
      const param = {
        Keyword: keyword
      };
      const res: ApiResponse<LookUpItem[]> = await PermissionProjectService.LookupAction(param);
        
      return res.content;
    } catch ( err ) {
      throw err;
    }
  }

  return (
    <CustomModal
      title={selectedItem ? 'Cập nhật chức vụ' : 'Thêm mới chức vụ'}
      isOpen={isOpen}
      onSave={() => {handleSubmit(onSubmit)();}}
      onClose={onCloseModal}
      size='lg'
    >
      <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="code"
              control={control}
              rules={FORM_SCHEMA.code}
              errors={errors}
              placeholder="Mã chức vụ"
              label="Mã chức vụ"
              fullWidth
              disabled={selectedItem ? true : false}
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="name"
              control={control}
              rules={FORM_SCHEMA.name}
              errors={errors}
              placeholder="Tên chức vụ"
              label="Tên chức vụ"
              fullWidth
            />
          </Grid>
          <Grid item xs={12} md={12}>
            <FormInput
              type="async-auto-complete-multi-select"
              name={`actionIds`}
              control={control}
              rules={FORM_SCHEMA.actionIds}
              errors={errors}
              // errors={errors}
              placeholder="Quyền"
              label="Quyền"
              fullWidth
              fetchOptions={handleGetLookUpAction} 
            />
          </Grid>
        </Grid>
      </form>
    </CustomModal>
  );
};

export default AddUpdateRoleProjectModal;
