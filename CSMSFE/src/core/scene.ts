'use client';

import * as THREE from 'three';
import * as OBC from 'openbim-components';
import {Settings} from "./src/settings";
import {Initializer} from "./src/initializer.ts";
import {ContextMenu} from "./src/context-menu.ts";
import { ModelLoader } from './src/model-loader.ts';

export class Scene {

    public _contextMenu: ContextMenu
    public _initializer: Initializer;
    public _components: OBC.Components;
    public _loader: ModelLoader;
    public _settings: Settings;

    constructor(container: HTMLDivElement) {
        const components = new OBC.Components();
        this._components = components;
        this._initializer = new Initializer(components, container);
        this._settings = new Settings(components);
        this._contextMenu = new ContextMenu(components);
        this._loader = new ModelLoader(components);

        (window as any).OBC = OBC;
        (window as any).THREE = THREE;


    }

    async updateSettings(config: any) {
        await this._settings.update(config);
    }

}
