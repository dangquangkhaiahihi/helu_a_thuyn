import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Button, Card, Grid, LinearProgress, Stack, Typography } from '@mui/material';
import PermissionProjectService from '@/api/instance/permission-project';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import { RoleProject } from '@/common/DTO/RoleProjectDTO/RoleProjectDTO';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX } from '@/common/default-config';
import ListRoleProject from './list-role-project';
import AddIcon from '@mui/icons-material/Add';
import FormSearchRoleProject from './form-search-role-project';
import AddUpdateRoleProjectModal from './add-update-role-project';
import CustomModal from '@/components/modalCustom';

const ProjectSettingRole = ({  }: any) => {
    const { projectId } = useParams();
    // const dispatch = useDispatch();

    // START SET UP FOR PAGINATION
    const [isFirstLoad, setIsFirstLoad] = useState<boolean>(true);
    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);

    const [data, setData] = useState<RoleProject[]>([]);
    const [order, setOrder] = useState<string>(DEFAULT_ORDER);
    const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
    
    const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
    const [rowsPerPage, setRowsPerPage] = useState<number>(6);

    const [totalItemCount, setTotalItemCount] = useState<number>(DEFAULT_PAGE_INDEX);

    useEffect(() => {
        if ( isFirstLoad ) return;
        handleFilter(page, rowsPerPage, `${orderBy} ${order}`);
    }, [order, orderBy, page])

    useEffect(() => {
        if ( isFirstLoad ) return;
        handleFilter(DEFAULT_PAGE_INDEX, rowsPerPage, `${orderBy} ${order}`);
    }, [rowsPerPage])
    
    useEffect(() => {
        setIsFirstLoad(false);
        handleFilter();
    }, [])

    // END SET UP FOR PAGINATION
    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<RoleProject | null>(null);
    const [filterParam, setFilterParam] = useState<{
        name: string,
        code: string,
    }>({
        name: "",
        code: "",
    });

    const handleFilter = async (
        pageIndex = page, 
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({...params});
        setIsLoadingFilter(true);
        
        try {
            const res: ApiResponse<PagedListContent<RoleProject>> = await PermissionProjectService.FilterRoleByProjectId(projectId, pageIndex, pageSize, sortExpression, params);
            setData(res.content.items);
            setTotalItemCount(res.content.totalItemCount);
        } catch ( err ) {
            console.error("handleFilter error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const handleDeleteRoleProject = async (id: string) => {
        setIsLoadingFilter(true);
        try {
            await PermissionProjectService.Delete(id);
            onCloseDeleteModal();
            handleFilter();
        } catch (err) {
            console.error("handleDeleteRole error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const onOpenAddUpdateModal = (row: RoleProject | null) => {
        setIsOpenAddUpdate(true);
        setSelectedItem(row);
    };

    const onCloseAddUpdateModal = () => {
        setIsOpenAddUpdate(false);
        setSelectedItem(null);
    };

    const onOpenDeleteModal = (row: RoleProject) => {
        setIsOpenDelete(true);
        setSelectedItem(row);
    };

    const onCloseDeleteModal = () => {
        setIsOpenDelete(false);
        setSelectedItem(null);
    };

	return (
        <Card variant='outlinedElevation'>
            <Grid container
                direction="row"
                alignItems="start"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                maxWidth={"100%"}
            >
                <Grid item xs={12} md={6}>
                    <Typography variant="h3" color="primary" mb={3}>
                        Chức vụ trong dự án
                    </Typography>
                </Grid>
                <Grid item xs={12} md={6} display={"flex"} justifyContent={"flex-end"}>
                    <Button
                        type="button"
                        variant="contained"
                        disableElevation
                        startIcon={<AddIcon />}
                        color='primary'
                        onClick={(e) => {
                            e.preventDefault();
                            onOpenAddUpdateModal(null);
                        }}
                    >
                        Thêm mới
                    </Button>
                </Grid>
            </Grid>

            <Stack spacing={5}>
                <FormSearchRoleProject
                    handleFilter={handleFilter}
                />

                <Card component="section">
                    <LinearProgress color={"info"} sx={{ opacity: isLoadingFilter ? 1 : 0 }} />
                    <ListRoleProject
                        rows={data}
                        totalItemCount={totalItemCount}
                        handleFilterAction={handleFilter}
                        openAddUpdateModal={onOpenAddUpdateModal}
                        openDeleteModal={onOpenDeleteModal}
                        // Phân trang
                        setOrder={setOrder}
                        setOrderBy={setOrderBy}
                        setPage={setPage}
                        setRowsPerPage={setRowsPerPage}
                    />
                </Card>
            </Stack>

            <AddUpdateRoleProjectModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilter()}
            />

            <CustomModal
                title={"Xác nhận"}
                isOpen={isOpenDelete}
                onSave={() => { handleDeleteRoleProject(selectedItem?.id || "") }}
                onClose={onCloseDeleteModal}
                size='sm'
                titleClose='Hủy bỏ'
                titleSave='Xóa'
            >
                <Typography>Bạn có chắc muốn xóa bản ghi này không?</Typography>
            </CustomModal>
        </Card>
	);
}

export default ProjectSettingRole;