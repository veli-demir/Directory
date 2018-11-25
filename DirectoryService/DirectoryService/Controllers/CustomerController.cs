using DirectoryService.Dtos;
using DirectoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace DirectoryService.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private readonly ModelContainer context;

        public CustomerController() { this.context = new ModelContainer(); }

        // GET api/Customer/GetCustomer?
        [HttpGet]
        [ActionName("GetCustomer")]
        public ResponseObjectDto GetCustomer(int companyID, int customerID)
        {
            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + companyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                CustomerDto customerDto = (from c in context.Customer
                                           where c.CompanyID == companyID
                                           where c.ID == customerID
                                           select new CustomerDto
                                           {
                                               ID = c.ID,
                                               CompanyID = c.CompanyID,
                                               Name = c.Name,
                                               Email = c.Email,
                                               Description = c.Description,
                                               ImageUrl = c.ImageUrl,
                                               Status = c.Status,
                                           }).FirstOrDefault();

                if (customerDto != null)
                {
                    return new ResponseObjectDto(customerDto, 1, "Accept", 200);
                }
                else
                {
                    return new ResponseObjectDto(customerDto, 0, "No Content", 202);
                }
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }
        }

        // GET api/Customer/GetAllCustomersByPage?
        [HttpGet]
        [ActionName("GetAllCustomersByPage")]
        public ResponseObjectDto GetAllCustomersByPage(int companyID, int pageIndex, int pageSize, string sortColumn, bool sortOrder, int filterStatus, string filterSearch)
        {
            ModelState.Remove("sortColumn.String");
            ModelState.Remove("filterSearch.String");

            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + companyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                List<CustomerDto> customerDtoList = (from c in context.Customer.Include("CustomerPhone").Include("CustomerAddress").AsNoTracking()
                                                     where c.CompanyID == companyID
                                                     where c.Status >= filterStatus
                                                     where c.Name.Contains(filterSearch) || c.Email.Contains(filterSearch) || filterSearch == null
                                                     select new CustomerDto
                                                     {
                                                         ID = c.ID,
                                                         CompanyID = c.CompanyID,
                                                         Name = c.Name,
                                                         Email = c.Email,
                                                         Description = c.Description,
                                                         ImageUrl = c.ImageUrl,
                                                         Status = c.Status,

                                                         CustomerPhone = c.CustomerPhone.Select(x => new CustomerPhoneDto
                                                         {
                                                             ID = x.ID,
                                                             CustomerID = x.CustomerID,
                                                             Name = x.Name,
                                                             Description = x.Description,
                                                             PhoneNumber = x.PhoneNumber,
                                                             Status = x.Status,
                                                         }).ToList(),

                                                         CustomerAddress = c.CustomerAddress.Select(x => new CustomerAddressDto
                                                         {
                                                             ID = x.ID,
                                                             CustomerID = x.CustomerID,
                                                             Name = x.Name,
                                                             Description = x.Description,
                                                             Country = x.Country,
                                                             City = x.City,
                                                             District = x.District,
                                                             Address = x.Address,
                                                             Status = x.Status,
                                                         }).ToList(),
                                                     }).ToList();

                if (customerDtoList != null && customerDtoList.Count > 0)
                {
                    if (sortColumn == "name" && sortOrder)
                    {
                        customerDtoList = customerDtoList.OrderBy(x => x.Name).ToList();

                    }
                    else if (sortColumn == "name" && !sortOrder)
                    {
                        customerDtoList = customerDtoList.OrderByDescending(x => x.Name).ToList();
                    }
                    else if (sortColumn == "email" && sortOrder)
                    {
                        customerDtoList = customerDtoList.OrderBy(x => x.Email).ToList();

                    }
                    else if (sortColumn == "email" && !sortOrder)
                    {
                        customerDtoList = customerDtoList.OrderByDescending(x => x.Email).ToList();
                    }

                    return new ResponseObjectDto(customerDtoList.Skip((pageIndex - 1) * pageSize).Take(pageSize), customerDtoList.Count, "Accept", 200);
                }
                else
                {
                    return new ResponseObjectDto(customerDtoList, 0, "No Content", 202);
                }
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }
        }

        // POST api/Customer/CreateCustomer?
        [HttpPost]
        [ActionName("CreateCustomer")]
        public ResponseObjectDto CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + customerDto.CompanyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                Customer customer = new Customer()
                {
                    CompanyID = customerDto.CompanyID,
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    Description = customerDto.Description,
                    ImageUrl = customerDto.ImageUrl,
                    Status = customerDto.Status,
                    CreatorUID = 1,
                    CreatorIP = "::1",
                    CreatorRole = "company",
                    CreationDate = DateTime.Now,
                };

                context.Customer.Add(customer);

                context.SaveChanges();

                CustomerDto newCustomerDto = new CustomerDto()
                {
                    ID = customer.ID,
                    CompanyID = customer.CompanyID,
                    Name = customer.Name,
                    Email = customer.Email,
                    Description = customer.Description,
                    ImageUrl = customer.ImageUrl,
                    Status = customer.Status,
                };

                return new ResponseObjectDto(newCustomerDto, 1, "Accept", 200);
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }

        }

        // PUT api/Customer/CreateCustomer?
        [HttpPut]
        [ActionName("UpdateCustomer")]
        public ResponseObjectDto UpdateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + customerDto.CompanyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                Customer customer = (from c in context.Customer where c.CompanyID == customerDto.CompanyID where c.ID == customerDto.ID select c).FirstOrDefault();

                if (customer != null)
                {
                    customer.Name = customerDto.Name;
                    customer.Email = customerDto.Email;
                    customer.Description = customerDto.Description;
                    customer.ImageUrl = customerDto.ImageUrl;
                    customer.Status = customerDto.Status;
                }
                else
                {
                    return new ResponseObjectDto(null, 0, "No Content", 202);
                }

                context.SaveChanges();

                return new ResponseObjectDto(customerDto, 1, "Accept", 200);
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }
        }

        // DELETE api/Customer/DeleteCustomer?
        [HttpDelete]
        [ActionName("DeleteCustomer")]
        public ResponseObjectDto DeleteCustomer(int companyID, int customerID)
        {
            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + companyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                Customer customer = (from c in context.Customer where c.CompanyID == companyID where c.ID == customerID select c).FirstOrDefault();

                if (customer != null)
                {
                    customer.Status = -1;
                }
                else
                {
                    return new ResponseObjectDto(null, 0, "No Content", 202);
                }

                context.SaveChanges();

                return new ResponseObjectDto(null, 1, "Accept", 200);
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }
        }

        // DELETE api/Customer/RollbackCustomer?
        [HttpDelete]
        [ActionName("RollbackCustomer")]
        public ResponseObjectDto RollbackCustomer(int companyID, int customerID)
        {
            if (!ModelState.IsValid)
                return new ResponseObjectDto(null, 0, "Bad Request", 400);

            try
            {
                List<Claim> authorizedClaims = ((ClaimsIdentity)User.Identity).Claims.ToList();

                if (authorizedClaims[2].ToString() != "role: admin" && authorizedClaims[0].ToString() != "companyID: " + companyID)
                    return new ResponseObjectDto(null, 0, "Wrong Authority", 401);

                Customer customer = (from c in context.Customer where c.CompanyID == companyID where c.ID == customerID select c).FirstOrDefault();

                if (customer != null)
                {
                    customer.Status = 1;
                }
                else
                {
                    return new ResponseObjectDto(null, 0, "No Content", 202);
                }

                context.SaveChanges();

                return new ResponseObjectDto(null, 1, "Accept", 200);
            }
            catch (WebException exc)
            {
                return new ResponseObjectDto(null, 0, exc.Message, (int)(((HttpWebResponse)exc.Response)).StatusCode);
            }
        }
    }
}
