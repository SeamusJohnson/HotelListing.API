using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Services;

public class HotelsService(HotelListingDbContext context) : IHotelsService
{
    public async Task<IEnumerable<GetHotelDto>> GetHotelsAsync()
    {
        return await context.Hotels
            .Include(c => c.Country)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId,
                h.Country!.Name))
            .ToListAsync();
    }

    public async Task<GetHotelDto?> GetHotelAsync(int id)
    {
        var hotel = await context.Hotels
            .Where(h => h.Id == id)
            .Include(h => h.Country)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId,
                h.Country!.Name
                ))
            .FirstOrDefaultAsync();

        return hotel ?? null;
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto createDto)
    {
        var hotel = new Hotel
        {
            Name = createDto.Name,
            Address = createDto.Address,
            Rating = createDto.Rating,
            CountryId = createDto.CountryId
        };

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        return new GetHotelDto
        (
            hotel.Id,
            hotel.Name,
            hotel.Address,
            hotel.Rating,
            hotel.CountryId,
            string.Empty
        );
    }

    public async Task UpdateHotelAsync(int id, UpdateHotelDto updateDto)
    {
        var hotel = await context.Hotels.FindAsync(id) ?? throw new KeyNotFoundException("Hotel not found");

        hotel.Name = updateDto.Name;
        hotel.Address = updateDto.Address;
        hotel.Rating = updateDto.Rating;
        hotel.CountryId = updateDto.CountryId;
        context.Hotels.Update(hotel);
        await context.SaveChangesAsync();
    }

    public async Task DeleteHotelAsync(int id)
    {
        var hotel = await context.Hotels
            .Where(h => h.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> HotelExistsAsync(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }
}
