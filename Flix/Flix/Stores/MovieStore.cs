using Flix.Settings;
using Flix.Stores.Models;
using Flix.Stores.ProviderMappings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Flix.Stores;

public class MovieStore : MongoStore<Movie>, IMovieStore
{
	public MovieStore(IOptions<FlixDatabaseSettings> dbSettings) : base(dbSettings)
	{
	}

	public async Task AddMovieAsync(Movie movie)
	{
		await _collection.InsertOneAsync(movie);
	}

	public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
	{
		var movies = await _collection.Find(_ => true).ToListAsync();
		return movies;
	}

	public async Task<Movie> GetMovieByIdAsync(int id)
	{
		return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
	}

	public async Task<Movie> GetMovieByProviderIdAsync(string providerId, Provider provider)
	{
		var filter = Builders<Movie>.Filter.Eq(m => m.ProviderIds[provider], providerId.ToString());
		return await _collection.Find(filter).FirstOrDefaultAsync();
	}

	public async Task<bool> UpdateMovieAsync(Movie movie)
	{
		var filter = Builders<Movie>.Filter.Eq(m => m.Id, movie.Id);
		var result = await _collection.ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = false });
		return result.IsAcknowledged && result.ModifiedCount > 0;
	}
}