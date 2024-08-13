import { createSlice } from '@reduxjs/toolkit';

const InitState = {
  isLoading: false,
};

const useSlice = createSlice({
	name: 'appReducer',
	initialState: InitState,
	reducers: {
		setLoadingCircularOverlayRedux: (state, action) => {
			state.isLoading = action.payload;
		},
	},
});

export const { setLoadingCircularOverlayRedux } = useSlice.actions;

export default useSlice.reducer;