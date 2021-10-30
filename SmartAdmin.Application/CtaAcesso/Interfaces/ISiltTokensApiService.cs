
using SmartAdmin.Identity.Models;

namespace SmartAdmin.AppServices.CtaAcesso.Interfaces
{
    public interface ISiltTokensApiService : IService<SiltTokensApi>
    {
        void StoreToken(int idUsuario, string ipAddress, string refreshToken, string dataCriacao, string dataExpiracao);

        void StoreRefreshToken(int idUsuario, string ipAddress, string oldRefreshToken, string newRefreshToken, string dataCriacao, string dataExpiracao);

        bool RevokeToken(int idUsuario, string refreshToken, string ipAddress);
    }
}
