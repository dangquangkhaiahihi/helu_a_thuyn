import Card from '@mui/material/Card';
import Stack from '@mui/material/Stack';
import { useEffect, useState } from "react";
import ListIssue from "./list-issue/list-issue";
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import PageHeader from '@/components/pageHeader';
import Typography from '@mui/material/Typography';
import { Button, LinearProgress } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { Issue } from '@/common/DTO/Issue/IssueDTO';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import CustomModal from '@/components/modalCustom';
import IssueService from '@/api/instance/issue';
import { IssueQueryFilter } from '@/common/DTO/Issue/IssueQueryFilter';
import AddIssueModal from './add-issue/add-update-issue';
import FormSearchIssue from './search-issue';
import { useParams } from 'react-router-dom';

const IssueManagement = ({ }: any) => {
    const optionsType = [
        { value: '1', label: 'Type 1' },
        { value: '2', label: 'Type 2' },
        { value: '3', label: 'Type 3' },
        { value: '4', label: 'Type 4' },
        { value: '5', label: 'Type 5' },
        { value: '6', label: 'Type 6' }
    ];
    // const { modelId } = useParams();
   
    const [totalItemCount, setTotalItemCount] = useState<number>(0);
    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<Issue | null>(null);
    const [data, setData] = useState<Issue[]>([]);
    const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [order, setOrder] = useState<string>(DEFAULT_ORDER);
    const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
    const [rowsPerPage, setRowsPerPage] = useState<number>(DEFAULT_PAGE_SIZE);
    const [filterParam, setFilterParam] = useState<IssueQueryFilter>({
        id:"",
        name: "",
        type: "",
        status: "",
        description: "",
        createdBy: "",
        createdDate: "",
        modifiedBy: "",
        modifiedDate: "",
        modelId:"1",
        // reporter: "",
        // endDate: "",
        // assignee: "",
        // image: "",
    });

    useEffect(() => {
        handleFilterIssue();
    }, [])

    const handleFilterIssue = async (
        pageIndex = page,
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({ ...params });
        setIsLoadingFilter(true);
        try {
            
            const res: ApiResponse<PagedListContent<Issue>> = await IssueService.Filter(pageIndex, pageSize, sortExpression, params);
            setData(res.content.items);
            setTotalItemCount(res.content.totalItemCount);
        } catch (err) {
            console.error("handleFilterIssue error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };


    const handleGetLookup = async () => {
        try {
            // Fetch roles using Filter with required arguments
            const response = await IssueService.Filter(
                DEFAULT_PAGE_INDEX,          // pageIndex
                DEFAULT_PAGE_SIZE,           // pageSize
                DEFAULT_SORT_EXPRESSION,     // sortExpression
                null                         // search (assuming no filter is needed for lookup)
            );
            
            // Check if the response contains the expected structure
            if (response && response.content) {
                // Return mapped roles with value as a string
                return response.content.items.map(item => ({
                    label: item.createdBy,         // Role name
                    value: item.id.toString() // Convert ID to string
                }));
            } else {
                console.error('Unexpected response structure:', response);
                return [];
            }
        } catch (err) {
            console.error('Error fetching role data:', err);
            throw err;
        }
    };

    // const handleGetLookup = async (str: string) => {
    //     const defaultValues = {
    //         id:"",
    //         name: "",
    //         type: "",
    //         status: "",
    //         description: "",
    //         createdDate: "",
    //         createdBy: "",
    //         modifiedBy: "",
    //         modifiedDate: "",
    //         projectId:projectId??'',
    //         // reporter: "",
    //         // endDate: "",
    //         // assignee: "",
    //         // image: "",
    //     }

    //     try {
    //         const res: ApiResponse<PagedListContent<Issue>> = await IssueService.Filter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, defaultValues);
    //         return res.content.items.map(item => { return { label: item.name, value: item.id } })
    //     } catch (err) {
    //         throw err;
    //     }
    // };
    //delete issue by id
    const handleDeleteIssue = async (id: string | number) => {
        setIsLoadingFilter(true);
        try {
            await IssueService.Delete(id.toString());
            onCloseDeleteModal();
            handleFilterIssue();
        } catch (err) {
            console.error("handleDeleteIssue error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };
    const onOpenAddUpdateModal = (row: Issue) => {
        setIsOpenAddUpdate(true);
        setSelectedItem(row);
    }

    const onCloseAddUpdateModal = () => {
        setIsOpenAddUpdate(false);
        setSelectedItem(null);
    }

    const onOpenDeleteModal = (row: Issue) => {
        setIsOpenDelete(true);
        setSelectedItem(row);
    }

    const onCloseDeleteModal = () => {
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

                <Typography color="text.tertiary">Vấn đề</Typography>
            </Breadcrumbs>

            <PageHeader title="Danh sách vấn đề">
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
                <Card component="section">
                <LinearProgress color={"info"} sx={{opacity: isLoadingFilter ? 1 : 0}}/>
                <FormSearchIssue
                    handleFilter={handleFilterIssue}
                    optionsType={optionsType}
                    fetchOptionType_Async={handleGetLookup}
                />
                    <ListIssue
                        rows={data}
                        totalItemCount={totalItemCount}
                        handleFilterAction={handleFilterIssue}
                        openAddUpdateModal={onOpenAddUpdateModal}
                        openDeleteModal={onOpenDeleteModal}
                        setOrder={setOrder}
                        setOrderBy={setOrderBy}
                        setPage={setPage}
                        setRowsPerPage={setRowsPerPage}
                    />
                </Card>
            </Stack>
            <AddIssueModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilterIssue()}
                optionsType={optionsType}
            // fetchOptionType_Async={handleGetLookup}
            />
            <CustomModal
                title={"Xác nhận"}
                isOpen={isOpenDelete}
                onSave={() => { handleDeleteIssue(selectedItem?.id || "") }}
                onClose={onCloseDeleteModal}
                size='sm'
                titleClose='Hủy bỏ'
                titleSave='Xóa'
            >
                <Typography>Bạn có chắc muốn xóa vấn đề này không?</Typography>
            </CustomModal>

        </>


    );

}

export default IssueManagement;