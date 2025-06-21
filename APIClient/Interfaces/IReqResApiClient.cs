using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaftLabs.APIClient.Models;

namespace RaftLabs.APIClient.Interfaces
{
    public interface IReqResApiClient
    {
        Task<UserData> GetUserByIdAsync(int userId);
        Task<UserResponse> GetUsersByPageAsync(int pageNumber);
    }
}
