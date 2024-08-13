import CircularProgress, { circularProgressClasses } from '@mui/material/CircularProgress';

const LoadingIcon = () => {

    return <div style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        width: '100%',
        height: '100%',
    }}>
        <CircularProgress
            variant="determinate"
            sx={{
                color: (theme) =>
                    theme.palette.grey[theme.palette.mode === 'light' ? 200 : 800],
            }}
            size={20}
            thickness={5}
            value={200}
        />
        <CircularProgress
            variant="indeterminate"
            disableShrink
            color={"info"}
            sx={{
                animationDuration: '1000ms',
                position: 'absolute',
                [`& .${circularProgressClasses.circle}`]: {
                    strokeLinecap: 'round',
                },
            }}
            size={20}
            thickness={5}
        />
    </div>
};

export default LoadingIcon;
