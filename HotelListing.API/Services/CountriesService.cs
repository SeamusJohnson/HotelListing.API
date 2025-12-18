using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Constants;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.DTOs.Country;
using HotelListing.API.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Services;

public class CountriesService(HotelListingDbContext context, IMapper mapper) : ICountriesService
{
    // Fetch all countries
    public async Task<Result<IEnumerable<GetCountriesDto>>> GetCountriesAsync()
    {
        // Fetch all countries
        var countries = await context.Countries
            .ProjectTo<GetCountriesDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetCountriesDto>>.Success(countries);
    }

    // Fetch country by ID
    public async Task<Result<GetCountryDto>> GetCountryAsync(int id)
    {
        try
        {
            // Fetch country by ID
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return Result<GetCountryDto>.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
            }

            var result = mapper.Map<GetCountryDto>(country);

            return Result<GetCountryDto>.Success(result);
        }
        catch (Exception)
        {
            return Result<GetCountryDto>.Failure();
        }
    }

    // Update country details
    public async Task<Result> UpdateCountryAsync(int id, UpdateCountryDto updateDto)
    {
        try
        {
            // Validate ID consistency
            if (id != updateDto.CountryId)
            {
                return Result.BadRequest(new Error(ErrorCodes.Validation, "ID route value does not match payload ID."));
            }

            // Check if country exists
            var country = await context.Countries.FindAsync(id);
            if (country is null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
            }

            //Check for duplicate country name
            var duplicateName = await CountryExistsAsync(updateDto.Name);
            if (duplicateName)
                {
                    return Result.Failure(new Error(ErrorCodes.Conflict, $"Country with name '{updateDto.Name}' already exists."));
                }

            // Update country details
            mapper.Map(updateDto, country);
            context.Countries.Update(country);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure();
        }
    }

    // Create a new country
    public async Task<Result<GetCountryDto>> CreateCountryAsync(CreateCountryDto createDto)
    {
        try
        {
            // Check for existing country with the same name
            var exists = await CountryExistsAsync(createDto.Name);
            if (exists)
            {
                return Result<GetCountryDto>.Failure(new Error(ErrorCodes.Conflict, $"Country with name '{createDto.Name}' already exists."));
            }

            // Create and save new country
            var country = mapper.Map<Country>(createDto);

            // Add country to context and save
            context.Countries.Add(country);
            await context.SaveChangesAsync();

            // Prepare DTO for the created country
            var dto = mapper.Map<GetCountryDto>(country);

            return Result<GetCountryDto>.Success(dto);
        }
        catch (Exception)
        {
            return Result<GetCountryDto>.Failure();
        }
    }

    // Delete country by ID
    public async Task<Result> DeleteCountryAsync(int id)
    {
        try
        {
            // Find country by ID
            var country = await context.Countries.FindAsync(id);
            if (country is null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
            }

            // Remove country from context and save
            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure();
        }
    }

    // Check if country exists by ID
    public async Task<bool> CountryExistsAsync(int id)
    {
        return await context.Countries.AnyAsync(e => e.CountryId == id);
    }

    // Check if country exists by name
    public async Task<bool> CountryExistsAsync(string name)
    {
        return await context.Countries
            .AnyAsync(e => e.Name.ToLower().Trim() == name.ToLower().Trim());
    }
}
