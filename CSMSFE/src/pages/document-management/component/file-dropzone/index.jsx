import DocumentService from '@/api/instance/document';
import { mimeTypes } from '@/utils/configuration';
import React, {useEffect, useMemo, useState} from 'react';
import {useDropzone} from 'react-dropzone';

const baseStyle = {
  flex: 1,
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  padding: '20px',
  borderWidth: 2,
  borderRadius: 2,
  borderColor: '#eeeeee',
  borderStyle: 'dashed',
  backgroundColor: '#fafafa',
  color: '#bdbdbd',
  outline: 'none',
  transition: 'border .24s ease-in-out',
};

const focusedStyle = {
  borderColor: '#2196f3'
};

const acceptStyle = {
  borderColor: '#00e676'
};

const rejectStyle = {
  borderColor: '#ff1744'
};

function FileDropzone(props) {
    const [allowedExtensions, setAllowedExtensions] = useState([]);
    const handleGetAllowedFileExtensions = async (keyword) => {
        try {
            const param = {
                Keyword: keyword
            };
            const res = await DocumentService.GetLookupFileExtensions(param);
            setAllowedExtensions(res.content);
        } catch ( err ) {
            throw err;
        }
    }

    useEffect(() => {
        handleGetAllowedFileExtensions();
    }, [])

    const accept = useMemo(() => {
        const formattedExtensions = allowedExtensions.reduce((acc, ext) => {
            const mimeType = mimeTypes[ext.label];
            if (mimeType) {
                if (!acc[mimeType]) {
                acc[mimeType] = [];
                }
                acc[mimeType].push(ext.label);
            }
            return acc;
        }, {});
    
        return formattedExtensions;
    }, [allowedExtensions]);
    
    return (
        <>
            {
                accept ? (
                    <Wrapper
                        allowedExtensions={accept}
                        {...props}
                    />
                ) : <></>
            }
        </>
        
    );
}

const Wrapper = (props) => {
    const {
        allowedExtensions,
        callbackDropOrSelectFile
    } = {...props};
    const {
        getRootProps,
        getInputProps,
        isFocused,
        isDragAccept,
        isDragReject,
        open
    } = useDropzone({
        accept: allowedExtensions,
        onDragEnter: (acceptedFiles, fileRejections) => {
            if (fileRejections.length > 0) {
                console.log('File Rejections:', fileRejections);
            }
            // Handle accepted files
            console.log('Accepted files:', acceptedFiles);
        },
        onDrop: (acceptedFiles, fileRejections) => {
            if (fileRejections.length > 0) {
                console.log('File Rejections:', fileRejections);
                alert('Invalid file type!');
            }
            // Handle accepted files
            console.log('Accepted files:', acceptedFiles);
            callbackDropOrSelectFile(acceptedFiles);
            props.onCloseUploadDocument(false);
        },
        onFileDialogCancel: () => {
            props.onCloseUploadDocument(false);
        },

    });

    useEffect(() => {
        if ( props.isOpenUpload ) open();
    }, [props.isOpenUpload])

    const style = useMemo(() => ({
        ...baseStyle,
        ...(isFocused ? focusedStyle : {}),
        ...(isDragAccept ? acceptStyle : {}),
        ...(isDragReject ? rejectStyle : {})
    }), [
        isFocused,
        isDragAccept,
        isDragReject
    ]);

    return (
        <div className="container" style={props.style}>
            <div {...getRootProps({style})}>
                <input {...getInputProps()}/>
                <p>{props.dropzoneText || "Drag 'n' drop some files here, or click to select files"}</p>
            </div>
        </div>
    );
}

export default FileDropzone;