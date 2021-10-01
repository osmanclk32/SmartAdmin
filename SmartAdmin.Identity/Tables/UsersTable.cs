using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace SmartAdmin.Identity.Tables
{
    internal class UsersTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UsersTable(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            int rowsInserted;

            var query = new Query("cta_usuario").AsInsert(user);

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
                    Code = nameof(CreateAsync),
                    Description = $"Não foi possível inserir o usuário {user.NomeUsuario}."
                });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, int idTenant)
        {
            const string command = "delete " +
                                   "FROM cta_usuario " +
                                   "WHERE id_usuario = @Id and id_tenant = @id_tenant;";

            int rowsDeleted;

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                rowsDeleted = await sqlConnection.ExecuteAsync(command, new
                {
                    user.IdUsuario,
                    idTenant
                });
            }

            return rowsDeleted == 1
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Code = nameof(DeleteAsync),
                    Description = $"O usuário {user.NomeUsuario} não pôde ser excluído."
                });
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId,int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario " +
                                   "WHERE id = @userId and id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(command, new
                {
                    userId,
                    idTenant
                });
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName,int idTenant)
        {
          
            ApplicationUser appUser = null;

            var query = new Query("cta_usuario").Where("nome_usuario_normalizado", normalizedUserName).Where("id_tenant",idTenant);

       
            try
            {
                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    using (var db = new QueryFactory(sqlConnection, new PostgresCompiler()))
                    {
                        appUser = await db.FirstOrDefaultAsync<ApplicationUser>(query);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return appUser;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail,int idTenant)
        {
            ApplicationUser user = null;

            const string command = "SELECT * " +
                                   "FROM cta_usuario " +
                                   "WHERE email_normalizado = @NormalizedEmail and id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                user = await sqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(command, new
                {
                    NormalizedEmail = normalizedEmail,
                    IdTenant = idTenant
                });
            }

            return user;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            // A implementação aqui pode parecer um pouco estranha, no entanto, há um motivo para isso.
            // Os armazenamentos de identidade do ASP.NET Core seguem um padrão UOW (Unidade de Trabalho), o que praticamente significa que, quando uma operação é chamada, ela não necessariamente grava no banco de dados.
            // Ele rastreia as mudanças feitas e finalmente confirma no banco de dados. Os métodos UserStore apenas manipulam o usuário e apenas CreateAsync, UpdateAsync e DeleteAsync de IUserStore <>
            // escreve no banco de dados. Isso faz sentido porque, desta forma, evitamos a conexão com o banco de dados o tempo todo e também podemos confirmar todas as alterações de uma vez usando uma transação.
            var query = new Query("cta_usuario").AsUpdate(new
                {
                    id_tenant = user.IdTenant,
                    id_colaborador = user.IdColaborador,
                    id_situacao_cadastral = user.IdSituacaoCadastral,
                    nome_usuario = user.NomeUsuario,
                    nome_usuario_normalizado = user.NomeUsuarioNormalizado,
                    email = user.Email,
                    email_normalizado = user.EmailNormalizado,
                    email_confirmado = user.EmailConfirmado,
                   // senha_hash = user.Senha != null ? user.Senha : senha_hash,
                    carimbo_seguranca = user.CarimboSeguranca,
                    carimbo_concorrencia = user.CarimboConcorrencia,
                    telefone = user.Telefone,
                    telefone_confirmado = user.TelefoneConfirmado,
                    bloqueado = user.Bloqueado,
                    data_hora_fim_bloqueio =  user.DataHoraFimBloqueio,
                    qtde_falhas_acesso = user.QtdeFalhasAcesso
                })
                .Where("id_usuario", user.IdUsuario)
                .Where("id_tenant", user.IdTenant);


            //const string updateUserCommand =
            //    "UPDATE identity_users " +
            //    "SET username = @UserName, normalized_username = @NormalizedUserName, email = @Email, normalized_email = @NormalizedEmail, email_confirmed = @EmailConfirmed, " +
            //    "password_hash = @PasswordHash, security_stamp = @SecurityStamp, concurrency_stamp = @ConcurrencyStamp, phone_number = @PhoneNumber, " +
            //    "phone_number_confirmed = @PhoneNumberConfirmed, two_factor_enabled = @TwoFactorEnabled, lockout_end = @LockoutEnd, lockout_enabled = @LockoutEnabled, " +
            //    "access_failed_count = @AccessFailedCount " +
            //    "WHERE id = @Id;";

            using (var connection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                using (var db = new QueryFactory(connection, new PostgresCompiler()))
                {
                    using (var transaction = db.Connection.BeginTransaction())
                    {
                        await db.ExecuteAsync(query, transaction);

                        if (user.Claims?.Count() > 0)
                        {
                            query = new Query("cta_usuario_claims").AsDelete().Where("id_usuario", user.IdUsuario)
                                .Where("id_tenant", user.IdTenant);

                            await db.ExecuteAsync(query, transaction);

                            foreach (var claim in user.Claims)
                            {
                                var userClaim = new ApplicationUserClaims
                                {
                                    IdUsuario = user.IdUsuario,
                                    IdTenant = user.IdTenant,
                                    ClaimType = claim.Type,
                                    ClaimValue = claim.Value
                                };

                                query = new Query("cta_usuario_claims").AsInsert(userClaim);

                                await db.ExecuteAsync(query, transaction);
                            }
                        }

                        if (user.Logins?.Count() > 0)
                        {
                            query = new Query("cta_usuario_logins").AsDelete().Where("id_usuario", user.IdUsuario)
                                .Where("id_tenant", user.IdTenant);

                            await db.ExecuteAsync(query, transaction);

                            foreach (var login in user.Logins)
                            {
                                var userLogin = new ApplicationUserLogins
                                {
                                    IdUsuario = user.IdUsuario,
                                    IdTenant = user.IdTenant,
                                    ProvedorLogin = login.LoginProvider,
                                    ChaveProvedor = login.ProviderKey,
                                    NomeProvedor = login.ProviderDisplayName
                                };

                                query = new Query("cta_usuario_logins").AsInsert(userLogin);

                                await db.ExecuteAsync(query, transaction);
                            }
                        }

                        if (user.Roles?.Count() > 0)
                        {
                            query = new Query("cta_usuario_grupo").AsDelete().Where("id_usuario", user.IdUsuario)
                                .Where("id_tenant", user.IdTenant);

                            await db.ExecuteAsync(query, transaction);


                            foreach (var role in user.Roles)
                            {
                                var userRole = new ApplicationUserRole
                                {
                                    IdGrupo = role.IdGrupo,
                                    IdUsuario = user.IdUsuario,
                                    IdTenant = user.IdTenant
                                };

                                query = new Query("cta_usuario_grupo").AsInsert(userRole);

                                await db.ExecuteAsync(query, transaction);
                            }
                        }

                        if (user.Tokens?.Count() > 0)
                        {
                            query = new Query("cta_usuario_tokens").AsDelete().Where("id_usuario", user.IdUsuario)
                                .Where("id_tenant", user.IdTenant);

                            await db.ExecuteAsync(query, transaction);

                            foreach (var tokens in user.Tokens)
                            {
                                var userToken = new ApplicationUserTokens
                                {
                                    IdUsuario = user.IdUsuario,
                                    IdTenant = user.IdTenant,
                                    ProvedorLogin = tokens.ProvedorLogin,
                                    Nome = tokens.Nome,
                                    Valor = tokens.Valor
                                };

                                query = new Query("cta_usuario_tokens").AsInsert(userToken);

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
                                    Description =
                                        $"Não foi possível atualizar o usuário {user.NomeUsuario}. A operação não pôde ser revertida."
                                });
                            }

                            return IdentityResult.Failed(new IdentityError
                            {
                                Code = nameof(UpdateAsync),
                                Description =
                                    $"Não foi possível atualizar o usuário {user.NomeUsuario}. A operação não pôde ser revertida."
                            });
                        }
                    }
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario AS u " +
                                   "INNER JOIN cta_usuario_claims AS uc ON u.id_usuario = uc.id_usuario " +
                                   "WHERE uc.claim_type = @ClaimType AND uc.claim_value = @ClaimValue;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (await sqlConnection.QueryAsync<ApplicationUser>(command, new
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                })).ToList();
            }
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario AS u " +
                                   "INNER JOIN cta_usuario_grupo AS ur ON u.id_usuario = ur.id_usuario " +
                                   "INNER JOIN cta_grupo AS r ON ur.id_grupo = r.id_grupo " +
                                   "WHERE r.nome_grupo = @RoleName and u.id_tenant = @IdTenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (await sqlConnection.QueryAsync<ApplicationUser>(command, new
                {
                    RoleName = roleName,
                    IdTenant = idTenant
                })).ToList();
            }
        }
    }
}