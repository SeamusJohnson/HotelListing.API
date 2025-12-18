using HotelListing.API.Contracts;
using HotelListing.API.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : BaseApiController
{

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var results = await hotelsService.GetHotelsAsync();
        return ToActionResult(results);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var results = await hotelsService.GetHotelAsync(id);
        return ToActionResult(results);
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest("ID route value must match payload ID.");
        }

        var results = await hotelsService.UpdateHotelAsync(id, hotelDto);
        return ToActionResult(results);
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotelDto)
    {
        var results = await hotelsService.CreateHotelAsync(hotelDto);
        return ToActionResult(results);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
        var results = await hotelsService.DeleteHotelAsync(id);
        return ToActionResult(results);
    }
}
