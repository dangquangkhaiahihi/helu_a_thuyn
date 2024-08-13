import React from 'react';
import { ControllerRenderProps } from 'react-hook-form';
import { TextField, TextFieldProps, InputAdornment } from '@mui/material';

import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { DatePicker, DatePickerProps } from "@mui/x-date-pickers/DatePicker";
import Stack from "@mui/material/Stack";
import dayjs, { Dayjs } from "dayjs";
import Clear from "@mui/icons-material/Clear";
import IconButton from "@mui/material/IconButton";

interface IBasicDatePickerProps extends Omit<TextFieldProps, 'name' | 'error' | 'helperText'> {
    name: string;
    errors?: Record<string, any>;
    dirtyFields?: Record<string, boolean>;
    propsDatePicker?: DatePickerProps<Dayjs>;
    field: ControllerRenderProps<any, string>;
}

const BasicDatePickerInput: React.FC<IBasicDatePickerProps> =  ({
    propsDatePicker,
    name,
    errors,
    field,
    // dirtyFields,
}) => {
    const clearValue = () => {
        field.onChange(null);
    }
    
    return (
        <DemoContainer
            components={["DatePicker"]}
            sx={{padding:'0'}}
        >
            <DatePicker
                {...propsDatePicker}
                sx={{width: '100%'}}
                onChange={(date) => field.onChange(dayjs(date))}
                value={field.value ? dayjs(field.value) : null}
                format="DD/MM/YYYY"
                views={["day", "month", "year"]}
                slotProps={{
                    textField: {
                        onClearClick: clearValue,
                        name: name,
                        error: !!errors?.[name],
                        helperText: errors?.[name]?.message || ' ',
                        field: field
                    } as TextFieldProps
                }}
                slots={{
                    textField: PickerTextField
                }}
            />
        </DemoContainer>
    );
}

export default BasicDatePickerInput;

const PickerTextField = (
    {
        onClearClick,
        errors,
        name,
        dirtyFields,
        field,
        ...rest
    }: TextFieldProps & {
        onClearClick: () => void,
        errors: Record<string, any>;
        name: string;
        dirtyFields?: Record<string, boolean>;
        field: ControllerRenderProps<any, string>;
    }
) => {
    return (
        <TextField
            error={!!errors?.[name]}
            helperText={errors?.[name] ? errors[name]?.message : ' '}
            {...rest}
            InputProps={{
                ...rest.InputProps,
                endAdornment: mergeAdornments(
                    field?.value &&
                    <InputAdornment position="end">
                        <IconButton onClick={onClearClick} size='small'>
                            <Clear sx={{width: '20px', height: '20px'}}/>
                        </IconButton>
                    </InputAdornment>,
                    rest.InputProps?.endAdornment ?? null
                )
            }}
        />
    );
}

const mergeAdornments = (...adornments: React.ReactNode[]) => {
    const nonNullAdornments = adornments.filter((el) => el != null);
    if (nonNullAdornments.length === 0) {
        return null;
    }

    if (nonNullAdornments.length === 1) {
        return nonNullAdornments[0];
    }

    return (
        <Stack direction="row">
            {nonNullAdornments.map((adornment, index) => (
                <React.Fragment key={index}>{adornment}</React.Fragment>
            ))}
        </Stack>
    );
};