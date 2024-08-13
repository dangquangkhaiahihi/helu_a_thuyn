import React, { useEffect, useRef, useState } from 'react';
import { Box, Button, Card, CardContent, CardMedia, Checkbox, Collapse, Fade, LinearProgress, Popper, Stack, Tooltip, Typography } from '@mui/material';
import { UrlRouteCollection } from '@/common/url-route-collection';
import IconButton from '@mui/material/IconButton';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import AddIcon from '@mui/icons-material/Add';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import DriveFileMoveIcon from '@mui/icons-material/DriveFileMove';
import CreateNewFolderIcon from '@mui/icons-material/CreateNewFolder';
import PanToolAltOutlinedIcon from '@mui/icons-material/PanToolAltOutlined';
import { useClickOutside } from '@/utils/hooks/useClickOutside';
import Link from '@mui/material/Link';
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import { Model } from '@/common/DTO/Model/ModelDTO';

import 'dayjs/locale/vi';
import dayjs from 'dayjs';
dayjs.locale('vi');
import RelativeTime from "dayjs/plugin/relativeTime"
import IFCFileDropzone from '../ifc-file-dropzone';
dayjs.extend(RelativeTime);

interface IModelItemProps {
    modelItem: Model;
    setSelectedItem?: Function;
    setIsOpenAdd?: Function;
    setIsOpenUpdate?: Function;
    setIsOpenDelete?: Function;
    setIsOpenMove?: Function;
    setBeingMovedItem?: Function;
    handleGetDirectChildren?: Function;
    handleUploadFileToModel?: Function;
    
    // Move model logic
    moveModelMode?: boolean;
    setDestinationModel?: (id: string) => void;
    beingMovedModelId?: string;

    // Select multiple models
    setSelectedModels: Function;
}

