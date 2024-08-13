import React, { useMemo, useRef } from 'react';
import DocViewer, { DocViewerRef, DocViewerRenderers, IDocument } from "@cyntler/react-doc-viewer";
import { CookiesKeysCollection, getCookies } from '@/utils/configuration';

type DocToPreview = {
    uri: string;
}

interface IDocPreviewer {
    docs: DocToPreview[];
}

const MyLoadingRenderer = ({ document, fileName }: {document: IDocument | undefined; fileName: string;}) => {
    const fileText = fileName || document?.fileType || "";
  
    if (fileText) {
      return <div>Loading Renderer ({fileText})...</div>;
    }
  
    return <div>Loading Renderer...</div>;
};

const MyNoRenderer = (_props: {document: IDocument | undefined; fileName: string;}) => {
  
    return <div>Không xem được file</div>;
};

const DocPreviewer: React.FC<IDocPreviewer> = ({ docs }) => {
    const docViewerRef = useRef<DocViewerRef>(null);
    const headersDocViewer = {
        "Authorization": `${getCookies(CookiesKeysCollection.TOKEN_KEY)}`,
    };

    const memoizedDocs = useMemo(() => docs, [docs]);

    const memoizedConfig = useMemo(() => ({
        header: {
            disableHeader: true,
            // disableFileName: false,
            retainURLParams: true,
        },
        loadingRenderer: {
            overrideComponent: MyLoadingRenderer,
        },
        noRenderer: {
            overrideComponent: MyNoRenderer
        },
        csvDelimiter: ",", // "," as default,
        pdfZoom: {
            defaultZoom: 1.1, // 1 as default,
            zoomJump: 0.2, // 0.1 as default,
        },
        pdfVerticalScrollByDefault: true, // false as default
    
    }), []); 

    return (
        <DocViewer
            ref={docViewerRef}
            pluginRenderers={DocViewerRenderers}
            documents={memoizedDocs}
            config={memoizedConfig}
            style={{ height: 500 }}
            requestHeaders={headersDocViewer}
            prefetchMethod="GET"
        />
    )
};
export default DocPreviewer;