import React, {useRef, KeyboardEvent} from 'react';
import './comment.css'
import { IconButton} from '@mui/material';
import AttachFileIcon from '@mui/icons-material/AttachFile';
import {Comment} from '../../common/DTO/Comment/CommentList'
import CommentService from '@/api/instance/comment';

interface CommentAddProps {

    comments: (commentId: number, userName: string, createdDate: string, reply: Comment[], content: string) => void;
}

const addComment: React.FC<CommentAddProps> = ({comments}) => {

    const newTextRef = useRef<HTMLTextAreaElement>(null);
   
    const addNewComment=(tittle:string) =>{   
        comments(comments.length+1, "CCC","2000/02/02",[], tittle);  
        CommentService.Create({issueId:1,content:tittle})   
    }

    const handleKeyPress = (e: KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === "Enter" && newTextRef.current) {
            addNewComment(newTextRef.current.value);
            newTextRef.current.value = "";
            e.preventDefault();
        }
    };

    const btnaddComment = () => {
        if (newTextRef.current) {
            addNewComment(newTextRef.current.value);
            newTextRef.current.value = "";
        }
    }
    
    return (
            <div className="input">
                <img />
                <textarea ref={newTextRef} onKeyPress={handleKeyPress} placeholder="Write a comment..." />
                <div ><IconButton className='button-icon'><AttachFileIcon /></IconButton></div>
                <div className='button-container'><button className='button-add' onClick={btnaddComment}>Add</button></div>
            </div>
    );
};

export default addComment;