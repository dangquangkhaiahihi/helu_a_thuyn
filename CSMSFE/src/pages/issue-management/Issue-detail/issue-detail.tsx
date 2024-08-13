import React, { useEffect, useState } from 'react';
import { useLocation, useParams } from 'react-router-dom';
import { Container, Paper, Typography, Divider, CircularProgress } from '@mui/material';
import { Issue } from '@/common/DTO/Issue/IssueDTO';
import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';

const IssueDetail = () => {
  const { id } = useParams<{ id: string }>(); // Get the issue ID from the URL
  const [issue, setIssue] = useState<Issue | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const location = useLocation();
  const data = location.state || {} as Issue
  useEffect(() => {
    // Fetch the issue details based on the ID
    // const fetchIssue = async () => {
    //   try {
    //     // Replace with your actual API call
    //     const response = await fetch(`/issue/${id}`);
    //     if (!response.ok) {
    //       throw new Error('Failed to fetch issue details');
    //     }
    //     const data: Issue = await response.json();
    //     setIssue(data);
    //   } catch (error) {
    //     if (error instanceof Error) {
    //       setError(error.message);
    //     } else {
    //       setError('An unknown error occurred');
    //     }
    //   } finally {
    //     setLoading(false);
    //   }
    // };
    setIssue(data);
    // fetchIssue();
  }, [id]);

//   if (loading) {
//     return (
//       <Container>
//         <CircularProgress />
//         <Typography>Loading issue details...</Typography>
//       </Container>
//     );
//   }

  // if (error) {
  //   return (
  //     <Container>
  //       <Typography color="error">Error: {error}</Typography>
  //     </Container>
  //   );
  // }

  if (!issue) {
    return (
      <Container>
        <Typography>No issue found with ID {id}</Typography>
      </Container>
    );
  }

  return (
    <Container>
        <Breadcrumbs
                aria-label="breadcrumb"
                sx={{
                    marginTop: '15px',
                    textTransform: 'uppercase',
                }}
            >
                <Link underline="hover" href="#!">
                    Trang chủ
                </Link>

                <Typography color="text.tertiary">Vấn đề</Typography>
            </Breadcrumbs>

      <Paper elevation={6} style={{ padding: 60,fontFamily: 'Helvetica'}}>
        <Typography variant="h2" gutterBottom style={{ textAlign: 'center' ,fontFamily: 'Helvetica'}}>
          {issue.name}
        </Typography>
        <Divider />
                
        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Type: <a>{issue.type}</a>
        </Typography>
        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Status: {issue.status}
        </Typography>

       
        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Created By: {issue.createdBy}
        </Typography>
        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Modified By: {issue.modifiedBy}
        </Typography>


        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Created Date: {new Date(issue.createdDate).toLocaleString()}
        </Typography>
        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Modified Date: {new Date(issue.modifiedDate).toLocaleString()}
        </Typography>


        <Typography variant="subtitle1" gutterBottom style={{fontFamily: 'Helvetica'}}>
          Description: {issue.description}
        </Typography>
      </Paper>
    </Container>
  );
};

export default IssueDetail;



