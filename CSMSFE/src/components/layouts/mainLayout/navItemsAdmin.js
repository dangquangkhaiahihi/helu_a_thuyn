import { v4 as uuid } from 'uuid';
// Icons
import GridViewOutlinedIcon from '@mui/icons-material/GridViewOutlined';
import BarChartOutlinedIcon from '@mui/icons-material/BarChartOutlined';
import InventoryOutlinedIcon from '@mui/icons-material/InventoryOutlined';
import PaletteOutlinedIcon from '@mui/icons-material/PaletteOutlined';
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
import AutoStoriesOutlinedIcon from '@mui/icons-material/AutoStoriesOutlined';
import WidgetsOutlinedIcon from '@mui/icons-material/WidgetsOutlined';
import WebOutlinedIcon from '@mui/icons-material/WebOutlined';
import { UrlRouteCollection } from '@/common/url-route-collection';

/**
 * @example
 * {
 *	id: number,
 *	type: "group" | "item",
 *	title: string,
 *	Icon: NodeElement
 *	menuChildren?: {title: string, href: string}[]
 *  menuMinWidth?: number
 * }
 */

const NAV_LINKS_CONFIG = [
	{
		id: uuid(),
		type: 'item',
		title: 'Quản lý người dùng',
		Icon: WebOutlinedIcon,
		href: UrlRouteCollection.UserManagement,
	},
	{
		id: uuid(),
		type: 'item',
		title: 'Quản lý chức vụ',
		Icon: WebOutlinedIcon,
		href: UrlRouteCollection.RoleManagement,
	},
];

export default NAV_LINKS_CONFIG;
