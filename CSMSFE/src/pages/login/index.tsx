import { useState } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
// MUI
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import Card from '@mui/material/Card';
import Link from '@mui/material/Link';
import Stack from '@mui/material/Stack';
import { IconButton, InputAdornment, Tooltip } from '@mui/material';

// Icons
import LoginIcon from '@mui/icons-material/Login';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

//
import { Controller, Control, useForm } from 'react-hook-form';
import { isValidEmail } from '@/common/tools';
import AccountService from '@/api/instance/account';
import { enqueueSnackbar } from 'notistack';
import { GenerateDeviceId } from "@/common/tools";
import { CookiesKeysCollection, getCookies, removeCookies, setCookies } from '@/utils/configuration';
import { UrlRouteCollection } from '@/common/url-route-collection';


const FORM_SCHEMA = {
    email: {
        required: 'Đây là trường bắt buộc',
		validate: (value: string) => {
            return isValidEmail(value) || 'Email sai định dạng'
        },
    },
    password: {
        required: 'Đây là trường bắt buộc',
    },
    rememberMe: {},
};

function LoginCSMSPage() {
	return (
		<Card
			elevation={20}
			sx={{
				display: 'block',
				width: {
					xs: '95%',
					sm: '55%',
					md: '35%',
					lg: '25%',
				},
			}}
		>
			<Stack direction="column" spacing={5}>
				<Typography variant="h1">ĐĂNG NHẬP</Typography>
				<LoginForm />
			</Stack>
		</Card>
	);
}

function LoginForm() {
	// const navigate = useNavigate();
	const [isLoading, setIsLoading] = useState(false);
	const navigate = useNavigate();

	const { control, handleSubmit } = useForm({
		mode: 'onChange',
		defaultValues: {
            email: "",
			password: "",
            rememberMe: false,
			returnUrl: getCookies(CookiesKeysCollection.RETURN_URL) || UrlRouteCollection.Home,
			uUid: GenerateDeviceId(),
		},
	});
	const onSubmit = async (data: any) => {
		setIsLoading(true);
		try {
            const res = await AccountService.Login(data);
			setCookies(CookiesKeysCollection.TOKEN_KEY, res.content.token);
			setCookies(CookiesKeysCollection.REFRESH_TOKEN_KEY, res.content.refreshToken);
			removeCookies(CookiesKeysCollection.RETURN_URL);
			navigate(res.content.returnUrl);
        } catch ( err ) {
            enqueueSnackbar('Có lỗi xảy ra, vui lòng thử lại sau.', {
                variant: 'error'
            });
        } finally {
            setIsLoading(false);
        }
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<FormInputText
				type="text"
				name="email"
				label="Email"
				control={control}
				rules={FORM_SCHEMA.email}
			/>
			<FormInputText
				type="password"
				name="password"
				label="Mật khẩu"
				control={control}
				rules={FORM_SCHEMA.password}
			/>
			{/* <Typography variant="subtitle1">
				<Controller
					name="rememberMe"
					control={control}
					render={({ field }) => (
						<Checkbox {...field} checked={field.value} />
					)}
            	/> Ghi nhớ đăng nhập
			</Typography> */}

			<Link to="/resetPassword" component={RouterLink} color="tertiary.main">
				Quên mật khẩu
			</Link>
			<Button
				sx={{
					mt: 2,
					textTransform: 'uppercase',
					color: 'primary.contrastText',
					' &:not(:disabled)': {
						background: (theme) =>
							`linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.tertiary.main} 100%)`,
					},
					'&:hover': {
						background: (theme) =>
							`linear-gradient(90deg, ${theme.palette.primary.dark} 0%, ${theme.palette.tertiary.dark} 100%)`,
					},
				}}
				type="submit"
				variant="contained"
				disabled={isLoading}
				endIcon={
					isLoading ? (
						<CircularProgress
							color="secondary"
							size={25}
							sx={{
								my: 'auto',
							}}
						/>
					) : (
						<LoginIcon />
					)
				}
				fullWidth
				color="primary"
			>
				Đăng nhập
			</Button>
		</form>
	);
}

export default LoginCSMSPage;

export const FormInputText = ({ type, name, control, label, rules, ...otherProps }: {
	type: string;
	name: string;
	control: Control<any>;
	label: string;
	rules?: Record<string, any>;
}) => {
	const [fieldType, setFieldType] = useState<string>(type);
		
	return (
		<Controller
			name={name}
			control={control}
			rules={rules}
			render={({
				field: { onChange, value },
				fieldState: { error },
		}) => (
			<TextField
				{...otherProps}
				helperText={error ? error.message : null}
				error={!!error}
				onChange={onChange}
				value={value}
				type={fieldType}
				label={label}
				fullWidth
				variant="outlined"
				margin="normal"

				InputProps={{
					endAdornment: type === "password" && (
						<InputAdornment position="end"
							sx={{ cursor: 'pointer' }}
						>
							<Tooltip title={fieldType === "password" ? "Hiện mật khẩu" : "Ẩn mật khẩu"}>
								<IconButton onClick={() => {
									setFieldType(prev => {
										if ( prev === 'password') return "text";
										return "password";
									})
								}}>
									{ fieldType === "password" ? 
										<VisibilityIcon fontSize='small' /> :
										<VisibilityOffIcon fontSize='small' />
									}
								</IconButton>
							</Tooltip>
						</InputAdornment>
					),
				}}
			/>
		)}
		/>
	);
};