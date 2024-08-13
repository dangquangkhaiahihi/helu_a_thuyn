import FunctionButtonWrapper from "../../function-button-wrapper";
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import { useEffect, useRef, useState } from "react";
import { Accordion, AccordionDetails, AccordionSummary, Box, Card, CardContent, CardHeader, CardMedia, Checkbox, FormControlLabel, Slide, Typography } from "@mui/material";
import FunctionPanelWrapper from "../../pannel-wrapper";
import { FilteringExtension, ViewerEvent } from "@speckle/viewer";
import { useSpeckleViewer } from "@/components/speckleViewer/context/SpeckleViewerContext";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { useSelector } from "react-redux";
import { speckleModelInfoRedux } from "@/store/redux/speckle-viewer";
import { ModelSpeckleInfo } from "@/common/DTO/Model/ModelSpeckleViewInfoDTO";
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import RemoveRedEyeIcon from '@mui/icons-material/RemoveRedEye';

import 'dayjs/locale/vi';
import dayjs from 'dayjs';
dayjs.locale('vi');
import RelativeTime from "dayjs/plugin/relativeTime";
dayjs.extend(RelativeTime);

const ToggleModelButton: React.FC<any> = () => {
    const { viewer } = useSpeckleViewer();

    const [openTogglePanel, setOpenTogglePanel] = useState(false);
    const [visibilityMap, setVisibilityMap] = useState<{ [key: string]: boolean }>({});
    const [filtering, setFiltering] = useState<FilteringExtension | null>(null);
    const containerRef = useRef(null);

    useEffect(() => {
        if ( !viewer ) return;
        
        const temp = viewer.createExtension(FilteringExtension);
        setFiltering(temp);

        viewer.on(ViewerEvent.LoadComplete, async (id: string) => {
            setVisibilityMap(prevMap => ({ ...prevMap, [id]: true }));
        });
        // viewer.on(ViewerEvent.LoadComplete, async (id: string) => {
        //     const modeRootNodes = viewer.getWorldTree().root.model.children;
        //     for (const node of modeRootNodes) {
        //         const id = node.id;
        //         console.log("lololol", node);   
        //     }
        // });

    }, [viewer])

    const handleToggleVisibility = (id: string) => {
        if (!filtering || !viewer) {
            console.log("Filter not initialized");
            return;
        }
        const isVisible = visibilityMap[id];
        if (isVisible) filtering.hideObjects([id]);
        else filtering.showObjects([id]);

        const shadowCatcherPass = viewer.getRenderer()?.shadowcatcher?.shadowcatcherPass;
        if (shadowCatcherPass) {
            shadowCatcherPass.needsUpdate = true;
        }
        setVisibilityMap(prevMap => ({ ...prevMap, [id]: !isVisible }));
    };

    useEffect(() => {
        if ( !viewer ) return;
        console.log("visibilityMap", visibilityMap);

        const shadowCatcherPass = viewer.getRenderer()?.shadowcatcher?.shadowcatcherPass;
        if (shadowCatcherPass) {
            shadowCatcherPass.needsUpdate = true;
        }
    }, [visibilityMap])

    const speckleModelInfo = useSelector(speckleModelInfoRedux);
    useEffect(() => {
        console.log("speckleModelInfo", speckleModelInfo);
        
    }, [speckleModelInfo])

    return (
        <Box sx={{ position: 'relative', }} ref={containerRef}>
            <FunctionButtonWrapper tooltipTitle="Danh sách mô hình" onClick={() => {setOpenTogglePanel(prev => !prev)}}>
                <FormatListBulletedIcon />
            </FunctionButtonWrapper>

            <Slide in={openTogglePanel} mountOnEnter unmountOnExit direction="right" container={containerRef.current}>
                <FunctionPanelWrapper show={openTogglePanel}>
                    <Card sx={{padding: 0}}>
                        <CardHeader title="Danh sách mô hình" />
                        <CardContent>
                            {
                                speckleModelInfo ? (
                                    speckleModelInfo.speckleModelInfos.map((item, index) => {
                                        return <ToggleItem key={index} item={item} handleToggleVisibility={handleToggleVisibility}/>
                                    })
                                ) : <></>
                            }
                        </CardContent>
                    </Card>
                </FunctionPanelWrapper>
            </Slide>
        </Box>
    )
}

