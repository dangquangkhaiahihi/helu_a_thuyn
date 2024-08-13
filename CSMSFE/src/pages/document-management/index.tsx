import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Button, Card, Divider, Grid, IconButton, LinearProgress, Link, Tooltip, Typography } from '@mui/material';
import 'react-folder-tree/dist/style.css';
import DocumentService from '@/api/instance/document';
import { useParams } from 'react-router-dom';
import DocumentFolderTree from './component/document-folder-tree';
import PageHeader from '@/components/pageHeader';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import { enqueueSnackbar } from 'notistack';
import { DocumentDTO } from '@/common/DTO/Document/DocumentDTO';
import { ApiResponse } from '@/common/DTO/ApiResponse';
import useTreeStateCustom from '@/hooks/useTreeStateCustom';
import AddFolderDocumentModal from './add-folder-document';
import UpdateDocumentModal from './update-document';
import DeleteDocumentModal from './delete-document';
import FileDropzone from './component/file-dropzone';
import { getNodeAddress } from './component/helper';
import { DocumentUploadFileDTO } from '@/common/DTO/Document/DocumentUploadFileDTO';
import { useDispatch, useSelector } from 'react-redux';
import { setLoadingCircularOverlayRedux } from '@/store/redux/app';
import CloseIcon from '@mui/icons-material/Close';
import DocPreviewer from './component/doc-previewer';
import ApiEndpointCollection from '@/api/api-endpoint-collection';
import { UrlRouteCollection } from '@/common/url-route-collection';
import { fetchProjectInfo, projectInfoRedux } from '@/store/redux/project';

const loadingRootFolder: DocumentDTO = {
    "children": [],
    "id": -1,
    "isFile": false,
    "urlPath": "",
    "size": 0,
    "icon": "",
    "projectId": "",
    "parentId": 0,
    "name": "Đang tải...",
    "checked": 0,
    "isOpen": true,
    "_id": 0
};

const failRootFolder: DocumentDTO = {
    "id": -1,
    "isFile": false,
    "urlPath": '',
    "size": 1,
    "icon": "",
    "projectId": "",
    "parentId": 0,
    "name": "Lỗi - Không lấy được tài liệu của dự án.",
    "checked": 0,
    "isOpen": false
};

