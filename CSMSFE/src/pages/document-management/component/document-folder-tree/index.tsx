import React from 'react';
import './document.css'
import { IconButton, Tooltip } from '@mui/material';
import FolderTree, { IconProps, NodeData } from 'react-folder-tree';
import 'react-folder-tree/dist/style.css';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import ClearIcon from '@mui/icons-material/Clear';
import CreateNewFolderIcon from '@mui/icons-material/CreateNewFolder';
import NoteAddIcon from '@mui/icons-material/NoteAdd';
import FolderIcon from '@mui/icons-material/Folder';
import FolderOpenIcon from '@mui/icons-material/FolderOpen';
import KeyboardArrowRightIcon from '@mui/icons-material/KeyboardArrowRight';
import DriveFileMoveIcon from '@mui/icons-material/DriveFileMove';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import DownloadIcon from '@mui/icons-material/Download';
import PreviewIcon from '@mui/icons-material/Preview';
import { DocumentDTO } from '@/common/DTO/Document/DocumentDTO';
import { renderIcon } from '../helper/renderIcon';

interface IDocumentFolderTree {
    documentTree: NodeData;
    listenOnCheckbox: (newState: DocumentDTO, event: any) => void;
    onExpand: (nodeData: DocumentDTO) => void;
    onCollapse: (nodeData: DocumentDTO) => void;
    onOpenUpdateDocument: (nodeData: DocumentDTO) => void;
    onOpenAddFolderDocument: (nodeData: DocumentDTO) => void;
    onOpenDeleteDocument: (nodeData: DocumentDTO) => void;
    onOpenUploadDocument: (nodeData: DocumentDTO) => void;
    onOpenPreviewDocument: (nodeData: DocumentDTO) => void;
    onDownloadFile: (nodeData: DocumentDTO) => void;
}

const DocumentFolderTree = (props: IDocumentFolderTree) => {
    const {
        documentTree,
        listenOnCheckbox,
        onExpand,
        onCollapse,
        onOpenUpdateDocument,
        onOpenAddFolderDocument,
        onOpenDeleteDocument,
        onOpenUploadDocument,
        onOpenPreviewDocument,
        onDownloadFile
    } = {...props};

    const FolderIconRender: React.FC<IconProps> = ({ nodeData }) => {
        return <>
            {
                nodeData.isFile ? (
                    // Custom Icon cho từng extension ở đây
                    renderIcon(nodeData.fileExtension)
                ) : (
                    <FolderIcon fontSize='small' />
                )
            }
        </>
    };

    const FolderOpenIconRender: React.FC<IconProps> = ({ nodeData }) => {
        return <>
            {
                nodeData.isFile ? (
                    // Custom Icon cho từng extension ở đây
                    renderIcon(nodeData.fileExtension)
                ) : (
                    <FolderOpenIcon fontSize='small' />
                )
            }
        </>
    };

    const CaretRightIconRender: React.FC<IconProps> = ({ nodeData }) => {
        return <>
            {
                nodeData.isFile ? (
                    <></>
                ) : (
                    <IconButton size='small' onClick={() => onExpand(nodeData)}><KeyboardArrowRightIcon fontSize='small' /></IconButton>
                )
            }
        </>
    };

    const CaretDownIconRender: React.FC<IconProps> = ({ nodeData }) => {
        return <>
            {
                nodeData.isFile ? (
                    <></>
                ) : (
                    <IconButton size='small' onClick={() => onCollapse(nodeData)}><ExpandMoreIcon fontSize='small' /></IconButton>
                )
            }
        </>
    };

    const UpdateIconRender: React.FC<IconProps> = ({ nodeData }) => {
        // Root folder ==> Ko Update
        if ( nodeData.parentId === 0 ) {
            return null;    
        }

        return <Tooltip title="Chỉnh sửa"><IconButton size='small' onClick={() => onOpenUpdateDocument(nodeData)}><EditIcon fontSize='small' /></IconButton></Tooltip>
    };

    const DeleteIconRender: React.FC<IconProps> = ({ nodeData }) => {
        const { path, name, checked, isOpen, url, ...restData } = nodeData;

        const documentDto: DocumentDTO = {
            ...nodeData,
            id: restData.id,
            isFile: restData.isFile,
            parentId: restData.parentId,
            projectId: restData.projectId,
            fileExtension: restData.extension,
            icon: restData.icon,
            size: restData.size,
            urlPath: restData.urlPath,
        };
        
        // Root folder ==> Ko Xóa và Di chuyển
        if ( nodeData.parentId === 0 ) {
            return <>
                <Tooltip title="Thư mục mới"><IconButton size='small' onClick={() => {onOpenAddFolderDocument(documentDto)}}> <CreateNewFolderIcon fontSize='small' /></IconButton></Tooltip>
                <Tooltip title="Tài liệu mới"><IconButton size='small' onClick={() => {onOpenUploadDocument(documentDto)}}><NoteAddIcon fontSize='small' /></IconButton></Tooltip>
            </> 
        }

        return <>
            <Tooltip title="Xóa"><IconButton size='small' onClick={() => onOpenDeleteDocument(documentDto)}><DeleteIcon fontSize='small' /></IconButton></Tooltip>
            <Tooltip title="Di chuyển"><IconButton size='small' onClick={() => {}}><DriveFileMoveIcon fontSize='small' /></IconButton></Tooltip>
            {
                !restData.isFile ? (
                    <>
                        <Tooltip title="Thư mục mới"><IconButton size='small' onClick={() => {onOpenAddFolderDocument(documentDto)}}> <CreateNewFolderIcon fontSize='small' /></IconButton></Tooltip>
                        <Tooltip title="Tài liệu mới"><IconButton size='small' onClick={() => {onOpenUploadDocument(documentDto)}}><NoteAddIcon fontSize='small' /></IconButton></Tooltip>
                    </>
                ) : <>
                    <Tooltip title="Tải file xuống"><IconButton size='small' onClick={() => {onDownloadFile(documentDto)}}><DownloadIcon fontSize='small' /></IconButton></Tooltip>
                    <Tooltip title="Xem trước"><IconButton size='small' onClick={() => {onOpenPreviewDocument(documentDto)}}><PreviewIcon fontSize='small' /></IconButton></Tooltip>
                </>
            }
        </>
    };

    const CancelIconRender: React.FC<IconProps> = ({ onClick: defaultOnClick, nodeData }) => {
        const { path, name, checked, isOpen, url, ...restData } = nodeData;
        const handleClick = () => {
            console.log('icon clicked:', { path, name, url, ...restData });
            defaultOnClick();
        };

        return <Tooltip title="Đóng"><IconButton size='small' onClick={handleClick}><ClearIcon fontSize='small' /></IconButton></Tooltip>
    };


    return (
        <FolderTree
            data={documentTree}
            onChange={(newState, event) => {
                listenOnCheckbox(newState, event);
            }}
            initOpenStatus='custom'
            showCheckbox={true}
            iconComponents={{
                EditIcon: UpdateIconRender,
                CancelIcon: CancelIconRender,
                DeleteIcon: DeleteIconRender,
                FolderIcon: FolderIconRender,
                FolderOpenIcon: FolderOpenIconRender,
                CaretRightIcon: CaretRightIconRender,
                CaretDownIcon: CaretDownIconRender,
            }}
            onNameClick={({defaultOnClick, nodeData}) => {
                defaultOnClick();
                onExpand(nodeData);
            }}
        />
    );
};
export default DocumentFolderTree;



