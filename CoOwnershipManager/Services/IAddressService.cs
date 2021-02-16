using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoOwnershipManager.Data;

namespace CoOwnershipManager.Services
{
    public interface IAddressService
    {
        Task DeleteAsync(Address addresses);
        Task SaveSingleAsync(Address addresses);
        Task SaveManyAsync(Address[] addresses);
        Task SaveBulkAsync(Address[] addresses);
    }
}
