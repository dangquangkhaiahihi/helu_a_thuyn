
import { useEffect, useState } from 'react';
import { Control, SubmitHandler, UseFormHandleSubmit, UseFormReset, UseFormSetValue, UseFormWatch } from 'react-hook-form';
// MUI
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import Button from '@mui/material/Button';
// import MenuItem from '@mui/material/MenuItem';
import FormInput from '@/components/formInput';

// Icon
import SearchIcon from '@mui/icons-material/Search';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import FormSearchTogglableWrapper from '@/components/formSearchTogglableWrapper';

// Media query breakpoint
import { useMediaQuery } from "react-responsive";
import { DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION } from '@/common/default-config';
import LocationLookUpService from '@/api/instance/location-look-up';
import { ApiResponse, LookUpItem } from '@/common/DTO/ApiResponse';
import ProjectService from '@/api/instance/project';

interface IFormAdvanceSearchProjectProps {
    isOpenSearch: boolean;
    handleFilter: Function;
    //
    control: Control<any>;
    handleSubmit: UseFormHandleSubmit<any>;
    onSubmit: SubmitHandler<any>;
    errors: Record<string, any>;
    reset: UseFormReset<any>;
    watch: UseFormWatch<any>;
    setValue: UseFormSetValue<any>;
    FORM_SCHEMA: any;
    resetToDefaultPagination: Function;
}

const FormAdvanceSearchProject: React.FC<IFormAdvanceSearchProjectProps> = ({
    handleFilter,
    isOpenSearch,
    //
    control, handleSubmit, onSubmit, errors , reset, watch, setValue,
    FORM_SCHEMA, resetToDefaultPagination
}) => {
    //media query
    const isMD = useMediaQuery({ query: "(min-width: 900px)" });

    // START LOGIC SETUP LOOKUP LOCATION
    const provinceId = watch('ProvinceId');
    const districtId = watch('DistrictId');
    const [isProvinceInputDisabled, setIsProvinceInputDisabled] = useState<boolean>(false);
    const [isDistrictInputDisabled, setIsDistrictInputDisabled] = useState<boolean>(true);
    const [isCommuneInputDisabled, setIsCommuneInputDisabled] = useState<boolean>(true);

    useEffect(() => {
        // Trigger cái này để clear option của district
        setIsDistrictInputDisabled(true);
        
        // khi provinceId đổi giá trị thì clear luôn giá trị 2 thằng con
        setValue("DistrictId", "");
        setValue("CommuneId", "");
        setTimeout(() => {
            setIsDistrictInputDisabled(provinceId ? false : true);
        }, 200)

        if( !provinceId ){
            setIsProvinceInputDisabled(true);
            setTimeout(() => {
                setIsProvinceInputDisabled(false);
            }, 200)
        }
    }, [provinceId])

    useEffect(() => {
        // Trigger cái này để clear option của commune
        setIsCommuneInputDisabled(true);
        
        // khi districtId đổi giá trị thì clear luôn giá trị thằng con
        setValue("CommuneId", "");
        setTimeout(() => {
            setIsCommuneInputDisabled(districtId ? false : true);
        }, 200)
    }, [districtId])

    const handleGetLookUpProvince = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpProvince(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const handleGetLookUpDistrict = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword,
                provinceId: provinceId
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpDistrict(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const handleGetLookUpCommune = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword,
                districtId: districtId
            };
            const res: ApiResponse<LookUpItem[]> = await LocationLookUpService.LookUpCommune(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }
    // END LOGIC SETUP LOOKUP LOCATION

    const handleGetLookUpTypeProject = async (keyword: string) => {
        try {
            const param = {
                Keyword: keyword
            };
            const res: ApiResponse<LookUpItem[]> = await ProjectService.GetLookUpTypeProject(param);
            
            return res.content;
        } catch ( err ) {
            throw err;
        }
    }

    const onResetSearch = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();
        handleFilter(DEFAULT_PAGE_INDEX, DEFAULT_PAGE_SIZE, DEFAULT_SORT_EXPRESSION, null);
        reset();
        resetToDefaultPagination();
    }

	return (
        <Card component="section" sx={{mb: 3}} className={`_padding_transition ${isOpenSearch ? '' : 'hide'}`}>
            <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
                <FormSearchTogglableWrapper
                    isOpen={isOpenSearch}
                    numOfLines={isMD ? 3.5 : 5.5}
                >
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="date"
                            name="CreatedDate"
                            control={control}
                            rules={FORM_SCHEMA.CreatedDate}
                            errors={errors}
                            placeholder="Ngày tạo"
                            fullWidth
                            label="Ngày tạo"
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="Type"
                            control={control}
                            errors={errors}
                            placeholder="Loại dự án"
                            fullWidth
                            label="Loại dự án"
                            fetchOptions={handleGetLookUpTypeProject}
                        />
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="ProvinceId"
                            control={control}
                            errors={errors}
                            placeholder="Tỉnh/Thành phố"
                            fullWidth
                            label="Tỉnh/Thành phố"
                            fetchOptions={handleGetLookUpProvince}
                            disabled={isProvinceInputDisabled}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="DistrictId"
                            control={control}
                            errors={errors}
                            placeholder="Quận/Huyện"
                            fullWidth
                            label="Quận/Huyện"
                            fetchOptions={handleGetLookUpDistrict}
                            disabled={isDistrictInputDisabled}
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <FormInput
                            type="async-auto-complete"
                            name="CommuneId"
                            control={control}
                            errors={errors}
                            placeholder="Phường/Xã"
                            fullWidth
                            label="Phường/Xã"
                            fetchOptions={handleGetLookUpCommune}
                            disabled={isCommuneInputDisabled}
                        />
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
                </FormSearchTogglableWrapper>
            </form>
        </Card>
	);
}

export default FormAdvanceSearchProject;