const ModelItem: React.FC<IModelItemProps> = ( props ) => {
    const {
        modelItem,
        setSelectedItem,
        setIsOpenAdd,
        setIsOpenUpdate,
        setIsOpenDelete,
        setIsOpenMove,
        setBeingMovedItem,
        handleGetDirectChildren,
        handleUploadFileToModel,
        //
        moveModelMode,
        setDestinationModel,
        beingMovedModelId,
        //
        setSelectedModels
    } = props;
    
    const [isFirstLoad, setIsFirstLoad] = useState(true);
    const [expand, setExpand] = useState(false);
    const [hasChild, setHasChild] = useState<boolean | undefined>(modelItem.children && modelItem.children.length > 0);
    const [isModel, setIsModel] = useState<boolean>(modelItem.type === 'MODEL');
    const [isUpload, setIsUpload] = useState<boolean | undefined>(modelItem.isUpload);
    const [isChecked, setIsChecked] = useState<boolean>(false);

    useEffect(() => {
        if ( !isChecked ) setSelectedModels((prev: string[]) => prev.filter((modelId: string) => modelId !== modelItem.id));
        else setSelectedModels((prev: string[]) => [...prev, modelItem.id]);
    }, [isChecked])

    const [openFileInput, setOpenFileInput] = useState<boolean>(false);

    const [isLoading, setIsLoading] = useState<boolean>(false);

    useEffect(() => {
        setIsFirstLoad(false);
    }, [])

    useEffect(() => {
        if ( isFirstLoad ) return;
        setHasChild(modelItem.children && modelItem.children.length > 0);
        setIsModel(modelItem.type === 'MODEL');
        setIsUpload(modelItem.isUpload);
    }, [modelItem])

    const onExpand = async ( e: React.MouseEvent<HTMLDivElement, MouseEvent> ) => {
        e.stopPropagation();
        setExpand(prev => !prev);

        // Ko phải lo đến case nếu FOLDER ko có child ở DB, có thể lần nào ấn cũng gọi đi gọi lại api mà kết quả luôn rỗng, vì nếu 1 model đứng 1 mình ko có children thì nó là MODEL rồi :v
        if ( !isModel && !hasChild && handleGetDirectChildren ) {
            setIsLoading(true);
            await handleGetDirectChildren(modelItem.id);
            setIsLoading(false);
        }
    }

    const [openPopper, setOpenPopper] = React.useState(false);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

    const onOpenPopper = (event: React.MouseEvent<HTMLElement>) => {
        event.stopPropagation();
        setAnchorEl(event.currentTarget);
        setOpenPopper((prev) => !prev);
    };

    const canBeOpen = openPopper && Boolean(anchorEl);
    const id = canBeOpen ? 'transition-popper' : undefined;

    const popperRef = useRef(null);
    useClickOutside(popperRef, () => {
        setTimeout(() => {
            setOpenPopper(false);
        }, 100);
    });

    // if ( beingMovedModelId && modelItem.id === beingMovedModelId ) return null;

    useEffect(() => {
        setOpenPopper(false);
    }, [moveModelMode])
    
    return (
        <Card
            variant={( !isModel || modelItem.level === 1 ) ? "model-folder" : "model-model"}
            onClick={onExpand}
            sx={{ cursor: 'pointer' }}
        >
            <Box  sx={{ display: 'flex',  width: '100%' }} alignItems={'center'}>
                {
                    (!isModel) ?
                        <Tooltip title={expand ? "Đóng" : "Mở"} onClick={onExpand}>
                            <IconButton sx={{ aspectRatio: "1/1" }}>
                                {
                                    expand ? <ExpandLessIcon fontSize='small'/> : <ExpandMoreIcon fontSize='small'/>
                                }
                            </IconButton>
                        </Tooltip>
                        : <></>
                }
                {
                    isModel && isUpload ? (
                        <Checkbox
                            checked={isChecked}
                            onClick={() => setIsChecked(prev => !prev)}
                        />
                    ) : <></>
                }
                
                <CardMedia
                    sx={{
                        height: 40,
                        width: 40,
                        backgroundPosition: '50%',
                        backgroundSize: 'contain',
                        cursor: 'pointer',
                    }}
                    image={
                        !isModel ? 
                            "/assets/images/avatars/folder-ava.png" 
                        : isUpload ? 
                            "/assets/images/avatars/model-ava-uploaded.png" : 
                            "/assets/images/avatars/model-ava.png"
                    }
                    title={modelItem.name}
                />
                <CardContent sx={{display: 'flex', flex: 1, justifyContent: 'space-between'}}>
                    <Box sx={{display: 'flex', alignItems: 'center'}}>
                        <Typography variant="h3" 
                            component="div"
                            sx={{
                                cursor: 'pointer',
                                '&:hover': (isModel && isUpload) ? {
                                    color: '#1560BD',
                                    textDecoration: 'underline'
                                } : {}
                            }}
                        >
                            {
                                (isModel && isUpload) ? (
                                    <Link underline="hover" href={`${
                                        UrlRouteCollection.ProjectModelViewer
                                            .replace(":projectId", modelItem.projectID)
                                            .replace(":modelId", modelItem.id)
                                    }`} color={"inherit"}>
                                        {modelItem.name}
                                    </Link>
                                ) : <>{modelItem.name}</>
                            }
                        </Typography>

                        {
                            !moveModelMode ? (
                                <IconButton title='Hành động' sx={{ height: '30px', aspectRatio: "1/1" }}
                                    onClick={onOpenPopper}
                                >
                                    <MoreVertIcon fontSize='small'/>
                                </IconButton>
                            ) : null
                        }
                    </Box>
                    <Stack spacing={2} direction={'row'} display={!moveModelMode ? "flex" : "none"}>
                        {
                            <IFCFileDropzone
                                onDrop={(files: File[]) => {
                                    if (files.length > 0 && handleUploadFileToModel) {
                                        handleUploadFileToModel(modelItem.id, files[0]);
                                    }
                                }}
                                dropzoneText='Thả file .IFC vào hoặc click để chọn'
                                openFileInput={openFileInput}
                                setOpenFileInput={setOpenFileInput}
                                style={!isUpload && isModel ? { display: 'block' } : { display: 'none' }}
                            />
                        }
                        <Box sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'start',
                            justifyContent: 'center',
                            minWidth: 'fit-content'
                        }}>
                            <Typography gutterBottom variant="subtitle2" component="div" sx={{
                                display: 'flex',
                                alignItems: 'end',
                            }}>
                                <AccountCircleOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> {modelItem.createdBy}
                            </Typography>
                            <Typography gutterBottom variant="subtitle2" component="div" sx={{
                                display: 'flex',
                                alignItems: 'end',
                            }}>
                                <AccessTimeOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> cập nhật&nbsp;
                                <Box component="span" sx={{ fontWeight: 'bold' }}>{dayjs(modelItem.modifiedDate).fromNow()}</Box>
                            </Typography>
                        </Box>
                    </Stack>

                    <Stack spacing={2} direction={'row'} display={( moveModelMode && beingMovedModelId && modelItem.id !== beingMovedModelId ) ? "flex" : "none"}>
                        <Box sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'start',
                            justifyContent: 'center',
                            minWidth: 'fit-content'
                        }}>
                            <Button
                                type="button"
                                variant="contained"
                                disableElevation
                                startIcon={<PanToolAltOutlinedIcon />}
                                color='primary'
                                onClick={(e) => {
                                    e.stopPropagation();
                                    setDestinationModel && setDestinationModel(modelItem.id);
                                }}
                            >
                                Chọn
                            </Button>
                        </Box>
                    </Stack>
                </CardContent>
                
                <Popper id={id} open={openPopper} anchorEl={anchorEl} transition ref={popperRef} onClick={(e) => {e.stopPropagation()}}>
                    {({ TransitionProps }) => (
                        <Fade {...TransitionProps}>
                            <Card variant='action-popper'>
                                <Stack spacing={1}>
                                    <Button
                                        type="button"
                                        variant="action-popper"
                                        disableElevation
                                        fullWidth
                                        startIcon={<EditIcon color='warning' fontSize='small'/>}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            if ( setIsOpenUpdate && setSelectedItem ) {
                                                setIsOpenUpdate(true);
                                                setSelectedItem(modelItem);
                                            }
                                        }}
                                    >
                                        <span>Chỉnh sửa</span>
                                    </Button>
                                    {
                                        (!isModel || (isModel && !isUpload)) ? <>
                                            <Button
                                                type="button"
                                                variant="action-popper"
                                                disableElevation
                                                fullWidth
                                                startIcon={<CreateNewFolderIcon color='info' fontSize='small'/>}
                                                color="inherit"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    if ( setIsOpenAdd && setSelectedItem ) {
                                                        setIsOpenAdd(true);
                                                        setSelectedItem(modelItem);
                                                    }
                                                }}
                                            >
                                                Thêm mô hình
                                            </Button>
                                            {
                                                (isModel && !isUpload) ? (
                                                    <Button
                                                        type="button"
                                                        variant="action-popper"
                                                        disableElevation
                                                        fullWidth
                                                        startIcon={<FileUploadIcon color='success' fontSize='small'/>}
                                                        color="inherit"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            setOpenFileInput(true);
                                                        }}
                                                    >
                                                        Tải lên file mô hình 
                                                    </Button>
                                                ) : <></>
                                            }
                                        </> : <>
                                        {
                                                (isModel && isUpload) ? (
                                                    <Button
                                                        type="button"
                                                        variant="action-popper"
                                                        disableElevation
                                                        fullWidth
                                                        startIcon={<FileUploadIcon color='success' fontSize='small'/>}
                                                        color="inherit"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            setOpenFileInput(true);
                                                        }}
                                                    >
                                                        Tải lên phiên bản mới
                                                    </Button>
                                                ) : <></>
                                            }
                                        </>
                                    }
                                    <Button
                                        type="button"
                                        variant="action-popper"
                                        disableElevation
                                        fullWidth
                                        startIcon={<DriveFileMoveIcon color='primary' fontSize='small'/>}
                                        color="inherit"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            if ( setIsOpenMove && setBeingMovedItem ) {
                                                setIsOpenMove(true);
                                                setBeingMovedItem(modelItem);
                                            }
                                        }}
                                    >
                                        Di chuyển đến 
                                    </Button>
                                    <Button
                                        type="button"
                                        variant="action-popper"
                                        disableElevation
                                        fullWidth
                                        startIcon={<DeleteIcon color='error' fontSize='small'/>}
                                        color="inherit"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            if ( setIsOpenDelete && setSelectedItem ) {
                                                setIsOpenDelete(true);
                                                setSelectedItem(modelItem);
                                            }
                                        }}
                                    >
                                        Xóa 
                                    </Button>
                                </Stack>
                            </Card>
                        </Fade>
                    )}
                </Popper>
            </Box>
            
            {
                isLoading ? <LinearProgress color={"info"}/> : null
            }
            {
                !isModel ? 
                    modelItem.children?.map((item, index) => (
                        <Collapse key={index} in={expand}>
                            <ModelItem
                                {...props}
                                modelItem={item}
                            />
                        </Collapse>
                        
                    ))
                    : <></>
            }

            {
                ( !isModel ) ? (
                    <>
                        <Button
                            type="button"
                            variant="text"
                            disableElevation
                            fullWidth
                            startIcon={<AddIcon />}
                            color="cuaternary"
                            onClick={(e) => {
                                if ( expand ) e.stopPropagation();
                                if ( setIsOpenAdd && setSelectedItem ) {
                                    setIsOpenAdd(true);
                                    setSelectedItem(modelItem);
                                }
                            }}
                            sx={{ display: 'flex', justifyContent: 'start', paddingY: '10px', alignItems: 'center' }}
                        >
                            Thêm mới 
                        </Button>
                    </>
                ) : <></>
            }
        </Card>
    )
}

export default ModelItem;