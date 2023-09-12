using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonRegistryLibrary.Interfaces;
using PersonRegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PersonRegistryAPI.Controllers
{
    [Route("api/Person/{personId}/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IDataAccess<AddressModel> _addressDataAccess;
        private readonly IDataAccess<PersonModel> _personDataAccess;

        public AddressController(IDataAccess<PersonModel> personDataAccess, IDataAccess<AddressModel> addressDataAccess)
        {
            _addressDataAccess = addressDataAccess;
            _personDataAccess = personDataAccess;
        }

        // GET: api/<PersonController>
        [HttpGet]
        public IActionResult Get(int personId)
        {
            IActionResult output;
            AddressListModel data = new();

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == true)
                {
                    data.Data.AddRange(_addressDataAccess.GetAll().Where(x => x.PersonId == personId));
                    output = Ok(data);
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (System.Exception)
            {

                output = StatusCode(500);
            }

            return output;
        }

        // GET api/<PersonController>/5
        [HttpGet("{addressId}")]
        public IActionResult Get(int personId, int addressId)
        {
            IActionResult output;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == true &&
                    _addressDataAccess.CheckIfRecordExists(addressId) == true)
                {
                    output = Ok(_addressDataAccess.Get(addressId));
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (Exception)
            {

                output = StatusCode(500);
            }

            return output;
        }

        // POST api/<PersonController>
        [HttpPost("create")]
        public IActionResult Add([FromRoute] int personId, [FromBody] AddressModel address)
        {
            IActionResult output;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == true)
                {
                    if (ModelState.IsValid)
                    {
                        address.PersonId = personId;

                        _addressDataAccess.Add(address);
                        output = Ok(address);
                    }
                    else
                    {
                        output = BadRequest();
                    }
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (Exception)
            {
                output = StatusCode(500);
            }

            return output;

        }

        // POST api/<PersonController>
        [HttpPost("{addressId}/update")]
        public IActionResult Update([FromRoute] int personId, [FromRoute] int addressId, AddressModel address)
        {
            IActionResult output;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == true &&
                    _addressDataAccess.CheckIfRecordExists(addressId) == true)
                {
                    if (ModelState.IsValid)
                    {
                        address.Id = addressId;
                        address.PersonId = personId;

                        _addressDataAccess.Update(address);
                        output = Ok(address);
                    }
                    else
                    {
                        output= BadRequest();
                    }
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (Exception)
            {

                output = StatusCode(500);
            }

            return output;
        }

        [HttpPost("{addressId}/delete")]
        public IActionResult Delete([FromRoute] int personId, [FromRoute] int addressId)
        {
            IActionResult output;

            try
            {
                if (_personDataAccess.CheckIfRecordExists(personId) == true &&
                    _addressDataAccess.CheckIfRecordExists(addressId) == true)
                {
                    _addressDataAccess.Remove(new AddressModel { Id = addressId });
                    output = Ok();
                }
                else
                {
                    output = NotFound();
                }
            }
            catch (Exception)
            {

                output = StatusCode(500);
            }

            return output;
        }
    }
}
