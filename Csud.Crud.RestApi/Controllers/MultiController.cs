using Csud.Crud.Models.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class GroupController : RelationalController<Group, GroupAdd, Subject>
    {
    }

    [Route("api/task")]
    [ApiController]
    public class TaskController : RelationalController<TaskX, TaskAdd, ObjectX>
    {
    }

    //[Route("api/relation")]
    //[ApiController]
    //public class RelationController : RelationalController<RelationDetails, RelationDetailsAdd, Relation>
    //{
    //}

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
    //public class RelationalController : BaseController<Relation>
    //{
    //}

    //[Route("api/relationDetail")]
    //[ApiController]
    //public class RelationDetailController : BaseController<RelationDetails>
    //{
    //}
}
