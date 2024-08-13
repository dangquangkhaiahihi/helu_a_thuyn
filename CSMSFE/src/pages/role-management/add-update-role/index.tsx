import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { Grid } from '@mui/material';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import RoleService from '@/api/instance/role';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import { Role } from '@/common/DTO/Role/RoleDTO';
import { RoleCreateUpdateDTO } from '@/common/DTO/Role/RoleCreateUpdateDTO';
import { viewAlert } from '@/common/tools';

interface IAddUpdateRoleModalProps {
  isOpen: boolean;
  selectedItem: Role | null;
  onClose: () => void;
  onSuccess: () => void;
}

const FORM_SCHEMA = {
  code: { required: 'Đây là trường bắt buộc' },
  name: { required: 'Đây là trường bắt buộc' },
};

const AddUpdateRoleModal: React.FC<IAddUpdateRoleModalProps> = ({
  isOpen,
  selectedItem,
  onClose,
  onSuccess,
}) => {
  const { control, handleSubmit, formState: { errors }, reset, setValue } = useForm({
    mode: 'onChange',
    defaultValues: {
      id: '',
      code: '',
      name: '',
      createdBy: '',
      createdDate: '',
      modifiedBy: '',
      modifiedDate: '',
    },
  });

  useEffect(() => {
    if ( !isOpen ) return;

    selectedItem?.id && setValue('id', selectedItem.id);
    selectedItem?.code && setValue('code', selectedItem.code);
    selectedItem?.name && setValue('name', selectedItem.name);
  }, [isOpen]);

  const dispatch = useDispatch();
  const setLoadingCircularOverlay = (val: boolean) => {
    dispatch(setLoadingCircularOverlayRedux(val));
  };

  const onSubmit = async (data: any) => {
    let dto: RoleCreateUpdateDTO;

    if (selectedItem && selectedItem.id) {
      dto = {
        id: selectedItem.id.toString(),
        code: data.code,
        name: data.name,
      };
    } else {
      dto = {
        id: '', // Tạo hoặc xử lý ID tạo từ phía server
        code: data.code,
        name: data.name,
      };
    }

    try {
      await handleAddUpdateRole(dto);
      reset();
      onClose();
      onSuccess();
    } catch (err) {
      viewAlert('Có lỗi xảy ra, vui lòng thử lại sau.', 'error');
    }
  };

  const handleAddUpdateRole = async (data: RoleCreateUpdateDTO) => {
    setLoadingCircularOverlay(true);
    try {
      let res: ApiResponse<any>;
      if (!selectedItem?.id) {
        res = await RoleService.Create(data);
        viewAlert('Thêm mới thành công.', 'success');
      } else {
        res = await RoleService.Update(data);
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

  return (
    <CustomModal
      title={selectedItem ? 'Cập nhật vai trò' : 'Thêm mới vai trò'}
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
              placeholder="Mã vai trò"
              label="Mã vai trò"
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
              placeholder="Tên vai trò"
              label="Tên vai trò"
              fullWidth
            />
          </Grid>
        </Grid>
      </form>
    </CustomModal>
  );
};

export default AddUpdateRoleModal;
