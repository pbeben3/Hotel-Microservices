using CustomerService.Tests.Helpers;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Customers.Api.Extensions;
using Hotel.Customers.Api.Services;
using Hotel.Customers.CrossCutting.Dtos;
using Hotel.Customers.Storage;
using Hotel.Customers.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerService.Tests.UnitTests
{
    public class CustomerServiceTests
    {
        private readonly List<Customer> _customerData;
        private readonly Mock<DbSet<Customer>> _mockSet;
        private readonly Mock<CustomerDbContext> _mockContext;
        private readonly Hotel.Customers.Api.Services.CustomerService _service;

        public CustomerServiceTests()
        {
            _customerData = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), FirstName = "Jan", LastName = "Kowalski", Email = "jkow@example.com" },
            new Customer { Id = Guid.NewGuid(), FirstName = "Robert", LastName = "Lewandowski", Email = "rlew@example.com" }
        };

            var queryableData = _customerData.AsQueryable();
            _mockSet = DbSetMockHelper.CreateMockDbSet(queryableData);


            var options = new DbContextOptionsBuilder<CustomerDbContext>().Options;
            _mockContext = new Mock<CustomerDbContext>(options);
            _mockContext.Setup(c => c.Set<Customer>()).Returns(_mockSet.Object);

            _service = new Hotel.Customers.Api.Services.CustomerService(_mockContext.Object);
        }

        // Sprawdza, czy metoda Get zwraca wszystkich klientów z bazy.
        [Fact]
        public async Task Get_ReturnsAllCustomers()
        {
            var customers = await _service.Get();

            Assert.NotNull(customers);
            Assert.Equal(_customerData.Count, customers.Count());
            Assert.Contains(customers, c => c.FirstName == "Jan");
        }

        // Sprawdza, czy metoda GetById zwraca klienta, gdy istnieje w bazie.
        [Fact]
        public async Task GetById_ReturnsCustomer_WhenExists()
        {
            var id = _customerData[0].Id;

            var customer = await _service.GetById(id);

            Assert.NotNull(customer);
            Assert.Equal("Jan", customer.FirstName);
        }

        // Sprawdza, czy metoda GetById zwraca null, gdy klient nie istnieje.
        [Fact]
        public async Task GetById_ReturnsNull_WhenNotExists()
        {
            var id = Guid.NewGuid();

            var customer = await _service.GetById(id);

            Assert.Null(customer);
        }

        // Sprawdza, czy metoda Create poprawnie dodaje nowego klienta do bazy.
        [Fact]
        public async Task Create_AddsNewCustomer()
        {
            Customer? addedCustomer = null;
            _mockSet.Setup(m => m.Add(It.IsAny<Customer>())).Callback<Customer>(c => addedCustomer = c);

            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            var newDto = new CustomerDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Robert",
                LastName = "Kubica",
                Email = "rkub@example.com"
            };

            var result = await _service.Create(newDto);

            Assert.Equal(CrudOperationResultStatus.Success, result.Status);
            Assert.NotNull(addedCustomer);
            Assert.Equal("Robert", addedCustomer.FirstName);
        }

        // Sprawdza, czy metoda Update zwraca RecordNotFound, gdy klient nie istnieje.
        [Fact]
        public async Task Update_ReturnsRecordNotFound_WhenCustomerDoesNotExist()
        {
            var nonExistingDto = new CustomerDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Test",
                Email = "test@example.com"
            };

            var result = await _service.Update(nonExistingDto);

            Assert.Equal(CrudOperationResultStatus.RecordNotFound, result.Status);
        }

        // Sprawdza, czy metoda Delete poprawnie usuwa klienta, gdy istnieje.
        [Fact]
        public async Task Delete_ReturnsSuccess_WhenCustomerExists()
        {
            var idToDelete = _customerData[0].Id;

            _mockSet.Setup(m => m.Remove(It.IsAny<Customer>())).Callback<Customer>(c => _customerData.Remove(c));
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _service.Delete(idToDelete);

            Assert.Equal(CrudOperationResultStatus.Success, result.Status);
            Assert.DoesNotContain(_customerData, c => c.Id == idToDelete);
        }

        // Sprawdza, czy metoda Delete zwraca RecordNotFound, gdy klient nie istnieje.
        [Fact]
        public async Task Delete_ReturnsRecordNotFound_WhenCustomerDoesNotExist()
        {
            var id = Guid.NewGuid();

            var result = await _service.Delete(id);

            Assert.Equal(CrudOperationResultStatus.RecordNotFound, result.Status);
        }
    }
}
