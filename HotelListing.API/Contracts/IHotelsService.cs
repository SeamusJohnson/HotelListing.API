using HotelListing.API.DTOs.Hotel;

namespace HotelListing.API.Contracts
{
    public interface IHotelsService
    {
        Task<GetHotelDto> CreateHotelAsync(CreateHotelDto createDto);
        Task DeleteHotelAsync(int id);
        Task<GetHotelDto?> GetHotelAsync(int id);
        Task<IEnumerable<GetHotelDto>> GetHotelsAsync();
        Task<bool> HotelExistsAsync(int id);
        Task UpdateHotelAsync(int id, UpdateHotelDto updateDto);
    }
}