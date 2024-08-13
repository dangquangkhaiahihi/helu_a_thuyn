
import React, { ChangeEvent } from 'react';
import IconButton from '@mui/material/IconButton';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';

import { Project } from '@/common/DTO/Project/ProjectDTO';
import dayjs from 'dayjs';
import RelativeTime from "dayjs/plugin/relativeTime"
import { Box, Card, CardContent, CardMedia, FormControl, Grid, LinearProgress, MenuItem, Select, SelectChangeEvent, Stack, TablePagination, Typography } from '@mui/material';
import Link from '@mui/material/Link';
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import { DEFAULT_PAGE_INDEX  } from '@/common/default-config';
import { UrlRouteCollection } from '@/common/url-route-collection';

dayjs.extend(RelativeTime);

interface IListProjectProps {
    data: Project[];
    totalItemCount: number;
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

const ListProject: React.FC<IListProjectProps>  = (props) => {
    const {
        data,
        totalItemCount,
        isLoading,
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

	return (
        <Stack spacing={1}>
            <FormControl sx={{ m: 1, minWidth: 120 }} size="small">
                <Stack spacing={1} direction={"row"} justifyContent={"flex-end"}>
                    <Typography gutterBottom variant="h5" component="div" sx={{
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
                direction="row"
                alignItems="center"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                maxWidth={"100%"}
            >
                
                {
                    data.map((item, index) => (
                        <Grid key={index} item xs={12} md={6} lg={4} mb={2}>
                            <ProjectItem projectItem={item}/>
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
        </Stack>
	);
}

interface IProjectItemProps {
    projectItem: Project;
}

const ProjectItem: React.FC<IProjectItemProps> = ({
    projectItem
}) => {
    return (
        <Card variant="cardHover">
            <CardMedia
                sx={{
                    height: 140,
                    backgroundPosition: '50%',
                    backgroundSize: 'contain'
                }}
                image="/assets/images/avatars/default-project-ava.png"
                title={projectItem.name}
            />
            <CardContent>
                <Typography gutterBottom variant="h3"
                    component="div"
                    sx={{
                        cursor: 'pointer',
                            '&:hover': {
                            color: '#1560BD',
                            textDecoration: 'underline'
                        }
                    }}
                >
                    <Link underline="hover" href={`${UrlRouteCollection.ProjectModelManagement.replace(":projectId", projectItem.id)}`} color={"inherit"}>{projectItem.name}</Link>
                </Typography>
                <Typography gutterBottom variant="subtitle2" component="div" sx={{
                    display: 'flex',
                    alignItems: 'end',
                }}>
                    <AccountCircleOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> {projectItem.typeProjectName}
                </Typography>
                <Typography gutterBottom variant="subtitle2" component="div" sx={{
                    display: 'flex',
                    alignItems: 'end',
                }}>
                    <AccountCircleOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> người sở hữu
                </Typography>
                <Typography gutterBottom variant="subtitle2" component="div" sx={{
                    display: 'flex',
                    alignItems: 'end',
                }}>
                    <AccessTimeOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> cập nhật&nbsp;
                    <Box component="span" sx={{ fontWeight: 'bold' }}>{dayjs(projectItem.modifiedDate).fromNow()}</Box>
                </Typography>
            </CardContent>
        </Card>
    )
}

export default ListProject;