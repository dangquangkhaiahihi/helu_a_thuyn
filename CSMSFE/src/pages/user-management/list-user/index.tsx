import { useState } from 'react';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';
import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import LockResetIcon from '@mui/icons-material/LockReset';
import Chip from '@mui/material/Chip';
import { User } from '@/common/DTO/User/UserDTO';
import DataTable from '@/components/dataTable';
import headCellUser from './head-cell-user';
import UserService from '@/api/instance/user';
import LockOpenIcon from '@mui/icons-material/LockOpen';
import LockIcon from '@mui/icons-material/Lock';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { viewAlert } from '@/common/tools';
import CustomModal from '@/components/modalCustom';
import { Typography } from '@mui/material';
import dayjs from 'dayjs';

const ListUser = (props: any) => {
    const [selectedItem, setSelectedItem] = useState<User | null>(null);
    const [openActiveDeactive, setOpenActiveDeactive] = useState(false);

    const onOpenDetailDialog = (row: User) => {
        setOpenActiveDeactive(true);
        setSelectedItem(row);
    };

    const onCloseDetailDialog = () => {
        setOpenActiveDeactive(false);
        setSelectedItem(null);
    };

    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    };

    const handleResetPassword = async (row: User) => {
        setLoadingCircularOverlay(true);
        try {
            await UserService.ResetPassword(row.id);
            viewAlert('Đặt lại mật khẩu thành công.', 'success');
        } catch (err) {
            console.log('handleResetPassword err', err);
            viewAlert('Có lỗi xảy ra, vui lòng thực hiện lại sau.', 'error');
        } finally {
            setLoadingCircularOverlay(false);
        }
    };

    const onSuccess = () => {
        props.handleFilterAction();
        onCloseDetailDialog();
    }
    
    const handleActiveDeactiveUser = async () => {
        if ( !selectedItem ) return;

        setLoadingCircularOverlay(true);
        try {
            if (!selectedItem.status) {
                await UserService.Active(selectedItem.id);
                viewAlert('Mở khóa tài khoản thành công.', 'success');
            } else {
                await UserService.Deactive(selectedItem.id);
                viewAlert('Khóa tài khoản thành công.', 'success');
            }
            onSuccess();
        } catch (err) {
            console.log('handleStatusChange err', err);
            viewAlert('Có lỗi xảy ra, vui lòng thực hiện lại sau.', 'error');
        throw err;
        } finally {
        setLoadingCircularOverlay(false);
        }
    };

    return (
        <>
            <DataTable
                {...props}
                headCells={headCellUser}
                render={(row: User, index) => (
                    <TableRow hover tabIndex={-1} key={index}>
                        {/* <TableCell>{row.userName}</TableCell> */}
                        <TableCell>{row.fullName}</TableCell>
                        <TableCell>{row.email}</TableCell>
                        <TableCell>{row.phoneNumber}</TableCell>
                        <TableCell>{row.gender ? "Nam" : "Nữ"}</TableCell>
                        <TableCell>{row.roleName}</TableCell>
                        <TableCell>
                            <Chip
                                label={row.status ? 'Hoạt động' : 'Khóa'}
                                color={row.status ? 'success' : 'error'}
                            />
                        </TableCell>
                        <TableCell>
                            {dayjs(row.dateOfBirth).format("DD/MM/YYYY")}
                        </TableCell>
                        <TableCell>
                            {dayjs(row.modifiedDate).format("DD/MM/YYYY hh:mm:ss")}
                        </TableCell>
                        <TableCell>
                            <Tooltip title="Đặt lại mật khẩu" arrow>
                                <IconButton
                                    aria-label="reset-password"
                                    color="primary"
                                    size="small"
                                    sx={{ fontSize: 2 }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        handleResetPassword(row);
                                    }}
                                >
                                    <LockResetIcon fontSize="medium" />
                                </IconButton>
                            </Tooltip>
                        </TableCell>
                        <TableCell>
                            <Tooltip title="Chỉnh sửa" arrow>
                                <IconButton
                                    aria-label="edit"
                                    color="warning"
                                    size="small"
                                    sx={{ fontSize: 2 }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        props.openAddUpdateModal(row);
                                    }}
                                >
                                    <ModeEditOutlineOutlinedIcon fontSize="medium" />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title={row.status ? "Khóa tài khoản" : "Mở khóa tài khoản"} arrow>
                                <IconButton
                                    aria-label="lock_unlock"
                                    color="warning"
                                    size="small"
                                    sx={{ fontSize: 2 }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        onOpenDetailDialog(row);
                                    }}
                                >
                                    {
                                        row.status ? <LockIcon fontSize="medium" color='error'/> : <LockOpenIcon fontSize="medium" color='success'/>
                                    }
                                </IconButton>
                            </Tooltip>
                        </TableCell>
                    </TableRow>
                )}
            />

            <CustomModal
                isOpen={openActiveDeactive}
                onClose={onCloseDetailDialog}
                size='lg'
                title={"Xác nhận"}
                onSave={handleActiveDeactiveUser}
                titleClose='Hủy bỏ'
                titleSave='Xác nhận'
            >
                {
                    selectedItem?.status ? (
                        <Typography>Bạn có chắc muốn khóa tài khoản của người dùng <b>{selectedItem?.fullName}</b> không?</Typography>
                    ) : (
                        <Typography>Bạn có chắc muốn mở khóa tài khoản của người dùng <b>{selectedItem?.fullName}</b> không?</Typography>
                    )
                }
            </CustomModal>
        </>
    );
};

export default ListUser;
