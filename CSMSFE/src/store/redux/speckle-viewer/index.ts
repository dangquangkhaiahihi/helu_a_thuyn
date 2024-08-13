import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import ModelService from '@/api/instance/model';
import { ModelSpeckleViewInfoDTO, ModelSpeckleViewInfoRequest } from '@/common/DTO/Model/ModelSpeckleViewInfoDTO';

interface ISpeckleViewerState {
    loading: boolean;
    speckleModelInfo: ModelSpeckleViewInfoDTO | null;
}

const InitState: ISpeckleViewerState = {
    loading: false,
    speckleModelInfo: null,
};

// Async thunk
export const fetchSpeckleModelsInfo = createAsyncThunk(
    'speckleViewer/fetchSpeckleModelsInfo',
    async ( data: ModelSpeckleViewInfoRequest ) => {
        const response = await ModelService.GetSpeckleModelsInfo(data);
        return response.content;
    }
);

export const fetchFullSpeckleModelsInfo = createAsyncThunk(
    'speckleViewer/fetchFullSpeckleModelsInfo',
    async ( projectId: string ) => {
        const response = await ModelService.GetFullSpeckleModelsInfo(projectId);
        return response.content;
    }
);

const speckleViewerSlice = createSlice({
	name: 'speckleViewerReducer',
	initialState: InitState,
	reducers: {},
    extraReducers: (builder) => {
        builder.addCase(fetchSpeckleModelsInfo.pending, (state) => {
            state.loading = true;
        });
        builder.addCase(fetchSpeckleModelsInfo.fulfilled, (state: ISpeckleViewerState, action) => {
            state.loading = false;
            
            action.payload.speckleModelInfos.map(item => {
                // Có 1 version thì xem của đúng objectId của version đấy
                if ( item.speckleVersionInfos.length === 1 ) {
                    item.id = `${import.meta.env.VITE_SPECKLE_DOMAIN}/streams/${action.payload.speckleProjectId}/objects/${item.speckleVersionInfos[0].objectId}`
                } else {
                    // Chứa @ tức là đang xem version nào chứ ko phải version mới nhất
                    if ( item.speckleModelId.includes("@") ) {
                        // item.speckleModelId = speckleBranchId@speckleCommitId
                        const speckleCommitId = item.speckleModelId.split("@")[1];
                        item.id = `${import.meta.env.VITE_SPECKLE_DOMAIN}/streams/${action.payload.speckleProjectId}/objects/${item.speckleVersionInfos.find(version => version.commitId === speckleCommitId)?.objectId}`    
                    }
                    // Có nhiều version mà ko chứa @ tức là đang xem ở version mới nhất, cứ lấy objectId của version đứng đầu tiên (BE đã sort cho rồi)
                    else {
                        item.id = `${import.meta.env.VITE_SPECKLE_DOMAIN}/streams/${action.payload.speckleProjectId}/objects/${item.speckleVersionInfos[0].objectId}`
                    }
                }
            });

            state.speckleModelInfo = action.payload;
        });
        builder.addCase(fetchSpeckleModelsInfo.rejected, (state) => {
            state.loading = false;
            state.speckleModelInfo = null;
        });
        //
        builder.addCase(fetchFullSpeckleModelsInfo.pending, (state) => {
            state.loading = true;
        });
        builder.addCase(fetchFullSpeckleModelsInfo.fulfilled, (state, action) => {
            state.loading = false;
            state.speckleModelInfo = action.payload;
        });
        builder.addCase(fetchFullSpeckleModelsInfo.rejected, (state) => {
            state.loading = false;
            state.speckleModelInfo = null;
        });
    },
});

export const speckleModelInfoRedux = (state: { speckleViewer: ISpeckleViewerState }) => state.speckleViewer.speckleModelInfo;
export const speckleViewerLoadingRedux = (state: { speckleViewer: ISpeckleViewerState }) => state.speckleViewer.loading;
export default speckleViewerSlice.reducer;
