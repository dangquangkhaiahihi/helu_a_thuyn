import { useState } from 'react';
import { useForm } from 'react-hook-form';
// MUI
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import Button from '@mui/material/Button';
import FormInput from '@/components/formInput';

// Icon
import SearchIcon from '@mui/icons-material/Search';
import SearchOffIcon from '@mui/icons-material/SearchOff';

// Media query breakpoint
import { useMediaQuery } from "react-responsive";
import { IssueQueryFilter } from '@/common/DTO/Issue/IssueQueryFilter';
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';

const FORM_SCHEMA = {
    type: {},
    status: {},
    createdBy: {},
};

interface FormSearchIssueProps {
    handleFilter: Function;
    optionsType: Array<{ value: string; label: string }>;
    fetchOptionType_Async: (query: string) => Promise<{ value: string | number; label: string }[]>;
}

const FormSearchIssue: React.FC<FormSearchIssueProps> = ({
    handleFilter,
    optionsType,
    fetchOptionType_Async,
}) => {
    //media query
    const isMD = useMediaQuery({ query: "(min-width: 900px)" });

	const { control, handleSubmit, formState: { errors }, reset } = useForm<IssueQueryFilter>({
		mode: 'onChange',
		defaultValues: {
            type: "",
            status: "",
            createdBy: "",
		},
	});
    
	const onSubmit = (data: IssueQueryFilter) => {
        console.log(data);
        handleFilter(undefined, undefined, undefined, data);
    }

    const onResetSearch = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();
        handleFilter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, null);
        reset();
    }

	return (
        <Card component="section">
            <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
                <Grid container
                    direction="row"
                    justifyContent="center"
                    alignItems="center"
                    columnSpacing={3}
                >
                    <Grid item xs={3} md={3}>
                        <FormInput
                            type="text"
                            name="type"
                            control={control}
                            rules={FORM_SCHEMA.type}
                            errors={errors}
                            placeholder="Type"
                            label="Type"
                            fullWidth
                        />
                    </Grid>
                    

                    
                    <Grid item xs={3} md={3} style={{paddingBottom:'20.5px'}}>
                        <FormInput
                            type="select"
                            name="status"
                            control={control}
                            rules={FORM_SCHEMA.status}
                            errors={errors}
                            placeholder="Status"
                            fullWidth
                            label="Status"
                            options={[
                                { value: "Đang xử lý", label: "Đang xử lý" },
                                { value: "Chờ xử lý", label: "Chờ xử lý" },
                                { value: "Đã xử lý", label: "Đã xử lý" },
                            ]}
                        />
                    </Grid>
                    
                    <Grid item xs={3} md={3}>
                        <FormInput
                            type="async-auto-complete"
                            name="CreatedBy"
                            control={control}
                            rules={FORM_SCHEMA.createdBy}
                            errors={errors}
                            placeholder="CreatedBy"
                            fullWidth
                            label="Created By"
                            fetchOptions={()=>fetchOptionType_Async('')}
                        />
                    </Grid>

                </Grid>
                <Grid container
                    direction="row"
                    justifyContent="center"
                    alignItems="center"
                    columnSpacing={2}
                >
                    <Grid item>
                        <Button
                            type="submit"
                            variant="contained"
                            disableElevation
                            fullWidth
                            startIcon={<SearchIcon />}
                            color='primary'
                        >
                            Search
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
                            Reset
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Card>
	);
}

export default FormSearchIssue;
