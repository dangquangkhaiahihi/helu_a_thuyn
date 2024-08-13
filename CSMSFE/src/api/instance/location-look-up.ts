import Service from "../api-service";
import ApiEndpointCollection from "../api-endpoint-collection";
import { LookUpItem } from "@/common/DTO/ApiResponse";

const service = new Service();

const LocationLookUpService = {
    LookUpProvince: async (data: { Keyword?: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.LocationLookUp.Province, params);
        } catch ( err ) {
            throw err;
        }
    },

    LookUpDistrict: async (data: { Keyword?: string; provinceId: string; }) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);
            params.append("provinceId", data.provinceId);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.LocationLookUp.District, params);
        } catch ( err ) {
            throw err;
        }
    },

    LookUpCommune: async (data: {Keyword?: string; districtId: string;}) => {
        try {
            const params = new URLSearchParams();
            data?.Keyword && params.append("Keyword", data.Keyword);
            params.append("districtId", data.districtId);

            return await service.get<LookUpItem[]>(ApiEndpointCollection.LocationLookUp.Commune, params);
        } catch ( err ) {
            throw err;
        }
    },
}

export default LocationLookUpService;