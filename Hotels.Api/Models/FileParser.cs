using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hotels.Api.Models
{
    public class FileParser
    {
        private readonly HotelRepository hotelRepository;

        public FileParser(HotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public List<Hotel> GetLatestScandicVacanciesFromFile()
        {
            var list = new List<Hotel>();

            try
            {
                var filePath = GetLatestFilePathThatStartsWith("Scandic");
                var contents = File.ReadAllLines(filePath);

                foreach (var line in contents)
                {
                    var hotel = GetScandicHotel(line);
                    SetScandicHotelVacancies(hotel, line);
                    list.Add(hotel);
                }
            }
            catch
            {
                return new List<Hotel>();
            }

            return list;
        }

        public List<Hotel> GetLatestBestWesternVacanciesFromFile()
        {
            var list = new List<Hotel>();

            try
            {
                var filePath = GetLatestFilePathThatStartsWith("BestWestern");
                var json = File.ReadAllText(filePath);
                var jsonHotels = JsonConvert.DeserializeObject<List<JsonHotel>>(json);

                foreach (var jsonHotel in jsonHotels)
                {
                    var hotel = hotelRepository.GetByName(jsonHotel.Name);
                    hotel.Vacancies = jsonHotel.LedigaRum;
                    list.Add(hotel);
                }
            }
            catch
            {
                return new List<Hotel>();
            }

            return list;
        }

        private void SetScandicHotelVacancies(Hotel hotel, string line)
        {
            hotel.Vacancies = int.Parse(line.Split(",")[2]);
        }

        private Hotel GetScandicHotel(string line)
        {
            return hotelRepository.GetByName(line.Split(",")[1]);
        }

        private string GetLatestFilePathThatStartsWith(string startsWith)
        {
            var allFilePaths = Directory.GetFiles("Import");
            var filePaths = GetPathForFilesThatStartWith(startsWith, allFilePaths);

            if (filePaths.Count == 0)
                throw new FileNotFoundException($"No file that starts with '{startsWith}' was found");

            return GetLatestFilePath(filePaths);
        }

        private static List<string> GetPathForFilesThatStartWith(string startsWith, IEnumerable<string> allFilePaths)
        {
            return allFilePaths.Where(p => Path.GetFileNameWithoutExtension(p).StartsWith(startsWith)).ToList();
        }

        private string GetLatestFilePath(List<string> filePaths)
        {
            var latestFilePath = filePaths.First();

            foreach (var filePath in filePaths)
            {
                if (GetDateFromFileName(filePath) > GetDateFromFileName(latestFilePath))
                    latestFilePath = filePath;
            }

            return latestFilePath;
        }

        private DateTime GetDateFromFileName(string path)
        {
            var filename = Path.GetFileNameWithoutExtension(path);

            var year = int.Parse(Regex.Match(filename, @"\d{4}").ToString());
            var month = int.Parse(Regex.Match(filename, @"(?:-)(\d{2})(?:-)").Groups[1].ToString());
            var date = int.Parse(Regex.Match(filename, @"(?:-)(\d{2})$").Groups[1].ToString());

            return new DateTime(year, month, date);
        }
    }
}
