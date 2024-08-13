import { useEffect, useState } from 'react';
import Card from '@mui/material/Card';
import PageHeader from '@/components/pageHeader';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import { User } from '@/common/DTO/User/UserDTO';
import { Button, LinearProgress } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import AddUpdateUserModal from './add-update-user';
import ListUser from './list-user';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE } from '@common/default-config';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import UserService from '@/api/instance/user';
import { UserQueryFilter } from '@/common/DTO/User/UserQueryFilter';
import FormSearchUser from './form-search-user';
import { UrlRouteCollection } from '@/common/url-route-collection';

const UserManagement = () => {
    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const [data, setData] = useState<User[]>([]);
    const [order, setOrder] = useState<string>(DEFAULT_ORDER);
    const [orderBy, setOrderBy] = useState<string>(DEFAULT_ORDER_BY);
    const [page, setPage] = useState<number>(DEFAULT_PAGE_INDEX);
    const [rowsPerPage, setRowsPerPage] = useState<number>(DEFAULT_PAGE_SIZE);
    const [totalItemCount, setTotalItemCount] = useState<number>(0);
    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<User | null>(null);
    const [filterParam, setFilterParam] = useState<UserQueryFilter>({
        fullName: "",      
        email: "",         
        phoneNumber: "",   
    });

    useEffect(() => {
        handleFilterUser();
    }, []);

    const handleFilterUser = async (
        pageIndex = page,
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({ ...params });
        setIsLoadingFilter(true);
        try {
            const user: ApiResponse<PagedListContent<User>> = await UserService.Filter(pageIndex, pageSize, sortExpression, params);
            setData(user.content.items);
            setTotalItemCount(user.content.totalItemCount);
        } catch (err) {
            console.error("handleFilterUser error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    const onOpenAddUpdateModal = (row: User | null) => {
        setIsOpenAddUpdate(true);
        setSelectedItem(row);
    };

    const onCloseAddUpdateModal = () => {
        setIsOpenAddUpdate(false);
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

                <Typography color="text.tertiary">Quản lý người dùng</Typography>
            </Breadcrumbs>

            <PageHeader title="Quản lý người dùng">
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
                <FormSearchUser
                    handleFilter={handleFilterUser}
                />

                <Card component="section">
                    <LinearProgress color={"info"} sx={{ opacity: isLoadingFilter ? 1 : 0 }} />
                    <ListUser
                        rows={data}
                        totalItemCount={totalItemCount}
                        handleFilterAction={handleFilterUser}
                        openAddUpdateModal={onOpenAddUpdateModal}
                        // Paginate
                        setOrder={setOrder}
                        setOrderBy={setOrderBy}
                        setPage={setPage}
                        setRowsPerPage={setRowsPerPage}
                    />
                </Card>
            </Stack>

            <AddUpdateUserModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilterUser()}
            />
        </>
    );
}

export default UserManagement;
