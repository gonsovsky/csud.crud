using Csud.Crud.Models;
using Csud.Crud.Models.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Csud.RestApi.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : RelationController<TaskX>
    {
    }

    [Route("api/group")]
    [ApiController]
    public class GroupController : RelationController<Group>
    {
    }

    [Route("api/person")]
    [ApiController]
    public class PersonController : BaseController<Person>
    {
    }

    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController<Account>
    {
    }

    [Route("api/accountProvider")]
    [ApiController]
    public class AccountProviderController : BaseController<AccountProvider>
    {
    }

    [Route("api/object")]
    [ApiController]
    public class ObjectController : BaseController<ObjectX>
    {
    }

    [Route("api/subject")]
    [ApiController]
    public class SubjectController : BaseController<Subject>
    {
    }


    //[Route("api/relation")]
    //[ApiController]
    //public class RelationController : BaseController<Relation>
    //{
    //}

    //[Route("api/relationDetail")]
    //[ApiController]
    //public class RelationDetailController : BaseController<RelationDetails>
    //{
    //}
}
