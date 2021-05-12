namespace Csud.Crud.DBTool.Promt.Pages
{
    class Person : Base
    {
        public Person(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 999;
    }

    class Account : Base
    {
        public Account(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 555;
    }

    class AccountProvider : Base
    {
        public AccountProvider(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 10;
    }

    class TimeContext : Base
    {
        public TimeContext(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 11;
    }

    class StructContext : Base
    {
        public StructContext(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 22;
    }

    class SegmentContext : Base
    {
        public SegmentContext(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 33;
    }

    class RuleContext : Base
    {
        public RuleContext(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 44;
    }

    class CompositeContext : Base
    {
        public CompositeContext(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 55;
    }

    class Subject : Base
    {
        public Subject(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 200;
    }

    class Group : Base
    {
        public Group(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 100;
    }

    class ObjectX : Base
    {
        public ObjectX(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 200;
    }

    class TaskX : Base
    {
        public TaskX(EasyConsole.Program program) : base(program)
        {
        }

        public override int Count { get; set; } = 100;
    }
}
