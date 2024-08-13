
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
import dayjs from 'dayjs';
import { SomeTableQueryFilter } from '@/common/DTO/SomeTable/SomeTableQueryFilter';
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';

const FORM_SCHEMA = {
    NormalText: {},
    PhoneNumber: {},
    Email: {},
    StartDate: {},
    EndDate: {},
    Status: {},
    Type: {},
    // 3 trường dưới đây chỉ để demo
    TypeList: {},
    TypeAsync: {},
    TypeListAsync: {},
};

interface IFormSearchSomeTableProps {
    handleFilter: Function;
    optionsType: Array<{ value: string; label: string }>;
    fetchOptionType_Async: (query: string) => Promise<{ value: string | number; label: string }[]>;
}

const FormSearchSomeTable: React.FC<IFormSearchSomeTableProps> = ({
    handleFilter,
    optionsType,
    fetchOptionType_Async,
}) => {
    //media query
    const isMD = useMediaQuery({ query: "(min-width: 900px)" });

	const { control, handleSubmit, formState: { errors }, reset} = useForm({
		mode: 'onChange',
		defaultValues: {
			NormalText: "",
            PhoneNumber: "",
            Email: "",
            StartDate: "",
            EndDate: "",
            Status: "",
            Type: "",
            // 3 trường dưới đây chỉ để demo, trong api ko định nghĩa, ko filter theo 3 thằng này
            TypeList: [],
            TypeAsync: "",
            TypeListAsync: []
		},
	});
    
	const onSubmit = (data: SomeTableQueryFilter) => {
        if ( data.StartDate ) data.StartDate = dayjs(data.StartDate).format();
        if ( data.EndDate ) data.EndDate = dayjs(data.EndDate).format();
        console.log(data);
        handleFilter(undefined, undefined, undefined, data);
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
                    numOfLines={isMD ? 5 : 10}
                >
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="NormalText"
                            control={control}
                            rules={FORM_SCHEMA.NormalText}
                            errors={errors}
                            placeholder="Normal Text"
                            label="Normal Text"
                            fullWidth
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="PhoneNumber"
                            control={control}
                            rules={FORM_SCHEMA.PhoneNumber}
                            errors={errors}
                            placeholder="Phone Number"
                            fullWidth
                            label="Phone Number"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="text"
                            name="Email"
                            control={control}
                            rules={FORM_SCHEMA.Email}
                            errors={errors}
                            placeholder="Email"
                            fullWidth
                            label="Email"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="select"
                            name="Status"
                            control={control}
                            rules={FORM_SCHEMA.Status}
                            errors={errors}
                            placeholder="Status"
                            fullWidth
                            label="Status"
                            options={[
                                {value: "true", label: "True"},
                                {value: "false", label: "False"},
                            ]}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="date"
                            name="StartDate"
                            control={control}
                            rules={FORM_SCHEMA.StartDate}
                            errors={errors}
                            placeholder="Start Date"
                            fullWidth
                            label="Start Date"
                        />
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="date"
                            name="EndDate"
                            control={control}
                            rules={FORM_SCHEMA.EndDate}
                            errors={errors}
                            placeholder="End Date"
                            fullWidth
                            label="End Date"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="auto-complete"
                            name="Type"
                            control={control}
                            rules={FORM_SCHEMA.Type}
                            errors={errors}
                            placeholder="Type"
                            fullWidth
                            label="Type"
                            options={optionsType}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="auto-complete-multi-select"
                            name="TypeList"
                            control={control}
                            rules={FORM_SCHEMA.TypeList}
                            errors={errors}
                            placeholder="TypeList"
                            fullWidth
                            label="TypeList"
                            options={optionsType}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="TypeAsync"
                            control={control}
                            rules={FORM_SCHEMA.TypeAsync}
                            errors={errors}
                            placeholder="TypeAsync"
                            fullWidth
                            label="TypeAsync"
                            fetchOptions={fetchOptionType_Async}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete-multi-select"
                            name="TypeListAsync"
                            control={control}
                            rules={FORM_SCHEMA.TypeListAsync}
                            errors={errors}
                            placeholder="TypeListAsync"
                            fullWidth
                            label="TypeListAsync"
                            fetchOptions={fetchOptionType_Async}
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

export default FormSearchSomeTable;