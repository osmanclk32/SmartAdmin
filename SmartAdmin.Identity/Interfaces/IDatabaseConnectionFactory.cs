using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAdmin.Identity.Interfaces
{
    /// <summary>
    /// Responsável por retornar a conexão do banco de dados
    /// </summary>
    public interface IDatabaseConnectionFactory
    {
        /// <summary>
        /// Retorna Conexão de Banco de Dados
        /// </summary>
        /// <returns></returns>
        Task<IDbConnection> CreateConnectionAsync();
    }
}
