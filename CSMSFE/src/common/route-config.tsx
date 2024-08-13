import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import ActivityDetector, { ActivityDetectorProps } from "react-activity-detector";
import {
  getCookies,
  removeCookies,
  setIsShowDialogConfirmRefresh,
} from "@utils/configuration";

interface RouterComponentProps {
  path: string
  component: React.ComponentType<any>;
  layout: React.ComponentType<any>;
  isSetActive?: boolean;
  exact?: boolean;
}

const RouterComponent: React.FC<RouterComponentProps> = ({
  component: Component,
  layout: Layout,
  isSetActive = true,
  ...rest
}): JSX.Element => {
  const customActivityEvents: ActivityDetectorProps['activityEvents'] = [      
    "click",
    "mousemove",
    "keydown",
    "DOMMouseScroll",
    "mousewheel",
    "mousedown",
    "touchstart",
    "touchmove",
    "focus"
  ];

  const auth = "null";

  const onIdle = () => { 
    if (getCookies("isLockScreen") != "true")
    {
      setIsShowDialogConfirmRefresh(true);
    }
  };

  const onActive = () => {
    if (getCookies("isShowDialog") === "false") {
      removeCookies("isShowDialog");
    }
  };

  return auth ? <Outlet /> : <Navigate to="/login" />;

  // return (
  //   <Route
  //     {...rest}
  //     element={
  //       <>
  //         {isSetActive && (
  //           <ActivityDetector 
  //             activityEvents={customActivityEvents}
  //             enabled={true}
  //             timeout={5 * 60 * 1000}
  //             onIdle={onIdle}
  //             onActive={onActive}
  //           />
  //         )}
  //         <Layout>
  //           <Component />
  //         </Layout>
  //       </>
  //     }
  //   />
  // );
};

export default RouterComponent;
