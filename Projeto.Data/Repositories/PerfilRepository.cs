using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace Projeto.Data.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        //atributo
        private readonly string connectionString;

        //construtor para entrada de argumentos
        public PerfilRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Perfil entity)
        {
            var query = "insert into Perfil(Nome) values(@Nome)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Perfil entity)
        {
            var query = "update Perfil set Nome = @Nome where IdPerfil = @IdPerifl";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Perfil entity)
        {
            var query = "delete Perfil where IdPerfil = @IdPerfil";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Perfil> FindAll()
        {
            var query = "select * from Perfil";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Perfil>(query).ToList();
            }
        }

        public Perfil FindById(int id)
        {
            var query = "select * from Perfil where IdPerfil = @IdPerfil";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Perfil>(query, new { IdPerfil = id }).FirstOrDefault();
            }
        }

        
    }
}
