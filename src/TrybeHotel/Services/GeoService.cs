using System.Text.Json;
using TrybeHotel.Dto;
using TrybeHotel.Repository;

namespace TrybeHotel.Services
{
    public class GeoService : IGeoService
    {
         private readonly HttpClient _client;
         private const string _baseUrl = "https://nominatim.openstreetmap.org";
        public GeoService(HttpClient client)
        {
            _client = client;
        }

        // 11. Desenvolva o endpoint GET /geo/status
        public async Task<object?> GetGeoStatus()
        {
var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/status.php?format=json");
request.Headers.Add("Accept", "application/json");
request.Headers.Add("User-Agent", "aspnet-user-agent");

var response = await _client.SendAsync(request);

if (!response.IsSuccessStatusCode)
    return default;

var result = await response.Content.ReadFromJsonAsync<object>();

return result;
        }
        
        // 12. Desenvolva o endpoint GET /geo/address
        public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto)
        {
var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/search?street={geoDto.Address}&city={geoDto.City}&state={geoDto.State}&format=json&limit=1");
request.Headers.Add("Accept", "application/json");
request.Headers.Add("User-Agent", "aspnet-user-agent");

var response = await _client.SendAsync(request);

if (!response.IsSuccessStatusCode)
    return default!;

var jsonString = await response.Content.ReadAsStringAsync();

var results = JsonSerializer.Deserialize<List<GeoDtoResponse>>(jsonString);

return results!.FirstOrDefault();

        }

        // 12. Desenvolva o endpoint GET /geo/address
        public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository)
        {
            var originGeoLocation = await GetGeoLocation(geoDto);
            var hotels = repository.GetHotels();

            var hotelsByGeo = new List<GeoDtoHotelResponse>();

            foreach (var hotel in hotels)
            {
                var hotelGeoLocation = await GetGeoLocation(new GeoDto
                {
                    Address = hotel.Address,
                    City = hotel.CityName,
                    State = hotel.State
                });
                var distance = CalculateDistance(originGeoLocation.lat!, originGeoLocation.lon!, hotelGeoLocation.lat!, hotelGeoLocation.lon!);

                hotelsByGeo.Add(new GeoDtoHotelResponse
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityName = hotel.CityName,
                    State = hotel.State,
                    Distance = distance
                });
            }

            return hotelsByGeo;
        }
       

        public int CalculateDistance (string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny) {
            double latOrigin = double.Parse(latitudeOrigin.Replace('.',','));
            double lonOrigin = double.Parse(longitudeOrigin.Replace('.',','));
            double latDestiny = double.Parse(latitudeDestiny.Replace('.',','));
            double lonDestiny = double.Parse(longitudeDestiny.Replace('.',','));
            double R = 6371;
            double dLat = radiano(latDestiny - latOrigin);
            double dLon = radiano(lonDestiny - lonOrigin);
            double a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
            double distance = R * c;
            return int.Parse(Math.Round(distance,0).ToString());
        }

        public double radiano(double degree) {
            return degree * Math.PI / 180;
        }

    }
}