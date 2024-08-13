import { createSlice } from '@reduxjs/toolkit';

const InitState = {
  userInfo: null,
};

const useSlice = createSlice({
	name: 'accountReducer',
	initialState: InitState,
	reducers: {
		setUserInfo: (state, action) => {
			state.userInfo = action.payload;
		},
	},
});

export const { setUserInfo } = useSlice.actions;

export default useSlice.reducer;