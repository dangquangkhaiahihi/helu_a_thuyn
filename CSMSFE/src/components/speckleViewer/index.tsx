import { useEffect } from 'react';
import {
    Viewer,
    DefaultViewerParams,
    SpeckleLoader,
    SelectionExtension,
    MeasurementsExtension,
    UrlHelper,
    // FilteringExtension,
    // ViewerEvent,
} from "@speckle/viewer";
import { CameraController } from "@speckle/viewer";
import { BoxSelection } from './extension/BoxSelection'; 
import { PassReader } from './extension/PassReader'; 
import FunctionButtons from './components/function-buttons';
import { useSpeckleViewer } from './context/SpeckleViewerContext';
import { viewAlert, closeAlert } from '@/common/tools';

interface ISpeckleViewer {
    speckleProjectId: string,
    speckleModelId: string
}

const SpeckleViewer: React.FC<ISpeckleViewer> = ( props ) => {
    const {
        speckleProjectId,
        speckleModelId
    } = props;
    
    const { viewer, setViewer } = useSpeckleViewer();

    useEffect(() => {
        initViewer();
    }, [])

    useEffect(() => {
        loadObject();
    }, [viewer])
    
    const initViewer = async () => {
        if (viewer) return;

        const container = document.getElementById("renderer") as HTMLElement;

        /** Configure the viewer params */
        const params = DefaultViewerParams;
      
        /** Create Viewer instance */
        const speckleViewer = new Viewer(container, params);
        /** Initialise the viewer */
        await speckleViewer.init();
        /** Add the stock camera controller extension */
        speckleViewer.createExtension(CameraController);
        /** Add the measurement extension for extra interactivity */
        speckleViewer.createExtension(MeasurementsExtension);
        /** Add the selection extension for extra interactivity */
        speckleViewer.createExtension(SelectionExtension);
        /** Add our custom made extension */
        speckleViewer.createExtension(BoxSelection);
        speckleViewer.createExtension(PassReader);

        setViewer(speckleViewer);
    }

    const loadObject = async () => {
        if (!viewer) return;
    
        try {
            const urls = await UrlHelper.getResourceUrls(
                `${import.meta.env.VITE_SPECKLE_DOMAIN}/projects/${speckleProjectId}/models/${speckleModelId}`
            );

            const loaders = urls.map((url, index) => {
                const loader = new SpeckleLoader(
                    viewer.getWorldTree(),
                    url,
                    `Bearer ${import.meta.env.VITE_SPECKLE_TOKEN_READ}`
                );
    
                const loadingAlertId = viewAlert(`Đang tải mô hình ${index + 1}/${urls.length}`, 'loading');
    
                return viewer.loadObject(loader, true)
                    .then(() => {
                        viewAlert(`Tải mô hình ${index + 1}/${urls.length} thành công`, 'success');
                    })
                    .catch((_error) => {
                        viewAlert(`Tải mô hình ${index + 1}/${urls.length} thất bại`, 'error');
                    })
                    .finally(() => {
                        closeAlert(loadingAlertId);
                    });
            });
    
            await Promise.allSettled(loaders);
    
        } catch (err) {
            viewAlert(`Tải mô hình thất bại ${err}`, 'error');
        }
    };
    

    return (
        <div>
            <div id="renderer" style={{
                width:'100%',height:'100%',left:'0px',top:'0px',position:'absolute'
            }}/>
            <FunctionButtons />
        </div>
    )
}

export default SpeckleViewer;