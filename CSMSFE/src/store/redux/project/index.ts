import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { Project } from '@/common/DTO/Project/ProjectDTO';
import ProjectService from '@/api/instance/project';

interface IProjectInfoState {
    projectInfo: Project | null;
}

const InitState: IProjectInfoState = {
    projectInfo: null,
};

// Async thunk
export const fetchProjectInfo = createAsyncThunk(
    'project/fetchProjectInfo',
    async ( projectId: string ) => {
        const response = await ProjectService.GetById(projectId);
        return response.content;
    }
);

const projectSlice = createSlice({
	name: 'projectReducer',
	initialState: InitState,
	reducers: {},
    extraReducers: (builder) => {
        builder.addCase(fetchProjectInfo.fulfilled, (state: IProjectInfoState, action) => {
            state.projectInfo = action.payload;
        });
        builder.addCase(fetchProjectInfo.rejected, (state) => {
            state.projectInfo = null;
        });
    },
});

export const projectInfoRedux = (state: { project: IProjectInfoState }) => state.project.projectInfo;
export default projectSlice.reducer;
