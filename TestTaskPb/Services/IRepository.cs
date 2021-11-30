using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskPb.Models;
using TestTaskPb.Models.DB;

namespace TestTaskPb.Services
{
    public interface IRepository
    {
        
        // This method return Id Guid of created row
        /// <summary>        
        /// Returns guid if sucess and empty string when error.   
        /// </summary>
        string AddCashRequest(FullRequest request);
        // This method return Id Guid of created row
        /// <summary>        
        /// Returns guid if sucess and empty string when error.   
        /// </summary>
        string AddCashRequestDetails(string cashRequestId, FullRequest request);
        // This method return Id Guid of created row
        /// <summary>        
        /// Returns guid if sucess and empty string when error.   
        /// </summary>
        string AddIpAdress(string ip);
        // This method return department by adress
        /// <summary>        
        /// Returns guid if sucess and empty string when error.   
        /// </summary>
        Department GetDepartment(string adress);
        // This method return client by id
        Client GetClient(string id);

        // This method consume all logic for store incoming request
        /// <summary>        
        /// Returns guid of CashRequest if sucess and empty string when error.   
        /// </summary>
        string SaveCashRequest(FullRequest request);
        // This method returns stored cash requests by id
        Array GetStoredCashRequest(string id);
        // This method returns stored cash requests
        Array GetStoredCashRequest(string clientId, string departmentAdress);
        
    }
}
