import React from 'react';
import { Controller, Control, ControllerRenderProps } from 'react-hook-form';
import { TextFieldProps } from '@mui/material';

import Autocomplete from '@mui/material/Autocomplete';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';

import CloseIcon from '@mui/icons-material/Close';
import CheckIcon from '@mui/icons-material/Check';
import InputLabel from '@mui/material/InputLabel';

import InputAdornment from "@mui/material/InputAdornment";
import BasicDatePickerInput from './date-picker-input';
import BasicSelectInput from './select-input';
import AsyncAutocomplete from './async-auto-complete-input';

interface BaseFormInputProps extends Omit<TextFieldProps, 'name' | 'error' | 'helperText'> {
  name: string;
  rules?: Record<string, any>;
  control: Control<any>;
  errors?: Record<string, any>;
  dirtyFields?: Record<string, boolean>;
  element?: React.ElementType;
  children?: React.ReactNode;
  sx? : any;
  disabled?: boolean;
  accept?: any;
  hidehelpertext?: boolean;
}

interface DropdownSelectProps extends BaseFormInputProps {
  type: "select" |
    "auto-complete" |
    "auto-complete-multi-select";
  options: Array<{ value: string; label: string }>;
  fetchOptions?: never;
}

interface AsyncDropdownSelectProps extends BaseFormInputProps {
  type: "async-auto-complete" | "async-auto-complete-multi-select";
  options?: never;
  fetchOptions: (query: string) => Promise<{ value: string; label: string }[]>;
}

interface OtherInputProps extends BaseFormInputProps {
  type: "text" | "number" | "password" | "date" | "textarea" | "file";
  options?: never;
  fetchOptions?: never;
}

type IFormInputProps = DropdownSelectProps | OtherInputProps | AsyncDropdownSelectProps;

const FormInput: React.FC<IFormInputProps> = (props) => {
  const {
    name,
    rules,
    control,
    errors,
    dirtyFields,
    element: InputComponent = TextField,
    children,
    label,
    type,
    options,
    sx,
    disabled,
    fetchOptions,
    ...otherProps
  } = props;

  const renderInputBasedOnType = (field: ControllerRenderProps<any, string>) => {
    
    switch (type) {
      case "date":
        return (
          <BasicDatePickerInput
            name={name}
            errors={errors}
            dirtyFields={dirtyFields}
            field={field}
          />
        )
      case "select":
        return (
          <BasicSelectInput
            name={name}
            errors={errors}
            dirtyFields={dirtyFields}
            field={field}
            options={options}
            placeholder={props.placeholder}
          />
        )
      case "auto-complete":
      case "auto-complete-multi-select":
        console.log("options", options);
        console.log("field.value", field.value);
        return (
          <Autocomplete
            {...field}
            multiple={type === 'auto-complete-multi-select'}
            options={options}
            getOptionLabel={(option) => option.label}
            value={type === 'auto-complete' ? 
              (options.find(option => option.value === field.value) || null) :
              (field.value || []).map((val: string) => options.find(option => option.value === val) || {label: "Dữ liệu không tồn tại"})
            }
            onChange={(_, data) => {
              if (type === 'auto-complete') {
                field.onChange(data?.value || '');
              } else {
                field.onChange(data.map((item: any) => item.value));
              }
            }}
            renderInput={(params) => (
              <TextField
                  {...params}
                  {...otherProps}
                  name={name}
                  error={!!errors?.[name]}
                  helperText={(errors) ? (errors?.[name]?.message || ' ') : ''}
              />
            )}
          />
        )
      case "async-auto-complete":
      case "async-auto-complete-multi-select":
        return (
          <AsyncAutocomplete
            name={name}
            errors={errors}
            field={field}
            fetchOptions={fetchOptions}
            isMulti={type === 'async-auto-complete-multi-select'}
            placeholder={otherProps.placeholder}
            disabled={disabled}
            {...otherProps}
          />
        )
      case "textarea":
        return (
          <TextField
            error={!!errors?.[name]}
            helperText={errors ? (errors?.[name] ? errors[name]?.message : ' ') : ''}
            {...otherProps}
            {...field}
            multiline
            minRows={5}
            maxRows={10}
            InputProps={{
              endAdornment: dirtyFields?.[name] && (
                <InputAdornment position="end" sx={{ mr: otherProps?.select ? '16px' : '' }}>
                  {errors?.[name] ? <CloseIcon color="error" /> : <CheckIcon color="success" />}
                </InputAdornment>
              ),
            }}
          >
            {children}
          </TextField>
        )
      case "file":
        return (
          <TextField
            {...otherProps}
            error={!!errors?.[name]}
            helperText={errors ? (errors?.[name] ? errors[name]?.message : ' ') : ''}
            InputProps={{
              inputProps: {
                accept: otherProps.accept, // Allow file type restrictions if needed
              },
              endAdornment: dirtyFields?.[name] && (
                <InputAdornment position="end" sx={{ mr: otherProps?.select ? '16px' : '' }}>
                  {errors?.[name] ? <CloseIcon color="error" /> : <CheckIcon color="success" />}
                </InputAdornment>
              ),
            }}
            inputRef={field.ref}
            type="file"
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
              const files = e.target.files;
              if (files) {
                field.onChange(files); // Update field value with the selected files
              }
            }}
          />
        );

      default:
        return (
          <TextField
            error={!!errors?.[name]}
            helperText={errors ? (errors?.[name] ? errors[name]?.message : ' ') : ''}
            disabled={disabled}
            {...otherProps}
            {...field}
            InputProps={{
              endAdornment: dirtyFields?.[name] && (
                <InputAdornment position="end" sx={{ mr: otherProps?.select ? '16px' : '' }}>
                  {errors?.[name] ? <CloseIcon color="error" /> : <CheckIcon color="success" />}
                </InputAdornment>
              ),
            }}
          >
            {children}
          </TextField>
        )
    }
  }
  
  return (
    <Controller
      control={control}
      name={name}
      rules={rules}
      render={({ field }) => (
        <Box sx={sx || { mb: 2 }}>
          <InputLabel htmlFor={name}>
            {label}
          </InputLabel>
          
          {renderInputBasedOnType(field)}
        </Box>
      )}
    />
  );
};

export default FormInput;