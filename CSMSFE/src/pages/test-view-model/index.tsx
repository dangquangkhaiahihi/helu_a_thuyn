import { useEffect } from 'react';
import {
    Viewer,
    DefaultViewerParams,
    SpeckleLoader,
    SelectionExtension,
    MeasurementsExtension,
} from "@speckle/viewer";
import { CameraController } from "@speckle/viewer";
import { BoxSelection } from "./BoxSelection";
import { PassReader } from "./PassReader";

const TestViewModel = ({  }) => {
    const init = async () => {
        const container = document.getElementById("renderer") as HTMLElement;

        /** Configure the viewer params */
        const params = DefaultViewerParams;
        params.showStats = true;
        params.verbose = true;
      
        /** Create Viewer instance */
        const viewer = new Viewer(container, params);
        /** Initialise the viewer */
        await viewer.init();
        /** Add the stock camera controller extension */
        viewer.createExtension(CameraController);
        /** Add the selection extension for extra interactivity */
        viewer.createExtension(SelectionExtension);
        /** Add our custom made extension */
        viewer.createExtension(BoxSelection);
      
        viewer.createExtension(PassReader);

        viewer.createExtension(MeasurementsExtension);
      
        /** Create a loader for the speckle stream */
        // c6070bf62fb588a1ffe81e38a5d0ffae31b963678c
        // 5ccbd1d6dc8075bdbd9bc9b350ca8f6a1c6f9914ed
        const loader = new SpeckleLoader(
          viewer.getWorldTree(),
        //   "https://latest.speckle.dev/streams/f92e060177/objects/f93235ab379382b3f0c5eec7aeea3ed9",
        //   "http://127.0.0.1:3000/streams/bed0cf801f/objects/84941332d82ff3c39b05a23aa00c74fa",
        
            "https://app.speckle.systems/streams/0a7a25746a/objects/6abcf96123e84ba2506f93b0c529f10a",
            "Bearer 8bf6edc20860ac496ca4f4f63655244ac3cdbc14be"
        );
        /** Load the speckle data */
        await viewer.loadObject(loader, true);
    }


    useEffect(() => {
        init();
    }, [])

    return (
        <div>
            <h1>nope1</h1>
            <div id="renderer" style={{
                width:'100%',height:'100%',left:'0px',top:'0px',position:'absolute'
            }}></div>
        </div>
    )
}

export default TestViewModel;