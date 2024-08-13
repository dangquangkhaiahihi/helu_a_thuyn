import { lazy } from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import ScrollToTopOnRouteChange from '@utils/hocs/withScrollTopOnRouteChange';
import withLazyLoadably from '@utils/hocs/withLazyLoadably';

import MinimalLayout from '@/components/layouts/minimalLayout';
import TemplateLayout from '@/components/layouts/templateLayout';

import Page404 from '@pages/templates/errorPages/404';
import SomeTableManagement from '@pages/some-table-management';
import ProjectManagement from '@/pages/project-mamagement';

import MainLayout from '@/components/layouts/mainLayout';
import { UrlRouteCollection } from '@/common/url-route-collection';
import LoginCSMSPage from '@/pages/login';
import ViewModel from '@/pages/model-management/view-model';
import SpeckleViewerLayout from '@/components/speckleViewer/components/speckle-viwer-layout';
import HomeNavigatePage from '@/pages/home-navigate';
import ModelManagement from '@/pages/model-management';
import UserManagement from '@/pages/user-management';
import RoleManagement from '@/pages/role-management';
import DocumentManagement from '@/pages/document-management';
import IssueManagement from '@/pages/issue-management';
import ProjectSetting from '@/pages/project-mamagement/project-setting';

const IssueDetail = withLazyLoadably(lazy(() => import('@pages/issue-management/Issue-detail/issue-detail')));
const Dashboard1Page = withLazyLoadably(lazy(() => import('@pages/templates/dashboardsPages/dashboard1')));
const Dashboard2Page = withLazyLoadably(lazy(() => import('@pages/templates/dashboardsPages/dashboard2')));
const Dashboard3Page = withLazyLoadably(lazy(() => import('@pages/templates/dashboardsPages/dashboard3')));
const Dashboard4Page = withLazyLoadably(lazy(() => import('@pages/templates/dashboardsPages/dashboard4')));
const Dashboard5Page = withLazyLoadably(lazy(() => import('@pages/templates/dashboardsPages/dashboard5')));
const FormsComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/forms')));
const LoadersComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/loaders')));
const TablesComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/tables')));
const ModalComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/modal')));
const SnackbarComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/snackbar')));
const CarouselComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/carousel')));
const NavigationComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/componentsPages/navigation')));
const CardComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/uiComponentsPages/card')));
const CardHeaderComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/uiComponentsPages/cardHeader')));
const PageHeaderComponentPage = withLazyLoadably(lazy(() => import('@pages/templates/uiComponentsPages/pageHeader')));
const LoginPage = withLazyLoadably(lazy(() => import('@pages/templates/template/loginPages/login')));
const LoginSimplePage = withLazyLoadably(lazy(() => import('@pages/templates/template/loginPages/loginSimple')));
const LoginSplitPage = withLazyLoadably(lazy(() => import('@pages/templates/template/loginPages/loginSplit')));
const SignupSplitPage = withLazyLoadably(lazy(() => import('@pages/templates/template/signupPages/signupSplit')));
const SignupSimplePage = withLazyLoadably(lazy(() => import('@pages/templates/template/signupPages/signupSimple')));
const SignupPage = withLazyLoadably(lazy(() => import('@pages/templates/template/signupPages/signup')));
const Page403 = withLazyLoadably(lazy(() => import('@pages/templates/errorPages/403')));
const Page500 = withLazyLoadably(lazy(() => import('@pages/templates/errorPages/500')));
const Page503 = withLazyLoadably(lazy(() => import('@pages/templates/errorPages/503')));
const Page505 = withLazyLoadably(lazy(() => import('@pages/templates/errorPages/505')));
const Pricing1Page = withLazyLoadably(lazy(() => import('@pages/templates/pricingPages/pricing1')));
const Pricing2Page = withLazyLoadably(lazy(() => import('@pages/templates/pricingPages/pricing2')));
const EditProfilePage = withLazyLoadably(lazy(() => import('@pages/templates/editProfile')));
const NotificationsPage = withLazyLoadably(lazy(() => import('@pages/templates/notificationsPage')));
const SamplePage = withLazyLoadably(lazy(() => import('@pages/templates/sample')));
const ThemeTypographyPage = withLazyLoadably(lazy(() => import('@pages/templates/themePages/themeTypography')));
const ThemeColorsPage = withLazyLoadably(lazy(() => import('@pages/templates/themePages/themeColors')));
const ThemeShadowPage = withLazyLoadably(lazy(() => import('@pages/templates/themePages/themeShadow')));

const Comment = withLazyLoadably(lazy(() => import('@pages/comment')));
// const Document = withLazyLoadably(lazy(() => import('@pages/document')));

function AppRouter() {
	return (
		<RouterProvider router={routes} />
	);
}

export default AppRouter;

