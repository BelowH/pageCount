using System.Data;
using System.Reflection;
using pageCount.Domains.Models;

namespace pageCount;

public interface IDatabaseRepository
{
    
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
            const string query ="INSERT INTO count (countid, amount, primarycounttype, secondarycounttype, timestamp)" +
                                " VALUES (@CountId, @Amount, @PrimaryCountType, @SecondaryCountType, @Timestamp);";
            var param = new{count.CountId, count.Amount, count.PrimaryCountType, count.SecondaryCountType,count.Timestamp};
          
            

        }
        catch
        {
            // ignore
        }
        
    }
    
    
    
}