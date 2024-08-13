
import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
// MUI
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
// import MenuItem from '@mui/material/MenuItem';
import FormInput from '@/components/formInput';

// Icon
import AddIcon from '@mui/icons-material/Add';
import FilterListIcon from '@mui/icons-material/FilterList';
import FilterListOffIcon from '@mui/icons-material/FilterListOff';

import dayjs from 'dayjs';
import { ProjectQueryFilter } from '@/common/DTO/Project/ProjectQueryFilter';
import { IconButton, Stack, Tooltip, Typography, debounce } from '@mui/material';
import FormAdvanceSearchProject from './form-advance-search-project';
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';

const FORM_SCHEMA = {
    Name: {},
    ProjectRole: {},
    Type: {},
    CreatedDate: {
        validate: (value: any) => {
            return value ? (dayjs(value).isValid() || 'Ngày được chọn không hợp lệ') : true
        },
    },
    ProvinceId: {},
    DistrictId: {},
    CommuneId: {},
};

interface IPageHeaderFormFilterProps {
    handleFilter: Function;
    setIsOpenAddUpdate: Function;
    resetToDefaultPagination: Function;
}

const PageHeaderFormFilter: React.FC<IPageHeaderFormFilterProps> = ({
    handleFilter,
    setIsOpenAddUpdate,
    resetToDefaultPagination
}) => {
    
    const [isFirstLoad, setIsFirstLoad] = useState<boolean>(true);
    const [openAdvanceFilter, setOpenAdvanceFilter] = useState<boolean>(false);

	const { control, handleSubmit, formState: { errors }, reset, watch, setValue, getValues } = useForm({
		mode: 'onChange',
		defaultValues: {
			Name: "",
            ProjectRole: "",
            Type: "",
            CreatedDate: "",
            ProvinceId: "",
            DistrictId: "",
            CommuneId: "",
		},
	});
    const nameValue = watch('Name');
    const projectRoleValue = watch('ProjectRole');

    useEffect(() => {
        setIsFirstLoad(false);
    }, [])

    useEffect(() => {
        if (isFirstLoad) return;
        verify();

    }, [nameValue, projectRoleValue])
    
    const verify = useCallback(
        debounce(() => {
            handleSubmit(onSubmit)()
        }, 200),
        []
    );

	const onSubmit = (data: ProjectQueryFilter) => {
        if ( data.CreatedDate ) data.CreatedDate = dayjs(data.CreatedDate).format();
        handleFilter(undefined, undefined, undefined, data);
    }
    
    // Nếu đóng search advance thì clear hết các biến của search advance
    useEffect(() => {
        if ( isFirstLoad || openAdvanceFilter) return;
        resetAdvanceSearch();
    }, [openAdvanceFilter])

    const onSubmitResetAdvanceSearch = (data: ProjectQueryFilter) => {
        handleFilter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, data);
    }

    const resetAdvanceSearch = () => {
        if( getValues("CreatedDate") || getValues("ProvinceId") || getValues("DistrictId") || getValues("CommuneId") ) {
            setValue("CreatedDate", "");
            setValue("ProvinceId", "");
            setValue("DistrictId", "");
            setValue("CommuneId", "");
            handleSubmit(onSubmitResetAdvanceSearch)();
            resetToDefaultPagination();
        }
    }

	return (
        <>
            <Stack
                component="header"
                my={3}
                direction="row"
                justifyContent="space-between"
                alignItems="center"
                flexWrap="wrap"
                textTransform="uppercase"
                gap={{ xs: 1, md: 20}}
            >
                <Typography
                    variant="h5"
                    fontSize={18}
                    textTransform="inherit"
                    borderLeft={5}
                    borderColor="primary.400"
                    pl={1.5}
                    height="fit-content"
                >
                    Danh sách dự án
                </Typography>
                
                <Grid container
                    direction="row"
                    justifyContent="flex-end"
                    alignItems="center"
                    flex={{ md: '1', xs: 'none' }}
                    columnSpacing={2}
                >
                    <Grid item xs={12} md={5} mb={{ xs: 1, md: 0 }}>
                        <FormInput
                            type="text"
                            name="Name"
                            control={control}
                            placeholder="Tên dự án"
                            fullWidth
                            sx={{mb: 0}}
                        />
                    </Grid>
                    <Grid item flex={{xs: 1}} md={8}>
                        <FormInput
                            type="auto-complete"
                            name="ProjectRole"
                            control={control}
                            placeholder="Vị trí"
                            fullWidth
                            sx={{mb: 0}}
                            options={[
                                {value: "", label: "Đây là fix cứng, chưa có get lookup ProjectRole"},
                                {value: "1", label: "Người sở hữu"},
                                {value: "2", label: "Cộng tác viên"},
                            ]}
                        />
                    </Grid>
                    <Grid item
                        justifyContent={{
                            xs: "flex-end"
                        }}
                    >
                        <Button
                            type="button"
                            variant="contained"
                            disableElevation
                            startIcon={<AddIcon />}
                            color='primary'
                            onClick={(e) => {
                                e.preventDefault();
                                setIsOpenAddUpdate((prev: any) => !prev);
                            }}
                        >
                            Thêm mới
                        </Button>
                    </Grid>
                    <Grid item
                        justifyContent={{
                            xs: "flex-end"
                        }}
                    >
                        <Tooltip title={!openAdvanceFilter ? "Tìm kiếm nâng cao" : "Tắt tìm kiếm nâng cao"}>
                            <IconButton color={!openAdvanceFilter ? "primary" : "error"}
                                onClick={() => {setOpenAdvanceFilter(prev => !prev)}}
                            >
                                {!openAdvanceFilter ? <FilterListIcon /> : <FilterListOffIcon />}
                            </IconButton>
                        </Tooltip>
                    </Grid>
                </Grid>
            </Stack>
            <FormAdvanceSearchProject
                isOpenSearch={openAdvanceFilter}
                handleFilter={handleFilter}
                // form control chung với component này
                control={control}
                handleSubmit={handleSubmit}
                onSubmit={onSubmit}
                errors={errors}
                reset={reset}
                watch={watch}
                setValue={setValue}
                FORM_SCHEMA={FORM_SCHEMA}
                resetToDefaultPagination={resetToDefaultPagination}
            />
        </>
	);
}

export default PageHeaderFormFilter;