// MUI
// import Typography from '@mui/material/Typography';
// import TextField from '@mui/material/TextField';
// import Button from '@mui/material/Button';
// import CircularProgress from '@mui/material/CircularProgress';
import { UrlRouteCollection } from '@/common/url-route-collection';
import { Breadcrumbs, CardContent, CardMedia, Grid, Link, Typography } from '@mui/material';
import Card from '@mui/material/Card';


function HomeNavigatePage() {
	return (
		<>
			<Breadcrumbs
				aria-label="breadcrumb"
				sx={{
					marginY: '15px',
					textTransform: 'uppercase',
				}}
			>
				<Typography color="text.tertiary">Trang chủ</Typography>
			</Breadcrumbs>

			<Grid container
                direction="row"
                alignItems="center"
                flex={{ md: '1', xs: 'none' }}
                columnSpacing={2}
                maxWidth={"100%"}
            >
                <Grid item xs={12} md={6} mb={2}>
					<Card
						elevation={20}
						sx={{
							display: 'block',
							width: '100%',
							paddingBottom: '16px'
						}}
					>
						<Link underline="hover" href={`${UrlRouteCollection.ProjectManagement}`} color={"inherit"}>
							<CardMedia
								sx={{
									height: 140,
									backgroundPosition: '50%',
									backgroundSize: 'contain'
								}}
								image="/assets/images/avatars/default-project-ava.png"
								title={"Danh sách dự án"}
							/>
							<CardContent>
								<Typography gutterBottom variant="h3"
									component="div"
									align='center'
									sx={{
										cursor: 'pointer',
											'&:hover': {
											color: '#1560BD',
											textDecoration: 'underline'
										}
									}}
								>
									Danh sách dự án
								</Typography>
							</CardContent>
						</Link>
					</Card>
				</Grid>

				<Grid item xs={12} md={6} mb={2}>
					<Card
						elevation={20}
						sx={{
							display: 'block',
							width: '100%',
							paddingBottom: '16px'
						}}
					>
						<Link underline="hover" href={`${UrlRouteCollection.UserManagement}`} color={"inherit"}>
							<CardMedia
								sx={{
									height: 140,
									backgroundPosition: '50%',
									backgroundSize: 'contain'
								}}
								image="/assets/images/avatars/admin-ava.png"
								title={"Quản trị hệ thống"}
							/>
							<CardContent>
								<Typography gutterBottom variant="h3"
									component="div"
									align='center'
									sx={{
										cursor: 'pointer',
											'&:hover': {
											color: '#1560BD',
											textDecoration: 'underline'
										}
									}}
								>
									Quản trị hệ thống
								</Typography>
							</CardContent>
						</Link>
					</Card>
				</Grid>
            </Grid>
		</>
	);
}

export default HomeNavigatePage;