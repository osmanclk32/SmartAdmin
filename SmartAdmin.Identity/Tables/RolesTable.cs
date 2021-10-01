using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Microsoft.AspNetCore.Identity;

using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;

using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata;
using System.Transactions;
using System.Data;

namespace SmartAdmin.Identity.Tables
{
    internal class RolesTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RolesTable(IDatabaseConnectionFactory databaseConnectionFactory) =>
            _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            int rowsInserted;

            var query = new Query("cta_grupo").AsInsert(role);

            using (var connection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                using (var db = new QueryFactory(connection, new PostgresCompiler()))
                {
                    rowsInserted = await db.ExecuteAsync(query);
                }
            }

            return rowsInserted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = string.Empty,
                    Description = $"O Grupo com o nome {role.NomeGrupo} não pôde ser inserida."
                });
        }
        
        public async Task<IdentityResult> UpdateAsync(ApplicationRole role)
        {
            var query = new Query("cta_grupo").AsUpdate(role).Where("id_role", role.IdGrupo).Where("id_tenant",role.IdTenant);
            
            using (var connection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                using (var db = new QueryFactory(connection, new PostgresCompiler()))
                {
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        await db.ExecuteAsync(query, transaction);

                        if (role.Claims.Count() > 0)
                        {

                            query = new Query("cta_grupo_claims").AsDelete().Where("id_grupo", role.IdGrupo).Where("id_tenant", role.IdTenant);

                            await db.ExecuteAsync(query, transaction);

                            foreach (var claim in role.Claims)
                            {
                                var roleClaim = new ApplicationRoleClaims
                                {
                                    IdGrupo = role.IdGrupo,
                                    IdTenant = role.IdTenant,
                                    ClaimType = claim.Type,
                                    ClaimValue = claim.Value
                                };

                                query = new Query("cta_grupo_claims").AsInsert(roleClaim);

                                await db.ExecuteAsync(query, transaction);
                            }
                        }

                        try
                        {
                            transaction.Commit();
                        }
                        catch
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch
                            {
                                return IdentityResult.Failed(new IdentityError
                                {
                                    Code = nameof(UpdateAsync),
                                    Description = $"O Grupo com o nome {role.NomeGrupo} não pôde ser atualizada. A operação não pôde ser revertida."
                                });
                            }

                            return IdentityResult.Failed(new IdentityError
                            {
                                Code = nameof(UpdateAsync),
                                Description = $"O Grupo com o nome {role.NomeGrupo} não pôde ser atualizada. A operação não pôde ser revertida."
                            });
                        }
                    }
                }
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            
            int rowsDeleted;

            var query = new Query("cta_grupo").AsDelete().Where("id_grupo", role.IdGrupo).Where("id_tenant", role.IdTenant);

            using (var connection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                using (var db = new QueryFactory(connection, new PostgresCompiler()))
                {
                    rowsDeleted = await db.ExecuteAsync(query);
                }
            }
            
            return rowsDeleted == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"O Grupo com o nome {role.NomeGrupo} não pôde ser excluída."
            });
        }

        public async Task<ApplicationRole> FindByIdAsync(int roleId,int idTenant)
        {
            
            var query = new Query("cta_grupo").Select("*").Where("id_grupo", roleId).Where("id_tenant", idTenant);

            using var connection = await _databaseConnectionFactory.CreateConnectionAsync();
            
            using var db = new QueryFactory(connection, new PostgresCompiler());

            return await db.FirstOrDefaultAsync<ApplicationRole>(query);
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName,int idTenant)
        {

            var query = new Query("cta_grupo").Select("*").Where("nome_grupo", normalizedRoleName).Where("id_tenant", idTenant);

            using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

            using var db = new QueryFactory(connection, new PostgresCompiler());

            return await db.FirstOrDefaultAsync<ApplicationRole>(query);
            
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync(int idTenant)
        {
            var query = new Query("cta_grupo").Select("*").Where("id_tenant", idTenant);

            using var connection = await _databaseConnectionFactory.CreateConnectionAsync();

            using var db = new QueryFactory(connection, new PostgresCompiler());

            return await db.GetAsync<ApplicationRole>(query);
        }
    }
}
