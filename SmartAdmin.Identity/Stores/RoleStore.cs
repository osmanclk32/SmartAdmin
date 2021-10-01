using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartAdmin.Identity.Extensions;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;
using SmartAdmin.Identity.Tables;

namespace SmartAdmin.Identity.Stores
{
    // <summary>
    /// Store for Application Roles.  Makes calls to the table objects
    /// </summary>
    public class RoleStore : IQueryableRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>, IRoleStore<ApplicationRole>
    {
        private readonly RolesTable _rolesTable;
        private readonly RoleClaimsTable _roleClaimsTable;
        private readonly int _idTenant;

        public RoleStore(IDatabaseConnectionFactory databaseConnectionFactory, ApplicationTenant tenant)
        {
            _rolesTable = new RolesTable(databaseConnectionFactory);
            _roleClaimsTable = new RoleClaimsTable(databaseConnectionFactory);
            
            _idTenant = tenant.IdTenant; 
        }

        public IQueryable<ApplicationRole> Roles => Task.Run(() => _rolesTable.GetAllRolesAsync(_idTenant)).Result.AsQueryable();

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            role.ThrowIfNull(nameof(role));

            return _rolesTable.CreateAsync(role);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesTable.DeleteAsync(role);
        }

        public void Dispose() { }

        public Task<ApplicationRole> FindByIdAsync(int roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            roleId.ThrowIfNull(nameof(roleId));
          //  var isValidGuid = Guid.TryParse(roleId, out var roleGuid);

            //if (!isValidGuid)
            //{
            //    throw new ArgumentException("Parameter roleId is not a valid Guid.", nameof(roleId));
            //}

            return _rolesTable.FindByIdAsync(roleId,_idTenant);
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            normalizedRoleName.ThrowIfNull(nameof(normalizedRoleName));
            return _rolesTable.FindByNameAsync(normalizedRoleName,_idTenant);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.NomeGrupo);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.IdGrupo.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.NomeGrupo);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            normalizedName.ThrowIfNull(nameof(normalizedName));
            role.NomeGrupo = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            roleName.ThrowIfNull(nameof(roleName));
            role.NomeGrupo = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesTable.UpdateAsync(role);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            
            role.Claims = role.Claims ?? (await _roleClaimsTable.GetClaimsAsync(role.IdGrupo, role.IdTenant)).ToList();

            return role.Claims;
            
        }

        public async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims = role.Claims ?? (await _roleClaimsTable.GetClaimsAsync(role.IdGrupo, role.IdTenant)).ToList();
            var foundClaim = role.Claims.FirstOrDefault(x => x.Type == claim.Type);

            if (foundClaim != null)
            {
                role.Claims.Remove(foundClaim);
                role.Claims.Add(claim);
            }
            else
            {
                role.Claims.Add(claim);
            }
             
        }

        public async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims = role.Claims ?? (await _roleClaimsTable.GetClaimsAsync(role.IdGrupo, role.IdTenant)).ToList();
            role.Claims.Remove(claim);
        }
    }
}
