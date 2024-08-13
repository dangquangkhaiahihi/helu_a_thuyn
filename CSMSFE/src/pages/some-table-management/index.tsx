import { useEffect, useState } from 'react';
import Card from '@mui/material/Card';

import ListSomeTable from './list-some-table/list-some-table';

import PageHeader from '@/components/pageHeader';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { SomeTable } from '@/common/DTO/SomeTable/SomeTableDTO';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@common/default-config';
import FormSearchSomeTable from './form-search-some-table/form-search-some-table';
import SomeTableService from '@/api/instance/some-table';
// import { useDispatch } from '@/store';
// import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { Button, LinearProgress } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import AddUpdateSomeTableModal from './add-update-some-table';
import CustomModal from '@/components/modalCustom';
import { SomeTableQueryFilter } from '@/common/DTO/SomeTable/SomeTableQueryFilter';

const SomeTableManagement = ({  }: any) => {

    const optionsType = [
        {value: '1', label: 'Type 1'},
        {value: '2', label: 'Type 2'},
        {value: '3', label: 'Type 3'},
        {value: '4', label: 'Type 4'},
        {value: '5', label: 'Type 5'},
        {value: '6', label: 'Type 6'}
    ];

    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const [data, setData] = useState<SomeTable[]>([]);
    const [order, setOrder] = useState<string>(DEFAULT_ORDER);
    const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
    
    const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
    const [rowsPerPage, setRowsPerPage] = useState<number>(DEFAULT_PAGE_SIZE);

    const [totalItemCount, setTotalItemCount] = useState<number>(DEFAULT_PAGE_INDEX);
    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<SomeTable | null>(null);
    const [ filterParam, setFilterParam] = useState<SomeTableQueryFilter>({
        NormalText: "",
        PhoneNumber: "",
        Email: "",
        StartDate: "",
        EndDate: "",
        Status: "",
        Type: ""
    });

    useEffect(() => {
        handleFilterSomeTable();
    }, [])

    const handleFilterSomeTable = async (
        pageIndex = page, 
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({...params});
        setIsLoadingFilter(true);
        try {
            const res: ApiResponse<PagedListContent<SomeTable>> = await SomeTableService.Filter(pageIndex, pageSize, sortExpression, params);
            setData(res.content.items);
            setTotalItemCount(res.content.totalItemCount);
        } catch ( err ) {
            console.error("handleFilterSomeTable error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const handleGetLookup = async (str: string) => {
        const defaultValues = {
			NormalText: str,
            PhoneNumber: "",
            Email: "",
            StartDate: "",
            EndDate: "",
            Status: "",
            Type: ""
		}

        try {
            const res: ApiResponse<PagedListContent<SomeTable>> = await SomeTableService.Filter(1, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, defaultValues);
            return res.content.items.map(item => {return {label: item.normalText, value: item.id}})
        } catch ( err ) {
            throw err;
        }
    };

    const handleDeleteSomeTable = async (id: string | number) => {
        setIsLoadingFilter(true);
        try {
            await SomeTableService.Delete(id.toString());
            onCloseDeleteModal();
            handleFilterSomeTable();
        } catch ( err ) {
            console.error("handleDeleteSomeTable error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const onOpenAddUpdateModal  = (row: SomeTable) => {
        setIsOpenAddUpdate(true);
        setSelectedItem(row);
    }

    const onCloseAddUpdateModal  = () => {
        setIsOpenAddUpdate(false);
        setSelectedItem(null);
    }

    const onOpenDeleteModal  = (row: SomeTable) => {
        setIsOpenDelete(true);
        setSelectedItem(row);
    }

    const onCloseDeleteModal  = () => {
        setIsOpenDelete(false);
        setSelectedItem(null);
    }
      
	return (
        <>
            <Breadcrumbs
                aria-label="breadcrumb"
                sx={{
                    marginTop: '15px',
                    textTransform: 'uppercase',
                }}
            >
                <Link underline="hover" href="#!">
                    Trang chủ
                </Link>

                <Typography color="text.tertiary">Some Table</Typography>
            </Breadcrumbs>

            <PageHeader title="Quản lý some table">
				{/* <Button variant="contained">Button Action</Button> */}
                <Button
                    type="button"
                    variant="contained"
                    disableElevation
                    startIcon={<AddIcon />}
                    color='primary'
                    onClick={(e) => {
                        e.preventDefault();
                        setIsOpenAddUpdate((prev: any) => !prev);
                    }}
                >
                    Thêm mới
                </Button>
			</PageHeader>

			<Stack spacing={5}>
                <FormSearchSomeTable
                    handleFilter={handleFilterSomeTable}
                    optionsType={optionsType}
                    fetchOptionType_Async={handleGetLookup}
                />

                <Card component="section">
                    <LinearProgress color={"info"} sx={{opacity: isLoadingFilter ? 1 : 0}}/>
                    <ListSomeTable
                        rows={data}
                        totalItemCount={totalItemCount}
                        handleFilterAction={handleFilterSomeTable}
                        openAddUpdateModal={onOpenAddUpdateModal}
                        openDeleteModal={onOpenDeleteModal}
                        // Paginate
                        setOrder={setOrder}
                        setOrderBy={setOrderBy}
                        setPage={setPage}
                        setRowsPerPage={setRowsPerPage}
                    />
                </Card>
			</Stack>

            <AddUpdateSomeTableModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilterSomeTable()}
                optionsType={optionsType}
                fetchOptionType_Async={handleGetLookup}
            />
            
            <CustomModal
                title={"Xác nhận"}
                isOpen={isOpenDelete}
                onSave={() => {handleDeleteSomeTable(selectedItem?.id || "")}}
                onClose={onCloseDeleteModal}
                size='sm'
                titleClose='Hủy bỏ'
                titleSave='Xóa'
            >
                <Typography>Bạn có chắc muốn xóa bản ghi này không?</Typography>
            </CustomModal>
		</>
	);
}

export default SomeTableManagement;