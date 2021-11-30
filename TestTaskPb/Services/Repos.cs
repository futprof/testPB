using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskPb.Models;
using System.Data;
using TestTaskPb.Models.DB;
using Microsoft.Extensions.Options;

namespace TestTaskPb.Services
{
    public class Repos : IRepository
    {
        private readonly string _connectionString;
        

        public Repos(IOptions<MSSQLDBConfig> config)
        {
            _connectionString = config.Value.ConnectionString;
        }

       

        public string AddCashRequest(FullRequest request)
        {
            CashRequest cashRequest = new CashRequest {
                Id = Guid.NewGuid().ToString(),
                ClientId = request.ClientId,
                DepartmentId = GetDepartment(request.DepartemntAddress).Id
            };

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO [CashRequest] (Id, ClientId, DepartmentId)" +
                    "VALUES(@Id, @ClientId, @DepartmentId)";
                var rows = db.Execute(sqlQuery, cashRequest);
                if (rows > 0) return cashRequest.Id;
                return "";
            }
        }
                
        public string AddCashRequestDetails(string cashRequestId, FullRequest request)
        {
            string ipAdressId = AddIpAdress(request.IP);

            CashRequestDetails details = new CashRequestDetails {
                Id = Guid.NewGuid().ToString(),
                Date = DateTimeOffset.UtcNow,
                CashRequestId = cashRequestId,
                Amount = request.Amount,
                CurrencyId = request.Currency,
                StatusId = (int)RequestStatus.InProcessing
            };
            
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO [CashRequestDetails]" +
                    " VALUES (@Id, @Date, @CashRequestId, @Amount, @CurrencyId, @StatusId)";
                var rows = db.Execute(sqlQuery, details);
                if (rows > 0) return details.Id;
                return "";
            }
        }
               
        public string AddIpAdress(string ip)
        {
            var ipAdress = new IpAdress { Id = Guid.NewGuid().ToString(), Ip = ip };
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO [IpAdress] (Id, Ip) VALUES(@Id, @Ip)";
                var rows = db.Execute(sqlQuery, ipAdress);
                if (rows > 0) return ipAdress.Id;
                return "";
            }
        }

        public Client GetClient(string id)
        {
            throw new NotImplementedException();
        }

        public Department GetDepartment(string adress)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Department>("SELECT * FROM [Department] WHERE Adress = @adress", new { adress }).FirstOrDefault();                
            }
        }

        public string SaveCashRequest(FullRequest request)
        {
            string cashRequestId = AddCashRequest(request);
            AddCashRequestDetails(cashRequestId, request);
            return cashRequestId;
        }

        public Array GetStoredCashRequest(string id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM GetCashRequestById(@id)";
                var result = db.Query<StoredRequest>(sql, new { id }, commandType: CommandType.Text).ToArray();
                return result;
            }
        }

        public Array GetStoredCashRequest(string clientId, string departmentAdress)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM GetCashRequests(@clientId, @departmentAdress)";
                var result = db.Query<StoredRequest>(sql, new { clientId, departmentAdress }, commandType: CommandType.Text).ToArray();
                return result;
            }
        }
    }
}
