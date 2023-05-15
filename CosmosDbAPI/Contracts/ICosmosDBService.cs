using CosmosDbAPI.Models;

namespace CosmosDbAPI.Contracts
{
    public interface ICosmosDBService
    {
        Task<IEnumerable<Persons>> GetAllAsync(string query);
        Task<Persons>  GetByIdAsync(string id, string partitionkey);
        Task DeleteByIdAsync(string id, string partitionkey);
        Task UpdateByIdAsync(string id, Persons person);
        Task AddPersonAsync(Persons person);
    }
}
