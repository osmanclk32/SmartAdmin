using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

using SqlKata;

namespace SmartAdmin.Api
{
    public class AppConst
    {
        public const string GRANT_TYPE_PASSWORD = "password";
        public const string GRANT_TYPE_REFRESH = "refresh_token";
        public const string SMART_ADMIN_COOKIE  = "SmartAdminCookieMiddleware";
        public const string INVALID_USER_OR_PWD = "Usuário ou senha inválido(s)";
        public const string AUTH_SUCCESS = "Autenticação realizada com sucesso.";
        public const string USER_NOT_ALLOWED = "Falha ao autenticar, usuário sem permissão para fazer login.";
        public const string USER_IS_LOCKED_OUT = "Falha ao autenticar, conta de usuário bloqueada.";
        public const string INVALID_DATA_TOKEN = "Dados inválidos para obtenção de Token";
    }
}
