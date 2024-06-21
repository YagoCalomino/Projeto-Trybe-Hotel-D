using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var selectedRoom = _context.Rooms
                .Include(r => r.Hotel)
                    .ThenInclude(h => h!.City)
                .FirstOrDefault(r => r.RoomId == booking.RoomId);

            var userEntity = _context.Users.FirstOrDefault(u => u.Email == email);

            if (booking.GuestQuant > selectedRoom!.Capacity)
                throw new Exception("Guest quantity exceeds room capacity");

            var bookingEntity = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = userEntity!.UserId
            };

            _context.Bookings.Add(bookingEntity);
            _context.SaveChanges();

            var roomDetails = new RoomDto
            {
                RoomId = selectedRoom!.RoomId,
                Name = selectedRoom.Name,
                Capacity = selectedRoom.Capacity,
                Image = selectedRoom.Image,
                Hotel = new HotelDto
                {
                    HotelId = selectedRoom.Hotel!.HotelId,
                    Name = selectedRoom.Hotel.Name,
                    Address = selectedRoom.Hotel.Address,
                    CityId = selectedRoom.Hotel.CityId,
                    CityName = selectedRoom.Hotel.City!.Name,
                    State = selectedRoom.Hotel.City!.State
                }
            };

            return new BookingResponse
            {
                BookingId = bookingEntity.BookingId,
                CheckIn = bookingEntity.CheckIn,
                CheckOut = bookingEntity.CheckOut,
                GuestQuant = bookingEntity.GuestQuant,
                Room = roomDetails
            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r!.Hotel)
                        .ThenInclude(h => h!.City)
                .FirstOrDefault(b => b.BookingId == bookingId && b.User!.Email == email);

            if (booking == null)
                throw new Exception("Unauthorized");

            var roomDto = new RoomDto
            {
                RoomId = booking!.Room!.RoomId,
                Name = booking.Room.Name,
                Capacity = booking.Room.Capacity,
                Image = booking.Room.Image,
                Hotel = new HotelDto
                {
                    HotelId = booking.Room.Hotel!.HotelId,
                    Name = booking.Room.Hotel.Name,
                    Address = booking.Room.Hotel.Address,
                    CityId = booking.Room.Hotel.CityId,
                    CityName = booking.Room.Hotel.City!.Name,
                    State = booking.Room.Hotel.City!.State
                }
            };

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = roomDto
            };
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}