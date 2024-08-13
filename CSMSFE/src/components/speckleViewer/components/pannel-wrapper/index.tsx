// MUI
import { Box } from "@mui/material";
import React from "react";

interface IFunctionPanelWrapper {
    show: boolean;
    children: React.ReactElement<any>;
}

const FunctionPanelWrapper = React.forwardRef<HTMLElement, IFunctionPanelWrapper>((props, ref) => {
    const { show, children } = props;

    return (
        <Box 
            ref={ref}
            position='absolute' 
            display={show ? "block" : "none"} 
            left={'calc(100% + 8px)'}
            top={0}
            zIndex={10}
            maxWidth={'calc(100vw - 50px - 8px)'}
            width="400px"
            padding={1}
            sx={{
                borderRadius: "0.5rem",
                backgroundColor: "white",
                "& .MuiTouchRipple-root .MuiTouchRipple-child": {
                    borderRadius: "0.5rem"
                },
                boxShadow: "var(--tw-ring-offset-shadow, 0 0 #0000), var(--tw-ring-shadow, 0 0 #0000), var(--tw-shadow)",
                "--tw-shadow": "0 4px 6px -1px rgba(0,0,0,0.1), 0 2px 4px -2px rgba(0,0,0,0.1)",
                "--tw-shadow-colored": "0 4px 6px -1px var(--tw-shadow-color), 0 2px 4px -2px var(--tw-shadow-color)"
            }}
        >
            {children}
        </Box>
    );
});

export default FunctionPanelWrapper;
