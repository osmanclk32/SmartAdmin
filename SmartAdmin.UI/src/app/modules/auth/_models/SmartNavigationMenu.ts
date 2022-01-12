 export class SmartNavigationMenu
 {
   constructor(key: number,parentKey: number,tituloMenu: string,area: string,actionName: string,controllerName: string,imagem: string, active: boolean,nivel: number,tags: string,subMenus: SmartNavigationMenu[])
   {
       this.key = key;
       this.parentKey = parentKey;
       this.tituloMenu = tituloMenu;
       this.area = area;
       this.actionName = actionName;
       this.controllerName = controllerName;
       this.imagem = imagem;
       this.active = active;
       this.nivel = nivel;
       this.tags = tags;
       this.subMenus = subMenus;
   }

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

