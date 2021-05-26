using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class GroupController : OneToManyController<Group, GroupAdd, GroupEdit, Subject>
    {
        public GroupController(IOneToManyService<Group, GroupAdd, GroupEdit, Subject> svc) : base(svc)
        {
        }
    }

    [Route("api/task")]
    [ApiController]
    public class TaskController : OneToManyController<TaskX, TaskXAdd, TaskXEdit, ObjectX>
    {
        public TaskController(IOneToManyService<TaskX, TaskXAdd, TaskXEdit, ObjectX> svc) : base(svc)
        {
        }
    }

    [Route("api/relation")]
    [ApiController]
    public class RelationController : OneToManyController<RelationDetails, RelationDetailsAdd, RelationDetailsEdit, Relation>
    {
        public RelationController(IOneToManyService<RelationDetails, RelationDetailsAdd, RelationDetailsEdit, Relation> svc) : base(svc)
        {
        }
    }

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
    public class AccountController : OneController<Account, AccountAdd, AccountEdit>
    {
        public AccountController(IEntityService<Account> svc) : base(svc)
        {
        }
    }

    
    [Route("api/accountProvider")]
    [ApiController]
    public class AccountProviderController : OneController<AccountProvider, AccountProviderAdd, AccountProviderEdit>
    {
        public AccountProviderController(IEntityService<AccountProvider> svc) : base(svc)
        {
        }
    }

    [Route("api/object")]
    [ApiController]
    public class ObjectController : OneController<ObjectX, ObjectXAdd, ObjectXEdit>
    {
        public ObjectController(IEntityService<ObjectX> svc) : base(svc)
        {
        }
    }

    [Route("api/subject")]
    [ApiController]
    public class SubjectController : OneController<Subject, SubjectAdd, SubjectEdit>
    {
        public SubjectController(IEntityService<Subject> svc) : base(svc)
        {
        }
    }
}
