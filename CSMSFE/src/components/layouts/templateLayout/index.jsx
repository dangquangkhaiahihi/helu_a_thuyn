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

import navItems from './navItems';

// Components
import Footer from '@components/footer';
import MainHeader from '@components/mainHeader';
import Navbar from '@components/navbar';
import { isLoadingReduxCircularOverlay } from '@/store/redux/app/selectors';
import CircularLoadingOverlay from './loading-overlay';

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
function TemplateLayout({ container = 'lg', pb = true }) {
	const location = useLocation();
	const { pageTransitions } = useSelector(selectThemeConfig);
	const isLoading = useSelector(isLoadingReduxCircularOverlay);

	return (
		<Box display="flex" minHeight="100vh" flexDirection="column">
			{isLoading && <CircularLoadingOverlay/>}
			<Header />
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

function Header() {
	const { stickyHeader } = useSelector(selectThemeConfig);

	return (
		<>
			<MainHeader />
			<Navbar navItems={navItems} position={stickyHeader ? 'sticky' : 'static'} />
		</>
	);
}

export default TemplateLayout;
