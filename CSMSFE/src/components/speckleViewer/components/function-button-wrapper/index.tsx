// MUI
import Tooltip from "@mui/material/Tooltip";
import IconButton from "@mui/material/IconButton";
import { SvgIconProps } from '@mui/material/SvgIcon';

interface IFunctionButtonWrapper {
    tooltipTitle: string;
    children: React.ReactElement<SvgIconProps>;
    onClick: Function;
}

const FunctionButtonWrapper: React.FC<IFunctionButtonWrapper> = (props) => {
    const { tooltipTitle, children, onClick } = props;

    return (
        <Tooltip title={tooltipTitle} placement="top">
            <IconButton
                onClick={() => {onClick()}}
                size="small"
                sx={{
                    borderRadius: "0.5rem",
                    backgroundColor: "white",
                    ":hover": {
                        backgroundColor: "primary.light"
                    },
                    "& .MuiTouchRipple-root .MuiTouchRipple-child": {
                        borderRadius: "0.5rem"
                    },
                    boxShadow: "var(--tw-ring-offset-shadow, 0 0 #0000), var(--tw-ring-shadow, 0 0 #0000), var(--tw-shadow)",
                    "--tw-shadow": "0 4px 6px -1px rgba(0,0,0,0.1), 0 2px 4px -2px rgba(0,0,0,0.1)",
                    "--tw-shadow-colored": "0 4px 6px -1px var(--tw-shadow-color), 0 2px 4px -2px var(--tw-shadow-color)"
                }}
            >
                {children}
            </IconButton>
        </Tooltip>
    );
}

export default FunctionButtonWrapper;