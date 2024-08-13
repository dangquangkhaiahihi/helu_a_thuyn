import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import dayjs from 'dayjs';
import FormInput from '@/components/formInput';
import CustomModal from '@/components/modalCustom';
import { Grid } from '@mui/material';
import { isValidEmail, isValidPhoneNumber, viewAlert } from '@/common/tools';
import { User } from '@/common/DTO/User/UserDTO';
import { UserCreateUpdateDTO } from '@/common/DTO/User/UserCreateUpdateDTO';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import UserService from '@/api/instance/user';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import RoleService from '@/api/instance/role';

interface IAddUpdateUserModalProps {
  isOpen: boolean;
  selectedItem: User | null;
  onClose: () => void;
  onSuccess: () => void;
}

const FORM_SCHEMA = {
  userName: { required: 'Đây là trường bắt buộc' },
  roleId: { required: 'Đây là trường bắt buộc' },
  fullName: { required: 'Đây là trường bắt buộc' },
  phoneNumber: {
    required: 'Đây là trường bắt buộc',
    validate: (value: string) => isValidPhoneNumber(value) || 'Số điện thoại sai định dạng',
  },
  email: {
    validate: (value: string) => !value || isValidEmail(value) || 'Email sai định dạng',
  },
  dateOfBirth: {
    required: 'Đây là trường bắt buộc',
    validate: (value: any) => dayjs(value).isValid() || 'Ngày được chọn không hợp lệ',
  },
  gender: { required: 'Đây là trường bắt buộc' },
  address: { required: 'Đây là trường bắt buộc' },
  description: { required: 'Đây là trường bắt buộc' },
};

const AddUpdateUserModal: React.FC<IAddUpdateUserModalProps> = ({
  isOpen,
  selectedItem,
  onClose,
  onSuccess,
}) => {
  const { control, handleSubmit, formState: { errors }, reset, setValue, watch } = useForm({
    mode: 'onChange',
    defaultValues: {
      userName: '',
      fullName: '',
      email: '',
      roleId: '',
      dateOfBirth: '',
      phoneNumber: '',
      address: '',
      gender: 'true',
      description: '',
    },
  });

  const gender = watch("gender")
  const roleId = watch("roleId")

  useEffect(() => {
    console.log("gender, roleId", gender, roleId);
    
  }, [gender, roleId])

  const handleGetLookUpRole = async (keyword: string) => {
    try {
        const param = {
            Keyword: keyword
        };
        const res: ApiResponse<LookUpItem[]> = await RoleService.GetLookup(param);
        
        return res.content;
    } catch ( err ) {
        throw err;
    }
}

  useEffect(() => {
    console.log("selectedItem", selectedItem);
    
    if ( !isOpen ) return;

      selectedItem?.userName && setValue('userName', selectedItem.userName);
      selectedItem?.fullName && setValue('fullName', selectedItem.fullName);
      selectedItem?.email && setValue('email', selectedItem.email);
      selectedItem?.roleId && setValue('roleId', selectedItem.roleId);
      selectedItem?.dateOfBirth && selectedItem.dateOfBirth && setValue('dateOfBirth', selectedItem.dateOfBirth ? dayjs(selectedItem.dateOfBirth).format('YYYY-MM-DD') : '');
      selectedItem?.description && setValue('description', selectedItem.description);
      selectedItem?.phoneNumber && setValue('phoneNumber', selectedItem.phoneNumber);
      selectedItem?.address && setValue('address', selectedItem.address);
      typeof selectedItem?.gender === "boolean" && setValue('gender', selectedItem.gender.toString());
  }, [isOpen]);

  const dispatch = useDispatch();
  const setLoadingCircularOverlay = (val: boolean) => {
    dispatch(setLoadingCircularOverlayRedux(val));
  };

  const onSubmit = async (data: any) => {
    const dto: UserCreateUpdateDTO = {
      id: selectedItem?.id,
      email: data.email,
      userName: data.userName,
      fullName: data.fullName,
      roleId: data.roleId,
      dateOfBirth: dayjs(data.dateOfBirth).format(),
      gender: data.gender,
      address: data.address,
      description: data.description,
      phoneNumber: data.phoneNumber,
      avatar: data.avatar ? data.avatar[0] : null
    };

    try {
      await handleAddUpdateUser(dto);
      reset();
      onClose();
      onSuccess();
    } catch (err) {
      viewAlert('Có lỗi xảy ra, vui lòng thực hiện lại sau.', 'error');
    }
  };

  const handleAddUpdateUser = async (data: UserCreateUpdateDTO) => {
    setLoadingCircularOverlay(true);
    try {
        let res: ApiResponse<any>;
        if ( !data.id ) {
            res = await UserService.Create(data);
            viewAlert('Thêm mới thành công.', 'success');
        } else {
            res = await UserService.Update(data);
            viewAlert('Cập nhật thành công.', 'success');
        }
        console.log("handleAddUpdateSomeTable res", res);
        
    } catch ( err ) {
        console.log("handleAddUpdateSomeTable err", err);
        throw err;
    } finally {
        setLoadingCircularOverlay(false);
    }
}
  

  const onCloseModal = () => {
    onClose();
    reset();
  };

  const onInvalid = (errors: any) => {
    console.log('Form Submission Failed with Errors:', errors);
  };

  return (
    <CustomModal
      title={selectedItem ? 'Cập nhật người dùng' : 'Thêm mới người dùng'}
      isOpen={isOpen}
      onSave={() => handleSubmit(onSubmit, onInvalid)()}
      onClose={onCloseModal}
      size='lg'
    >
      <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="userName"
              control={control}
              rules={FORM_SCHEMA.userName}
              errors={errors}
              placeholder="Tên tài khoản"
              label="Tên tài khoản"
              fullWidth
              disabled={selectedItem ? true : false}
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
              label="Email"
              fullWidth
              disabled={selectedItem ? true : false}
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="fullName"
              control={control}
              rules={FORM_SCHEMA.fullName}
              errors={errors}
              placeholder="Họ & Tên"
              label="Họ & Tên"
              fullWidth
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="async-auto-complete"
              name="roleId"
              control={control}
              rules={FORM_SCHEMA.roleId}
              errors={errors}
              placeholder="Chức vụ"
              label="Chức vụ"
              fullWidth
              fetchOptions={handleGetLookUpRole} 
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="phoneNumber"
              control={control}
              rules={FORM_SCHEMA.phoneNumber}
              errors={errors}
              placeholder="Số điện thoại"
              label="Số điện thoại"
              fullWidth
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="date"
              name="dateOfBirth"
              control={control}
              rules={FORM_SCHEMA.dateOfBirth}
              errors={errors}
              placeholder="Ngày Sinh"
              label="Ngày sinh"
              fullWidth
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="auto-complete"
              name="gender"
              control={control}
              rules={FORM_SCHEMA.gender}
              errors={errors}
              label="Giới tính"
              fullWidth
              options={[
                { value: 'true', label: 'Nam' },
                { value: 'false', label: 'Nữ' },
              ]}
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="text"
              name="address"
              control={control}
              rules={FORM_SCHEMA.address}
              errors={errors}
              placeholder="Địa chỉ"
              label="Địa chỉ"
              fullWidth
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <FormInput
              type="file"
              name="avatar"
              control={control}
              placeholder="Ảnh"
              label="Ảnh"
              accept={['.jpg', '.png']}
            />
          </Grid>
        </Grid>
      </form>
    </CustomModal>
  );
};

export default AddUpdateUserModal;
