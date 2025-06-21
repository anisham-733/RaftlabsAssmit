using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaftLabs.APIClient.Models;
using System.Text.Json.Serialization;


namespace RaftLabs.APIClient.Models
{
    public class UserData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("first_name")]
        public string First_Name { get; set; }

        [JsonPropertyName("last_name")]
        public string Last_Name { get; set; }

        public UserData ToDomain() => new UserData
        {
            Id = Id,
            Email = Email,
            First_Name = First_Name,
            Last_Name = Last_Name,
        };


    }


}
