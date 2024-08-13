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
		type: 'group',
		title: 'Dashboard',
		Icon: BarChartOutlinedIcon,
		menuChildren: [
			{
				title: 'Dashboard01',
				href: '/template/dashboard1',
			},
			{
				title: 'Dashboard02',
				href: '/template/dashboard2',
			},
			{
				title: 'Dashboard03',
				href: '/template/dashboard3',
			},
			{
				title: 'Dashboard04',
				href: '/template/dashboard4',
			},
			{
				title: 'Dashboard05',
				href: '/template/dashboard5',
			},
		],
	},
	{
		id: uuid(),
		type: 'group',
		title: 'Components',
		Icon: GridViewOutlinedIcon,
		menuChildren: [
			{
				title: 'Forms',
				href: '/template/components/forms',
			},
			{
				title: 'Tables',
				href: '/template/components/tables',
			},
			{
				title: 'Modal',
				href: '/template/components/modal',
			},
			{
				title: 'Loaders',
				href: '/template/components/loaders',
			},
			{
				title: 'Snackbar/Toast',
				href: '/template/components/snackbar',
			},
			{
				title: 'Carousel',
				href: '/template/components/carousel',
			},
			{
				title: 'Navigation',
				// navbar
				href: '/template/components/navigation',
			},
			{
				title: 'UI Elements',
				type: 'group',
				menuChildren: [
					{
						title: 'Card',
						href: '/template/components/card',
					},
					{
						title: 'CardHeader',
						href: '/template/components/cardHeader',
					},
					{
						title: 'PageHeader',
						href: '/template/components/pageHeader',
					},
					/* {
						title: 'Paper',
						href: '/template/components/ui/paper',
					}, 
					{
						title: 'Buttons',
						href: '/template/components/buttons',
					},
					*/
				],
			},

			{
				title: 'Level 0',
				type: 'group',
				menuChildren: [
					{
						title: 'Level 1a',
						href: '/template/1a',
					},
					{
						title: 'Level 1b',
						type: 'group',
						menuChildren: [
							{
								title: 'Level 2a',
								href: '/template/2a',
							},
							{
								title: 'Level 2b',
								href: '/template/2b',
							},
							{
								title: 'Level 2c',
								type: 'group',
								menuChildren: [
									{
										title: 'Level 3a',
										href: '/template/3a',
									},
									{
										title: 'Level 3b',
										type: 'group',
										menuChildren: [
											{
												title: 'Level 4a',
												href: '/template/3b',
											},
										],
									},
									{
										title: 'Level 3c',
										href: '/template/3c',
									},
								],
							},
						],
					},
					{
						title: 'Level 1c',
						href: '/template/1c',
					},
				],
			},
		],
	},
	{
		id: uuid(),
		type: 'group',
		title: 'Pages',
		Icon: AutoStoriesOutlinedIcon,
		menuChildren: [
			{
				id: uuid(),
				title: 'Sign in',
				type: 'group',
				menuChildren: [
					{
						title: 'Sign in',
						href: '/template/login',
					},
					{
						title: 'Sign in Simple',
						href: '/template/login/simple',
					},
					{
						title: 'Sign in Split',
						href: '/template/login/split',
					},
				],
			},
			{
				id: uuid(),
				title: 'Sign up',
				type: 'group',
				menuChildren: [
					{
						title: 'Sign up',
						href: '/template/signup',
					},
					{
						title: 'Sign up Simple',
						href: '/template/signup/simple',
					},
					{
						title: 'Sign up Split',
						href: '/template/signup/split',
					},
				],
			},
			{
				title: 'Account Settings',
				href: '/template/pages/settings',
			},
			{
				title: 'Notifications',
				href: '/template/pages/notifications',
			},
			{
				id: uuid(),
				title: 'Error Pages',
				type: 'group',
				menuChildren: [
					{
						title: '403 Unauthorized',
						href: '/error/403',
					},
					{
						title: '404 Not Found',
						href: '/error/404',
					},
					{
						title: '500 Internal Server',
						href: '/error/500',
					},
					{
						title: '503 Service Unavailable',
						href: '/error/503',
					},
					{
						title: '505 Forbidden',
						href: '/error/505',
					},
				],
			},
			{
				id: uuid(),
				title: 'Pricing Pages',
				type: 'group',
				menuChildren: [
					{
						title: 'Pricing 1',
						href: '/template/pages/pricing/pricing1',
					},
					{
						title: 'Pricing 2',
						href: '/template/pages/pricing/pricing2',
					},
				],
			},
		],
	},
	{
		id: uuid(),
		type: 'group',
		title: 'Theme',
		Icon: PaletteOutlinedIcon,
		menuChildren: [
			{
				title: 'Paleta de Colores',
				href: '/template/theme/colors',
			},
			{
				title: 'Tipografia',
				href: '/template/theme/typography',
			},
			{
				title: 'Sombras',
				href: '/template/theme/boxShadow',
			},

			/* {
				title: 'Iconos',
				href: '/template/theme/icons',
			}, */
			// libraries/ packgaes ej.> moment
		],
	},
	{
		id: uuid(),
		type: 'item',
		title: 'Sample Tab',
		Icon: WebOutlinedIcon,
		href: '/template/samplePage',
	},
];

export default NAV_LINKS_CONFIG;
