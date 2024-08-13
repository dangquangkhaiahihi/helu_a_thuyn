// MUI
import Stack from "@mui/material/Stack";

//
import ToggleModelButton from "./toggle-model";

const FunctionButtons: React.FC<any> = () => {

    return (
        <Stack
            direction="column"
            spacing={1}
            justifyContent="space-between"
            alignItems="center"
            position="absolute"
            zIndex="999999"
            padding={1}
        >
            <ToggleModelButton />

            <ToggleModelButton />
            <ToggleModelButton />
            <ToggleModelButton />
        </Stack>
    )
}

export default FunctionButtons;