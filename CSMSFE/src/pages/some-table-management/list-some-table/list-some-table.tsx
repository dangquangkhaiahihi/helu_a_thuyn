import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';

import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';

import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import DeleteIcon from '@mui/icons-material/Delete';

import DataTable from '@/components/dataTable';
import headCellSomeTable from './head-cell-some-table';
import { SomeTable } from '@/common/DTO/SomeTable/SomeTableDTO';
import dayjs from 'dayjs';

const ListSomeTable = ( props : any) => {

	return (
		<DataTable
            {...props}
            headCells={headCellSomeTable}
            render={(row: SomeTable, index) => (
                <TableRow hover tabIndex={-1} key={row.id}>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>{row.email}</TableCell>
                    <TableCell>{row.normalText}</TableCell>
                    <TableCell>{row.phoneNumber}</TableCell>
                    <TableCell>{dayjs(row.startDate).format("DD/MM/YYYY")}</TableCell>
                    <TableCell>{dayjs(row.endDate).format("DD/MM/YYYY")}</TableCell>
                    <TableCell>{row.status ? "true" : "false"}</TableCell>
                    <TableCell>{row.type}</TableCell>
                    <TableCell>{row.createdBy}</TableCell>
                    <TableCell>{dayjs(row.createdDate).format("DD/MM/YYYY")}</TableCell>
                    <TableCell>{row.modifiedBy}</TableCell>
                    <TableCell>{dayjs(row.modifiedDate).format("DD/MM/YYYY")}</TableCell>
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
	);
}

export default ListSomeTable;