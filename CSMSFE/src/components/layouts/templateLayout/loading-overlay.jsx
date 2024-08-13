import React from 'react';
import CircularProgress, { circularProgressClasses } from '@mui/material/CircularProgress';

const CircularLoadingOverlay = () => {

    return <div className={'loading-overlay'}>
        <CircularProgress
            variant="determinate"
            sx={{
                color: (theme) =>
                    theme.palette.grey[theme.palette.mode === 'light' ? 200 : 800],
            }}
            size={70}
            thickness={6}
            value={300}
        />
        <CircularProgress
            variant="indeterminate"
            disableShrink
            color={"cuaternary"}
            sx={{
                animationDuration: '1000ms',
                position: 'absolute',
                [`& .${circularProgressClasses.circle}`]: {
                    strokeLinecap: 'round',
                },
            }}
            size={70}
            thickness={6}
        />
    </div>
};

export default CircularLoadingOverlay;
