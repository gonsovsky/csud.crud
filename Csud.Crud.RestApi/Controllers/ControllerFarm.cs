using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
   
    [Route("api/group")]
    [ApiController]
    public class GroupController : OneToManyController<Group, GroupAdd, Subject>
    {
        public GroupController(IOneToManyService<Group, GroupAdd, Subject> svc) : base(svc)
        {
        }
    }

    [Route("api/task")]
    [ApiController]
    public class TaskController : OneToManyController<TaskX, TaskAdd, ObjectX>
    {
        public TaskController(IOneToManyService<TaskX, TaskAdd, ObjectX> svc) : base(svc)
        {
        }
    }

    //[Route("api/relation")]
    //[ApiController]
    //public class RelationController : OneToManyController<RelationDetails, RelationDetailsAdd, Relation>
    //{
    //}

    [Route("api/person")]
    [ApiController]
    public class PersonController : EntityController<Person>
    {
        public PersonController(IEntityService<Person> svc) : base(svc)
        {
        }
    }

    [Route("api/account")]
    [ApiController]
    public class AccountController : EntityController<Account>
    {
        public AccountController(IEntityService<Account> svc) : base(svc)
        {
        }
    }

    [Route("api/accountProvider")]
    [ApiController]
    public class AccountProviderController : EntityController<AccountProvider>
    {
        public AccountProviderController(IEntityService<AccountProvider> svc) : base(svc)
        {
        }
    }

    [Route("api/object")]
    [ApiController]
    public class ObjectController : EntityController<ObjectX>
    {
        public ObjectController(IEntityService<ObjectX> svc) : base(svc)
        {
        }
    }

    [Route("api/subject")]
    [ApiController]
    public class SubjectController : EntityController<Subject>
    {
        public SubjectController(IEntityService<Subject> svc) : base(svc)
        {
        }
    }

    [Route("api/maintenance")]
    [ApiController]
    public partial class MaintenanceController : EntityController<AppImport>
    {
        public MaintenanceController(IEntityService<AppImport> svc) : base(svc)
        {
        }
    }


    //[Route("api/relation")]
    //[ApiController]
    //public class OneToManyController : EntityController<Relation>
    //{
    //}

    //[Route("api/relationDetail")]
    //[ApiController]
    //public class RelationDetailController : EntityController<RelationDetails>
    //{
    //}
}
