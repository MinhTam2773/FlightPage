using FlightSearch.Components.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightSearch.Data
{
    public class ReservationManager
    {
        private static string reservationFilePath;
        public static string jsonString;

        public static string GenerateReservationCode()
        {
            List<Reservation> reservations = GetReservations();
            Random random = new Random();
            string reservationCode;
            if (reservations != null)
            {
                List<string> reservationCodes = reservations.Select(r => r.ReservationCode).ToList();
                do
                {
                    reservationCode = ((char)('A' + random.Next(0, 26))).ToString() + random.Next(1000, 9999).ToString();
                }
                while (reservationCodes.Contains(reservationCode));
            }
            else
            {
                return ((char)('A' + random.Next(0, 26))).ToString() + random.Next(1000, 9999).ToString();
            }

            return reservationCode;
        }

        public static List<Reservation> GetReservations()
        {
            try
            {
                string reservationFileName = "reservations.json";
                reservationFilePath = Path.Combine(FileSystem.AppDataDirectory, reservationFileName);
                var jsonData = File.ReadAllText(reservationFilePath);
                return JsonSerializer.Deserialize<List<Reservation>>(jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erorr" + e.Message);
            }
            return null;
        }

        public static void SaveReservations(List<Reservation> reservations)
        {
            try
            {
                string reservationFileName = "reservations.json";
                reservationFilePath = Path.Combine(FileSystem.AppDataDirectory, reservationFileName);
                if (File.Exists(reservationFilePath))
                {
                    jsonString = JsonSerializer.Serialize(reservations);
                }
                else
                {
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                    jsonString = JsonSerializer.Serialize(reservations, options);
                }
                File.WriteAllText(reservationFilePath, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erorr" + e.Message);
            }
        }
    }
}
