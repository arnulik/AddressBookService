using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookService.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AddressBookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext personContext;

        public PersonController(PersonContext context)
        {
            personContext = context;

            //Data for demo
            if(personContext.PersonList.Count() == 0)
            {
                personContext.PersonList.Add(new Person {
                    FirstName = "Ержан",
                    LastName = "Калугин",
                    Email ="erj.kalugin@gmail.com",
                    Description ="sales manager",
                    SecondName = "Асылханулы"
                });
                personContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonList()
        {
            return await personContext.PersonList.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await personContext.PersonList.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> СreatePerson(Person person)
        {
            personContext.PersonList.Add(person);
            await personContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, Person item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            personContext.Entry(item).State = EntityState.Modified;
            await personContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await personContext.PersonList.FindAsync(id);
            if(person == null)
            {
                return NotFound();
            }

            personContext.PersonList.Remove(person);
            await personContext.SaveChangesAsync();

            return NoContent();
        }
    }
}