export default ToggleModelButton;

  
//     /** Alternative way of adding the model buttons */
//     /*
//     const modeRootNodes = viewer.getWorldTree().root.model.children;
//     for (const node of modeRootNodes) {
//       const id = node.id;
//       pane
//         .addButton({
//           title: `Hide/Show ${id}`,
//         })
//         .on("click", () => {
//           if (visibilityMap[id]) filtering.hideObjects([id]);
//           else filtering.showObjects([id]);
//           viewer.getRenderer().shadowcatcher.shadowcatcherPass.needsUpdate = true;
//           visibilityMap[id] = !visibilityMap[id];
//         });
//     }*/
//   }

// TODO: Làm sao để biết version nào đang đc xem, onclick vào version đang ko đc xem thì load model mới và unload model cũ như nào
// TODO: Hover vào thì tô màu lên model cho biết m vừa đi qua thằng này đấy

interface IToggleItem {
    item: ModelSpeckleInfo;
    handleToggleVisibility: (id: string) => void;
}
const ToggleItem: React.FC<IToggleItem> = ({ item, handleToggleVisibility }) => {
    
    return (
        <Accordion slotProps={{ transition: { unmountOnExit: true } }} >
            <AccordionSummary
                expandIcon={<ExpandMoreIcon />}
                aria-controls="panel1-content"
                id="panel1-header"
            >
                <FormControlLabel
                    control={
                        <Checkbox
                            defaultChecked
                            onClick={(e) => {
                                e.stopPropagation();
                                handleToggleVisibility(item.id);
                            }} name={item.name}
                        />
                    }
                    label={item.name}
                />
            </AccordionSummary>
            <AccordionDetails sx={{ padding: '8px' }}>
                <Typography gutterBottom variant="subtitle2" component="div">
                    Phiên bản
                </Typography>
                {
                    item.speckleVersionInfos.map((version, index) => {
                        return <Card key={index} variant="model-folder">
                            <Box  sx={{ display: 'flex',  width: '100%' }} alignItems={'center'}>
                                <CardMedia
                                    sx={{
                                        height: 30,
                                        width: 30,
                                        backgroundPosition: '50%',
                                        backgroundSize: 'contain',
                                        cursor: 'pointer',
                                    }}
                                    image={"/assets/images/avatars/model-ava-uploaded.png"}
                                    title={`Phiên bản ${index + 1}`}
                                />
                                <CardContent>
                                    <Typography gutterBottom variant="body1" component="div" sx={{
                                        display: 'flex',
                                        alignItems: 'end',
                                    }}>
                                        Phiên bản {index + 1}
                                    </Typography>
                                    <Typography gutterBottom variant="body1" component="div" sx={{
                                        display: 'flex',
                                        alignItems: 'end',
                                    }}>
                                        <AccessTimeOutlinedIcon sx={{ mr: 1 }} fontSize="small"/> {dayjs(version.modifiedDate).fromNow()}
                                    </Typography>
                                    <Typography gutterBottom variant="body1" component="div" sx={{
                                        display: 'flex',
                                        alignItems: 'end',
                                    }}>
                                        <CalendarMonthIcon sx={{ mr: 1 }} fontSize="small"/> {dayjs(version.modifiedDate).format("DD/MM/YYYY hh:mm:ss")}
                                    </Typography>
                                </CardContent>
                                <Box>
                                    <Typography gutterBottom variant="body2" component="span" color="primary" sx={{
                                        display: 'flex',
                                        alignItems: 'end',
                                    }}>
                                        <RemoveRedEyeIcon sx={{ mr: 1 }} fontSize="small" /> Đang xem
                                    </Typography>
                                </Box>
                            </Box>
                        </Card>;
                    })
                }
            </AccordionDetails>
        </Accordion>
    )
}

// TODO: Case chọn xem full model thì xử lý sao
