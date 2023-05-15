using CosmosDbAPI.Contracts;
using CosmosDbAPI.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDbAPI.Service
{
    public class CosmosDBService:ICosmosDBService
    {
        private Container _container;
        public CosmosDBService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddPersonAsync(Persons person)
        {
            await _container.CreateItemAsync(person, new PartitionKey(person.FirstName));
        }
        public async Task DeleteByIdAsync(string id, string partitionkey)
        {
            await _container.DeleteItemAsync<Persons>(id, new PartitionKey(partitionkey));
        }
        public async Task<Persons> GetByIdAsync(string id, string partitionkey)
        {
            try
            {
                var response = await _container.ReadItemAsync<Persons>(id, new PartitionKey(partitionkey));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<Persons>> GetAllAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Persons>(new QueryDefinition(queryString));
            var results = new List<Persons>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateByIdAsync(string id, Persons person)
        {
            await _container.UpsertItemAsync(person, new PartitionKey(person.FirstName));
        }
    }
}
