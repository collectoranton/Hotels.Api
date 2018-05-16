using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Api.Models
{
    public class FileParser
    {
        public List<Hotel> GetHotelsFromFile(string path)
        {
            var contents = File.ReadAllLines(path);
            var list = new List<Hotel>();

            foreach (var line in contents)
            {
                list.Add(ParseScandic(line));
            }

            return list;
        }

        private Hotel ParseScandic(string line)
        {
            var split = line.Split(",");

            return new Hotel
            {
                Region = RegionRepository.GetRegion(split[0]),
                Name = split[1],
                Vacancies = int.Parse(split[2])
            };
        }
    }
}
