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
import ViewInArIcon from '@mui/icons-material/ViewInAr';
import FolderIcon from '@mui/icons-material/Folder';
import SettingsIcon from '@mui/icons-material/Settings';

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
		title: 'Mô hình',
		Icon: ViewInArIcon,
		href: UrlRouteCollection.ProjectModelManagement,
	},
	{
		id: uuid(),
		type: 'item',
		title: 'Vấn đề',
		Icon: WebOutlinedIcon,
		href: UrlRouteCollection.ProjectIssueManagement,
	},
	{
		id: uuid(),
		type: 'item',
		title: 'Tài liệu',
		Icon: FolderIcon,
		href: UrlRouteCollection.ProjectDocumentManagement,
	},
	{
		id: uuid(),
		type: 'item',
		title: 'Cài đặt',
		Icon: SettingsIcon,
		href: UrlRouteCollection.ProjectSetting,
	},
];

export default NAV_LINKS_CONFIG;
