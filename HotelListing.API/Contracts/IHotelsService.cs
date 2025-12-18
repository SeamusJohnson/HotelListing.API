using HotelListing.API.DTOs.Hotel;
using HotelListing.API.Results;

namespace HotelListing.API.Contracts
{
    public interface IHotelsService
    {
        Task<Result<GetHotelDto>> CreateHotelAsync(CreateHotelDto createDto);
        Task<Result> DeleteHotelAsync(int id);
        Task<Result<GetHotelDto>> GetHotelAsync(int id);
        Task<Result<IEnumerable<GetHotelDto>>> GetHotelsAsync();
        Task<bool> HotelExistsAsync(string name, int countryId);
        Task<Result> UpdateHotelAsync(int id, UpdateHotelDto updateDto);
    }
}