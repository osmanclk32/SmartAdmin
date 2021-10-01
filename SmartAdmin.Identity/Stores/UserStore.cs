using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using SmartAdmin.Identity.Extensions;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;
using SmartAdmin.Identity.Tables;

namespace SmartAdmin.Identity.Stores
{

    /// <summary>
    /// Representa o acesso aos dados do usuário
    /// </summary>
    public class UserStore :
        IQueryableUserStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserPhoneNumberStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>,
        IUserSecurityStampStore<ApplicationUser>,
        IUserClaimStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserAuthenticationTokenStore<ApplicationUser>,
        IUserAuthenticatorKeyStore<ApplicationUser>,
        IUserStore<ApplicationUser>
    {
        private readonly UsersTable _usersTable;
        private readonly UserRolesTable _usersRolesTable;
        private readonly RolesTable _rolesTable;
        private readonly UserClaimsTable _usersClaimsTable;
        private readonly UserLoginsTable _usersLoginsTable;
        private readonly UserTokensTable _userTokensTable;
        private readonly int _idTenant;

        public UserStore(IDatabaseConnectionFactory databaseConnectionFactory, ApplicationTenant tenant)
        {
            _usersTable = new UsersTable(databaseConnectionFactory);
            _usersRolesTable = new UserRolesTable(databaseConnectionFactory);
            _rolesTable = new RolesTable(databaseConnectionFactory);
            _usersClaimsTable = new UserClaimsTable(databaseConnectionFactory);
            _usersLoginsTable = new UserLoginsTable(databaseConnectionFactory);
            _userTokensTable = new UserTokensTable(databaseConnectionFactory);
            _idTenant = tenant.IdTenant;
        }

        public void Dispose() { }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.ThrowIfNull(nameof(user));

            return Task.FromResult(user.IdUsuario.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.ThrowIfNull(nameof(user));

            return Task.FromResult(user.NomeUsuario);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.ThrowIfNull(nameof(user));
            
            user.NomeUsuario = userName;

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.NomeUsuarioNormalizado);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.NomeUsuarioNormalizado = normalizedName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.ThrowIfNull(nameof(user));

            return _usersTable.CreateAsync(user);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.CarimboConcorrencia = Guid.NewGuid().ToString();
            return _usersTable.UpdateAsync(user);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            int idUsuario;

            bool convertido = Int32.TryParse(userId, out idUsuario);

            if (convertido)
            {
                return _usersTable.FindByIdAsync(idUsuario, _idTenant);
            }

            return null;
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _usersTable.FindByNameAsync(normalizedUserName,_idTenant);
        }

