import SpeckleViewer from "@/components/speckleViewer";
import { SpeckleViewerProvider } from "@/components/speckleViewer/context/SpeckleViewerContext";
import { fetchFullSpeckleModelsInfo, fetchSpeckleModelsInfo, speckleModelInfoRedux } from "@/store/redux/speckle-viewer";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";

const ViewModel = () => {
    const { projectId, modelId } = useParams();

    const dispatch = useDispatch();

    const handleGetSpeckleModelsInfo = async () => {
        dispatch(fetchSpeckleModelsInfo({
            projectId: projectId || "",
            requestModelsInfo: modelId || ""
        }));
    }

    const handleGetFullSpeckleModelsInfo = async () => {
        dispatch(fetchFullSpeckleModelsInfo(projectId || ""));
    }

    const speckleModelInfo = useSelector(speckleModelInfoRedux);

    useEffect(() => {
        if ( modelId === "all" ) {
            handleGetFullSpeckleModelsInfo();
        } else {
            handleGetSpeckleModelsInfo();
        }
    }, [])

    const [speckleModelId, setSpeckleModelId] = useState<string>("");
    useEffect(() => {
        if ( !speckleModelInfo ) return;
        const joinedModelIds = speckleModelInfo.speckleModelInfos.map(info => info.speckleModelId).join(',');
        setSpeckleModelId(joinedModelIds);
    }, [speckleModelInfo])

	return (
        <SpeckleViewerProvider>
            {
                speckleModelInfo && speckleModelId ? (
                    <SpeckleViewer
                        speckleProjectId={speckleModelInfo.speckleProjectId}
                        speckleModelId={speckleModelId}
                    />
                ) : <></>
            }
        </SpeckleViewerProvider>
	);
}

export default ViewModel;