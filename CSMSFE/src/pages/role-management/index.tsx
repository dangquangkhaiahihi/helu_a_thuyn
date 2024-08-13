import { useEffect, useState } from 'react';
import Card from '@mui/material/Card';
import PageHeader from '@/components/pageHeader';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { Role } from '@/common/DTO/Role/RoleDTO';
import { Button, LinearProgress } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import ListRole from './list-role';
import CustomModal from '@/components/modalCustom';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from '@common/default-config';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import RoleService from '@/api/instance/role';
import { RoleQueryFilter } from '@/common/DTO/Role/RoleQueryFilter';
import AddUpdateRoleModal from './add-update-role';
import FormSearchRole from './form-search-role';
import UpdateSecurityMatrixRoleModal from './update-security-matrix-role';
import { UrlRouteCollection } from '@/common/url-route-collection';

const RoleManagement = () => {

    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const [data, setData] = useState<Role[]>([]);
    const [order, setOrder] = useState<string>(DEFAULT_ORDER);
    const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
    const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
    const [rowsPerPage, setRowsPerPage] = useState<number>(DEFAULT_PAGE_SIZE);
    const [totalItemCount, setTotalItemCount] = useState<number>(0);
    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [isOpenUpdateSecurityMatrix, setIsOpenUpdateSecurityMatrix] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<Role | null>(null);
    const [filterParam, setFilterParam] = useState<RoleQueryFilter>({});

    useEffect(() => {
        handleFilterRole();
    }, []);

    const handleFilterRole = async (
        pageIndex = page,
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({ ...params });
        setIsLoadingFilter(true);
        try {
            const response: ApiResponse<PagedListContent<Role>> = await RoleService.Filter(pageIndex, pageSize, sortExpression, params);
            setData(response.content.items);
            setTotalItemCount(response.content.totalItemCount);
        } catch (err) {
            console.error("handleFilterRole error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };
   
    const handleDeleteRole = async (id: string | number) => {
        setIsLoadingFilter(true);
        try {
            await RoleService.Delete(id.toString());
            onCloseDeleteModal();
            handleFilterRole();
        } catch (err) {
            console.error("handleDeleteRole error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const onOpenAddUpdateModal = (row: Role | null) => {
        setIsOpenAddUpdate(true);
        setSelectedItem(row);
    };

    const onCloseAddUpdateModal = () => {
        setIsOpenAddUpdate(false);
        setSelectedItem(null);
    };

    const onOpenDeleteModal = (row: Role) => {
        setIsOpenDelete(true);
        setSelectedItem(row);
    };

    const onCloseDeleteModal = () => {
        setIsOpenDelete(false);
        setSelectedItem(null);
    };

    const onOpenUpdateSecurityMatrixModal = (row: Role | null) => {
        setIsOpenUpdateSecurityMatrix(true);
        setSelectedItem(row);
    };

    const onCloseUpdateSecurityMatrixModal = () => {
        setIsOpenUpdateSecurityMatrix(false);
        setSelectedItem(null);
    };

    return (
        <>
            <Breadcrumbs
                aria-label="breadcrumb"
                sx={{
                    marginTop: '15px',
                    textTransform: 'uppercase',
                }}
            >
                <Link underline="hover" href={UrlRouteCollection.Home}>
                    Trang chủ
                </Link>
                <Typography color="text.tertiary">Quản lý chức vụ</Typography>
            </Breadcrumbs>

            <PageHeader title="Quản lý chức vụ">
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
            </PageHeader>

            <Stack spacing={5}>
                <FormSearchRole
                    handleFilter={handleFilterRole}
                />

                <Card component="section">
                    <LinearProgress color={"info"} sx={{ opacity: isLoadingFilter ? 1 : 0 }} />
                    <ListRole
                        rows={data}
                        totalItemCount={totalItemCount}
                        handleFilterAction={handleFilterRole}
                        openAddUpdateModal={onOpenAddUpdateModal}
                        openDeleteModal={onOpenDeleteModal}
                        onOpenUpdateSecurityMatrixModal={onOpenUpdateSecurityMatrixModal}
                        // Phân trang
                        setOrder={setOrder}
                        setOrderBy={setOrderBy}
                        setPage={setPage}
                        setRowsPerPage={setRowsPerPage}
                    />
                </Card>
            </Stack>
            <AddUpdateRoleModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilterRole()}
            />

            <UpdateSecurityMatrixRoleModal
                isOpen={isOpenUpdateSecurityMatrix}
                selectedItem={selectedItem}
                onClose={onCloseUpdateSecurityMatrixModal}
                onSuccess={() => handleFilterRole()}
            />

            <CustomModal
                title={"Xác nhận"}
                isOpen={isOpenDelete}
                onSave={() => { handleDeleteRole(selectedItem?.id || "") }}
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

export default RoleManagement;
