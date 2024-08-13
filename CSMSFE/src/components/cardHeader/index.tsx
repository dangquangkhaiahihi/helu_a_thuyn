import { ReactNode } from 'react';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';

const VARIANTS_CONFIG = {
  small: {
    headerMb: 0,
    titleMb: 0.5,
    titleVariant: 'h5' as const,
  },
  medium: {
    headerMb: 3,
    titleMb: 1,
    titleVariant: 'h4' as const,
  },
  large: {
    headerMb: 3,
    titleMb: 1,
    titleVariant: 'h3' as const,
  },
};

interface CardHeaderProps {
  title: string;
  subtitle?: string;
  size?: 'small' | 'medium' | 'large';
  sx?: object;
  children?: ReactNode;
}

function CardHeader({ children, title, subtitle, size = 'medium', sx }: CardHeaderProps) {
  return (
    <Stack
      mb={VARIANTS_CONFIG?.[size]?.headerMb}
      direction="row"
      justifyContent="space-between"
      flexWrap="wrap"
      sx={sx}
    >
      <Stack>
        <Typography
          variant={VARIANTS_CONFIG?.[size]?.titleVariant}
          mb={VARIANTS_CONFIG?.[size]?.titleMb}
          fontWeight="500"
          textTransform="uppercase"
        >
          {title}
        </Typography>
        {subtitle && (
          <Typography variant="body1" color="text.secondary">
            {subtitle}
          </Typography>
        )}
      </Stack>
      {children}
    </Stack>
  );
}

export default CardHeader;