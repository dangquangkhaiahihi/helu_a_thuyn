import React, {useEffect, useMemo} from 'react';
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
  transition: 'border .24s ease-in-out'
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

function IFCFileDropzone(props) {
    const {
        getRootProps,
        getInputProps,
        isFocused,
        isDragAccept,
        isDragReject,
        open
    } = useDropzone({
        accept: {
            'model/ifc': ['.ifc'],
        },
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
            props.onDrop(acceptedFiles);
            props.setOpenFileInput(false);
        },
        onFileDialogCancel: () => {
            props.setOpenFileInput(false);
        }
    });

    useEffect(() => {
        if ( props.openFileInput ) open();
    }, [props.openFileInput])

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


export default IFCFileDropzone;