        public IQueryable<ApplicationUser> Users { get; }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.EmailConfirmado);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.EmailConfirmado = confirmed;
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _usersTable.FindByEmailAsync(normalizedEmail, _idTenant);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.EmailNormalizado);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.EmailNormalizado = normalizedEmail;
            return Task.CompletedTask;
        }

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.ThrowIfNull(nameof(user));
            login.ThrowIfNull(nameof(login));

            user.Logins = user.Logins ?? (await GetLoginsAsync(user)).ToList();

            var foundLogin = user.Logins.SingleOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);

            if (foundLogin == null)
            {
                user.Logins.Add(login);
            }
        }

        public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            loginProvider.ThrowIfNull(nameof(loginProvider));
            providerKey.ThrowIfNull(nameof(providerKey));
            user.Logins = user.Logins ?? (await GetLoginsAsync(user)).ToList();
            var login = user.Logins.SingleOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

            if (login != null)
            {
                user.Logins.Remove(login);
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.Logins = user.Logins ?? (await _usersLoginsTable.GetLoginsAsync(user, _idTenant)).ToList();
            return user.Logins;
        }

        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            loginProvider.ThrowIfNull(nameof(loginProvider));
            return _usersLoginsTable.FindByLoginAsync(loginProvider, providerKey,_idTenant);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.ThrowIfNull(nameof(user));

            passwordHash.ThrowIfNull(nameof(passwordHash));

            user.SenhaHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.ThrowIfNull(nameof(user));

            return Task.FromResult(user.SenhaHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.ThrowIfNull(nameof(user));

            return Task.FromResult(!string.IsNullOrEmpty(user.SenhaHash));
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
        //    user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(false);
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            stamp.ThrowIfNull(nameof(stamp));
            user.CarimboSeguranca = stamp;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.CarimboSeguranca);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return await _usersClaimsTable.GetClaimsAsync(user, _idTenant);
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            claims.ThrowIfNull(nameof(claims));
            user.Claims = user.Claims ?? (await GetClaimsAsync(user, cancellationToken)).ToList();

            foreach (var claim in claims)
            {
                var foundClaim = user.Claims.FirstOrDefault(x => x.Type == claim.Type);

                if (foundClaim != null)
                {
                    user.Claims.Remove(foundClaim);
                    user.Claims.Add(claim);
                }
                else
                {
                    user.Claims.Add(claim);
                }
            }
        }

        public async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            claim.ThrowIfNull(nameof(claim));
            newClaim.ThrowIfNull(nameof(newClaim));
            user.Claims = user.Claims ?? (await GetClaimsAsync(user, cancellationToken)).ToList();
            var foundClaim = user.Claims.FirstOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);

            if (foundClaim != null)
            {
                foundClaim = newClaim;
            }
            else
            {
                user.Claims.Add(newClaim);
            }
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            claims.ThrowIfNull(nameof(claims));
            user.Claims = user.Claims ?? (await GetClaimsAsync(user, cancellationToken)).ToList();

            foreach (var claim in claims)
            {
                user.Claims.Remove(claim);
            }
        }

        public Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            claim.ThrowIfNull(nameof(claim));
            return _usersTable.GetUsersForClaimAsync(claim);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.DataHoraFimBloqueio);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.DataHoraFimBloqueio = lockoutEnd?.UtcDateTime;
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.QtdeFalhasAcesso++;
            return Task.FromResult(user.QtdeFalhasAcesso);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.QtdeFalhasAcesso = 0;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.QtdeFalhasAcesso);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            return Task.FromResult(user.Bloqueado);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.Bloqueado = enabled;
            return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            roleName.ThrowIfNull(nameof(roleName));
            var role = await _rolesTable.FindByNameAsync(roleName,_idTenant);

            if (role == null)
            {
                return;
            }

            user.Roles = user.Roles ?? (await _usersRolesTable.GetRolesAsync(user, _idTenant)).ToList();

            if (await IsInRoleAsync(user, roleName, cancellationToken))
            {
                return;
            }

            user.Roles.Add(new ApplicationRole
            {
                NomeGrupo = roleName,
                IdGrupo = role.IdGrupo
            });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            roleName.ThrowIfNull(nameof(roleName));
            user.Roles = user.Roles ?? (await _usersRolesTable.GetRolesAsync(user, _idTenant)).ToList();
            var role = user.Roles.SingleOrDefault(x => x.NomeGrupo == roleName);

            if (role != null)
            {
                user.Roles.Remove(role);
            }
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.Roles = user.Roles ?? (await _usersRolesTable.GetRolesAsync(user, _idTenant)).ToList();
            return user.Roles.Select(x => x.NomeGrupo).ToList();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            roleName.ThrowIfNull(nameof(roleName));
            user.Roles = user.Roles ?? (await _usersRolesTable.GetRolesAsync(user, _idTenant)).ToList();
            return user.Roles.Any(x => x.NomeGrupo == roleName);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _usersTable.GetUsersInRoleAsync(roleName, _idTenant);
        }

        public async Task SetTokenAsync(ApplicationUser user, string loginProvider, string name, string value,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            loginProvider.ThrowIfNull(nameof(loginProvider));
            user.Tokens = user.Tokens ?? (await _userTokensTable.GetTokensAsync(user.IdUsuario,_idTenant)).ToList();

            user.Tokens.Add(new ApplicationUserTokens
            {
                IdUsuario = user.IdUsuario,
                IdTenant = user.IdTenant,
                ProvedorLogin = loginProvider,
                Nome = name,
                Valor = value
            });
        }

        public async Task RemoveTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            loginProvider.ThrowIfNull(nameof(loginProvider));
            user.Tokens = user.Tokens ?? (await _userTokensTable.GetTokensAsync(user.IdUsuario,_idTenant)).ToList();
            var token = user.Tokens.SingleOrDefault(x => x.ProvedorLogin == loginProvider && x.Nome == name);
            user.Tokens.Remove(token);
        }

        public async Task<string> GetTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            loginProvider.ThrowIfNull(nameof(loginProvider));
            name.ThrowIfNull(nameof(name));
            user.Tokens = user.Tokens ?? (await _userTokensTable.GetTokensAsync(user.IdUsuario,_idTenant)).ToList();
            return user.Tokens.SingleOrDefault(x => x.ProvedorLogin == loginProvider && x.Nome == name)?.Valor;
        }

        public Task SetAuthenticatorKeyAsync(ApplicationUser user, string key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAuthenticatorKeyAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
