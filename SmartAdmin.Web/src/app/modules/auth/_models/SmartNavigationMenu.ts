 export interface SmartNavigationMenu
 {
    key: number | null;
    parentKey: number | null;
    tituloMenu: string;
    area: string;
    actionName: string;
    controllerName: string;
    imagem: string;
    active: boolean;
    nivel: number;
    tags: string;
    subMenus: SmartNavigationMenu[];
}
