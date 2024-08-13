import { Outlet } from 'react-router-dom';
// MUI
import Box from '@mui/material/Box';

// Components
import MainHeader from '@components/mainHeader';
import { useSelector } from 'react-redux';
import CircularLoadingOverlay from '@/components/layouts/mainLayout/loading-overlay';
import { speckleViewerLoadingRedux } from '@/store/redux/speckle-viewer';
// import { routeGuard } from '@/utils/hocs/routerGuard';

function SpeckleViewerLayoutWithoutRouterGuard() {
	const isLoading = useSelector(speckleViewerLoadingRedux);

	return (
		<Box display="flex" minHeight="100vh" flexDirection="column">
			{isLoading && <CircularLoadingOverlay/>}
			<MainHeader />
			<Box
				component="main"
			>
				<Outlet />
			</Box>
		</Box>
	);
}

// const SpeckleViewerLayout = routeGuard(SpeckleViewerLayoutWithoutRouterGuard);
const SpeckleViewerLayout = SpeckleViewerLayoutWithoutRouterGuard;

export default SpeckleViewerLayout;