import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import ListModel from './list-model';

import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import { DEFAULT_ORDER, DEFAULT_ORDER_BY, DEFAULT_PAGE_INDEX } from '@common/default-config';
import ModelService from '@/api/instance/model';
import PageHeaderFormFilter from './components/page-header-form-filter';
import { Model } from '@/common/DTO/Model/ModelDTO';
import { ModelQueryFilter } from '@/common/DTO/Model/ModelQueryFilter';
import { UrlRouteCollection } from '@/common/url-route-collection';
import { useDispatch, useSelector } from 'react-redux';
import { fetchProjectInfo, projectInfoRedux } from '@/store/redux/project';

const ModelManagement = ({  }: any) => {
    const { projectId } = useParams();
    const dispatch = useDispatch();
    const projectInfo = useSelector(projectInfoRedux);

    useEffect(() => {
        if ( projectInfo ) return;
        dispatch(fetchProjectInfo(projectId || ""));
    }, [])

    // END SET UP FOR PAGINATION
    const [isFirstLoad, setIsFirstLoad] = useState<boolean>(true);
    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);

    const [data, setData] = useState<Model[]>([]);
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

    const [isOpenAdd, setIsOpenAdd] = useState<boolean>(false);
    // const [isOpenDelete, setIsOpenDelete] = useState<boolean>(false);
    const [filterParam, setFilterParam] = useState<ModelQueryFilter>({
        Name: "",
        CreatedBy: "",
        Status: ""
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
            const res: ApiResponse<PagedListContent<Model>> = await ModelService.FilterModelByProjectId(projectId, pageIndex, pageSize, sortExpression, params);
            setData(res.content.items);
            setTotalItemCount(res.content.totalItemCount);
        } catch ( err ) {
            console.error("handleFilter error", err);
        } finally {
            setIsLoadingFilter(false);
        }
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

                <Link underline="hover" href={UrlRouteCollection.ProjectManagement}>
                    Danh sách dự án
                </Link>
                <Typography color="text.tertiary">{projectInfo?.name}</Typography>
                <Typography color="text.tertiary">Danh sách mô hình</Typography>
            </Breadcrumbs>

            <PageHeaderFormFilter
                handleFilter={handleFilter}
                setIsOpenAdd={setIsOpenAdd}
                resetToDefaultPagination={resetToDefaultPagination}
            />

            <ListModel
                isLoading={isLoadingFilter}
                data={data}
                setData={setData}
                totalItemCount={totalItemCount}
                isOpenAddFromProps={isOpenAdd}
                setIsOpenAddFromProps={setIsOpenAdd}
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
		</>
	);
}

export default ModelManagement;