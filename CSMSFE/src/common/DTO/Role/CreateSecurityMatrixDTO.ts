export interface ScreenDTO {
    screenId: string;
    actions: ActionDTO[];
}

interface ActionDTO {
    actionId: string;
}

export interface CreateSecurityMatrixDTO {
    roleId: string;
    screens: ScreenDTO[];
}
