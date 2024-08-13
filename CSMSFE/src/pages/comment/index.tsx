import React, { useEffect, useState } from 'react';
import './comment.css'
import AddComment from './addComment'
import ListComment from './listComment';
import { fetchData } from './commentService';
import { Comment } from '../../common/DTO/Comment/CommentList'
import { ApiResponse } from '@/common/DTO/ApiResponse';
import CommentServive from '@api/instance/comment';
import CommentService from '@api/instance/comment';


const CommentSection: React.FC = () => {

    let res: ApiResponse<any>;
    const [commentData, setCommentData] = useState<Comment[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const getData = async () => {
            setLoading(true);
            try {
                const comments:any = await CommentService.List("1");
                    console.log('Processesd  Comment:', comments.content.comments);
                    setCommentData(comments.content.comments);
                    console.log('Comments:',commentData);                                  
            } catch (err) {
                console.error('Error fetching comments:', err);
            } finally {
                    setLoading(false);
            }
        }
        getData();
    }, []);

    const addComment = async (commentId: number, userName: string, createdDate: string, reply: Comment[], content: string) => {
        const newComment: Comment = {
            commentId,
            userName,
            createdDate,
            // reply,
            content,
        };
        setCommentData([...commentData, newComment]);
        // res = await CommentServive.Create(newComment);
    };
    const deleteComment = async (id: number) => {
        setCommentData(comments => comments.filter(comment => comment.commentId !== id));
        res = await CommentServive.Delete(id.toString());
    }
    return (
        <div className='comment-body'>
            <div>
                <ListComment comments={commentData} handleDelete={deleteComment}></ListComment>
            </div>
            <AddComment comments={addComment}></AddComment>
        </div>

    );
};
export default CommentSection;
