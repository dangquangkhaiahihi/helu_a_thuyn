import { useEffect, useState } from 'react';
// import { useParams } from 'react-router-dom';

import ListProject from './list-project';

import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX } from '@common/default-config';
import { Project } from '@/common/DTO/Project/ProjectDTO';
import { ProjectQueryFilter } from '@/common/DTO/Project/ProjectQueryFilter';
import ProjectService from '@/api/instance/project';
import PageHeaderFormFilter from './page-header-form-filter';
import AddUpdateProjectModal from './add-update-project';
import { UrlRouteCollection } from '@/common/url-route-collection';

const ProjectManagement = ({  }: any) => {
    // const { projectId } = useParams();

    // START SET UP FOR PAGINATION
    const [isFirstLoad, setIsFirstLoad] = useState<boolean>(true);
    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);

    const [data, setData] = useState<Project[]>([]);
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

    const resetToDefaultPagination = () => {
        setOrder(DEFAULT_ORDER);
        setOrderBy(DEFAULT_ORDER_BY);
        setPage(DEFAULT_PAGE_INDEX);
        setRowsPerPage(6);
    }
    
    useEffect(() => {
        setIsFirstLoad(false);
        handleFilter();
    }, [])

    // END SET UP FOR PAGINATION

    const [isOpenAddUpdate, setIsOpenAddUpdate] = useState<boolean>(false);
    // const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [selectedItem, setSelectedItem] = useState<Project | null>(null);
    const [filterParam, setFilterParam] = useState<ProjectQueryFilter>({
        Name: "",
        Type: "",
    });

    const handleFilter = async (
        pageIndex = page, 
        pageSize = rowsPerPage,
        sortExpression = `${orderBy} ${order}`,
        params = filterParam,
    ) => {
        setFilterParam({...params});
        setIsLoadingFilter(true);
        console.log("run this", params);
        
        try {
            const res: ApiResponse<PagedListContent<Project>> = await ProjectService.Filter(pageIndex, pageSize, sortExpression, params);
            setData(res.content.items);
            setTotalItemCount(res.content.totalItemCount);
        } catch ( err ) {
            console.error("handleFilter error", err);
        } finally {
            setIsLoadingFilter(false);
        }
    };

    // const handleDelete = async (id: string | number) => {
    //     setIsLoadingFilter(true);
    //     try {
    //         await ProjectService.Delete(id.toString());
    //         onCloseDeleteModal();
    //         handleFilter();
    //     } catch ( err ) {
    //         console.error("handleDelete error", err);
    //     } finally {
    //         setIsLoadingFilter(false);
    //     }
    // };

    // const onOpenAddUpdateModal  = (item: Project) => {
    //     setIsOpenAddUpdate(true);
    //     setSelectedItem(item);
    // }

    const onCloseAddUpdateModal  = () => {
        setIsOpenAddUpdate(false);
        setSelectedItem(null);
    }

    // const onOpenDeleteModal  = (row: Project) => {
    //     setIsOpenDelete(true);
    //     setSelectedItem(row);
    // }

    // const onCloseDeleteModal  = () => {
    //     setIsOpenDelete(false);
    //     setSelectedItem(null);
    // }
      
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

                <Typography color="text.tertiary">Danh sách dự án</Typography>
            </Breadcrumbs>

            <PageHeaderFormFilter
                handleFilter={handleFilter}
                setIsOpenAddUpdate={setIsOpenAddUpdate}
                resetToDefaultPagination={resetToDefaultPagination}
            />

            <ListProject
                isLoading={isLoadingFilter}
                data={data}
                totalItemCount={totalItemCount}
                // Paginate
                order={order}
                orderBy={orderBy}
                page={page}
                rowsPerPage={rowsPerPage}
                setOrder={setOrder}
                setOrderBy={setOrderBy}
                setPage={setPage}
                setRowsPerPage={setRowsPerPage}
            />

            <AddUpdateProjectModal
                isOpen={isOpenAddUpdate}
                selectedItem={selectedItem}
                onClose={onCloseAddUpdateModal}
                onSuccess={() => handleFilter()}
            />
		</>
	);
}

export default ProjectManagement;