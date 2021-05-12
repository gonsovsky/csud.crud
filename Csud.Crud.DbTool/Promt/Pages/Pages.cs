using Csud.Crud.DBTool.Promt.ConsoleEx;

namespace Csud.Crud.DBTool.Promt.Pages
{
    class Person : Base
    {
        public Person(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 99;
    }

    class Account : Base
    {
        public Account(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 55;
    }

    class AccountProvider : Base
    {
        public AccountProvider(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 3;
    }

    class TimeContext : Base
    {
        public TimeContext(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 7;
    }

    class StructContext : Base
    {
        public StructContext(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 8;
    }

    class SegmentContext : Base
    {
        public SegmentContext(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 9;
    }

    class RuleContext : Base
    {
        public RuleContext(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 10;
    }

    class CompositeContext : Base
    {
        public CompositeContext(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 11;
    }

    class Subject : Base
    {
        public Subject(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 33;
    }

    class Group : Base
    {
        public Group(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 20;
    }

    class ObjectX : Base
    {
        public ObjectX(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 33;
    }

    class TaskX : Base
    {
        public TaskX(ConsoleProgram consoleProgram) : base(consoleProgram)
        {
        }

        public override int Count { get; set; } = 20;
    }
}
