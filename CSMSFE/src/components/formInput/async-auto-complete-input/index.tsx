import React, { useEffect, useState } from 'react';
import { ControllerRenderProps } from 'react-hook-form';
import { Autocomplete, TextField, CircularProgress, debounce } from '@mui/material';

interface IAsyncAutocompleteProps {
  name: string;
  errors?: Record<string, any>;
  placeholder?: string;
  fetchOptions: (query: string) => Promise<{ value: string ; label: string }[]>;
  field: ControllerRenderProps<any, string>;
  isMulti: boolean;
  disabled?: boolean;
  hidehelpertext?: boolean;
}

const AsyncAutocomplete: React.FC<IAsyncAutocompleteProps> = ({
  name,
  errors,
  field,
  fetchOptions,
  isMulti,
  disabled,
  ...rest
}) => {
  const [isFirstLoad, setIsFirstLoad] = useState<boolean>(true);
  const [options, setOptions] = useState<{ value: string | number; label: string }[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  const [optionSelected, setOptionSelected] = useState<any>('');
  const [arrOptionsSelected, setArrOptionsSelected] = useState<any[]>([]);

  const fetchOptionsFirstLoad = async () => {
    try {
      const dataOption = await fetchOptions('');
      if ( isMulti ) {
        setArrOptionsSelected(field.value.map((val: string) => dataOption.find((option) => option.value == val)).filter(Boolean));  
      } else {
        setOptionSelected(dataOption.find((option) => option.value == field.value) || '')
      }
      
      setOptions(dataOption);
    } catch (err) {
      setOptions([]);
    }
  }

  useEffect(() => {
    setIsFirstLoad(false);
    if ( disabled ) return;
    fetchOptionsFirstLoad();
  }, [])

  useEffect(() => {
    if (isFirstLoad ) return;
    if ( !options.length || !field.value ) return
    // console.log("options, field", options, field);

    if ( isMulti ) {
      setArrOptionsSelected(field.value.map((val: string) => options.find((option) => option.value == val)).filter(Boolean));  
    } else {
      setOptionSelected(options.find((option) => option.value == field.value) || '')
    }
    
  }, [options, field])

  useEffect(() => {
    if (isFirstLoad ) return;

    if ( !disabled ) {
      triggerFetchOptions();
    } else {
      setOptionSelected('');
      setOptions([]);
    }

  }, [disabled])

  const triggerFetchOptions = async () => {
    setLoading(true);
    try {
      const fetchedOptions = await fetchOptions("");
      setOptions(fetchedOptions);
    } catch (err) {
      setOptions([]);
    } finally {
      setLoading(false);
    }
  }

  const handleInputChange = debounce(async (_event: any, value: string) => {
    setLoading(true);
    try {
      const fetchedOptions = await fetchOptions(value);
      setOptions(fetchedOptions);
    } catch (err) {
      setOptions([]);
    } finally {
      setLoading(false);
    }
  }, 300);

  return (
    <Autocomplete
      options={options}
      multiple={isMulti}
      getOptionLabel={(option) => option?.label || ''}
      loading={loading}
      onInputChange={(event, value) => handleInputChange(event, value)}
      value={(() => {
          if (isMulti && Array.isArray(field.value)) {
            return arrOptionsSelected;
          }
          return optionSelected;
        })()
      }
      onChange={(_, data) => {
        if (!isMulti) {
          field.onChange(data ? data.value : '');
          setOptionSelected(data ? data : '');
        } else {
          let countMap = new Map();

          // Count occurrences of each unique pair
          data.forEach((item: { value: string | number; label: string }) => {
              let key = `${item.label}:${item.value}`;
              if (countMap.has(key)) {
                  countMap.set(key, countMap.get(key) + 1);
              } else {
                  countMap.set(key, 1);
              }
          });

          // Filter out elements that appear exactly twice
          let result = data.filter((item: { value: string | number; label: string }) => {
              let key = `${item.label}:${item.value}`;
              return countMap.get(key) !== 2;
          });

          field.onChange(result.map((item: any) => item.value));
          setArrOptionsSelected(result);
        }
      }}
      disabled={disabled}
      renderInput={(params) => (
        <TextField
          {...params}
          {...rest}
          disabled={disabled}
          // error={!!errors?.[name]}
          error={(errors?.message || errors?.[name]?.message) ? true : false}
          helperText={!rest.hidehelpertext ? (errors?.[name]?.message || errors?.message || ' ') : ""}
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <>
                {loading ? (
                  <CircularProgress color="inherit" size={20} />
                ) : null}
                {params.InputProps.endAdornment}
              </>
            ),
          }}
        />
      )}
    />
  );
};

export default AsyncAutocomplete;
