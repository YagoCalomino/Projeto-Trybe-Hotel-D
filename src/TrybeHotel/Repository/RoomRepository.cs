using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
           return _context.Rooms.Where(r => r.HotelId == HotelId).Select(room => new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = room.Hotel!.HotelId,
                    Name = room.Hotel.Name,
                    Address = room.Hotel.Address,
                    CityId = room.Hotel.CityId,
                    CityName = room.Hotel.City!.Name,
                    State = room.Hotel.City!.State
                }
            });
        }

        // 8. Refatore o endpoint POST /room
            public RoomDto AddRoom(Room room)
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();

                return _context.Rooms.Where(r => r.RoomId == room.RoomId).Select(room => new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel!.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = room.Hotel.City!.Name,
                        State = room.Hotel.City!.State
                    }
                }).First();
        }

        public void DeleteRoom(int RoomId)
        {
           var room = _context.Rooms.Include(r => r.Hotel).Where(r => r.RoomId == RoomId).First();
            _context.Rooms.Remove(room);
            _context.SaveChanges(); 
        }
    }
}