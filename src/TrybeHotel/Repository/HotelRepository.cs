using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository //iniciando projeto
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        //  5. Refatore o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels.Select(hotel => new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = hotel.City!.Name,
                State = hotel.City!.State
            });
        }

        // 6. Refatore o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var createdHotel = _context.Hotels.Where(h => h.HotelId == hotel.HotelId).Include(h => h.City).First();

            return new HotelDto
            {
                HotelId = createdHotel.HotelId,
                Name = createdHotel.Name,
                Address = createdHotel.Address,
                CityId = createdHotel.CityId,
                CityName = createdHotel.City!.Name,
                State = createdHotel.City!.State
            };
        }
    }
}