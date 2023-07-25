using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ReminderSchedule.Repositories
{
    public abstract class GenericRepository<T> 
    {

        //prop
       
        private readonly IConfiguration configuration;
        protected string connectionString = "";

        public GenericRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetSection("ConnectionStrings:ReminderDB").Value;
        }

        public async Task<IEnumerable<T>> GetAll() 
        {
            var classname = typeof(T).Name;
            var sqlCommand = $"SELECT * FROM {classname} ORDER BY [RemindDate], [RemindTime]";
            using (var db = new SqlConnection(connectionString)) 
            {
                var result = await db.QueryAsync<T>(sqlCommand);
                return result.ToList();
            }
        }
        public async Task<T> GetById(int Id)
        {
            var classname = typeof(T).Name;
            var sqlCommand = $"SELECT * FROM {classname} WHERE [Id] = @Id";
            using (var db = new SqlConnection(connectionString))
            {
                var result = await db.QueryAsync<T>(sqlCommand,new { id = Id});
                return result.FirstOrDefault() ;
            }
        }

        public abstract Task<int> Add(T model);

        public abstract Task<int> Update(T model);

        public abstract Task<int> Delete(int id);
    }


}
