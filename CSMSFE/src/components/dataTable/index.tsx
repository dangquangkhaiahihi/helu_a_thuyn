import React, { useState, MouseEvent, ChangeEvent, useEffect } from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import TableContainer, { TableContainerProps } from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TablePagination from '@mui/material/TablePagination';
import TableSortLabel from '@mui/material/TableSortLabel';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from '@/common/default-config';

interface HeadCell {
  id: string;
  numeric: boolean;
  sortable: boolean;
  label: string;
}

interface EnhancedTableHeadProps {
  order: 'asc' | 'desc';
  orderBy: string;
  onRequestSort: (event: MouseEvent<unknown>, property: string) => void;
  headCells: HeadCell[];
}

interface EnhancedTableProps {
  rows: any[];
  headCells: HeadCell[];
  render: (row: any, index: number) => React.ReactNode;
  stickyHeader?: boolean;
  tableContainerProps?: TableContainerProps;
  totalItemCount: number;
  handleFilterAction: (
    pageIndex?: number, 
    pageSize?: number, 
    sortExpression?: string, 
    searchParam?: any
  ) => void;
  
  setOrder: Function;
  setOrderBy: Function;
  setPage: Function;
  setRowsPerPage: Function;
}

// function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
//   if (b[orderBy] < a[orderBy]) {
//     return -1;
//   }
//   if (b[orderBy] > a[orderBy]) {
//     return 1;
//   }
//   return 0;
// }

// function getComparator<Key extends keyof any>(
//   order: 'asc' | 'desc',
//   orderBy: Key
// ): (a: { [key in Key]: number | string }, b: { [key in Key]: number | string }) => number {
//   return order === 'desc'
//     ? (a, b) => descendingComparator(a, b, orderBy)
//     : (a, b) => -descendingComparator(a, b, orderBy);
// }

const EnhancedTableHead: React.FC<EnhancedTableHeadProps> = ({ order, orderBy, onRequestSort, headCells }) => {
  const createSortHandler = (property: string, isSortable: boolean) => (event: MouseEvent<unknown>) => {
    if ( !isSortable ) return;
    onRequestSort(event, property);
  };

  return (
    <TableHead sx={{ bgcolor: 'background.default' }}>
      <TableRow>
        {headCells.map((headCell) => {
          
          return (
            <TableCell
              key={headCell.id}
              align={headCell.numeric ? 'right' : 'left'}
              sortDirection={orderBy === headCell.id ? order : false}
            >
              <TableSortLabel
                active={orderBy === headCell.id}
                direction={headCell.sortable && orderBy === headCell.id ? order : 'asc'}
                onClick={createSortHandler(headCell.id, headCell.sortable)}
              >
                {headCell.label}
              </TableSortLabel>
            </TableCell>
          )
        })}
      </TableRow>
    </TableHead>
  );
};

const EnhancedTable: React.FC<EnhancedTableProps> = (props) => {
  const {
    rows,
    headCells,
    render,
    stickyHeader,
    tableContainerProps,
    totalItemCount,
    handleFilterAction,
  } = props;
  const [order, setOrder] = useState<'asc' | 'desc'>(DEFAULT_ORDER);
  const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
  const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
  const [rowsPerPage, setRowsPerPage] = useState<number>(DEFAULT_PAGE_SIZE);

  useEffect(() => { props.setOrder(order); }, [order])
  useEffect(() => { props.setOrderBy(orderBy); }, [orderBy])
  useEffect(() => { props.setPage(page) }, [page])
  useEffect(() => { props.setRowsPerPage(rowsPerPage) }, [rowsPerPage])

  const handleRequestSort = (_event: MouseEvent<unknown>, property: string) => {
    const isAsc = orderBy === property && order === 'asc';
    let sort = isAsc ? 'desc' as const : 'asc' as const;

    setOrder(sort);
    setOrderBy(property);

    handleFilterAction(page + 1, rowsPerPage, `${property} ${sort}`);
  };

  const handleChangePage = (_event: unknown, newPage: number) => {
    console.log("handleChangePagehandleChangePage", newPage);
    
    setPage(newPage + 1);

    handleFilterAction(newPage + 1, rowsPerPage, `${orderBy} ${order}`);
  };

  const handleChangeRowsPerPage = (event: ChangeEvent<HTMLInputElement>) => {
    const val: number = parseInt(event.target.value, 10);
    setRowsPerPage(val);
    setPage(DEFAULT_PAGE_INDEX);

    handleFilterAction(DEFAULT_PAGE_INDEX, val, `${orderBy} ${order}`);
  };

  return (
    <>
      <TableContainer {...tableContainerProps}>
        <Table
          sx={{ width: '100%' }}
          stickyHeader={stickyHeader}
          aria-labelledby="tableTitle"
          size={'small'}
        >
          <EnhancedTableHead
            order={order}
            orderBy={orderBy}
            onRequestSort={handleRequestSort}
            headCells={headCells}
          />
          <TableBody>
            {rows.map((row, i) => render(row, i))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        showFirstButton
        showLastButton
        rowsPerPageOptions={[5, 10, 25, 50]}
        component="div"
        count={totalItemCount}
        rowsPerPage={rowsPerPage}
        page={page - 1}
        onPageChange={handleChangePage}
        onRowsPerPageChange={handleChangeRowsPerPage}
        labelRowsPerPage="Số bản ghi mỗi trang : "
      />
    </>
  );
};

export default EnhancedTable;