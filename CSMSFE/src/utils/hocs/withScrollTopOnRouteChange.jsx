import { useEffect } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import scrollToTop from '@/utils/helpers/scrollToTop';

function WithScrollTopOnRouteChange() {
	const location = useLocation();
	const { pathname } = location;

	console.log("path", pathname);

	useEffect(() => {
		scrollToTop();
	}, [pathname]);

	return <Outlet />
}

export default WithScrollTopOnRouteChange;