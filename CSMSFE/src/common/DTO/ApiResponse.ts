export interface PagedListContent<T> {
    items: T[];
    pageCount: number;
    totalItemCount: number;
    pageIndex: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export interface LookUpItem {
    value: string;
    label: string;
}

export interface ApiResponse<T> {
    content: T;
    err: {
        errorMessage: string;
        errorType: string;
    };
}

