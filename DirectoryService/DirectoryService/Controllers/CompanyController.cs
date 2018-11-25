using DirectoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DirectoryService.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ModelContainer context;

        public CompanyController() { this.context = new ModelContainer(); }

        // GET api/values
        public IEnumerable<Company> Get()
        {

            List<Company> company = (from c in context.Company where c.Status > 0 select c).ToList();

            return company;
        }

        // GET api/values/5
        public Company Get(int id)
        {
            Company company = (from c in context.Company where c.ID == id && c.Status > 0 select c).FirstOrDefault();

            return company;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            Company company = new Company();
            company.Name = value;

            context.Company.Add(company);

            context.SaveChanges();

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            Company company = (from c in context.Company where c.ID == id select c).FirstOrDefault();

            if(company !=null) {

                company.Name = value;
            }

            context.SaveChanges();
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            Company company = (from c in context.Company where c.ID == id select c).FirstOrDefault();

            if (company != null)
            {
                company.Status = -1;
            }

            context.SaveChanges();
        }
    }
}
