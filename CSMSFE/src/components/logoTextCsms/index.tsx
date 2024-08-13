import { UrlRouteCollection } from '@/common/url-route-collection';
import Box from '@mui/material/Box';
import { Link } from 'react-router-dom';

function LogoTextCsms() {

	return (
		<Box
			width={150}
			sx={{
				fontFamily: "EpilepsjaFill",
				height: '34.6px',
				fontSize: '45px',
				lineHeight: '34.6px',
				background: '-webkit-linear-gradient(0deg, #2B3C8E, #0C8CD2)',
				WebkitBackgroundClip: 'text',
    			WebkitTextFillColor: 'transparent',
				cursor: 'pointer'
			}}
			component={Link} to={`${UrlRouteCollection.Home}`}
		>
			<span style={{display: 'block', marginTop: '-18px'}}>csms</span>
		</Box>
	);
}

export default LogoTextCsms;
