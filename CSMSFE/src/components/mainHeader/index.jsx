import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import LoggedUser from './loggedUser';
import SearchBar from './searchBar';
import LogoTextCsms from '../logoTextCsms';

function MainHeader() {
	return (
		<Box bgcolor="background.paper" component="header" py={1.5} zIndex={1}>
			<Stack
				component={Container}
				maxWidth="lg"
				direction="row"
				height={50}
				justifyContent="space-between"
				alignItems="center"
				flexWrap="wrap"
				spacing={3}
				overflow="hidden"
			>
				<Stack direction="row" alignItems="center" spacing={1}>
					<LogoTextCsms />
				</Stack>
				<LoggedUser />
			</Stack>
		</Box>
	);
}

export default MainHeader;
