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

        public bool UpdateScandicHotelsFromFile()
        {
            try
            {
                var filePath = GetLatestScandicFilePath();
                var contents = File.ReadAllLines(filePath);

                foreach (var line in contents)
                {
                    var hotel = GetScandicHotel(line);
                    SetScandicHotelVacancies(hotel, line);
                    hotelRepository.Update(hotel);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool UpdateBestWesternHotelsFromFile()
        {
            try
            {
                var filePath = GetLatestBestWesternFilePath();
                var json = File.ReadAllText(filePath);
                var jsonHotels = JsonConvert.DeserializeObject<List<JsonHotel>>(json);

                foreach (var jsonHotel in jsonHotels)
                {
                    var hotel = hotelRepository.GetByName(jsonHotel.Name);
                    hotel.Vacancies = jsonHotel.LedigaRum;
                    hotelRepository.Update(hotel);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void SetScandicHotelVacancies(Hotel hotel, string line)
        {
            hotel.Vacancies = int.Parse(line.Split(",")[2]);
        }

        private Hotel GetScandicHotel(string line)
        {
            var split = line.Split(",");

            return hotelRepository.GetByName(split[1]);
        }

        private string GetLatestScandicFilePath()
        {
            var filePaths = Directory.GetFiles("Import");
            var scandicFilePaths = filePaths.Where(p => Path.GetFileNameWithoutExtension(p).StartsWith("Scandic")).ToList();

            if (scandicFilePaths.Count == 0)
                throw new FileNotFoundException("No Scandic file found");

            var latestFilePath = scandicFilePaths[0];

            foreach (var scandicFilePath in scandicFilePaths)
            {
                if (GetDateFromFileName(scandicFilePath) > GetDateFromFileName(latestFilePath))
                    latestFilePath = scandicFilePath;
            }

            return latestFilePath;
        }

        private string GetLatestBestWesternFilePath()
        {
            var filePaths = Directory.GetFiles("Import");
            var bestWesternFilePaths = filePaths.Where(p => Path.GetFileNameWithoutExtension(p).StartsWith("BestWestern")).ToList();

            if (bestWesternFilePaths.Count == 0)
                throw new FileNotFoundException("No BestWestern file found");

            var latestFilePath = bestWesternFilePaths[0];

            foreach (var scandicFilePath in bestWesternFilePaths)
            {
                if (GetDateFromFileName(scandicFilePath) > GetDateFromFileName(latestFilePath))
                    latestFilePath = scandicFilePath;
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
