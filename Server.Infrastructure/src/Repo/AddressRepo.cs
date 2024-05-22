using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo
{
    public class AddressRepo : IAddressRepo
    {
        private readonly AppDbContext _context;
        public AddressRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Address> CreateAddressAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressByIdAsync(Guid id)
        {
            var adrToDelete = await GetAddressByIdAsync(id);
            if (adrToDelete != null)
            {
                _context.Addresses.Remove(adrToDelete);
                await _context.SaveChangesAsync();
                return true; // Return true indicating successful deletion
            }
            else
            {
                return false; // Return false indicating user not found or deletion failed
            }
        }

        public async Task<Address> GetAddressByIdAsync(Guid id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
        }

        // public async Task<IEnumerable<Address>> GetAddressesByParamsAsync(QueryOptions options)
        // {
        //     IQueryable<Address> query = _context.Addresses;

        //     // Apply search filter if a search key is provided
        //     if (!string.IsNullOrWhiteSpace(options.SearchKey))
        //     {
        //         query = query.Where(a => a.AddressLine.Contains(options.SearchKey) ||
        //                                   a.City.Contains(options.SearchKey));
        //     }

        //     // Apply sorting if sort type and sort order are specified
        //     if (options.sortType.HasValue && options.sortOrder.HasValue)
        //     {
        //         switch (options.sortType.Value)
        //         {
        //             case SortType.byCity:
        //                 query = options.sortOrder.Value == SortOrder.asc ? query.OrderBy(a => a.City) : query.OrderByDescending(a => a.City);
        //                 break;
        //         }
        //     }

        //     // Apply pagination
        //     int skipCount = options.PageSize * options.PageNo;
        //     query = query.Skip(skipCount).Take(options.PageSize);

        //     return await query.ToListAsync();
        // }

        public async Task<IEnumerable<Address>> GetAddressesByUserAsync(Guid userId, QueryOptions? options)
        {
            IQueryable<Address> query = _context.Addresses.Where(adr => adr.UserId == userId);
            // Apply search filter if a search key is provided
            if (!string.IsNullOrWhiteSpace(options.SearchKey))
            {
                query = query.Where(a => a.AddressLine.Contains(options.SearchKey) ||
                                          a.City.Contains(options.SearchKey));
            }

            // Apply sorting if sort type and sort order are specified
            if (options.SortBy.HasValue && options.OrderBy.HasValue)
            {
                switch (options.SortBy.Value)
                {
                    case SortType.ByCity:
                        query = options.OrderBy.Value == SortOrder.Ascending ? query.OrderBy(a => a.City) : query.OrderByDescending(a => a.City);
                        break;
                }
            }

            // Apply pagination
            int skipCount = options.PageSize * (options.PageNo - 1);
            query = query.Skip(skipCount).Take(options.PageSize);

            return await query.ToListAsync();

        }


        public async Task<Address> GetDefaultAddressAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                // Find the default address for the user
                var defaultAddressId = user.DefaultAddressId;
                return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == defaultAddressId);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SetDefaultAddressAsync(Guid userId, Guid addressId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                // Find the default address for the user
                user.DefaultAddressId = addressId;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Address> UpdateAddressByIdAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}