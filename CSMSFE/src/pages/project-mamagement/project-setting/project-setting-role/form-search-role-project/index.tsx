import { useState } from 'react';
import { useForm } from 'react-hook-form';
// MUI
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import Button from '@mui/material/Button';
// import MenuItem from '@mui/material/MenuItem';
import FormInput from '@/components/formInput';

// Icon
import SearchIcon from '@mui/icons-material/Search';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import FormSearchTogglableWrapper from '@/components/formSearchTogglableWrapper';

// Media query breakpoint
import { useMediaQuery } from "react-responsive";

import { RoleQueryFilter } from '@/common/DTO/Role/RoleQueryFilter';
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';

const FORM_SCHEMA = {
    code: {},
    name: {},
};

interface IFormSearchRoleProjectProps {
    handleFilter: Function;
}

const FormSearchRoleProject: React.FC<IFormSearchRoleProjectProps> = ({
    handleFilter,
}) => {
    // Media query
    const isMD = useMediaQuery({ query: "(min-width: 900px)" });

    const { control, handleSubmit, formState: { errors }, reset } = useForm<RoleQueryFilter>({
        mode: 'onChange',
        defaultValues: {
            name: "",
            code: "",
        },
    });

    const onSubmit = (data: RoleQueryFilter) => {
        console.log(data);
        handleFilter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, data);
    }

    const [isOpenSearch, setIsOpenSearch] = useState(false);

    const onResetSearch = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();
        handleFilter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, null);
        reset();
    }

    return (
        <Card component="section">
            <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
                <FormSearchTogglableWrapper
                    isOpen={isOpenSearch}
                    numOfLines={isMD ? 1 : 2}
                >
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="name"
                            control={control}
                            rules={FORM_SCHEMA.name}
                            errors={errors}
                            placeholder="Tên chức vụ"
                            label="Tên chức vụ"
                            fullWidth
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="code"
                            control={control}
                            rules={FORM_SCHEMA.code}
                            errors={errors}
                            placeholder="Mã vai trò"
                            fullWidth
                            label="Mã vai trò"
                        />
                    </Grid>

                </FormSearchTogglableWrapper>
                <Grid container
                    direction="row"
                    justifyContent="center"
                    alignItems="center"
                    columnSpacing={2}
                >
                    <Grid item>
                        <Button
                            type={'button'}
                            onClick={(e) => {
                                e.preventDefault();
                                setIsOpenSearch(prev => !prev);
                            }}
                            variant="contained"
                            disableElevation
                            fullWidth
                            startIcon={isOpenSearch ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                            color={isOpenSearch ? 'error' : 'info'}
                        >
                            {isOpenSearch ? 'Đóng' : 'Mở'}
                        </Button>
                    </Grid>
                    <Grid item>
                        <Button
                            type="submit"
                            variant="contained"
                            disableElevation
                            fullWidth
                            startIcon={<SearchIcon />}
                            color='primary'
                        >
                            Tìm kiếm
                        </Button>
                    </Grid>
                    <Grid item>
                        <Button
                            type="button"
                            variant="contained"
                            disableElevation
                            fullWidth
                            startIcon={<SearchOffIcon />}
                            color="error"
                            onClick={onResetSearch}
                        >
                            Xóa lọc
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Card>
    );
}

export default FormSearchRoleProject;
