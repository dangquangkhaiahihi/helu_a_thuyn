import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';

import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';

import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import DeleteIcon from '@mui/icons-material/Delete';

import DataTable from '@/components/dataTable';
import headCellRoleProject from './head-cell-role-project';
import dayjs from 'dayjs'; // Import dayjs
import { RoleProject } from '@/common/DTO/RoleProjectDTO/RoleProjectDTO';

const ListRoleProject = (props: any) => {
    return (
        <>
            <DataTable
                {...props}
                headCells={headCellRoleProject}
                render={(row: RoleProject, index) => (
                    <TableRow hover tabIndex={-1} key={index}>
                        <TableCell>{row.name}</TableCell>
                        <TableCell>{row.code}</TableCell>
                        <TableCell>{dayjs(row.createdDate).format('DD/MM/YYYY hh:ss:mm')}</TableCell>
                        <TableCell>{row.createdBy}</TableCell>
                        <TableCell>{dayjs(row.modifiedDate).format('DD/MM/YYYY hh:ss:mm')}</TableCell>
                        <TableCell>{row.modifiedBy}</TableCell>
                        <TableCell sx={{display: 'flex', height: '55px'}}>
                            <Tooltip title="Chỉnh sửa" arrow>
                                <IconButton
                                    aria-label="edit"
                                    color="warning"
                                    size="small"
                                    sx={{ fontSize: 20 }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        props.openAddUpdateModal(row);
                                    }}
                                >
                                    <ModeEditOutlineOutlinedIcon fontSize="medium" />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title="Xóa" arrow>
                                <IconButton
                                    aria-label="edit"
                                    color="error"
                                    size="small"
                                    sx={{ fontSize: 2 }}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        props.openDeleteModal(row);
                                    }}
                                >
                                    <DeleteIcon fontSize="medium" />
                                </IconButton>
                            </Tooltip>
                        </TableCell>
                    </TableRow>
                )}
            />
        </>
    );
};

export default ListRoleProject;
