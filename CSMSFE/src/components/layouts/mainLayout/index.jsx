import { Outlet, useLocation } from 'react-router-dom';
import withScrollTopFabButton from '@utils/hocs/withScrollTopFabButton';
import WidthPageTransition from '@utils/hocs/widthPageTransition';

import StoreProvider, { useSelector } from '@/store';
import { selectThemeConfig } from '@store/theme/selectors';
// MUI
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Fab from '@mui/material/Fab';
// Icons
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';

import navItemsAdmin from './navItemsAdmin';
import navItemsUser from './navItemsUser';

// Components
import Footer from '@components/footer';
import MainHeader from '@components/mainHeader';
import Navbar from '@components/navbar';
import { isLoadingReduxCircularOverlay } from '@/store/redux/app/selectors';
import CircularLoadingOverlay from './loading-overlay';
import { routeGuard } from '@/utils/hocs/routerGuard';

function FabButton() {
	/* <Fab
		size="small"
		aria-label="scroll back to top"
		sx={{ bgcolor: 'primary.light' }}
	>
		<KeyboardArrowUpIcon color="primary" />
	</Fab> */
	return (
		<Fab size="small" aria-label="scroll back to top" color="primary">
			<KeyboardArrowUpIcon />
		</Fab>
	);
}
// navbarType = "ADMIN" | "USER" | undefined
function MainLayout({ container = 'lg', pb = true, navbarType="" }) {
	const location = useLocation();
	const { pageTransitions } = useSelector(selectThemeConfig);
	const isLoading = useSelector(isLoadingReduxCircularOverlay);

	return (
		<Box display="flex" minHeight="100vh" flexDirection="column">
			{isLoading && <CircularLoadingOverlay/>}
			<Header navbarType={navbarType}/>
			<Container
				maxWidth={container}
				component="main"
				sx={{
					flex: '1 0 auto',
					...(pb && {
						pb: 5,
					}),
				}}
			>
				{pageTransitions ? (
					<WidthPageTransition location={location.key}>
						<Outlet />
					</WidthPageTransition>
				) : (
					<Outlet />
				)}
			</Container>
			{withScrollTopFabButton(FabButton)}
			<Footer />
		</Box>
	);
}

function Header(props) {
	const { navbarType } = props;
	const { stickyHeader } = useSelector(selectThemeConfig);

	return (
		<>
			<MainHeader />
			{
				navbarType &&
				
				<Navbar navItems={
					navbarType === "ADMIN" ? navItemsAdmin : navItemsUser
				} position={stickyHeader ? 'sticky' : 'static'} />
			}
		</>
	);
}

export default routeGuard(MainLayout);
// export default MainLayout;
