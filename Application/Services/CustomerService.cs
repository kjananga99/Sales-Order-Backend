using SalesOrderAPI.Application.DTOs;
using SalesOrderAPI.Application.Interfaces;
using SalesOrderAPI.Domain.Entities;

namespace SalesOrderAPI.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address1 = c.Address1,
                Address2 = c.Address2,
                Address3 = c.Address3,
                Suburb = c.Suburb,
                State = c.State,
                PostCode = c.PostCode
            });
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                Address3 = customer.Address3,
                Suburb = customer.Suburb,
                State = customer.State,
                PostCode = customer.PostCode
            };
        }
    }
}