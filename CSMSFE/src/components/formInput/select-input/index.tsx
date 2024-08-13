import React, { useState } from 'react';
import { ControllerRenderProps } from 'react-hook-form';
import { Select, MenuItem, FormControl, FormHelperText, IconButton, InputLabel, InputAdornment, SelectProps } from '@mui/material';
import Clear from '@mui/icons-material/Clear';

interface IBasicSelectProps extends Omit<SelectProps, 'name' | 'error' | 'helperText'> {
    name: string;
    errors?: Record<string, any>;
    dirtyFields?: Record<string, boolean>;
    options: Array<{ value: string; label: string }>;
    field: ControllerRenderProps<any, string>;
    placeholder?: string;
}

const BasicSelectInput: React.FC<IBasicSelectProps> = ({
    name,
    errors,
    field,
    options,
    dirtyFields,
    placeholder,
    ...rest
}) => {
    const [value, setValue] = useState(field.value || '');

    const clearValue = () => {
        setValue('');
        field.onChange('');
    }

    return (
        <FormControl fullWidth error={!!errors?.[name]}>
            <InputLabel htmlFor={name}>{rest.label}</InputLabel>
            <Select
                {...rest}
                {...field}
                value={value}
                onChange={(e) => {
                    setValue(e.target.value as string);
                    field.onChange(e.target.value);
                }}
                endAdornment={
                    value ? (
                        <InputAdornment position="end">
                            <IconButton onClick={clearValue} size='small'>
                                <Clear sx={{width: '20px', height: '20px'}}/>
                            </IconButton>
                        </InputAdornment>
                    ) : null
                }
            >
                {placeholder && !value && (
                    <MenuItem value="" disabled>
                        {placeholder}
                    </MenuItem>
                )}
                {options.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                        {option.label}
                    </MenuItem>
                ))}
            </Select>
            {errors?.[name] && <FormHelperText>{errors?.[name]?.message || ' '}</FormHelperText>}
        </FormControl>
    );
}

export default BasicSelectInput;
