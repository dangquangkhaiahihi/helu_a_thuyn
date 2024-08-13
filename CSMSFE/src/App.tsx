import '@styles/styles.css';
import '@styles/custom-styles.css';
import '@styles/custom-react-folder-tree.css';
import "@styles/viewer-styles.module.css";
import '@fontsource/rubik/300.css';
import '@fontsource/rubik/400.css';
import '@fontsource/rubik/500.css';
import '@fontsource/rubik/700.css';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import "@cyntler/react-doc-viewer/dist/index.css";

import StoreProvider from '@/store';
import AppRouter from '@utils/routes';
import MUITheme from '@utils/theme';
import { Provider as SnackbarProvider } from '@/components/snackbar';
import CustomizationLayout from '@components/layouts/customization';

import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';

import 'dayjs/locale/vi';
import dayjs from 'dayjs';
dayjs.locale('vi');

function App() {
  return (
	<LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale={"vi"}>
		<StoreProvider>
			<MUITheme>
			<SnackbarProvider>
				<CustomizationLayout />
				<AppRouter />
			</SnackbarProvider>
			</MUITheme>
		</StoreProvider>
	</LocalizationProvider>
  );
}

export default App;
