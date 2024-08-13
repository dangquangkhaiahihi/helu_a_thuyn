
import React, { ChangeEvent, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';

import IconButton from '@mui/material/IconButton';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import ViewInArIcon from '@mui/icons-material/ViewInAr';
import PanToolAltOutlinedIcon from '@mui/icons-material/PanToolAltOutlined';

import { Model } from '@/common/DTO/Model/ModelDTO';
import { Button, FormControl, Grid, LinearProgress, MenuItem, Select, SelectChangeEvent, Stack, TablePagination, Typography } from '@mui/material';

import { DEFAULT_PAGE_INDEX  } from '@/common/default-config';
import AddModelModal from '../add-model';
import { addNode, deleteNode, flattenTree, moveNode, organizeDataIntoTree, updateNode, updateTreeAfterUploadModel } from '../components/helper/helper';
import UpdateModelModal from '../update-model';
import DeleteModelModal from '../delete-model';
import { ApiResponse, PagedListContent } from '@/common/DTO/ApiResponse';
import ModelService from '@/api/instance/model';
import ModelItem from '../components/model-item';
import { useDispatch } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import { ModelMoveDTO } from '@/common/DTO/Model/ModelMoveDTO';
import { enqueueSnackbar } from 'notistack';
import { ModelUploadDTO } from '@/common/DTO/Model/ModelUploadDTO';
import { UrlRouteCollection } from '@/common/url-route-collection';

interface IListModelProps {
    data: Model[];
    setData: Function;
    totalItemCount: number;
    isOpenAddFromProps: boolean;
    setIsOpenAddFromProps: Function;
    //
    order: string;
    orderBy: string;
    page: number;
    rowsPerPage: number;
    setOrder: Function;
    setOrderBy: Function;
    setPage: Function;
    setRowsPerPage: Function;
    isLoading: boolean;
}

const ListModel: React.FC<IListModelProps>  = (props) => {
    const { projectId = "" } = useParams();

    const {
        data,
        setData,
        totalItemCount,
        isLoading,
        isOpenAddFromProps,
        setIsOpenAddFromProps,
        //
        order,
        orderBy,
        page,
        rowsPerPage,
        setOrder,
        setOrderBy,
        setPage,
        setRowsPerPage,
    } = props;

    const [treeData, setTreeData] = useState<Model[]>([]);
    const [dataTemp, setDataTemp] = useState<Model[]>([]);

    useEffect(() => {
        const convertDataToTreeData = async () => {
            const result = await organizeDataIntoTree(data);
            setTreeData(result);
        }
        convertDataToTreeData();
        setDataTemp(data);
    }, [data])

    useEffect(() => {
        const convertTreeDataToFlatData = async () => {
            const result = await flattenTree(treeData);
            setDataTemp(result);
        }
        convertTreeDataToFlatData();
    }, [treeData])

    const handleGetDirectChildren = async ( parentId: string ) => {
        try {
            const res: ApiResponse<PagedListContent<Model>> = await ModelService.GetDirectChildren(parentId, null);
            if ( res.content.items && Array.isArray(res.content.items) )
            setData([...data, ...res.content.items]);
        } catch ( err ) {
            console.error("handleGetDirectChildren error", err);
        }
    };

    const handleChangeOrder = (orderVal: 'asc' | 'desc') => {
        setOrder(orderVal)
    };

    const handleChangeOrderBy = (event: SelectChangeEvent) => {
        const orderByVal = event.target.value as string;
        setOrderBy(orderByVal);
    };
    
    const handleChangePage = (_event: unknown, newPage: number) => {
        setPage(newPage + 1);
    
    };
    
    const handleChangeRowsPerPage = (event: ChangeEvent<HTMLInputElement>) => {
        const val: number = parseInt(event.target.value, 10);
        setRowsPerPage(val);
        setPage(DEFAULT_PAGE_INDEX);
    };

    // Modal functionality
    const [isOpenAdd, setIsOpenAdd] = useState(false);
    useEffect(() => {setIsOpenAdd(isOpenAddFromProps)}, [isOpenAddFromProps])
    const [isOpenUpdate, setIsOpenUpdate] = useState(false);
    const [isOpenDelete, setIsOpenDelete] = useState(false);
    const [isOpenMove, setIsOpenMove] = useState(false);

    const [selectedItem, setSelectedItem] = useState<Model | null>(null);

    const [beingMovedItem, setBeingMovedItem] = useState<Model | null>(null);
    const dispatch = useDispatch();
    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }
    const handleMoveModel = async ( destinationId: string = "") => {
        setLoadingCircularOverlay(true);
        try {
            const moveData: ModelMoveDTO = {
                id: beingMovedItem?.id || "",
                newParentId: destinationId
            }
            await ModelService.Move(moveData);
            enqueueSnackbar('Di chuyển mô hình thành công.', {
                variant: 'success'
            });

            // Thực hiện thay đổi trên FE
            if ( !beingMovedItem ) return;
            const newTreeData = await moveNode(dataTemp, beingMovedItem.id, destinationId)
            setTreeData(newTreeData);
            setIsOpenMove(false);
            setSelectedItem(null);
        } catch ( err ) {
            console.error("handleRemoveModel error", err);
        } finally {
            setLoadingCircularOverlay(false);
        }
    };

    const handleUploadFileToModel = async ( modelId: string, file: File ) => {
        setLoadingCircularOverlay(true);
        try {
            const uploadData: ModelUploadDTO = {
                modelId: modelId,
                file: file
            }
            
            await ModelService.UploadFile(uploadData);
            enqueueSnackbar('Tải lên mô hình thành công.', {
                variant: 'success'
            });

            // Thực hiện thay đổi trên FE
            const newTreeData = await updateTreeAfterUploadModel(dataTemp, modelId);
            setTreeData(newTreeData);
        } catch ( err ) {
            console.error("handleRemoveModel error", err);
        } finally {
            setLoadingCircularOverlay(false);
        }
    };

    const [selectedModels, setSelectedModels] = useState<string[]>([]);

	return (
        <Stack spacing={1}>
            <FormControl sx={{ m: 1, minWidth: 120 }} size="small">
                <Stack spacing={1} direction={"row"} justifyContent={"flex-end"}>
                    <Typography gutterBottom variant="subtitle1" component="div" sx={{
                        display: 'flex',
                        alignItems: 'center',
                    }}>
                        Sắp xếp
                    </Typography>
                    
                    <IconButton onClick={() => {handleChangeOrder(order === 'asc' ? 'desc' : 'asc')}}>
                        {
                            order === "desc" ? <ArrowDownwardIcon fontSize="small"/> : <ArrowUpwardIcon fontSize='small'/>
                        }
                    </IconButton>
                    <Select
                        value={orderBy}
                        onChange={handleChangeOrderBy}
                    >
                        <MenuItem value={'name'}>Tên</MenuItem>
                        <MenuItem value={'createdDate'}>Ngày tạo</MenuItem>
                        <MenuItem value={'modifiedDate'}>Ngày chỉnh sửa</MenuItem>
                    </Select>
                </Stack>
            </FormControl>
            
            <LinearProgress color={"info"} sx={{opacity: isLoading ? 1 : 0}}/>
            <Grid container
                direction="column"
                alignItems="start"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                maxWidth={"100%"}
            >
                <Grid item xs={12} mb={2} sx={{width: "100%", display: 'flex', justifyContent: 'end', gap: "10px"}}>
                    {
                        isOpenMove ? (
                            <>
                                <Button
                                    type="button"
                                    variant="contained"
                                    disableElevation
                                    // startIcon={<PanToolAltOutlinedIcon />}
                                    color='error'
                                    onClick={(e) => {
                                        e.preventDefault();
                                        setIsOpenMove(false);
                                    }}
                                >
                                    Hủy
                                </Button>  
                                <Button
                                    type="button"
                                    variant="contained"
                                    disableElevation
                                    startIcon={<PanToolAltOutlinedIcon />}
                                    color='primary'
                                    onClick={(e) => {
                                        e.preventDefault();
                                        // setIsOpenMove(false);
                                        handleMoveModel();
                                    }}
                                >
                                    Chọn
                                </Button>    
                            </>
                        )
                         : <></>
                    }
                    {
                        selectedModels.length > 0 ? (
                            <Button
                                type="button"
                                variant="contained"
                                disableElevation
                                startIcon={<ViewInArIcon />}
                                color='info'
                                component={Link} to={`${UrlRouteCollection.ProjectModelViewer
                                    .replace(":projectId", projectId)
                                    .replace(":modelId", selectedModels.join(","))
                                }`}
                            >
                                Xem các mô hình này
                            </Button>
                        ) : <></>
                    }
                </Grid>
                
                {
                    treeData.map((item, index) => (
                        <Grid key={index} item xs={12} mb={2} sx={{width: "100%"}}>
                            <ModelItem
                                modelItem={item}
                                setSelectedItem={setSelectedItem}
                                setIsOpenAdd={setIsOpenAdd}
                                setIsOpenUpdate={setIsOpenUpdate}
                                setIsOpenDelete={setIsOpenDelete}
                                setIsOpenMove={setIsOpenMove}
                                setBeingMovedItem={setBeingMovedItem}
                                handleGetDirectChildren={handleGetDirectChildren}
                                handleUploadFileToModel={handleUploadFileToModel}
                                //
                                moveModelMode={isOpenMove}
                                beingMovedModelId={beingMovedItem?.id || ""}
                                setDestinationModel={handleMoveModel}
                                setSelectedModels={setSelectedModels}
                            />
                        </Grid>
                    ))
                }

            </Grid>

            <TablePagination
                showFirstButton
                showLastButton
                rowsPerPageOptions={[6, 12, 24, 48]}
                component="div"
                count={totalItemCount}
                rowsPerPage={rowsPerPage}
                page={page - 1}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                labelRowsPerPage="Số bản ghi mỗi trang : "
            />

            <AddModelModal
                isOpen={isOpenAdd}
                onClose={() => {
                    setIsOpenAdd(false);
                    setIsOpenAddFromProps(false);
                    setSelectedItem(null);
                }}
                onSuccess={async (newNode: Model) => {
                    const newTreeData = await addNode(treeData, selectedItem?.path || [], newNode);
                    setTreeData(newTreeData);
                }}
                selectedItem={selectedItem}
            />
            
            <UpdateModelModal
                isOpen={isOpenUpdate}
                onClose={() => {
                    setIsOpenUpdate(false);
                    setSelectedItem(null);
                }}
                onSuccess={async (newName: string) => {
                    if ( !selectedItem ) return;
                    const newTreeData = await updateNode(treeData, selectedItem.path, newName);
                    setTreeData(newTreeData);
                }}
                selectedItem={selectedItem}
            />
            
            <DeleteModelModal
                isOpen={isOpenDelete}
                onClose={() => {
                    setIsOpenDelete(false);
                    setSelectedItem(null);
                }}
                onSuccess={async () => {
                    if ( !selectedItem ) return;
                    const newTreeData = await deleteNode(treeData, selectedItem.path)
                    setTreeData(newTreeData);
                }}
                selectedItem={selectedItem}
            />
        </Stack>
	);
}

export default ListModel;