import React, { createContext, useContext, useState } from 'react';
import { Viewer } from '@speckle/viewer';

interface SpeckleViewerContextType {
  viewer: Viewer | null;
  setViewer: (viewer: Viewer | null) => void;
}

const SpeckleViewerContext = createContext<SpeckleViewerContextType | undefined>(undefined);

export const SpeckleViewerProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [viewer, setViewer] = useState<Viewer | null>(null);

  return (
    <SpeckleViewerContext.Provider value={{ viewer, setViewer }}>
      {children}
    </SpeckleViewerContext.Provider>
  );
};

export const useSpeckleViewer = () => {
  const context = useContext(SpeckleViewerContext);
  if (context === undefined) {
    throw new Error('useViewer must be used within a SpeckleViewerProvider');
  }
  return context;
};
