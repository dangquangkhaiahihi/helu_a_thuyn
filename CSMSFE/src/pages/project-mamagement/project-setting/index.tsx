import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';

import Breadcrumbs from '@mui/material/Breadcrumbs';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import { fetchProjectInfo, projectInfoRedux } from '@/store/redux/project';
import { UrlRouteCollection } from '@/common/url-route-collection';
import PageHeader from '@/components/pageHeader';
import { Box, Grid, Tab, Tabs } from '@mui/material';
import ProjectSettingInfo from './project-setting-info';
import ProjectSettingCollaborator from './project-setting-collaborator';
import ProjectSettingRole from './project-setting-role';

const ProjectSetting = ({  }: any) => {
    const { projectId } = useParams();
    const dispatch = useDispatch();
    const projectInfo = useSelector(projectInfoRedux);

    useEffect(() => {
        if ( projectInfo ) return;
        dispatch(fetchProjectInfo(projectId || ""));
    }, [])

    const [value, setValue] = useState(0);

    const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

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
                <Typography color="text.tertiary">Cài đặt</Typography>
            </Breadcrumbs>

            <PageHeader title="Cài đặt"><></></PageHeader>

            <Grid container
                direction="row"
                alignItems="start"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                maxWidth={"100%"}
            >
                <Grid item xs={12} md={2}>
                    <Tabs
                        orientation="vertical"
                        variant="scrollable"
                        value={value}
                        onChange={handleChange}
                        aria-label="Vertical tabs example"
                        sx={{ borderRight: 1, borderColor: 'divider' }}
                    >
                        <Tab label="Thông tin dự án" {...a11yProps(0)} />
                        <Tab label="Danh sách chức vụ" {...a11yProps(1)} />
                        <Tab label="Cộng tác" {...a11yProps(2)} />
                    </Tabs>
                </Grid>
                <Grid item xs={12} md={10}>
                    <TabPanel value={value} index={0}>
                            <ProjectSettingInfo />
                        </TabPanel>
                        <TabPanel value={value} index={1}>
                            <ProjectSettingRole />
                        </TabPanel>
                        <TabPanel value={value} index={2}>
                            <ProjectSettingCollaborator />
                        </TabPanel>
                </Grid>
            </Grid>
        </>
    );
}

export default ProjectSetting;

function a11yProps(index: number) {
    return {
        id: `vertical-tab-${index}`,
        'aria-controls': `vertical-tabpanel-${index}`,
    };
}

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;
  
    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            style={{width: '100%'}}
            {...other}
        >
            {value === index && (
                <Box sx={{ p: 3, pt: 0 }}>
                    {children}
                </Box>
            )}
        </div>
    );
}