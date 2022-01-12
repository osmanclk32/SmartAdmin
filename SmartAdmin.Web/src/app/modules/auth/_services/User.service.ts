import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiResult } from '../_models';
import { SmartNavigationMenu } from '../_models/SmartNavigationMenu';

@Injectable({
  providedIn: 'root'
})
export class UserService
{

    constructor(private http: HttpClient) { }

    listaItensMenu(idGrupoUsuario: number, idTenant: number) : Observable<SmartNavigationMenu[]>
    {
        let itensMenuUrl: string = `${environment.apiEndpoint}/usuario/lista-itens-menu/${idGrupoUsuario}/${idTenant}`;

        return this.http.get<ApiResult<SmartNavigationMenu[]>>(itensMenuUrl)
                   .pipe(map(response => response.data));
    }

}
