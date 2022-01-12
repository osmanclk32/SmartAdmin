import { DynamicComponent } from './../_models/DynamicComponent';
import { CadEmpresaModule } from './../../configuracoes/cad-empresa/cad-empresa.module';
import { Injectable,  InjectionToken, Type, Injector,  Inject, NgModuleFactory, ViewContainerRef, Compiler} from '@angular/core';

export interface LazyModules
{
  [key: string]: {loadChildren: () => Promise<NgModuleFactory<any> | Type<any>>};
}

export const lazyMap: LazyModules =
{
    cadastro_empresa: {loadChildren: () => import('src/app/modules/configuracoes/cad-empresa/cad-empresa.module').then(m => m.CadEmpresaModule) },

};

export const LAZY_MODULES_MAP = new InjectionToken('LAZY_MODULES_MAP', {
  factory: () => lazyMap
});

export type ModuleWithRoot = Type<any> & { rootComponent: Type<any> };

@Injectable({providedIn: 'root'})
export class LazyService
{

      constructor(
      private injector: Injector,
      private compiler: Compiler,
      @Inject(LAZY_MODULES_MAP) private modulesMap: LazyModules
    ) {}

    async loadAndRenderComponents(data: any, container: ViewContainerRef)
    {
        container.clear();

        for await (const { type, data: componentData } of data.components)
        {
            let moduleOrFactory = await this.modulesMap[type].loadChildren();
            let moduleFactory;

            if (moduleOrFactory instanceof NgModuleFactory)
            {
                moduleFactory = moduleOrFactory;                // AOT
            }
            else
            {
                moduleFactory =  await this.compiler.compileModuleAsync(moduleOrFactory);   //JIT
            }

            const moduleRef = moduleFactory.create(this.injector);

            const rootComponent = (moduleFactory.moduleType as ModuleWithRoot).rootComponent;

            const factory = moduleRef.componentFactoryResolver.resolveComponentFactory(rootComponent);

            const componentRef = container.createComponent(factory);

            (componentRef.instance as DynamicComponent).data = componentData;
        }
  }

}
