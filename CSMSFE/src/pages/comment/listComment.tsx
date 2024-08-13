import React, { useState } from 'react';
import './comment.css'
import { Pagination } from '@mui/material';
import { IconButton } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import {Comment} from '../../common/DTO/Comment/CommentList'
import EditIcon from '@mui/icons-material/Edit';

interface CommentListProps {
    comments: Comment[];
    handleDelete: (id: number) => void;
}

const listComment: React.FC<CommentListProps> = ({ comments, handleDelete }) => {

    // const [commentData, setCommentData] = useState<Comment[]>(comments);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setitemsPerPage] = useState(4);

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const pageItems = comments.slice(indexOfFirstItem, indexOfLastItem);

    const handleChangPage = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(Number(value));
    }

    const displayComment = (commneListData: Comment[]) => {
        return (
            <div className="comment-section">
                {commneListData.map((comment, index) => {
                    return <div className="single-comment" key={index}>
                        <img />
                        <div className="single-container">
                            <p>{comment.userName}</p>
                            <p>{comment.content}</p>
                        </div>
                        <div className="times">
                            <span>{comment.createdDate}</span>
                            <span><a href='#'>0 reply</a></span>
                        </div>
                        <div><EditIcon></EditIcon></div>
                        <div><IconButton className='button-delete' onClick={() => handleDelete(comment.commentId)}><ClearIcon /></IconButton></div>
                    </div>
                })}
            </div>
        );
    }

    return (
        <div>
            <div>
                {displayComment(pageItems)}
            </div>
            <div>
                <Pagination
                    count={Math.ceil(comments.length / itemsPerPage)}
                    onChange={handleChangPage}
                    color="primary" />
            </div>
        </div>
    );
};

export default listComment;