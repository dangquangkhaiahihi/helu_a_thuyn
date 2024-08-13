import React from 'react';
import Grid from '@mui/material/Grid';

interface IFormInputProps {
  isOpen: boolean;
  numOfLines: number;
  children: React.ReactNode;
}

const FormSearchTogglableWrapper: React.FC<IFormInputProps> = ({
  isOpen,
  numOfLines,
  children
}) => {
  const style = {
    '--height-multiplier': `${numOfLines}`,
  } as React.CSSProperties;

  return (
    <Grid container
      direction="row"
      justifyContent="center"
      alignItems="center"
      columnSpacing={2}
      className={`_form_search ${isOpen ? '_show' : '_hide'}`}
      style={style}
  >
    {children}
  </Grid>
  );
}

export default FormSearchTogglableWrapper;