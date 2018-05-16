using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                    SetHotelVacancies(hotel, line);
                    hotelRepository.Update(hotel);
                }
            }
            catch (Exception e)
            {
                var poo = e.Message;
                return false;
            }

            return true;
        }

        private void SetHotelVacancies(Hotel hotel, string line)
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

            if (filePaths.Length == 0)
                throw new FileNotFoundException("No Scandic file found");

            var latestFilePath = filePaths[0];

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
