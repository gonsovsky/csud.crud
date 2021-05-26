using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Csud.Crud.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Csud.Crud
{
    public static class CsudService
    {
        public static void ConfigureServices(IServiceCollection services)
        {

            services.TryAdd(ServiceDescriptor.Singleton(typeof(PostgreService),
                typeof(PostgreService)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(MongoService),
                typeof(MongoService)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IDbService),
                typeof(DbService)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Account>),
                typeof(EntityService<Account>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<AccountProvider>),
                typeof(EntityService<AccountProvider>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Person>),
                typeof(EntityService<Person>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<ObjectX>),
                typeof(EntityService<ObjectX>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Subject>),
                typeof(EntityService<Subject>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<AppImport>),
                typeof(EntityService<AppImport>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<TaskX>),
                typeof(EntityService<TaskX>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<ObjectX>),
                typeof(EntityService<ObjectX>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Group>),
                typeof(EntityService<Group>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Context>),
                typeof(EntityService<Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<TimeContext>),
                typeof(EntityService<TimeContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<StructContext>),
                typeof(EntityService<StructContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<RuleContext>),
                typeof(EntityService<RuleContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<AttributeContext>),
                typeof(EntityService<AttributeContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<SegmentContext>),
                typeof(EntityService<SegmentContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<CompositeContext>),
                typeof(EntityService<CompositeContext>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context>),
                typeof(OneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<RuleContext, RuleContextAdd, RuleContextEdit, Context>),
                typeof(OneToOneService<RuleContext, RuleContextAdd, RuleContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<SegmentContext, SegmentContextAdd, SegmentContextEdit, Context>),
                typeof(OneToOneService<SegmentContext, SegmentContextAdd, SegmentContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<StructContext, StructContextAdd, StructContextEdit, Context>),
                typeof(OneToOneService<StructContext, StructContextAdd, StructContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<AttributeContext, AttributeContextAdd, AttributeContextEdit, Context>),
                typeof(OneToOneService<AttributeContext, AttributeContextAdd, AttributeContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<Group, GroupAdd, GroupEdit, Subject>),
                typeof(OneToManyService<Group, GroupAdd, GroupEdit, Subject>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<TaskX, TaskXAdd, TaskXEdit, ObjectX>),
                typeof(OneToManyService<TaskX, TaskXAdd, TaskXEdit, ObjectX>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<CompositeContext, CompositeContextAdd, CompositeContextEdit, Context>),
                typeof(OneToManyService<CompositeContext, CompositeContextAdd, CompositeContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<Relation>),
                typeof(EntityService<Relation>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<RelationDetails>),
                typeof(EntityService<RelationDetails>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToManyService<RelationDetails, RelationDetailsAdd, RelationDetailsEdit, Relation>),
                typeof(OneToManyService<RelationDetails, RelationDetailsAdd, RelationDetailsEdit, Relation>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context>),
                typeof(OneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IEntityService<RuleContext>),
                typeof(EntityService<RuleContext>)));

            services.AddSingleton(typeof(IContextService), typeof(ContextService));
        }
    }
}
