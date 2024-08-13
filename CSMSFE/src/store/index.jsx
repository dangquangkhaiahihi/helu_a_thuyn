import { configureStore } from '@reduxjs/toolkit';
import { Provider as ReduxProvider, useSelector, useDispatch } from 'react-redux';
import themeReducer from './theme';
import appReducer from './redux/app';
import accountReducer from './redux/account';
import speckleViewerReducer from './redux/speckle-viewer';
import projectReducer from './redux/project';

const store = configureStore({
	devTools: import.meta.env.VITE_ENV !== 'production',
	reducer: {
		theme: themeReducer,
		app: appReducer,
		account: accountReducer,
		speckleViewer: speckleViewerReducer,
		project: projectReducer
	},
});

function Provider({ children }) {
	return <ReduxProvider store={store}>{children}</ReduxProvider>;
}

export { useSelector, useDispatch };
export default Provider;
