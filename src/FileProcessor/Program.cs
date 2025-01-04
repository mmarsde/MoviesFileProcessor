using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace FileProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var file = Path.Combine("Assets", "movies.csv");
        
        var config = new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture) {
            MissingFieldFound = null,
            ShouldSkipRecord = row => row.Row.Parser.RawRecord.StartsWith(" - "),
        };
        
        using var streamReader = new StreamReader(file);
        using var csvReader = new CsvReader(streamReader, config);

        csvReader.Context.RegisterClassMap<MovieItemMap>();
        var records = csvReader.GetRecords<MovieItem>().OrderByDescending(r => r.ReleaseDate).Take(100).ToList();
        
        records.ForEach(record => Console.WriteLine(record.ToString()));
        
        Console.WriteLine($"Number of records processed: {records.Count}");
    }

    public class MovieItem
    {
        [Name("Release_Date")]
        public DateTime ReleaseDate { get; init; }
        [Name("Title")]
        public string Title { get; init; }
        [Name("Overview")]
        public string Overview { get; init; }
        [Name("Popularity")]
        public double Popularity { get; init; }
        [Name("Vote_Count")]
        public int VoteCount { get; init; }
        [Name("Vote_Average")]
        public double VoteAverage { get; init; }
        [Name("Original_Language")]
        public string OriginalLanguage { get; init; }
        [Name("Genre")]
        public string Genre { get; init; }
        [Name("Poster_Url")]
        public string PosterUrl { get; init; }
        public override string ToString() => $"Movie: {Title}, YearReleased: {ReleaseDate.ToShortDateString()}, Genre: {Genre}, VoteAverage: {VoteAverage}";
    }
    
    public sealed class MovieItemMap : ClassMap<MovieItem>
    {
        public MovieItemMap()
        {
            Map(m => m.ReleaseDate).Name("Release_Date").Default(default(DateTime));
            Map(m => m.Title).Name("Title");
            Map(m => m.Overview).Name("Overview");
            Map(m => m.Popularity).Name("Popularity").Default(default(double));
            Map(m => m.VoteCount).Name("Vote_Count").Default(default(int));
            Map(m => m.VoteAverage).Name("Vote_Average").Default(default(double));
            Map(m => m.OriginalLanguage).Name("Original_Language");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.PosterUrl).Name("Poster_Url");
        }
    }
}