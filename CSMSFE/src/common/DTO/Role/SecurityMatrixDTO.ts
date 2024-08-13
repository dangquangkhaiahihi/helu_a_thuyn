export interface SecurityMatrixDTO {
    screenId: string;
    screenName: string;
    actions: Action[];
}

interface Action {
    actionId: string;
    actionName: string;
}
