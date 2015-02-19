﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Architecture.Business.Exception;
using Architecture.Business.Facade.Interface;
using Microsoft.Ajax.Utilities;

namespace Architecture.Web.Code.Controller
{
    public abstract class BaseApiController : ApiController
    {
        public IBusinessLogicFacade BusinessLogicFacade { get; private set; }

        protected new virtual ClaimsPrincipal User
        {
            get { return HttpContext.Current.User as ClaimsPrincipal; }
        }

        protected BaseApiController(IBusinessLogicFacade businessLogicFacade)
        {
            BusinessLogicFacade = businessLogicFacade;
        }

        protected IHttpActionResult HandleGet<T>(Func<T> func)
        {
            try
            {
                return Ok(func());
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }
       
        protected IHttpActionResult HandlePost<TId, T>(Func<Tuple<TId, Dictionary<string, IList<string>>>> func, T t)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = func();
                if (res.Item2.Count > 0)
                {
                    AddToModelState(res.Item2);
                    return BadRequest(ModelState);                    
                }
                var link = Url.Link(Const.DefaultApiNameString, new { id = res.Item1 });
                return Created(link, t);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        protected IHttpActionResult HandlePutOrDelete(Func<Dictionary<string, IList<string>>> function)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var res = function();
                if (res.Count > 0)
                {
                   AddToModelState(res);
                   return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (OptimisticConcurrencyException)
            {
                return ResponseMessage(new HttpResponseMessage((HttpStatusCode)428));
            }
        }

        protected void AddToModelState(Dictionary<string, IList<string>> errors)
        {
            errors.ForEach(pair => pair.Value.ForEach(s => ModelState.AddModelError(pair.Key, s)));
        }

    }

}