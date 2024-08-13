import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';
import PersonOffOutlinedIcon from '@mui/icons-material/PersonOffOutlined';
import ModeEditOutlineOutlinedIcon from '@mui/icons-material/ModeEditOutlineOutlined';
import DataTable from '@/components/dataTable';
import headCellListIssue from './head-cell-list-issue';
import { Link } from 'react-router-dom';
import {  } from 'react-router-dom';
import dayjs from 'dayjs';
const ListIssue = (props : any) => {
	
	return (
		<DataTable
            {...props}
            headCells={headCellListIssue}
            render={(row: any) => (
                    <TableRow hover tabIndex={-1} key={row.id}>
                    <TableCell ><Link  to={`${row.id}`} state={row} style={{ textDecoration: 'none', color: 'blue' }}>{row?.name}</Link></TableCell>
                    <TableCell >{row?.type}</TableCell>
                    <TableCell >{row?.status}</TableCell>
                    <TableCell >{row?.description}</TableCell>
                    <TableCell >{row?.createdBy}</TableCell>
                    <TableCell >{dayjs(row?.createdDate).format("DD/MM/YYYY")}</TableCell>                  
                    <TableCell >{row?.modifiedBy}</TableCell>
                    <TableCell >{dayjs(row?.modifiedDate).format("DD/MM/YYYY")}</TableCell>
                    <TableCell align="center">
                        <Tooltip title="Chỉnh sửa" arrow>
                            <IconButton
                                aria-label="edit"
                                color="warning"
                                size="small"
                                sx={{ fontSize: 2 , display: 'inline-table', marginRight: '5px'}}
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
                                sx={{ fontSize: 2, display: 'inline-table', marginRight: '5px'}}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    props.openDeleteModal(row);
                                }}
                            >
                                <PersonOffOutlinedIcon fontSize="medium" />
                            </IconButton>
                        </Tooltip>
                    </TableCell>
                </TableRow>
            )}
        />
	);
	
}

export default ListIssue;