const routes = createBrowserRouter([
	{
		element: <ScrollToTopOnRouteChange />,
		path: "/",
		children: [
			{
				path: UrlRouteCollection.Login, // "/login"
				element: <MinimalLayout />,
				children: [
					{
						path: "",
						element: <LoginCSMSPage />,
					}
				],
			},
			{
				path: UrlRouteCollection.ProjectModelViewer, // "/du-an/:projectId/mo-hinh/:modelId"
				element: <SpeckleViewerLayout />,
				children: [
					{
						path: "",
						element: <ViewModel />,
					}
				],
			},
			{
				path: UrlRouteCollection.Home, // "/"
				element: <MainLayout />,
				children: [
					{
						path: "",
						element: <HomeNavigatePage />,
					},
					{
						path: UrlRouteCollection.ProjectManagement, // "/du-an"
						element: <ProjectManagement />,
					},
				]
			},
			{
				path: UrlRouteCollection.Home, // "/"
				element: <MainLayout navbarType="USER"/>,
				children: [
					{
						path: UrlRouteCollection.ProjectDashboard, // "/du-an/:projectId"
						element: <div>ProjectDashboard</div>
					},
					{
						path: UrlRouteCollection.ProjectModelManagement, // "/du-an/:projectId/mo-hinh"
						element: <ModelManagement />
					},
					{
						path: UrlRouteCollection.ProjectDocumentManagement, // "/du-an/:projectId/tai-lieu"
						element: <DocumentManagement />
					},
					{
						path: UrlRouteCollection.ProjectIssueManagement, // "/du-an/:projectId/van-de"
						element: <IssueManagement />
					},
					{
						path: UrlRouteCollection.ProjectIssueDetail, // "/du-an/:projectId/van-de/issueID"
						element: <IssueDetail />
					},
					{
						path: UrlRouteCollection.ProjectSetting, // "/du-an/:projectId/cai-dat"
						element: <ProjectSetting />
					},
					{
						path: "error",
						children: [
							{
								path: "404",
								element: <Page404/>,
							},
							{
								path: "403",
								element: <Page403/>,
							},
							{
								path: "500",
								element: <Page500/>,
							},
							{
								path: "503",
								element: <Page503/>,
							},
							{
								path: "505",
								element: <Page505/>,
							}
						]
					},
				]
			},
			{
				path: UrlRouteCollection.Home, // "/"
				element: <MainLayout navbarType="ADMIN"/>,
				children: [
					{
						path: UrlRouteCollection.UserManagement, // "/quan-ly-tai-khoan"
						element: <UserManagement/>
					},
					{
						path: UrlRouteCollection.RoleManagement, // "/quan-ly-chuc-vu"
						element: <RoleManagement/>
					},
				]
			}
		],
	},
	{
		element: <ScrollToTopOnRouteChange />,
		path: "/template",
		children: [
			{
				path: "",
				element: <TemplateLayout />,
				children: [
				
					{
						path: "",
						element: <SomeTableManagement />,
					},
					{
						path: "samplePage",
						element: <SamplePage />,
					},
					{
						path: "dashboard1",
						element: <Dashboard1Page />,
					},
					{
						path: "dashboard2",
						element: <Dashboard2Page />,
					},
					{
						path: "dashboard3",
						element: <Dashboard3Page />,
					},
					{
						path: "dashboard4",
						element: <Dashboard4Page />,
					},
					{
						path: "dashboard5",
						element: <Dashboard5Page />,
					},
					{
						path: "Comment",
						element: <Comment />
					}
				]
			},
			{
				path: "components",
				element: <TemplateLayout />,
				children: [
					{
						path: "",
						element: <div>component</div>,
					},
					{
						path: "forms",
						element: <FormsComponentPage />,
					},
					{
						path: "loaders",
						element: <LoadersComponentPage />,
					},
					{
						path: "tables",
						element: <TablesComponentPage />,
					},
					{
						path: "modal",
						element: <ModalComponentPage />,
					},
					{
						path: "snackbar",
						element: <SnackbarComponentPage />,
					},
					{
						path: "carousel",
						element: <CarouselComponentPage />,
					},
					{
						path: "navigation",
						element: <NavigationComponentPage />,
					},
					{
						path: "card",
						element: <CardComponentPage />,
					},
					{
						path: "cardHeader",
						element: <CardHeaderComponentPage />,
					},
					{
						path: "pageHeader",
						element: <PageHeaderComponentPage />,
					},
				]
			},
		
			{
				path: "theme",
				element: <TemplateLayout />,
				children: [
					{
						path: "",
						element: <div>theme</div>,
					},
					{
						path: "typography",
						element: <ThemeTypographyPage />,
					},
					{
						path: "colors",
						element: <ThemeColorsPage />,
					},
					{
						path: "boxShadow",
						element: <ThemeShadowPage />,
					}
				]
			},
			{
				path: "pages",
				element: <TemplateLayout />,
				children: [
					{
						path: "",
						element: <div>pages</div>,
					},
					{
						path: "settings",
						element: <EditProfilePage />,
					},
					{
						path: "notifications",
						element: <NotificationsPage />,
					},
					{
						path: "pricing",
						children: [
							{
								path: "",
								element: <div>pages pricing</div>,
							},
							{
								path: "pricing1",
								element: <Pricing1Page />,
							},
							{
								path: "pricing2",
								element: <Pricing2Page />,
							},
						]
					}
				]
			},
			{
				path: "login",
				element: <MinimalLayout />,
				children: [
					{
						path: "",
						element: <LoginPage />,
					},
					{
						path: "simple",
						element: <LoginSimplePage />,
					},
					{
						path: "split",
						element: <LoginSplitPage />,
					}
				],
			},
			{
				path: "signup",
				element: <MinimalLayout />,
				children: [
					{
						path: "",
						element: <SignupPage />,
					},
					{
						path: "simple",
						element: <SignupSimplePage />,
					},
					{
						path: "split",
						element: <SignupSplitPage />,
					}
				],
			},
		],
	},
	{
		path: "*", // Wildcard path for unmatched routes
		element: <Page404 />,
	},
		

]);