import { sceneHandler } from "@core/scene-handler";
import { Action } from "./actions";

export const executeCore = async (action: Action) => {
  if (action.type === "START") {
    const { container } = action.payload;
    return sceneHandler.start(container);
  } else if (action.type === "UPDATE_SETTINGS") {
    const { settings } = action.payload;
    sceneHandler.updateSettings(settings);
  }
};
