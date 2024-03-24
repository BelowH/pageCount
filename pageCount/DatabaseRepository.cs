using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.Data.Sqlite;
using pageCount.Domains.Models;

namespace pageCount;

public interface IDatabaseRepository
{

    public Task<bool> AddCount(Count count);

    public Task<IEnumerable<Count>?> GetByCountType(string primaryCountType, string secondaryCountType = "");

    public bool TestConnection();

}

public class DatabaseRepository : IDatabaseRepository
{


    private readonly string _connectionString;
    
    public DatabaseRepository(IConfiguration configuration)
    {
        string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        string dbPath = Path.Combine(workingDirectory, configuration["dbPath"]!);
        _connectionString = $"Data Source={dbPath}";
    }

    public async Task<bool> AddCount(Count count)
    {
        int affectedRows = 0;
        try
        {
            const string query ="INSERT INTO countdata (countId, userId , amount, primaryCountType, secondaryCountType, timestamp)" +
                                " VALUES (@CountId,@UserId, @Amount, @PrimaryCountType, @SecondaryCountType, @Timestamp);";
            var param = new{count.CountId, count.UserId, count.Amount, count.PrimaryCountType, count.SecondaryCountType,count.Timestamp};

            using IDbConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            affectedRows = await dbConnection.ExecuteAsync(query, param);
        }
        catch
        {
            // ignore
        }
        return affectedRows > 0;
    }

    public async Task<IEnumerable<Count>?> GetByCountType(string primaryCountType, string secondaryCountType = "")
    {
        IEnumerable<Count>? counts = null;
        try
        {
            string query =
                "SELECT countid, userId, amount, primarycounttype, secondarycounttype, timestamp FROM countdata WHERE primarycounttype = @PrimaryCountType";
            if (!string.IsNullOrWhiteSpace(secondaryCountType))
            {
                query += " AND secondarycounttype = @SecondaryCountType";
            }
            var param = new { PrimaryCountType = primaryCountType, SecondaryCountType = secondaryCountType };

            using IDbConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            counts = await dbConnection.QueryAsync<Count>(query, param);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // ignore
        }

        return counts;
    }

    public bool TestConnection()
    {
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}