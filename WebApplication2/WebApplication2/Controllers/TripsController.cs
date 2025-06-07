using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly TripsDbContext _context;

    public TripsController(TripsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _context.TripDbfirsts
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTripDbfirsts)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom);

        var totalTrips = query.Count();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = totalPages,
            trips = trips.Select(t => new
            {
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                Countries = t.IdCountries.Select(c => new { c.Name }),
                Clients = t.ClientTripDbfirsts.Select(ct => new
                {
                    ct.IdClientNavigation.FirstName,
                    ct.IdClientNavigation.LastName
                })
            })
        };

        return Ok(result);
    }
    
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        var client = await _context.ClientDbfirsts
            .Include(c => c.ClientTripDbfirsts)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
            return NotFound($"klient o id {idClient} nie istenieje.");

        if (client.ClientTripDbfirsts.Any())
        {
            return BadRequest($"nie mozna usunac klient o id {idClient} – klient jest dopisany do wycieczki/wycieczek.");
        }

        _context.ClientDbfirsts.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent(); // 204 - successfully deleted
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTripAsync(int idTrip, [FromBody] AssignClientToTripDto request)
    {
        
        var trip = await _context.TripDbfirsts.FindAsync(idTrip);
        if (trip == null)
            return NotFound($"Wycieczka o {idTrip} nie istenieje.");
        
        if (trip.DateFrom <= DateTime.Now)
            return BadRequest("Nie moge dopisac do wycieczki ktora juz sie zakonczyla lub jest rozpoczeta.");

        
        var existingClient = await _context.ClientDbfirsts
            .FirstOrDefaultAsync(c => c.Pesel == request.Pesel);
        
        if (existingClient != null)
        {
            
            var alreadyAssigned = await _context.ClientTripDbfirsts
                .AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
            
            if (alreadyAssigned)
                return BadRequest("Klient jest juz przypisany do wycieczki.");

            
            _context.ClientTripDbfirsts.Add(new ClientTripDbfirst
            {
                IdClient = existingClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            });

            await _context.SaveChangesAsync();
            return Ok("Klient jest juz zapisany do wycieczki.");
        }

        
        var newClient = new ClientDbfirst
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Telephone = request.Telephone,
            Pesel = request.Pesel
        };

        _context.ClientDbfirsts.Add(newClient);
        await _context.SaveChangesAsync(); 

        _context.ClientTripDbfirsts.Add(new ClientTripDbfirst
        {
            IdClient = newClient.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = request.PaymentDate
        });

        await _context.SaveChangesAsync();
        return Ok("Klient stworzony i dodany do wycieczki.");
    }

}