const DocumentManagement: React.FC = () => {
    const { projectId } = useParams();
    const dispatch = useDispatch();
    const projectInfo = useSelector(projectInfoRedux);

    useEffect(() => {
        if ( projectInfo ) return;
        dispatch(fetchProjectInfo(projectId || ""));
    }, [])

    const {
        deleteNode,
        setTreeState,
        findTargetNode,
        findTargetPathByProp,
        findAllTargetPathByProp,
        treeState
    } = useTreeStateCustom({ initialTreeState: loadingRootFolder });
    
    const [checkedIds, setCheckedIds] = useState<number[]>([]);
    const [treeStateTemp, setTreeStateTemp] = useState<DocumentDTO | null>(null);
    const listenOnCheckbox = (
        newState: DocumentDTO,
      ) => {
        setTreeStateTemp(newState);

        const listCheckedPaths: any[] = findAllTargetPathByProp(newState, 'checked', 1);
        const checkedIdsResult: number[] = [];
        listCheckedPaths.forEach(path => {
            const node: DocumentDTO = findTargetNode(newState, path);
            node && node.id && checkedIdsResult.push(node.id);
        });
        setCheckedIds(checkedIdsResult);
    };

    useEffect(() => {
        console.log("treeState", treeState);
    }, [treeState])

    const [isLoadingFilter, setIsLoadingFilter] = useState<boolean>(false);
    const handleGetDocumentTreeLevel1ByProjectId = async () => {
        setIsLoadingFilter(true);
        try {
            const result: ApiResponse<DocumentDTO> = await DocumentService.GetDocumentTreeLevel1ByProjectId(projectId || "");
            setTreeState(result.content);
        } catch (err) {
            console.log("handleGetDocumentTreeByProjectId", err);
            setTreeState(failRootFolder);
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        } finally {
            setIsLoadingFilter(false);
        }
    }

    const handleGetDocumentsByParentId = async (parentId: number): Promise<DocumentDTO[]> => {
        setIsLoadingFilter(true);
        try {
            const childrenRes: ApiResponse<DocumentDTO[]> = await DocumentService.GetDocumentsByParentId(parentId);
            return childrenRes.content;
        } catch (err) {
            console.log("handleGetDocumentsByParentId", err);
            setTreeState(failRootFolder);
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
            return [];
        } finally {
            setIsLoadingFilter(false);
        }
    }

    useEffect(() => {
        handleGetDocumentTreeLevel1ByProjectId();
    }, []);

    // Logic model action with document
    const [isOpenAddDocument, setIsOpenAddDocument] = useState(false);
    const [isOpenUpdateModal, setIsOpenUpdateModal] = useState(false);
    const [isOpenDeleteModal, setIsOpenDeleteModal] = useState(false);
    const [selectedItem, setSelectedItem] = useState<DocumentDTO | null>(null);

    const onOpenAddFolderDocument = (nodeData: DocumentDTO) => {
        setIsOpenAddDocument(true);
        setSelectedItem(nodeData);
    }

    const onCloseAddFolderDocument = () => {
        setIsOpenAddDocument(false);
        setSelectedItem(null);
    }

    const onOpenUpdateDocument = (nodeData: DocumentDTO) => {
        setIsOpenUpdateModal(true);
        setSelectedItem(nodeData);
    }

    const onCloseUpdateDocument = () => {
        setIsOpenUpdateModal(false);
        setSelectedItem(null);
    }

    const onOpenDeleteDocument = (nodeData: DocumentDTO) => {
        setIsOpenDeleteModal(true);
        setSelectedItem(nodeData);
    }

    const onCloseDeleteDocument = () => {
        setIsOpenDeleteModal(false);
        setSelectedItem(null);
    }

    const [openedDocument, setOpenedDocument] = useState<number[]>([]);
    // Update state function
    const onExpand_UpdateState = async (nodeData: DocumentDTO) => {
        const nodeId = nodeData.id || -1;
        const isRootDocumentOfProject = (nodeId == treeState.id);
        const nodeOpenedBefore = openedDocument.includes( nodeId );
        const isFolder = !nodeData.isFile;
        let children: DocumentDTO[] = [];

        if ( !nodeOpenedBefore && !isRootDocumentOfProject && isFolder ) {
            setOpenedDocument(prev => [...prev, nodeId]);
            children = await handleGetDocumentsByParentId( nodeId );
        }
        
        setTreeState((prev: any) => {
            let root;
            if ( treeStateTemp ) {
                root = { ...treeStateTemp };
                setTreeStateTemp(null);
            } else {
                root = { ...prev };
            }
            
            const nodeDataPath = findTargetPathByProp(root, "id", nodeData.id);
            const targetNode: DocumentDTO = findTargetNode(root, nodeDataPath);
            targetNode.isOpen = true;

            if( children.length > 0 ) {
                targetNode.children = [...children];
            }
            return {...root};
        })
    }

    const onCollapse_UpdateState = (nodeData: DocumentDTO) => {
        setTreeState((prev: any) => {
            let root;
            if ( treeStateTemp ) {
                root = { ...treeStateTemp };
                setTreeStateTemp(null);
            } else {
                root = { ...prev };
            }
            const nodeDataPath = findTargetPathByProp(root, "id", nodeData.id);
            const targetNode = findTargetNode(root, nodeDataPath);
            targetNode.isOpen = false;
            return {...root};
        })
    }

    const onAddNode_UpdateState = (documentRes: DocumentDTO, destinationDocument: DocumentDTO) => {
        setTreeState((prev: any) => {
            let root;
            if ( treeStateTemp ) {
                root = { ...treeStateTemp };
                setTreeStateTemp(null);
            } else {
                root = { ...prev };
            }
            const nodeDataPath = findTargetPathByProp(root, "id", destinationDocument.id);
            const destinationNode = findTargetNode(root, nodeDataPath);
            destinationNode.children.push(documentRes);
            return {...root};
        })
    }

    const onUpdateNode_UpdateState = (newName: string, destinationDocument: DocumentDTO) => {
        setTreeState((prev: any) => {
            let root;
            if ( treeStateTemp ) {
                root = { ...treeStateTemp };
                setTreeStateTemp(null);
            } else {
                root = { ...prev };
            }
            const nodeDataPath = findTargetPathByProp(root, "id", destinationDocument.id);
            const destinationNode = findTargetNode(root, nodeDataPath);
            destinationNode.name = newName;
            return {...root};
        })
    }

    const onDeleteNode_UpdateState = ( deletedIds: number[] ) => {
        setTreeState((prev: any) => {
            let root;
            if ( treeStateTemp ) {
                root = { ...treeStateTemp };
                setTreeStateTemp(null);
            } else {
                root = { ...prev };
            }
            //reverse để xóa từ dưới lên
            deletedIds.reverse().forEach(deletedId => {
                const nodeDataPath = findTargetPathByProp(root, "id", deletedId);
                deleteNode(nodeDataPath);
            });
            
            return {...root};
        })
    }
    
    // File Upload
    const [isOpenUpload, setIsOpenUpload] = useState(false);
    const [selectedFolder, setSelectedFolder] = useState<DocumentDTO | null>(null);

    const onOpenUploadDocument = (nodeData: DocumentDTO) => {
        
        const nodeDataPath = findTargetPathByProp(treeState, "id", nodeData.id);
        const nodeDataFound = findTargetNode(treeState, nodeDataPath);

        setIsOpenUpload(true);
        setSelectedFolder(nodeDataFound);
    }

    const onCloseUploadDocument = () => {
        setIsOpenUpload(false);
    }

    const setLoadingCircularOverlay = (val: boolean) => {
        dispatch(setLoadingCircularOverlayRedux(val));
    }

    const handleUploadFile = async (data: DocumentUploadFileDTO): Promise<DocumentDTO> => {
        setLoadingCircularOverlay(true);
        try {
            const res = await DocumentService.UploadFile(data);
            return res.content;
        } catch (err) {
            console.log("handleAdd err", err);
            throw err;
        } finally {
            setLoadingCircularOverlay(false);
        }
    }

    // File preview
    type DocToPreview = {
        uri: string;
    }
    const [docs, setDocs] = useState<DocToPreview[]>([ {uri: ""} ]);
    const [isOpenPreview, setIsOpenPreview] = useState<boolean>(false);
    
    useEffect(() => {
        setIsOpenPreview(docs[0].uri ? true : false);
    }, [docs])

    const onOpenPreviewDocument = (nodeData: DocumentDTO) => {
        const nodeDataPath = findTargetPathByProp(treeState, "id", nodeData.id);
        const targetNode: DocumentDTO = findTargetNode(treeState, nodeDataPath);
        
        if ( targetNode.id != selectedFolder?.id && targetNode.isFile ) {
            setSelectedFolder(targetNode);
            
            // setDocs([{ uri: `${import.meta.env.VITE_MEDIA_URL}/${targetNode.urlPath}` }])
            setDocs([{ uri: `${import.meta.env.VITE_API_ENDPOINT}/${ApiEndpointCollection.Document.Download.replace("{id}", targetNode.id?.toString() || "")}` }])
        }
    }

    const onClosePreviewDocument = () => {
        setDocs( [{uri: ""}] );
        setSelectedFolder(null);
    }
    
    const [selectedFolderBreadcrumbs, setSelectedFolderBreadcrumbs] = useState<string>("");

    useEffect(() => {
        if (!selectedFolder || !treeState) return;
        const nodePath = findAllTargetPathByProp(treeState, "id", selectedFolder.id);
        
        const nodeBreadcrumbs = getNodeAddress(treeState, nodePath[0]);
        setSelectedFolderBreadcrumbs(nodeBreadcrumbs || "");
    }, [selectedFolder])

    // File download
    
    const onDownloadFile = async (nodeData: DocumentDTO) => {
        const nodeDataPath = findTargetPathByProp(treeState, "id", nodeData.id);
        const targetNode: DocumentDTO = findTargetNode(treeState, nodeDataPath);
        
        const fileContent: BlobPart = await DocumentService.Download(targetNode.id?.toString() || "");
        const url = window.URL.createObjectURL(new Blob([fileContent]));
        const link = document.createElement('a');
        link.href = url;
        const filename = nodeData.name;
        
        link.setAttribute('download', filename);
        
        // Append the anchor to the body
        document.body.appendChild(link);
        
        // Programmatically click the anchor to trigger the download
        link.click();
        
        // Clean up by removing the anchor and revoking the object URL
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
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
                <Link underline="hover" href={UrlRouteCollection.Home}>
                    Trang chủ
                </Link>

                <Link underline="hover" href={UrlRouteCollection.ProjectManagement}>
                    Danh sách dự án
                </Link>
                <Typography color="text.tertiary">{projectInfo?.name}</Typography>
                <Typography color="text.tertiary">Danh sách tài liệu</Typography>
            </Breadcrumbs>

            <PageHeader title="Quản lý tài liệu">
                {
                    checkedIds.length > 0 ? (
                        <Button
                            type="button"
                            variant="contained"
                            disableElevation
                            startIcon={<DeleteForeverIcon />}
                            color='error'
                            onClick={(e) => {
                                e.preventDefault();
                                setIsOpenDeleteModal(true);
                            }}
                        >
                            Xóa mục đã chọn
                        </Button>
                    ) : <></>
                }
            </PageHeader>

            <Grid container
                direction="row"
                justifyContent="center"
                alignItems="flex-start"
            >
                <Grid item md={isOpenPreview ? 6 : 12} xs={12}>
                    <Card component="section" sx={{overflow:'auto'}}>
                        <LinearProgress color={"info"} sx={{opacity: isLoadingFilter ? 1 : 0}}/>
                        <DocumentFolderTree
                            documentTree={treeState || loadingRootFolder}
                            listenOnCheckbox={listenOnCheckbox}
                            onExpand={onExpand_UpdateState}
                            onCollapse={onCollapse_UpdateState}
                            onOpenUpdateDocument={onOpenUpdateDocument}
                            onOpenAddFolderDocument={onOpenAddFolderDocument}
                            onOpenDeleteDocument={onOpenDeleteDocument}
                            onOpenUploadDocument={onOpenUploadDocument}
                            onOpenPreviewDocument={onOpenPreviewDocument}
                            onDownloadFile={onDownloadFile}
                        />
                    </Card>
                </Grid>
                <FileDropzone
                    dropzoneText="Tải lên tài liệu"
                    isOpenUpload={isOpenUpload}
                    onCloseUploadDocument={onCloseUploadDocument}
                    callbackDropOrSelectFile={async (files: File[]) => {
                        if ( !selectedFolder ) return;
                        if ( files.length > 0 ) {
                            const uploadFileRes = await handleUploadFile({
                                file: files[0],
                                isFile: true,
                                projectId: projectId || "",
                                parentId: selectedFolder.id || -1
                            });
                            onAddNode_UpdateState(uploadFileRes, selectedFolder);
                            setSelectedFolder(prev => {return {...prev, children: [...prev?.children || [], uploadFileRes]}})
                        }
                    }}
                    style={{display: 'none'}}
                />
                <Grid item md={6} xs={12} display={isOpenPreview ? "block" : "none"}>
                    <Card component="div">
                        <Grid container justifyContent={"space-between"}>
                            <Typography variant="subtitle1" sx={{ display: "flex", alignItems: "center"}}>Xem trước: {selectedFolderBreadcrumbs}</Typography>
                            <Tooltip title="Đóng">
                                <IconButton onClick={onClosePreviewDocument}>
                                    <CloseIcon />
                                </IconButton>
                            </Tooltip>
                        </Grid>
                        
                        <Divider sx={{ my: 1 }}/>

                        <DocPreviewer docs={docs} />
                    </Card>
                </Grid>
            </Grid>

            <AddFolderDocumentModal
                isOpen={isOpenAddDocument}
                selectedItem={selectedItem}
                onClose={onCloseAddFolderDocument}
                onSuccess={(documentRes: DocumentDTO, destinationDocument: DocumentDTO) => {
                    onAddNode_UpdateState(documentRes, destinationDocument);
                    onExpand_UpdateState(destinationDocument);
                }}
            />

            <UpdateDocumentModal
                isOpen={isOpenUpdateModal}
                selectedItem={selectedItem}
                onClose={onCloseUpdateDocument}
                onSuccess={(newName: string, destinationDocument: DocumentDTO) => {
                    onUpdateNode_UpdateState(newName, destinationDocument);
                }}
            />
            
            <DeleteDocumentModal
                isOpen={isOpenDeleteModal}
                selectedItem={selectedItem}
                deleteIds={ selectedItem ? [selectedItem.id || -1] : checkedIds}
                onClose={onCloseDeleteDocument}
                onSuccess={(deletedIds: number[]) => {
                    onDeleteNode_UpdateState(deletedIds);
                }}
            />
        </>
    )
};
export default DocumentManagement;