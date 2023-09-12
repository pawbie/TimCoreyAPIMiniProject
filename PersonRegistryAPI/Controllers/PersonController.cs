using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonRegistryLibrary.Interfaces;
using PersonRegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonRegistryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IDataAccess<PersonModel> _personDataAccess;
        private readonly IDataAccess<AddressModel> _addressDataAccess;
        private readonly ILogger<PersonController> _logger; 

        public PersonController(IDataAccess<PersonModel> personDataAccess, IDataAccess<AddressModel> addressDataAccess, ILogger<PersonController> logger)
        {
            _personDataAccess = personDataAccess;
            _addressDataAccess = addressDataAccess;
            _logger = logger;
        }

        // GET: api/<PersonController>
        [HttpGet]
        public IActionResult Get()
        {
            IActionResult output;
            PersonListModel data = new();

            data.Data = _personDataAccess.GetAll().ToList();
            output = Ok(data);

            return output;
        }

        // GET api/<PersonController>/5
        [HttpGet("{personId}")]
        public IActionResult GetById(int personId)
        {
            IActionResult output;

            try 
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == false)
                {
                    output = NotFound();
                }
                else
                {
                    var person = _personDataAccess.Get(personId);
                    output = Ok(person);
                }          
            }
            catch (Exception)
            {

                output = base.StatusCode(500);
            }

            return output;
        }

        // POST api/<PersonController>
        [HttpPost("create")]
        public IActionResult Add([FromBody] PersonModel person)
        {
            IActionResult output;

            try
            {
                if (ModelState.IsValid == false)
                {
                    output = BadRequest();
                }
                else
                {
                    _personDataAccess.Add(person);
                    output = Ok(person);
                }
            }
            catch (Exception)
            {
                output = StatusCode(500);
            }

            return output;
        }

        // POST api/<PersonController>
        [HttpPost("{personId}/update")]
        public IActionResult Update([FromRoute] int personId, PersonModel person)
        {
            IActionResult output = null;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == false)
                {
                    output = NotFound();
                }
                else if (ModelState.IsValid == false)
                {
                    output = BadRequest();
                }
                else
                {
                    person.Id = personId;
                    _personDataAccess.Update(person);
                    output = Ok(person);
                }
            }
            catch (Exception)
            {

                output = StatusCode(500);
            }

            return output;
        }

        [HttpPost("{personId}/delete")]
        public IActionResult Delete(int personId)
        {
            IActionResult output;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId))
                {
                    _personDataAccess.Remove(new PersonModel { Id = personId });
                    output = Ok();
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (Exception)
            {
                output = BadRequest();
            }

            return output;
        }
    }
}
